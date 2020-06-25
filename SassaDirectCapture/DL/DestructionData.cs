using Oracle.ManagedDataAccess.Client;
using SASSADirectCapture.EntityModels;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace SASSADirectCapture
{
    public class DestructionData
    {
        public Shared _data;
        public DestructionStatus DestructionStatus;
        public ExclusionType ExclusionTypes;
        public PStatus ProcessStatus;
        public int regionId;
        public string userName;

        public DestructionData(int _regionId)
        {
            regionId = _regionId;
            _data = new Shared();
            ExclusionTypes = new ExclusionType();
            DestructionStatus = new DestructionStatus();
        }

        public List<string> DestructionYears
        {
            get
            {
                List<string> years = new List<string>();
                int y = 2005;
                years.Add("<2004");
                while (y < DateTime.Now.Year)
                {
                    years.Add(y++.ToString());
                }
                return years;
            }
        }


        public List<string> UndestroyedYears()
        {
            using (Entities context = new Entities())
            {
                var existingyears = context.DC_EXCLUSION_BATCH.Where(ex => ex.REGION_ID == regionId).Select(ex => ex.EXCLUSION_YEAR).ToList();
                return DestructionYears.Except(existingyears).ToList();
            }
        }


        public void UpdateDestructionStatus(string pension_no, string status)
        {

            try
            {
                using (Entities context = new Entities())
                {
                    DC_DESTRUCTION de = context.DC_DESTRUCTION.Where(d => d.PENSION_NO == pension_no).First();
                    de.STATUS = status;
                    de.STATUS_DATE = DateTime.Now.ToString("yyyyMMdd");
                    context.SaveChanges();
                }
            }
            catch 
            {
                throw;
            }
        }

        public void AddExclusion(string exclusionType, string pension_no, string UserName)
        {
            using (Entities context = new Entities())
            {
                //check for valid exclusionType
                if (!ExclusionTypes.Contains(exclusionType)) return;
                //Check if duplicate
                if (context.DC_EXCLUSIONS.Where(d => d.ID_NO == pension_no).Any()) return;
                //Check if Destruction record exists
                if (!context.DC_DESTRUCTION.Where(d => d.PENSION_NO == pension_no).Any()) return;
                //todo: Check if batch exists for this year/region
                DC_EXCLUSIONS exclusion = new DC_EXCLUSIONS
                {
                    EXCLUSION_TYPE = exclusionType,
                    REGION_ID = regionId,
                    ID_NO = pension_no,
                    USERNAME = UserName,
                    EXCLUSION_BATCH_ID = 0,
                    EXCL_DATE = DateTime.Now
                };
                UpdateDestructionStatus(pension_no, "Excluded");
                context.DC_EXCLUSIONS.Add(exclusion);
                context.SaveChanges();
            }
        }

        public int AddExclusionBatch(int region_id, string UserName, string destructionYear)
        {
            using (Entities context = new Entities())
            {
                DC_EXCLUSION_BATCH exclusionb = new DC_EXCLUSION_BATCH
                {
                    REGION_ID = region_id,
                    EXCLUSION_YEAR = destructionYear.Replace(" ", ""),
                    CREATED_BY = UserName,
                    CREATED_DATE = DateTime.Now.ToString("yyyyMMdd")
                };

                context.DC_EXCLUSION_BATCH.Add(exclusionb);
                context.SaveChanges();
                return exclusionb.BATCH_ID;
            }
        }

        public void UpdateExclusionBatch(int batchId, int regionId)
        {
            try
            {
                using (Entities context = new Entities())
                {
                    foreach (var excl in context.DC_EXCLUSIONS.Where(e => e.REGION_ID == regionId && e.EXCLUSION_BATCH_ID == 0).ToList())
                    {
                        excl.EXCLUSION_BATCH_ID = batchId;
                        DC_DESTRUCTION dd = context.DC_DESTRUCTION.Where(d => d.PENSION_NO == excl.ID_NO).First();
                        dd.EXCLUSIONBATCH_ID = batchId;
                    }
                    context.SaveChanges();
                }
            }
            catch 
            {
                throw;
            }
        }

        //public List<VIEW_DESTRUCTION_BATCH> GetApprovalBatches()
        //{
        //    using (Entities context = new Entities())
        //    {
        //        var query =
        //        from b in context.DC_DESTRUCTION_BATCH
        //        join r in context.DC_REGION on b.REGION_ID.ToString().Trim() equals r.REGION_ID.Trim()
        //        select new VIEW_DESTRUCTION_BATCH() {
        //            APPROVED_BY = b.APPROVED_BY,
        //            BATCH_ID = b.BATCH_ID,
        //            DESTRUCTION_YEAR = b.DESTRUCTION_YEAR,
        //            REGION_NAME = r.REGION_NAME,
        //            CREATED_BY = b.CREATED_BY,
        //            CREATED_DATE =b.CREATED_DATE }; 
        //        return query.Where(e => e.APPROVED_BY == null ).ToList();
        //    }
        //}


        public int AddApprovalBatch(int region_id, string UserName, string destructionYear)
        {
            using (Entities context = new Entities())
            {
                if (context.DC_DESTRUCTION_BATCH.Where(d => d.DESTRUCTION_YEAR == destructionYear.Replace(" ", "")).Any())
                {
                    return 0;
                }
                else
                {
                    var exbaches = context.DC_EXCLUSION_BATCH.Where(e => e.EXCLUSION_YEAR == destructionYear);
                    if (exbaches.Any())
                    {
                        foreach (var ex in exbaches.ToList())
                        {
                            ex.APPROVED_BY = UserName;
                            ex.APPROVED_DATE = DateTime.Now.ToString("yyyyMMdd");
                        }
                    }
                }
                //Tag the exclusion Batch with Approver
                DC_DESTRUCTION_BATCH approvalb = new DC_DESTRUCTION_BATCH
                {
                    REGION_ID = region_id,
                    DESTRUCTION_YEAR = destructionYear.Replace(" ", ""),
                    CREATED_BY = UserName,
                    CREATED_DATE = DateTime.Now.ToString("yyyyMMdd"),
                    APPROVED_BY = UserName,
                    APPROVED_DATE = DateTime.Now.ToString("yyyyMMdd")
                };

                context.DC_DESTRUCTION_BATCH.Add(approvalb);
                context.SaveChanges();
                if (destructionYear.Trim().StartsWith("<"))
                {
                    destructionYear = "0000";
                }
                //Tag all records for the destruction year without exclusions with the batch id
                //Set the status to Approved
                //Set the approver in the DESTRUCTION batch
                string sql = string.Format("UPDATE DC_DESTRUCTION dx " +
                            "SET dx.DESTRUCTION_BATCH_ID ={0}, dx.STATUS = 'Approved', dx.STATUS_DATE = '" + DateTime.Now.ToString("yyyyMMdd") + "' " +
                            "WHERE SUBSTR(dx.DESTRUCTIO_DATE,0,4) = '{1}' AND dx.EXCLUSIONBATCH_ID = 0 AND dx.Status ='TDWFound'", approvalb.BATCH_ID, destructionYear);

                _data.ExecuteNonQuery(sql);
                return approvalb.BATCH_ID;
            }
        }

        public List<VIEW_EXCLUSION_BATCH> GetExclusionBatches(string year)
        {
            using (Entities context = new Entities())
            {
                var regions =
                    (from r in context.DC_REGION
                     select new VIEW_REGION()
                     {
                         RegionString = r.REGION_ID,
                         RegionName = r.REGION_NAME
                     }).ToList();
                return context.DC_EXCLUSION_BATCH
                 .Where(e => e.APPROVED_BY == null && e.EXCLUSION_YEAR == year)
                 .Select(x =>
                 new VIEW_EXCLUSION_BATCH()
                 {
                     REGION_ID = x.REGION_ID,
                     APPROVED_BY = x.APPROVED_BY,
                     BATCH_ID = x.BATCH_ID,
                     EXCLUSION_YEAR = x.EXCLUSION_YEAR,
                     CREATED_BY = x.CREATED_BY,
                     CREATED_DATE = x.CREATED_DATE
                 })
                 .AsEnumerable() // database query ends here, the rest is a query in memory
                 .Join(regions, f => f.REGION_ID, p => p.RegionId, (f, p) =>
                 new VIEW_EXCLUSION_BATCH()
                 {
                     REGION_NAME = p.RegionName,
                     APPROVED_BY = f.APPROVED_BY,
                     BATCH_ID = f.BATCH_ID,
                     EXCLUSION_YEAR = f.EXCLUSION_YEAR,
                     CREATED_BY = f.CREATED_BY,
                     CREATED_DATE = f.CREATED_DATE
                 })
                 .ToList();
            }
            //using (Entities context = new Entities())
            //{
            //    return context.DC_EXCLUSION_BATCH.Where(e => e.APPROVED_BY == null && e.EXCLUSION_YEAR == year).ToList();
            //    //.Join(context.DC_REGION,
            //    //    batch => (string)batch.REGION_ID,
            //    //    region => region.REGION_ID,
            //    //    (batch, region) => batch)
            //}
        }

        public List<VIEW_EXCLUSION_BATCH> GetApprovedBatches()
        {
            using (Entities context = new Entities())
            {
                var regions =
                    (from r in context.DC_REGION
                     select new VIEW_REGION()
                     {
                         RegionString = r.REGION_ID,
                         RegionName = r.REGION_NAME
                     }).ToList();

                return context.DC_DESTRUCTION_BATCH
                 .Where(e => e.APPROVED_BY != null)
                 .Select(x =>
                 new VIEW_EXCLUSION_BATCH()
                 {
                     REGION_ID = x.REGION_ID,
                     APPROVED_BY = x.APPROVED_BY,
                     BATCH_ID = x.BATCH_ID,
                     EXCLUSION_YEAR = x.DESTRUCTION_YEAR,
                     APPROVED_DATE = x.APPROVED_DATE
                 })
                 .AsEnumerable() // database query ends here, the rest is a query in memory
                 .Join(regions, f => f.REGION_ID, p => p.RegionId, (f, p) =>
                 new VIEW_EXCLUSION_BATCH()
                 {
                     REGION_NAME = p.RegionName,
                     APPROVED_BY = f.APPROVED_BY,
                     BATCH_ID = f.BATCH_ID,
                     EXCLUSION_YEAR = f.EXCLUSION_YEAR,
                     APPROVED_DATE = f.APPROVED_DATE
                 })
                 .ToList();
            }
            //using (Entities context = new Entities())
            //{
            //    return context.DC_DESTRUCTION_BATCH.Where(e => e.APPROVED_BY != null).ToList();
            //}
        }

        public List<DC_EXCLUSIONS> getExclusions(int regionId)
        {
            using (Entities context = new Entities())
            {
                return context.DC_EXCLUSIONS.Where(e => e.REGION_ID == regionId && e.EXCLUSION_BATCH_ID == 0).OrderBy(f => f.EXCL_DATE).ToList();
            }
        }

        public string getTDWcsv(int batchId)
        {
            //Build the CSV file data as a Comma separated string.
            string csv = string.Empty;
            string sql = string.Format("SELECT d.PENSION_NO,f.REGION,f.CONTAINER_CODE,CONTAINER_ALTCODE,FILEFOLDER_CODE,FILEFOLDER_ALTCODE,d.DESTRUCTIO_DATE,NAME,SURNAME from TDW_FILE_LOCATION f " +
                        "JOIN DC_DESTRUCTION d ON d.PENSION_NO = f.DESCRIPTION " +
                        "WHERE DESTRUCTION_BATCH_ID = {0} ", batchId);
            DataTable dt = _data.GetTable(sql);
            foreach (DataColumn column in dt.Columns)
            {
                //Add the Header row for CSV file.
                csv += column.ColumnName + ',';
            }

            //Add new line.
            csv += "\r\n";

            foreach (DataRow row in dt.Rows)
            {
                foreach (DataColumn column in dt.Columns)
                {
                    //Add the Data rows.
                    csv += row[column.ColumnName].ToString().Replace(",", ";") + ',';
                }

                //Add new line.
                csv += "\r\n";
            }

            return csv;
        }

        public string TDWExists(string pension_no)
        {
            string sql = "select * from TDW_FILE_LOCATION Where DESCRIPTION = '" + pension_no + "'";
            try
            {
                using (OracleConnection con = new OracleConnection(_data.connectionString))
                {
                    OracleCommand command = new OracleCommand(sql, con)
                    {
                        XmlCommandType = OracleXmlCommandType.None
                    };
                    command.Connection.Open();
                    if (command.ExecuteReader().HasRows) return "TDWFound";
                    return "TDWNotFound";
                }
            }
            catch
            {
                throw;
            }
        }
        public void RefreshFromSocPenData(string year)
        {
            string sql = @"INSERT INTO DC_DESTRUCTION d (PENSION_NO, DESTRUCTIO_DATE, STATUS_DATE, STATUS ) " +
                           "SELECT DISTINCT PENSION_NO, '" + year + "0101" + "', TO_CHAR(SYSDATE,'YYYYMMDD'),'Selected' " +
                            "FROM DC_DESTRUCTION_LIST dl " +
                            "WHERE PENSION_NO NOT IN(SELECT PENSION_NO FROM DC_DESTRUCTION) " +
                            "AND SUBSTR(STATUS_DATE,1,4) = '" + year + "'";
            _data.ExecuteNonQuery(sql);
            string updateregionSQL = "Update DC_DESTRUCTION D " +
                            "SET Region_ID = " +
                            "(select r.Region_ID from TDW_FILE_LOCATION F " +
                            "Join DC_REGION R on LOWER(TRIM(R.Region_Name)) = LOWER(TRIM(F.region)) " +
                            "Join DC_DESTRUCTION D ON F.DESCRIPTION = D.PENSION_NO " +
                            "WHERE ROWNUM = 1)";
            _data.ExecuteNonQuery(updateregionSQL);

        }

    }

    public class DestructionStatus : List<string>
    {
        public DestructionStatus()
        {
            this.InsertRange(0, new List<string>() { "Selected", "Excluded", "TDWFound", "TDWNotFound", "Approved", "Destroyed", "Exception" });
        }
    }

    public class ExclusionType : List<String>
    {
        public ExclusionType()
        {
            this.InsertRange(0, new List<string>() { "PAIA", "FRAUD", "LEGAL", "DEBTORS", "CCA", "AUDIT" });
        }
    }

    public enum PStatus
    {
        Success = 1,
        DestructionEntryNotFound = 2,
        ExclusionDuplicate = 3,
        DataError = 4,
    }
}