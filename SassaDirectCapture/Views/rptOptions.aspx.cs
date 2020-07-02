//using ClosedXML.Excel;
using SASSADirectCapture.BL;
using SASSADirectCapture.DL;
using SASSADirectCapture.Sassa;
using System;
using System.Data;
using System.IO;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SASSADirectCapture.Reports
{
    public partial class rptOptions : SassaPage
    {
        #region Public Fields

        protected DestructionProcess dProcess;
        protected int RegionId;
        #endregion Public Fields

        #region Public Methods

        protected void Page_Load(object sender, EventArgs e)
        {

            RegionId = int.Parse(UserSession.Office.RegionId);

            dProcess = new DestructionProcess(RegionId, UserSession.SamName);
            if (!IsPostBack)
            {
                Session["pageIndex"] = "";
                divError.Visible = false;
                btnExcelExport.Visible = false;
                divResult.Visible = false;
                divDestructionStatus.Visible = false;
                divMissingFiles.Visible = false;
                divActivityLog.Visible = false;
                yearRange.Visible = false;
                dateRange.Visible = false;
                ddRegion.Visible = false;
                ddGrantType.Visible = false;
                lblOptions.Visible = false;
                btnExcelExport.Visible = false;
                btnRun.Visible = false;
                ddDestructionStatus.Visible = false;
            }
        }

        //public void GetActiveClosedXMLWorkbook(XLWorkbook wb)
        //{
        //    var eventLog = new EventLog();

        //    string path = Path.GetTempPath() + "Applicants Register.xlsx";

        //    try
        //    {
        //        wb.SaveAs(path);
        //    }
        //    catch (Exception ex)
        //    {
        //        eventLog.WriteEntry(ex.Message);
        //    }
        //}

        #endregion Public Methods

        #region Protected Methods

        protected void btnExcelExport_Click(object sender, EventArgs e)
        {
            DataTable dt = Session["dt"] as DataTable;

            if (dt.Rows.Count > 0)
            {
                string FileName = drpOptions.SelectedItem.Text + DateTime.Now + ".xls";
                ExportXcel(dt, FileName);
            }
        }

        protected void btnRun_Click(object sender, EventArgs e)
        {
            DataTable dt = new DataTable();
            string dateFrom;
            string dateTo;
            string rptOptionIndex = Session["Index"].ToString();
            string office_id;
            string localOfficeType;
            string region_id = ddRegion.SelectedValue;
            string grant_type = ddGrantType.SelectedValue;
            string status = ddDestructionStatus.SelectedValue.StartsWith("-") ? string.Empty : ddDestructionStatus.SelectedValue;

            try
            {

                localOfficeType = UserSession.Office.OfficeType;

                if (rptOptionIndex != "1" && rptOptionIndex != "2")
                {
                    //use selected office
                    office_id = ddOffice.SelectedValue;

                    if (!String.IsNullOrEmpty(region_id) && String.IsNullOrEmpty(office_id))
                    {
                        lblMsg.Text = "Please select an office for this report.";
                        divError.Visible = true;
                        return;
                    }
                    //pull input dates for all but destruction Report
                    if (txtDateFrom.Value == string.Empty || txtDateTo.Value == string.Empty)
                    {
                        lblMsg.Text = "Please enter a From and To Date";
                        divError.Visible = true;

                        return;
                    }
                    //Assign dates to variables
                    dateFrom = txtDateFrom.Value;
                    dateTo = txtDateTo.Value;
                    try
                    {
                        if (Convert.ToDateTime(dateFrom) > Convert.ToDateTime(dateTo))
                        {
                            divError.Visible = true;
                            lblMsg.Text = "Please note From Date cannot be greater than To Date";

                            return;
                        }
                    }
                    catch
                    {
                        divError.Visible = true;
                        lblMsg.Text = "A date provided for this filter was invalid.";

                        return;
                    }
                }
                else
                {
                    //Get Office ID from session
                    office_id = UserSession.Office.OfficeId;
                    //pull input dates for destruction Report
                    if (ddYears.SelectedValue == string.Empty)
                    {
                        lblMsg.Text = "Please select a Year";
                        divError.Visible = true;

                        return;
                    }
                    //Assign dates to variables
                    dateFrom = ddYears.SelectedValue;
                    dateTo = ddYears.SelectedValue;
                }
                if (rptOptionIndex == "3")
                {
                    if (ddGrantType.SelectedValue == String.Empty)
                    {
                        lblMsg.Text = "Please select a Grant Type";
                        divError.Visible = true;

                        return;
                    }
                }
                //result
                DL.clsBusinessLogic bl = new clsBusinessLogic(dateFrom, dateTo, rptOptionIndex, office_id, localOfficeType, region_id, grant_type, status);

                dt = bl.Report();

                Session["dt"] = dt;

                BindData();
            }
            finally
            {
                dt.Dispose();
            }
        }

        protected void ddRegion_SelectedIndexChanged(object sender, EventArgs e)
        {
            //populate for missing files and Activity report
            if (ddRegion.SelectedValue == "") return;
            if (drpOptions.SelectedIndex < 2) return;
            ddOffice.DataSource = util.getRegionOffices(ddRegion.SelectedValue.Split(':')[1]);
            ddOffice.DataBind();
            ddOffice.DataTextField = "OFFICE_NAME";
            ddOffice.DataValueField = "OFFICE_ID";
            ddOffice.DataBind();
        }
        protected void drpOptions_SelectedIndexChanged(object sender, EventArgs e)
        {
            var rptOptionIndex = this.drpOptions.SelectedIndex.ToString();
            Session["Index"] = rptOptionIndex;
            Session["dt"] = null;
            btnExcelExport.Visible = false;

            grdMissingFiles.DataSource = null;
            grdMissingFiles.DataBind();

            grdDestruction.DataSource = null;
            grdDestruction.DataBind();

            grdDestructionStatus.DataSource = null;
            grdDestructionStatus.DataBind();

            grdActivityLog.DataSource = null;
            grdActivityLog.DataBind();

            grdActivityPivot.DataSource = null;
            grdActivityPivot.DataBind();

            lblOptions.Visible = true;
            btnRun.Visible = true;
            ddOffice.Visible = false;
            divError.Visible = false;
            switch (rptOptionIndex)
            {
                case "0":
                    yearRange.Visible = false;
                    dateRange.Visible = false;
                    ddRegion.Visible = false;
                    ddGrantType.Visible = false;
                    lblOptions.Visible = false;
                    btnExcelExport.Visible = false;
                    btnRun.Visible = false;
                    ddDestructionStatus.Visible = false;
                    break;

                case "1":
                    yearRange.Visible = true;
                    dateRange.Visible = false;
                    ddYears.Visible = true;
                    ddRegion.Visible = true;

                    var statusList = new DestructionData(RegionId).DestructionStatus;
                    statusList.Insert(0, "-- All Statusses --");
                    ddDestructionStatus.DataSource = statusList;
                    ddDestructionStatus.DataBind();

                    ddDestructionStatus.Visible = true;
                    ddYears.DataSource = dProcess.dData.DestructionYears;
                    //ddYears.DataSource = util.YearList();
                    ddYears.DataBind();
                    break;
                case "2":
                    yearRange.Visible = true;
                    dateRange.Visible = false;
                    ddYears.Visible = true;
                    ddRegion.Visible = true;
                    ddDestructionStatus.Visible = false;
                    //ddYears.DataSource = util.YearList();
                    ddYears.DataSource = dProcess.dData.DestructionYears;
                    ddYears.DataBind();
                    break;
                case "3":
                    yearRange.Visible = false;
                    dateRange.Visible = true;
                    ddRegion.Visible = true;
                    ddGrantType.Visible = true;
                    ddOffice.Visible = true;
                    ddDestructionStatus.Visible = false;
                    break;

                case "4":
                case "5":
                    yearRange.Visible = false;
                    dateRange.Visible = true;
                    ddRegion.Visible = true;
                    ddOffice.Visible = true;
                    ddDestructionStatus.Visible = false;
                    break;
            }
        }

        protected void ExportXcel(DataTable dt, string ReportName)
        {


            GridView gv = new GridView
            {
                DataSource = dt
            };
            gv.DataBind();

            Response.Clear();
            Response.Buffer = true;
            Response.ClearContent();
            Response.ClearHeaders();
            Response.Charset = "";
            StringWriter strwriter = new StringWriter();
            HtmlTextWriter htmltextwriter = new HtmlTextWriter(strwriter);
            Response.Cache.SetCacheability(HttpCacheability.NoCache);
            Response.ContentType = "application/ms-excel";
            Response.AddHeader("Content-Disposition", "attachment;filename=" + ReportName);
            gv.GridLines = GridLines.Both;
            gv.HeaderStyle.Font.Bold = true;
            gv.AllowSorting = false;
            gv.RenderControl(htmltextwriter);
            Response.Write(strwriter.ToString().Replace("&#39;", ""));
            Response.End();
        }

        //grdActivityLog_PageIndexChanging
        protected void grdActivityLog_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            grdActivityLog.PageIndex = e.NewPageIndex;
            BindData();
        }

        protected void grdActivityPivot_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            grdActivityPivot.PageIndex = e.NewPageIndex;
            BindData();
        }

        protected void grdDestruction_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            grdDestruction.PageIndex = e.NewPageIndex;
            BindData();
        }

        protected void grdDestructionStatus_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            grdDestructionStatus.PageIndex = e.NewPageIndex;
            BindData();
        }

        protected void grdMissingFiles_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            grdMissingFiles.PageIndex = e.NewPageIndex;
            BindData();
        }

        #endregion Protected Methods

        #region Private Methods

        //BIND THE DATA FROM THE CURRENT SESSION
        private void BindData()
        {
            divError.Visible = false;
            btnExcelExport.Visible = false;
            if (Session["Index"] == null) Session["Index"] = "1";
            string index = Session["Index"].ToString();
            divResult.Visible = false;
            divMissingFiles.Visible = false;
            divActivityLog.Visible = false;
            divActivityPivot.Visible = false;

            DataTable dt = Session["dt"] as DataTable;
            if (dt.Rows.Count == 0)
            {
                divError.Visible = true;
                lblMsg.Text = "No data to display";
                return;
            }
            else
            {
                yearRange.Visible = false;
                dateRange.Visible = false;
                ddRegion.Visible = false;
                ddGrantType.Visible = false;
                ddOffice.Visible = false;
                ddDestructionStatus.Visible = false;
                lblOptions.Visible = false;
            }

            try
            {
                btnExcelExport.Visible = true;
                btnRun.Visible = false;
                if (index == "1")
                {
                    divResult.Visible = true;
                    grdDestruction.DataSource = dt;
                    grdDestruction.DataBind();

                    if (grdDestruction.Rows.Count > 0)
                        btnExcelExport.Visible = true;
                }

                if (index == "2")
                {
                    divDestructionStatus.Visible = true;
                    grdDestructionStatus.DataSource = dt;
                    grdDestructionStatus.DataBind();
                }
                if (index == "3")
                {
                    divMissingFiles.Visible = true;
                    grdMissingFiles.DataSource = Session["dt"];
                    grdMissingFiles.DataBind();
                }
                if (index == "4")
                {
                    divActivityLog.Visible = true;
                    grdActivityLog.DataSource = Session["dt"];
                    grdActivityLog.DataBind();
                }
                if (index == "5")
                {
                    divActivityPivot.Visible = true;
                    grdActivityPivot.DataSource = Session["dt"];
                    grdActivityPivot.DataBind();
                }
            }
            catch (Exception ex)
            {
                divError.Visible = true;
                lblMsg.Text = ex.Message;
            }
        }

        private string ConvertSortDirectionToSql(SortDirection sortDirection)
        {
            string newSortDirection = String.Empty;

            switch (sortDirection)
            {
                case SortDirection.Ascending:
                    newSortDirection = "ASC";
                    break;

                case SortDirection.Descending:
                    newSortDirection = "DESC";
                    break;
            }

            return newSortDirection;
        }

        //private void RenderDataTableOnClosedXlSheet(DataTable dt)
        //{
        //    var wb = new XLWorkbook();
        //    var eventLog = new EventLog();

        //    try
        //    {
        //        // Add a DataTable as a worksheet
        //        wb.Worksheets.Add(dt);

        //        GetActiveClosedXMLWorkbook(wb);
        //    }
        //    catch (Exception ex)
        //    {
        //        eventLog.WriteEntry(ex.Message);
        //    }
        //}

        #endregion Private Methods
    }
}
