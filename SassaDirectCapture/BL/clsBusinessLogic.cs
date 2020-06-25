using System;
using System.Data;

namespace SASSADirectCapture.DL
{
    public class clsBusinessLogic
    {
        #region Global Variables

        private readonly string FromDate_;

        private readonly string grant_type_;

        private readonly string office_id_;

        private readonly string office_type_;

        private readonly string region_id_;

        private readonly string selectedIndex_;

        private readonly string ToDate_;

        private readonly string status_;

        #endregion Global Variables

        #region Constructor

        public clsBusinessLogic(string dateFrom, string dateTo, string selectedIndex, string office_id, string officeType, string region_id, string grant_type, string status)
        {
            FromDate_ = dateFrom;
            ToDate_ = dateTo;
            selectedIndex_ = selectedIndex;
            office_id_ = office_id;
            office_type_ = officeType;
            grant_type_ = grant_type;
            region_id_ = region_id;
            status_ = status;
        }

        #endregion Constructor

        #region Queries

        public DataTable Report()
        {
            DL.ReportDataAccess rptDa = new DL.ReportDataAccess();

            DataTable dt;

            try
            {
                dt = rptDa.getReportData(FromDate_, ToDate_, selectedIndex_, office_id_, office_type_, region_id_, grant_type_, status_);
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return dt;
        }

        #endregion Queries
    }
}