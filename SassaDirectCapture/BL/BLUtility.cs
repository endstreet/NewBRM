using SASSADirectCapture.EntityModels;
using SASSADirectCapture.Services;
using System;
using System.Collections.Generic;
using System.Data;
using System.DirectoryServices.AccountManagement;
using System.Linq;
using System.Web;

namespace SASSADirectCapture.BL
{
    public class BLUtility
    {
        #region Variables
        public object KUAFCHILDRENs { get; private set; }
        public UserSession UserSession { get; set; }
        #endregion Variables

        public BLUtility(UserSession us)
        {
            UserSession = us;
        }

        #region GET Methods

        public static int getFiveYearsLater(string nowString)
        {
            string fiveYearsLater = String.Empty;
            string nowCCYY = nowString.Substring(0, 4);
            int nowNumber = int.Parse(nowCCYY);
            int thenNumber = nowNumber + 6;//Changed to 6 years, since we're only comparing years, not months or days.
            return thenNumber;
        }

        // check if a file with this BRM number already exists
        public bool checkBRMExists(string brmno)
        {
            bool theBRMExists = false;

            using (Entities en = new Entities())
            {
                DC_FILE f = en.DC_FILE.Where(k => k.BRM_BARCODE.ToLower() == brmno.ToLower())
                   .FirstOrDefault();
                if (f != null)
                {
                    theBRMExists = true;
                }
            }
            return theBRMExists;
        }

        public string CheckExclusions(string idno)
        {
            try
            {
                if (idno == null) { return ""; }
                using (Entities en = new Entities())
                {
                    DC_EXCLUSIONS exc = en.DC_EXCLUSIONS.Where(k => k.ID_NO == idno).FirstOrDefault();
                    if (exc == null)
                    {
                        return "";
                    }
                    else
                    {
                        return "Excluded";
                    }
                }
            }
            catch (Exception)
            {
                return "";
            }
        }

        public string getBoxArchYear(string boxno)
        {
            string ayear = string.Empty;

            using (Entities en = new Entities())
            {
                DC_FILE f = en.DC_FILE.Where(bn => bn.TDW_BOXNO == boxno)
                   .FirstOrDefault();
                if (f != null)
                {
                    ayear = f.ARCHIVE_YEAR;
                }
            }
            return ayear;
        }

        public Dictionary<string, decimal> getBoxTypes()
        {
            Dictionary<string, decimal> boxTypes = new Dictionary<string, decimal>();

            using (Entities context = new Entities())
            {
                var query = (from d in context.DC_BOX_TYPE
                             where d.IS_TRANSPORT == "N"
                             select d);

                boxTypes.Add("", 0);

                foreach (DC_BOX_TYPE trans in query)
                {
                    boxTypes.Add(trans.BOX_TYPE, trans.BOX_TYPE_ID);
                }
            }

            return boxTypes;
        }

        public Dictionary<string, decimal> getBoxTypesTransfer()
        {
            Dictionary<string, decimal> boxTypes = new Dictionary<string, decimal>();

            using (Entities context = new Entities())
            {
                var query = (from d in context.DC_BOX_TYPE
                             where d.IS_TRANSPORT == "Y"
                             select d);

                boxTypes.Add("", 0);

                foreach (DC_BOX_TYPE trans in query)
                {
                    boxTypes.Add(trans.BOX_TYPE, trans.BOX_TYPE_ID);
                }
            }

            return boxTypes;
        }

        public string getCLM(string brmno)
        {
            string clmno = string.Empty;

            using (Entities en = new Entities())
            {
                DC_FILE f = en.DC_FILE.Where(k => k.BRM_BARCODE == brmno)
                   .FirstOrDefault();
                if (f != null)
                {
                    clmno = f.UNQ_FILE_NO;
                }
            }
            return clmno;
        }

        public Dictionary<string, string> getDepartmentStakeholders(int iDepID)
        {
            Dictionary<string, string> dict = new Dictionary<string, string>();
            dict.Add(string.Empty, "<--Please Select-->");

            using (Entities context = new Entities())
            {
                var query = from r in context.DC_STAKEHOLDER
                            where r.DEPARTMENT_ID == iDepID
                            select r;

                foreach (DC_STAKEHOLDER result in query.OrderBy(e => e.NAME).OrderBy(e => e.SURNAME))
                {
                    dict.Add(result.STAKEHOLDER_ID.ToString(), result.NAME + " " + result.SURNAME);
                }
            }
            return dict;
        }

        public Dictionary<string, string> getDistrictsByRegion(string region)
        {
            Dictionary<string, string> dist = new Dictionary<string, string>();

            if (region == null) throw new Exception("Session Lost!");
            using (Entities context = new Entities())
            {
                var query = (from d in context.CUST_DISTRICT
                             where d.REGNUM.Trim() == region
                             select d).ToList();

                dist.Add("", "---Select District---");

                foreach (CUST_DISTRICT trans in query)
                {
                    dist.Add(trans.DISTRICTID.ToString(), trans.DISTRICT);
                }
            }
            return dist;
        }

        public Dictionary<string, string> getECDistricts()
        {
            Dictionary<string, string> dist = new Dictionary<string, string>();

            using (Entities context = new Entities())
            {
                var query = (from d in context.DC_DISTRICT_EC.OrderBy(e => e.DISTRICT_NAME)
                             select d);

                dist.Add("", "");

                foreach (DC_DISTRICT_EC trans in query)
                {
                    dist.Add(trans.DISTRICT_NAME, trans.DISTRICT_CODE);
                }
            }

            return dist;
        }

        public string getGrantName(string grantid)
        {
            using (Entities en = new Entities())
            {
                DC_GRANT_TYPE grant = en.DC_GRANT_TYPE.Where(g => g.TYPE_ID == grantid).FirstOrDefault();
                if (grant != null)
                {
                    return grant.TYPE_NAME;
                }
                else
                {
                    return null;
                }
            }
        }

        public Dictionary<string, string> getGrantTypes()
        {
            Dictionary<string, string> dict = new Dictionary<string, string>();

            using (Entities context = new Entities())
            {
                var query = from r in context.DC_GRANT_TYPE
                            select r;

                foreach (DC_GRANT_TYPE trans in query)
                {
                    dict.Add(trans.TYPE_ID, trans.TYPE_NAME);
                }
            }

            return dict;
        }

        public string getIDList(string idno)
        {
            string sIDs = "";
            using (Entities en = new Entities())
            {
                sIDs = (from spn in en.SOCPENBRMs.Where(k => k.PENSION_NO == idno
                                                                            || k.OLD_ID1 == idno
                                                                            || k.OLD_ID2 == idno
                                                                            || k.OLD_ID3 == idno
                                                                            || k.OLD_ID4 == idno
                                                                            || k.OLD_ID5 == idno
                                                                            || k.OLD_ID6 == idno
                                                                            || k.OLD_ID7 == idno
                                                                            || k.OLD_ID8 == idno
                                                                            || k.OLD_ID9 == idno
                                                                            || k.OLD_ID10 == idno)
                        select ("'" + spn.PENSION_NO + "'"
                                  + (spn.OLD_ID1 == null ? "" : (",'" + spn.OLD_ID1 + "'"))
                                  + (spn.OLD_ID2 == null ? "" : (",'" + spn.OLD_ID2 + "'"))
                                  + (spn.OLD_ID3 == null ? "" : (",'" + spn.OLD_ID3 + "'"))
                                  + (spn.OLD_ID4 == null ? "" : (",'" + spn.OLD_ID4 + "'"))
                                  + (spn.OLD_ID5 == null ? "" : (",'" + spn.OLD_ID5 + "'"))
                                  + (spn.OLD_ID6 == null ? "" : (",'" + spn.OLD_ID6 + "'"))
                                  + (spn.OLD_ID7 == null ? "" : (",'" + spn.OLD_ID7 + "'"))
                                  + (spn.OLD_ID8 == null ? "" : (",'" + spn.OLD_ID8 + "'"))
                                  + (spn.OLD_ID9 == null ? "" : (",'" + spn.OLD_ID9 + "'"))
                                  + (spn.OLD_ID10 == null ? "" : (",'" + spn.OLD_ID10 + "'"))
                              )
                       ).FirstOrDefault();
            }

            if (sIDs == null || sIDs == "")
            {
                sIDs = "'" + idno + "'";
            }
            return sIDs;
        }

        public string getLCTypeDescription(decimal LCTypeID)
        {
            using (Entities en = new Entities())
            {
                DC_LC_TYPE LCType = en.DC_LC_TYPE.Where(g => g.PK == LCTypeID).FirstOrDefault();
                if (LCType != null)
                {
                    return LCType.DESCRIPTION;
                }
                else
                {
                    return null;
                }
            }
        }

        public Dictionary<string, decimal> getLCTypes()
        {
            Dictionary<string, decimal> dicLCTypes = new Dictionary<string, decimal>();

            using (Entities context = new Entities())
            {
                var query = (from d in context.DC_LC_TYPE
                             select d);

                //dicLCTypes.Add("", 0);

                foreach (DC_LC_TYPE trans in query)
                {
                    dicLCTypes.Add(trans.DESCRIPTION, trans.PK);
                }
            }

            return dicLCTypes;
        }

        //Todo: cache this
        public Dictionary<string, string> getLocalOffices(string regionID)
        {
            Dictionary<string, string> dict = new Dictionary<string, string>
            {
                { string.Empty, "<--Please Select-->" }
            };

            using (Entities context = new Entities())
            {
                var query = from r in context.DC_LOCAL_OFFICE.OrderBy(e => e.OFFICE_NAME).Where(f => f.REGION_ID == regionID)
                            select r;

                foreach (DC_LOCAL_OFFICE office in query)
                {
                    dict.Add(office.OFFICE_ID.ToString(), office.OFFICE_NAME + (office.OFFICE_TYPE == "RMC" ? " (RMC)" : ""));
                }
            }

            return dict;
        }

        public IEnumerable<DC_LOCAL_OFFICE> getRegionOffices(string regionID)
        {
            //Dictionary<string, string> dict = new Dictionary<string, string>();
            //dict.Add(string.Empty, "<--Please Select-->");
            List<DC_LOCAL_OFFICE> roffices;

            using (Entities context = new Entities())
            {
                roffices = (from r in context.DC_LOCAL_OFFICE.OrderBy(e => e.OFFICE_NAME).Where(f => f.REGION_ID == regionID)
                            select r).ToList();

                //BRM-126 changes
                //foreach (DC_LOCAL_OFFICE office in query)
                //{
                //    string isRMC = "";
                //    string isDO = " (DO)";
                //    if (office.OFFICE_TYPE == "RMC")
                //    {
                //        isRMC = " (RMC)";
                //        office.OFFICE_NAME = office.OFFICE_NAME + isRMC;
                //    }
                //    if (office.OFFICE_NAME.Contains(isDO))
                //    {
                //        office.OFFICE_NAME = office.OFFICE_NAME.TrimEnd();
                //        if (isRMC != "")
                //        {
                //            office.OFFICE_NAME = office.OFFICE_NAME.Replace(isRMC, string.Empty);
                //        }
                //    }
                //    dict.Add(office.OFFICE_ID.ToString(), office.OFFICE_NAME);
                //}

                //foreach (DC_LOCAL_OFFICE office in query)
                //{
                //    string isRMC = "";

                //    if (office.OFFICE_TYPE == "RMC")
                //    {
                //        isRMC = " (RMC)";
                //    }
                //    dict.Add(office.OFFICE_ID.ToString(), office.OFFICE_NAME + isRMC);
                //}
            }
            roffices.Insert(0, new DC_LOCAL_OFFICE { OFFICE_NAME = "-- Select --", OFFICE_ID = "" });
            return roffices;
        }

        public string getNameSurname(string idno)
        {
            string fullname = string.Empty;

            using (Entities en = new Entities())
            {
                SOCPENBRM s = en.SOCPENBRMs.Where(k => k.PENSION_NO == idno
                                                    || k.OLD_ID1 == idno
                                                    || k.OLD_ID2 == idno
                                                    || k.OLD_ID3 == idno
                                                    || k.OLD_ID4 == idno
                                                    || k.OLD_ID5 == idno
                                                    || k.OLD_ID6 == idno
                                                    || k.OLD_ID7 == idno
                                                    || k.OLD_ID8 == idno
                                                    || k.OLD_ID9 == idno
                                                    || k.OLD_ID10 == idno)
                   .FirstOrDefault();
                if (s != null)
                {
                    fullname = s.NAME + " " + s.SURNAME;
                }
            }
            return fullname;
        }

        public string getNameSurnameSRD(string SRDNo)
        {
            string fullname = string.Empty;

            using (Entities en = new Entities())
            {
                IEnumerable<SocpenSRD> query = en.Database.SqlQuery<SocpenSRD>
                                                        (@"select srdben.NAME as FIRSTNAME,
                                                                  srdben.SURNAME as LASTNAME
                                                            from SOCPEN_SRD_BEN srdben
                                                            where cast(srdben.SRD_NO as NUMBER) = cast('" + SRDNo + @"' as NUMBER)");

                SocpenSRD s = query.FirstOrDefault();
                if (s != null)
                {
                    fullname = s.FIRSTNAME + " " + s.LASTNAME;
                }
            }
            return fullname;
        }

        public string getNoBoxesInPicklist(string picklistno)
        {
            var mycount = 0;

            if (picklistno == null) { return null; }
            using (Entities en = new Entities())
            {
                var query = from r in en.DC_BOXPICKLIST.Where(k => k.UNQ_PICKLIST == picklistno)
                            select r;
                mycount = query.Count();
            }
            return mycount.ToString();
        }

        public string getPickListAY(string picklistno)
        {
            if (picklistno == null) { return null; }
            using (Entities en = new Entities())
            {
                DC_BOXPICKLIST plist = en.DC_BOXPICKLIST.Where(k => k.UNQ_PICKLIST == picklistno).FirstOrDefault();
                if (plist != null)
                {
                    return plist.ARCHIVE_YEAR;
                }
                else
                {
                    return null;
                }
            }
        }

        public Dictionary<string, string> getPicklistByUser(int userid)
        {
            Dictionary<string, string> dict = new Dictionary<string, string>();

            using (Entities context = new Entities())
            {
                var query = from p in context.DC_PICKLIST.OrderBy(e => e.UNQ_PICKLIST)
                            where p.USERID == userid
                            select p;
            }
            return dict;
        }

        public string getPickListForUser(int userid)
        {
            string thispicklist = String.Empty;

            using (Entities en = new Entities())
            {
                DC_PICKLIST plist = en.DC_PICKLIST.Where(k => k.USERID == userid && k.PICKLIST_STATUS == "N").FirstOrDefault();
                if (plist != null)
                {
                    return plist.UNQ_PICKLIST;
                }
                else
                {
                    return null;
                }
            }
        }

        public string getPickListRegistryType(string picklistno)
        {
            if (picklistno == null) { return null; }
            using (Entities en = new Entities())
            {
                DC_PICKLIST plist = en.DC_PICKLIST.Where(k => k.UNQ_PICKLIST == picklistno).FirstOrDefault();
                if (plist != null)
                {
                    return plist.REGISTRY_TYPE;
                }
                else
                {
                    return null;
                }
            }
        }

        public string getRegion(string whichfield, string region_id)
        {
            //REGION_ID
            //REGION_NAME
            //REGION_CODE

            using (Entities en = new Entities())
            {
                DC_REGION reg = en.DC_REGION.Where(k => k.REGION_ID == region_id).FirstOrDefault();
                if (reg != null)
                {
                    if (whichfield == "code")
                    {
                        return reg.REGION_CODE;
                    }
                    else
                    {
                        return reg.REGION_NAME;
                    }
                }
                return null;
            }
        }

        public Dictionary<string, string> getRegions()
        {
            Dictionary<string, string> dict = new Dictionary<string, string>();
            dict.Add(string.Empty, "<--Please Select-->");

            using (Entities context = new Entities())
            {
                var query = from r in context.DC_REGION.OrderBy(e => e.REGION_NAME)
                            select r;

                foreach (DC_REGION region in query)
                {
                    dict.Add(region.REGION_ID.ToString(), region.REGION_NAME);
                }
            }

            return dict;
        }

        public Dictionary<string, string> getRequestCategory()
        {
            Dictionary<string, string> dict = new Dictionary<string, string>();
            dict.Add(string.Empty, "<--Please Select-->");

            using (Entities context = new Entities())
            {
                var query = from r in context.DC_REQ_CATEGORY.OrderBy(e => e.CATEGORY_DESCR)
                            select r;

                foreach (DC_REQ_CATEGORY reason in query)
                {
                    dict.Add(reason.CATEGORY_ID.ToString(), reason.CATEGORY_DESCR);
                }
            }

            return dict;
        }

        public Dictionary<string, string> getRequestCategoryType(int catid)
        {
            Dictionary<string, string> dict = new Dictionary<string, string>();
            dict.Add(string.Empty, "<--Please Select-->");

            using (Entities context = new Entities())
            {
                var query = from r in context.DC_REQ_CATEGORY_TYPE
                            join c in context.DC_REQ_CATEGORY_TYPE_LINK
                                   on r.TYPE_ID equals c.TYPE_ID
                            where c.CATEGORY_ID == catid
                            select r;

                foreach (DC_REQ_CATEGORY_TYPE type in query.OrderBy(e => e.TYPE_DESCR))
                {
                    dict.Add(type.TYPE_ID.ToString(), type.TYPE_DESCR);
                }
            }
            return dict;
        }

        public Dictionary<string, string> getTransactionTypes()
        {
            Dictionary<string, string> dict = new Dictionary<string, string>();

            using (Entities context = new Entities())
            {
                var query = from r in context.DC_TRANSACTION_TYPE.OrderBy(e => e.TYPE_NAME)
                            select r;

                foreach (DC_TRANSACTION_TYPE trans in query)
                {
                    dict.Add(trans.TYPE_ID.ToString(), trans.TYPE_NAME);
                }
            }

            return dict;
        }

        public Dictionary<string, string> getTransactionTypesByService(string servcat)
        {
            Dictionary<string, string> dict = new Dictionary<string, string>();

            using (Entities context = new Entities())
            {
                var query = from r in context.DC_TRANSACTION_TYPE.OrderBy(e => e.TYPE_NAME)
                            where r.SERVICE_CATEGORY == servcat
                            select r;

                dict.Add("", "---Select Transaction Type---");

                foreach (DC_TRANSACTION_TYPE trans in query)
                {
                    dict.Add(trans.TYPE_ID.ToString(), trans.TYPE_NAME);
                }
            }
            return dict;
        }

        public string getUserFullName(string userid)
        {
            string usernaam = "unknown";

            using (Entities en = new Entities())
            {
                int useridnommer = int.Parse(userid);

                KUAF usr = en.KUAFs.Where(k => k.ID == useridnommer).FirstOrDefault();
                if (usr != null)
                {
                    usernaam = usr.FIRSTNAME + " " + usr.LASTNAME;
                }
            }
            return usernaam;
        }

        #endregion GET Methods

        #region SET Methods

        public void clearBoxSession()
        {
            HttpContext.Current.Session["BoxNo"] = "";
            HttpContext.Current.Session["BRM"] = "";
            HttpContext.Current.Session["courier"] = "";
            HttpContext.Current.Session["workorder"] = "";
        }

        public void clearLocalOfficeSession()
        {
            UserSession.Office = new UserOffice();
        }

        public bool setUserGroup(string userLogin)
        {
            string sUserRole = "";

            using (PrincipalContext pc = new PrincipalContext(ContextType.Domain, "SASSA.LOCAL"))
            {
                //Debug.WriteLine("Test1");
                UserPrincipal user = UserPrincipal.FindByIdentity(pc, IdentityType.SamAccountName, userLogin);
                //Debug.WriteLine("user.SamAccountName = " + user.SamAccountName);

                GroupPrincipal gpUser = GroupPrincipal.FindByIdentity(pc, System.Web.Configuration.WebConfigurationManager.AppSettings["AD_Users"].ToString());
                GroupPrincipal gpSupervisor = GroupPrincipal.FindByIdentity(pc, System.Web.Configuration.WebConfigurationManager.AppSettings["AD_Supervisors"].ToString());
                GroupPrincipal gpTransporter = GroupPrincipal.FindByIdentity(pc, System.Web.Configuration.WebConfigurationManager.AppSettings["AD_Transporters"].ToString());

                if (user != null && gpUser != null && user.IsMemberOf(gpUser))
                {
                    sUserRole = "N";
                    if (gpSupervisor != null && user.IsMemberOf(gpSupervisor))
                    {
                        sUserRole = "Y";
                    }
                    else if (gpTransporter != null && user.IsMemberOf(gpTransporter))
                    {
                        sUserRole = "T";
                    }
                }
                //else
                //{
                //    new SASSA_Authentication().RedirectToLoginPage();
                //}
            }

            if (sUserRole == "")
            {
                return false;
            }
            else
            {
                UserSession.Roles.Add(sUserRole);
                HttpContext.Current.Session["us"] = UserSession;

                return true;
            }
        }

        public bool updateBatchDetails(int batchno, string workorderno, string courier)
        {
            Boolean isOK = true;

            try
            {
                using (Entities db = new Entities())
                {
                    DC_BATCH batch = db.DC_BATCH.Find(batchno);

                    if (batch == null)
                    {
                        isOK = false;
                    }
                    else
                    {
                        batch.COURIER_NAME = courier;
                        batch.WAYBILL_NO = workorderno;
                        batch.WAYBILL_DATE = System.DateTime.Now;
                    }
                    db.DC_ACTIVITY.Add(CreateActivity("Batching", "Update Batch Details"));

                    db.SaveChanges();
                }
            }
            catch (Exception)
            {
                isOK = false;
            }
            return isOK;
        }

        public void getLocalOffice()
        {

            if (string.IsNullOrEmpty(UserSession.SamName)) throw new Exception("UserSessioner Unknown");
            if (UserSession.Office.OfficeName != null) return;

            using (Entities context = new Entities())
            {
                //Try local office from database.
                if (!(from lo in context.DC_LOCAL_OFFICE
                     join lou in context.DC_OFFICE_KUAF_LINK
                         on lo.OFFICE_ID equals lou.OFFICE_ID
                     where lou.USERNAME == UserSession.SamName
                     select lo).Any())
                {
                    DC_LOCAL_OFFICE ioffice = (from lo in context.DC_LOCAL_OFFICE
                                               join lou in context.DC_OFFICE_KUAF_LINK
                                                   on lo.OFFICE_ID equals lou.OFFICE_ID
                                               select lo).FirstOrDefault();
                    //Attach to first or default office.
                    updateUserLocalOffice(UserSession.SamName, ioffice.OFFICE_ID);
                    //try again..
                    getLocalOffice();
                }

                DC_LOCAL_OFFICE value = (from lo in context.DC_LOCAL_OFFICE
                                             join lou in context.DC_OFFICE_KUAF_LINK
                                                 on lo.OFFICE_ID equals lou.OFFICE_ID
                                             where lou.USERNAME == UserSession.SamName
                                             select lo).FirstOrDefault();

                UserSession.Office.OfficeName = value.OFFICE_NAME;
                UserSession.Office.OfficeId = value.OFFICE_ID;
                UserSession.Office.OfficeType = value.OFFICE_TYPE;
                UserSession.Office.RegionId = value.REGION_ID;



                DC_REGION reg = context.DC_REGION.Where(k => k.REGION_ID == UserSession.Office.RegionId).FirstOrDefault();
                UserSession.Office.RegionCode = reg.REGION_CODE;
                UserSession.Office.RegionName = reg.REGION_NAME;

            }
            UserSession.Office.OfficeType = UserSession.Office.OfficeType != string.Empty ? UserSession.Office.OfficeType : "LO"; //Default to local office
            //UserSession.Office.RegionCode = UserSession.Office.RegionCode != string.Empty ? UserSession.Office.RegionCode : "";
            UserSession.IsIntitialized = true;
            HttpContext.Current.Session["us"] = UserSession;
        }

        public bool updateUserLocalOffice(string userLogin, string officeID)
        {


            DC_OFFICE_KUAF_LINK officeLink;
            using (Entities db = new Entities())
            {
                if (db.DC_OFFICE_KUAF_LINK.Where(okl => okl.USERNAME == userLogin).Any())
                {
                    try
                    {
                        officeLink = db.DC_OFFICE_KUAF_LINK.Where(okl => okl.USERNAME == userLogin).First();
                    }
                    catch
                    {
                        throw new Exception("officeLink = db.DC_OFFICE_KUAF_LINK.Where(okl => okl.USERNAME == userLogin).First();");
                    }
                }
                else
                {
                    try
                    {
                        officeLink = new DC_OFFICE_KUAF_LINK() { OFFICE_ID = officeID, USERNAME = userLogin };
                        db.DC_OFFICE_KUAF_LINK.Add(officeLink);
                        db.SaveChanges();
                    }
                    catch(Exception ex)
                    {
                        throw new Exception(ex.Message + ex.InnerException);
                    }
                }
                officeLink.OFFICE_ID = officeID;


                try
                {
                    db.DC_ACTIVITY.Add(CreateActivity("Office", "Update User/LocalOffice link"));
                    db.SaveChanges();
                }
                catch
                {
                    throw new Exception(@"db.SaveChanges()");
                }
            }

            return true;
        }

        #endregion SET Methods

        #region Public Methods

        public IEnumerable<string> YearList()
        {
            List<string> years = new List<string>();
            int start = 1930;
            int end = DateTime.Now.Year;
            for (int i = start; i <= end; i++)
            {
                years.Add(i.ToString());
            }
            return years;
        }

        public DC_ACTIVITY CreateActivity(string Area, string Activity)
        {
            return new DC_ACTIVITY {ACTIVITY_DATE = DateTime.Now, REGION_ID = UserSession.Office.RegionId, OFFICE_ID = decimal.Parse(UserSession.Office.OfficeId), USERID = 0, USERNAME = UserSession.SamName, AREA = Area, ACTIVITY = Activity, RESULT = "OK" };
        }
        #endregion Public Methods
    }
}
