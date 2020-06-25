using SASSADirectCapture.EntityModels;
using SASSADirectCapture.Sassa;
using System;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SASSADirectCapture.Views
{
    public partial class FileRequestIn : SassaPage
    {
        #region Private Fields

        private Entities en = new Entities();

        #endregion Private Fields

        #region Public Methods

        public IQueryable<FileRequest> GetFileRequestHistory()
        {
            var x = UserSession.Office.OfficeId;

            // Retrieve all files that have been submitted by local offices in the same region as the RMC office.
            // Only show files that have not been updated or closed.

            IQueryable<FileRequest> query;
            try
            {
                //fr.SCANNED_DATE == null                   &&
                query =
                   from fr in en.DC_FILE_REQUEST
                   join region in en.DC_REGION on fr.REGION_ID equals region.REGION_ID
                   join lo in en.DC_LOCAL_OFFICE on fr.REQUESTED_OFFICE_ID equals lo.OFFICE_ID
                   join lou in en.DC_LOCAL_OFFICE on lo.REGION_ID equals lou.REGION_ID
                   //join fi in en.DC_FILE on fr.UNQ_FILE_NO equals fi.UNQ_FILE_NO
                   where fr.CLOSED_DATE == null && lou.OFFICE_ID == x
                   orderby fr.REQUESTED_DATE descending, fr.ID_NO

                   select new FileRequest
                   {
                       UNQ_FILE_NO = fr.UNQ_FILE_NO,
                       ID_NO = fr.ID_NO,
                       BRM_BARCODE = fr.BRM_BARCODE,
                       MIS_FILE_NO = fr.MIS_FILE_NO,
                       BIN_ID = fr.BIN_ID,
                       BOX_NUMBER = fr.BOX_NUMBER,
                       POSITION = fr.POSITION,
                       TDW_BOXNO = fr.TDW_BOXNO,
                       NAME = fr.NAME,
                       SURNAME = fr.SURNAME,
                       REGION_ID = fr.REGION_ID,
                       REQUESTED_DATE = fr.REQUESTED_DATE,
                       //ARCHIVE_YEAR = fi.ARCHIVE_YEAR,
                       REQUESTED_OFFICE_NAME = lo.OFFICE_NAME,
                       RELATED_MIS_FILE_NO = fr.RELATED_MIS_FILE_NO,
                       REGION_NAME = region.REGION_NAME,
                       //GRANT_NAME = grant.TYPE_NAME,
                       //GRANT_TYPE = fr.GRANT_TYPE,
                       SCANNED_DATE = fr.SCANNED_DATE,
                       CLOSED_DATE = fr.CLOSED_DATE,
                       //APPLICATION_DATE = fr.APP_DATE.Value,
                       SCANNED_PHYSICAL_IND = fr.SCANNED_PHYSICAL_IND,
                       REQUEST_CAT_ID = fr.REQ_CATEGORY,
                       REQUEST_CAT_TYPE_ID = fr.REQ_CATEGORY_TYPE,
                       REQUEST_CAT_DETAIL = fr.REQ_CATEGORY_DETAIL,
                       PICKLIST_STATUS = fr.PICKLIST_STATUS
                   };

                if (query.Any())
                {
                    //TODO
                    //SET SEssion or something so that we can show the number of records in the main page menu bar
                    //foreach (FileRequest fileReq in query)
                    //{
                    //    if (fileReq.CLOSED_DATE == null)
                    //    {
                    //        fileReq.STATUS = "In Progress";
                    //    }
                    //}
                }
            }
            catch (Exception ex)
            {
                lblError.Text = ex.Message;
                divError.Style.Add("display", "");
                return null;
            }

            return query;
        }

        public IQueryable<FileRequest> GetRMCPicklist()
        {
            var x = UserSession.Office.OfficeId;
            string pstatus = "In Progress";
            string region = UserSession.Office.RegionId;

            // Retrieve all files that have been submitted by local offices in the same region as the RMC office.
            // Only show files that have aRE iN Progress and not sent to TDW.

            IQueryable<FileRequest> query;
            try
            {
                //fr.SCANNED_DATE == null &&
                query =
                    from fr in en.DC_FILE_REQUEST
                    where fr.CLOSED_DATE == null
                    && fr.REGION_ID == region
                    && fr.SENT_TDW != "Y"
                    && fr.PICKLIST_STATUS == pstatus
                    orderby fr.REQUESTED_DATE descending, fr.ID_NO

                    select new FileRequest
                    {
                        UNQ_FILE_NO = fr.UNQ_FILE_NO,
                        ID_NO = fr.ID_NO,
                        BRM_BARCODE = fr.BRM_BARCODE,
                        MIS_FILE_NO = fr.MIS_FILE_NO,
                        BIN_ID = fr.BIN_ID,
                        BOX_NUMBER = fr.BOX_NUMBER,
                        POSITION = fr.POSITION,
                        TDW_BOXNO = fr.TDW_BOXNO,
                        NAME = fr.NAME,
                        SURNAME = fr.SURNAME,
                        REQUESTED_DATE = fr.REQUESTED_DATE,
                        RELATED_MIS_FILE_NO = fr.RELATED_MIS_FILE_NO,
                        SCANNED_PHYSICAL_IND = fr.SCANNED_PHYSICAL_IND,
                        REQUEST_CAT_ID = fr.REQ_CATEGORY,
                        REQUEST_CAT_TYPE_ID = fr.REQ_CATEGORY_TYPE,
                        REQUEST_CAT_DETAIL = fr.REQ_CATEGORY_DETAIL,
                        PICKLIST_STATUS = fr.PICKLIST_STATUS
                    };

                if (query.Any())
                {
                    //divPrintRMC.Style.Add("display", "");
                    //btnPrintRMC.Visible = true;
                    //divPrintRMC.Attributes["style"] = "";
                    //ScriptManager.RegisterStartupScript(this, GetType(), "ShowPrintRMC", "document.getElementByID('divPrintRMC').style.display = '';", true);
                }
                else
                {
                    //divPrintRMC.Style.Add("display", "none");
                    //btnPrintRMC.Visible = false;
                    divPrintRMC.Attributes["style"] = "display: none";
                    //ScriptManager.RegisterStartupScript(this, GetType(), "HidePrintRMC", "document.getElementByID('divPrintRMC').style.display = 'none';", true);
                }
            }
            catch (Exception ex)
            {
                lblError.Text = ex.Message;
                divError.Style.Add("display", "");
                return null;
            }

            return query;
        }

        public IQueryable<FileRequest> GetTDWPicklist()
        {
            var x = UserSession.Office.OfficeId;
            string pstatus = "In Progress";
            string region = UserSession.Office.RegionId;//Session["CSUserOfficeRegion"].ToString();
            // Retrieve all files that have been submitted by local offices in the same region as the RMC office.
            // Only show files that have aRE iN Progress and not sent to TDW.

            IQueryable<FileRequest> query;
            try
            {
                //query =
                //    from fr in en.DC_FILE_REQUEST
                //    join region in en.DC_REGION on fr.REGION_ID equals region.REGION_ID into region1
                //    from region in region1.DefaultIfEmpty()
                //    //join region in en.DC_REGION on fr.REGION_ID equals region.REGION_ID into grt1
                //    //from grant in grt1.DefaultIfEmpty()
                //    join lo in en.DC_LOCAL_OFFICE on fr.REQUESTED_OFFICE_ID equals lo.OFFICE_ID
                //    join lou in en.DC_LOCAL_OFFICE on lo.REGION_ID equals lou.REGION_ID
                //    where fr.SCANNED_DATE == null
                //    && fr.CLOSED_DATE == null
                //    && fr.SENT_TDW == "Y"
                //    orderby fr.REQUESTED_DATE, fr.ID_NO

                //fr.SCANNED_DATE == null                &&
                query = en.Database.SqlQuery<FileRequest>($@"SELECT
                                                                FR.UNQ_FILE_NO
                                                                , FR.ID_NO
                                                                , FR.BRM_BARCODE
                                                                , FR.MIS_FILE_NO
                                                                , FR.BIN_ID
                                                                , FR.BOX_NUMBER
                                                                , FR.POSITION
                                                                , FR.TDW_BOXNO
                                                                , FR.NAME
                                                                , FR.SURNAME
                                                                , FR.REQUESTED_DATE
                                                                , FR.RELATED_MIS_FILE_NO
                                                                , FR.SCANNED_PHYSICAL_IND
                                                                , FR.REQ_CATEGORY AS REQUEST_CAT_ID
                                                                , FR.REQ_CATEGORY_TYPE AS REQUEST_CAT_TYPE_ID
                                                                , FR.REQ_CATEGORY_DETAIL AS REQUEST_CAT_DETAIL
                                                                , FR.PICKLIST_STATUS
                                                                , FR.APPLICATION_STATUS
                                                                , F.GRANT_TYPE
                                                                , CAST(F.ARCHIVE_YEAR AS NUMBER(4)) AS ARCHIVE_YEAR
                                                            FROM DC_FILE_REQUEST FR
                                                            LEFT JOIN DC_FILE F ON FR.BRM_BARCODE = F.BRM_BARCODE
                                                                AND FR.MIS_FILE_NO = F.FILE_NUMBER
                                                            WHERE FR.CLOSED_DATE IS NULL
                                                            AND FR.REGION_ID = '{region}'
                                                            AND FR.PICKLIST_STATUS = '{pstatus}'
                                                            AND FR.SENT_TDW = 'Y'
                                                            ORDER BY FR.REQUESTED_DATE DESC, FR.ID_NO").AsQueryable();

                if (query.Any())
                {
                    //divPrintTDW.Style.Add("display", "");
                    //btnPrintTDW.Visible = true;
                    divPrintTDW.Attributes["style"] = "";
                    //ScriptManager.RegisterStartupScript(this, GetType(), "ShowPrintTDW", "document.getElementByID('divPrintTDW').style.display = '';", true);
                }
                else
                {
                    //divPrintTDW.Style.Add("display", "none");
                    //btnPrintTDW.Visible = false;
                    divPrintTDW.Attributes["style"] = "display: none";
                    //ScriptManager.RegisterStartupScript(this, GetType(), "HidePrintTDW", "document.getElementByID('divPrintTDW').style.display = 'none';", true);
                }
            }
            catch (Exception ex)
            {
                lblError.Text = ex.Message;
                divError.Style.Add("display", "");
                return null;
            }

            return query;
        }

        public override void VerifyRenderingInServerForm(Control control)
        {
            //required to avoid the run time error "
            //Control 'GridView1' of type 'Grid View' must be placed inside a form tag with runat=server."
        }

        protected static string Get_ArchiveYear(string sd1, string sd2, string sd3, string sd4)
        {
            int tryDate1 = 0;
            int tryDate2 = 0;
            int tryDate3 = 0;
            int tryDate4 = 0;

            if (!string.IsNullOrEmpty(sd1)) { int.TryParse(sd1, out tryDate1); }
            if (!string.IsNullOrEmpty(sd2)) { int.TryParse(sd2, out tryDate2); }
            if (!string.IsNullOrEmpty(sd3)) { int.TryParse(sd3, out tryDate3); }
            if (!string.IsNullOrEmpty(sd4)) { int.TryParse(sd4, out tryDate4); }

            int lastdate;
            if (tryDate1 > tryDate2) { lastdate = tryDate1; } else { lastdate = tryDate2; }
            if (tryDate3 > lastdate) { lastdate = tryDate3; }
            if (tryDate4 > lastdate) { lastdate = tryDate4; }

            //lastdate substring and replace yyyy;
            //int fiveyearslater = lastdate + 50000;
            //return fiveyearslater.ToString();
            return lastdate == 0 ? null : lastdate.ToString().Substring(0, 4);
        }

        private class SOCPEN_BRM
        {
            #region Public Properties

            public string PENSION_NO { get; set; }

            public string STATUS_DATE1 { get; set; }

            public string STATUS_DATE2 { get; set; }

            public string STATUS_DATE3 { get; set; }

            public string STATUS_DATE4 { get; set; }

            #endregion Public Properties
        }

        private class SOCPEN_CHILD_STATUS
        {
            #region Public Properties

            public string ID_NO { get; set; }

            public string PENSION_NO { get; set; }

            public string STATUS_CODE { get; set; }

            public DateTime STATUS_DATE { get; set; }

            #endregion Public Properties
        }

        #endregion Public Methods

        #region Protected Methods

        protected void btnExcelExport_Click(object sender, EventArgs e)
        {
            Response.Clear();
            Response.Buffer = true;
            Response.AddHeader("content-disposition", "attachment;filename=TDW_Picklist_" + DateTime.Now.Date.ToString("dd-MM-yyyy") + ".xls");
            Response.Charset = "";
            Response.ContentType = "application/vnd.ms-excel";
            using (StringWriter sw = new StringWriter())
            {
                HtmlTextWriter hw = new HtmlTextWriter(sw);

                //To Export all pages
                fileGridView3.AllowPaging = false;
                fileGridView3.SelectMethod = "GetTDWPicklist";
                fileGridView3.DataBind();

                fileGridView3.HeaderRow.BackColor = Color.White;
                foreach (TableCell cell in fileGridView3.HeaderRow.Cells)
                {
                    cell.BackColor = fileGridView3.HeaderStyle.BackColor;
                }
                foreach (GridViewRow row in fileGridView3.Rows)
                {
                    row.BackColor = Color.White;
                    foreach (TableCell cell in row.Cells)
                    {
                        if (row.RowIndex % 2 == 0)
                        {
                            cell.BackColor = fileGridView3.AlternatingRowStyle.BackColor;
                        }
                        else
                        {
                            cell.BackColor = fileGridView3.RowStyle.BackColor;
                        }
                        cell.CssClass = "textmode";
                    }
                }

                fileGridView3.RenderControl(hw);

                //style to format numbers to string
                string style = @"<style> .textmode { mso-number-format:\@; } </style>";
                Response.Write(style);
                Response.Output.Write(sw.ToString());
                Response.Flush();
                Response.End();
            }
        }

        protected void btnPrintRMC_Click(object sender, EventArgs e)
        {
            ClientScript.RegisterStartupScript(Page.GetType(), "printRMC", "printPicklist('RMC');", true);
        }

        protected void btnPrintTDW_Click(object sender, EventArgs e)
        {
            ClientScript.RegisterStartupScript(Page.GetType(), "printRMC", "printPicklist('TDW');", true);
        }

        protected void fileGridView_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            fileGridView.PageIndex = e.NewPageIndex;
            fileGridView.SelectMethod = "GetFileRequestHistory";
        }

        protected void fileGridView2_DataBound(object sender, EventArgs e)
        {
            //if (fileGridView2.Rows.Count == 0)
            //{
            //    divPrintRMC.Attributes["Style"] = "display: none";
            //}
            //else
            //{
            //    divPrintRMC.Attributes["Style"] = "";
            //}
        }

        protected void fileGridView2_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            fileGridView2.PageIndex = e.NewPageIndex;
            fileGridView2.SelectMethod = "GetRMCPicklist";
        }

        protected void fileGridView3_DataBound(object sender, EventArgs e)
        {
            if (fileGridView3.Rows.Count == 0)
            {
                divPrintTDW.Attributes["Style"] = "display: none";
            }
            else
            {
                divPrintTDW.Attributes["Style"] = "";
            }
        }

        protected void fileGridView3_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            fileGridView3.PageIndex = e.NewPageIndex;
            fileGridView3.SelectMethod = "GetTDWPicklist";
        }

        protected void hiddenBtnData_Click(object sender, EventArgs e)
        {
            //Button is clicked via javascript to reload the grid after a successful update on a file.
            fileGridView.SelectMethod = "GetFileRequestHistory";
        }

        protected void hiddenBtnRMC_Click(object sender, EventArgs e)
        {
            fileGridView2.SelectMethod = "GetRMCPicklist";
        }

        protected void hiddenBtnTDW_Click(object sender, EventArgs e)
        {
            fileGridView3.SelectMethod = "GetTDWPicklist";
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            //Dont need to handle authentication as this is done on the master page load.
            if (!IsPostBack)
            {
                //ClientScript.RegisterStartupScript(Page.GetType(), "showPanel1", "window.document.getElementById('tab-historic').style.display = '';", true);
            }

            //Load grid with file results.
            fileGridView.SelectMethod = "GetFileRequestHistory";
        }

        #endregion Protected Methods
    }
}