using SASSADirectCapture.EntityModels;
using SASSADirectCapture.Sassa;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SASSADirectCapture.Views
{
    public partial class BoxCover : SassaPage
    {
        #region Public Fields

        public string cBarCodeCount = string.Empty;

        public string cBatchPrintedAlready = string.Empty;

        public string cMyValue = string.Empty;

        public string cPrintDiv = string.Empty;

        #endregion Public Fields

        #region Private Fields

        private Entities en = new Entities();

        #endregion Private Fields

        #region Public Methods

        public IQueryable<FileEntity> GetAllFilesByBoxNo()
        {
            string boxNo = Session["BoxNo"].ToString();
            var x = en.DC_FILE.Where(bn => bn.TDW_BOXNO == boxNo).OrderBy(f => f.UNQ_FILE_NO)
                    .Select(f => new FileEntity
                    {
                        CLM_UNIQUE_CODE = f.UNQ_FILE_NO,
                        BRM_BARCODE = f.BRM_BARCODE,
                        APPLICANT_NO = f.APPLICANT_NO,
                        FIRST_NAME = f.USER_FIRSTNAME,
                        LAST_NAME = f.USER_LASTNAME,
                        GRANT_TYPE_NAME = f.DC_GRANT_TYPE.TYPE_NAME
                    });
            int howmany = x.Count();

            if (howmany > 0) { boxGridView.PageSize = howmany; }

            List<FileEntity> fe = new List<FileEntity>();
            foreach (var item in x)
            {
                FileEntity f = item;
                fe.Add(f);
            }

            txtNoFiles.InnerText = fe.Count.ToString();
            return fe.AsQueryable();
        }

        #endregion Public Methods

        #region Protected Methods

        protected void batchGridView_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            boxGridView.PageIndex = e.NewPageIndex;
            boxGridView.SelectMethod = "GetAllFilesByboxNo";
            //DoMethod();
        }

        protected void btnPrintClose_Click(object sender, EventArgs e)
        {
            decimal batchNo = 0.0M;

            string boxNo = Request.QueryString["boxNo"].ToString();
            if (cbxPrintFileCovers.Checked)
            {
                ClientScript.RegisterStartupScript(Page.GetType(), "PrintFileCovers", "window.open('FileCoverBulk.aspx?BatchNo=-1&BoxNo=-1&TDWBoxNo=" + boxNo + "', 'Print Bulk Cover Sheets');", true);
            }

            if (Request.QueryString.ToString().Contains("batchNo"))
            {
                Decimal.TryParse(Request.QueryString["batchNo"].ToString(), out batchNo);
                var z = UserSession.Office.OfficeId;
                string sUserLogin = UserSession.SamName;
                bool bUseADAuth = bool.Parse(System.Web.Configuration.WebConfigurationManager.AppSettings["UseADAuth"].ToString());

                DC_BATCH batch = en.DC_BATCH
                    .Where(b => b.BATCH_NO == batchNo)
                    .Where(oid => oid.OFFICE_ID == z)
                    .FirstOrDefault();
                if (batch != null && batch.BATCH_CURRENT.ToLower() == "y") //If the user refreshes the Batch coversheet, it shouldn't create a duplicate current batch
                {
                    batch.BATCH_CURRENT = "N";
                    batch.BATCH_STATUS = "Closed"; // Changed from Sent to Closed
                    batch.UPDATED_DATE = DateTime.Now;


                    batch.UPDATED_BY_AD = sUserLogin;


                    try
                    {
                        en.DC_ACTIVITY.Add(util.CreateActivity("Boxing", "Print Cover/Close"));
                        en.SaveChanges();

                        DC_BATCH b = new DC_BATCH();
                        b.BATCH_STATUS = "Open";
                        b.BATCH_CURRENT = "Y";
                        b.OFFICE_ID = UserSession.Office.OfficeId;
                        b.UPDATED_DATE = DateTime.Now;

                        b.UPDATED_BY_AD = sUserLogin;


                        b.REG_TYPE = batch.REG_TYPE;

                        en.DC_BATCH.Add(b);
                        en.DC_ACTIVITY.Add(util.CreateActivity("Boxing", "Create New Batch"));
                        en.SaveChanges();

                        cBatchPrintedAlready = "y";

                        divError.Visible = false;
                        divSuccess.Visible = true;
                        lblSuccess.Text = "Batch <b>" + batchNo + "</b> successfully closed.";

                        ClientScript.RegisterStartupScript(Page.GetType(), "save", "try {window.opener.UpdateCurrentGrid();} catch (Exception) {}", true);
                    }
                    catch (Exception ex)
                    {
                        lblError.Text = ex.Message;
                        divError.Visible = true;
                        ClientScript.RegisterStartupScript(Page.GetType(), "error", "alert('Unable to save data : " + ex.Message + "')", true);
                    }
                }

                System.Web.HttpContext.Current.Response.Redirect("~/Default.aspx");
                ClientScript.RegisterStartupScript(Page.GetType(), "print", "javascript:printDiv('PrintArea', true) ", true);
                ClientScript.RegisterStartupScript(Page.GetType(), "closethis", "javascript:opener.window.focus();window.close();", true);
            }
            else if (Request.QueryString.ToString().Contains("bxf") && Request.QueryString["bxf"].ToString().ToLower() == "y") //If this is from the audit boxing process
            {
                //string boxNo = Request.QueryString["boxNo"].ToString();

                try
                {
                    var x = en.DC_FILE
                            .Where(f => f.TDW_BOXNO == boxNo);

                    if (x == null)
                    {
                        return;
                    }

                    foreach (DC_FILE file in x)
                    {
                        file.MIS_REBOX_STATUS = "Completed";
                        file.UPDATED_DATE = DateTime.Now;


                        file.UPDATED_BY_AD = UserSession.SamName;

                    }
                    en.DC_ACTIVITY.Add(util.CreateActivity("Boxing", "Mark files completed"));
                    en.SaveChanges();
                }
                catch (Exception ex)
                {
                    lblError.Text = ex.Message;
                    divError.Visible = true;
                }

                ClientScript.RegisterStartupScript(Page.GetType(), "save", "try {window.opener.updateReboxFull();} catch (Exception) {} window.close();", true);
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            Page.Title = string.Empty;
            DoMethod();
        }

        #endregion Protected Methods

        #region Private Methods

        private string Build()
        {
            const int noOfRecordsToBreakAfter = 40; //15;25;
            string s = string.Empty;

            var j = GetAllFilesByBoxNo();
            var t = j.Count();

            var cnt = 1;
            var m = (cnt - 1) * noOfRecordsToBreakAfter;

            s = s + BuildPrint();//tempo

            if (t == 0)
            {
                s = s.Replace("%%barcodeno%%", (cnt - 1).ToString());
                s = s.Replace("%%trData%%", string.Empty);
            }
            else
            {
                while (m < t)
                {
                    m = (cnt - 1) * noOfRecordsToBreakAfter;
                    cnt++;
                    var k = j
                       .Skip(m)
                       .Take(noOfRecordsToBreakAfter);

                    if ((k != null) && (k.Count() == 0))
                    {
                        break;
                    }

                    StringBuilder sb = new StringBuilder();
                    foreach (var item in k)
                    {
                        sb.Append(@"<tr>");
                        sb.Append(@"    <td>" + item.CLM_UNIQUE_CODE + "</td>");
                        sb.Append(@"    <td>" + item.BRM_BARCODE + "</td>");
                        sb.Append(@"    <td>" + item.APPLICANT_NO + "</td>");
                        sb.Append(@"    <td>" + item.FULL_NAME + "</td>");
                        sb.Append(@"	<td>" + item.GRANT_TYPE_NAME + "</td>");
                        sb.Append(@"</tr>");
                    }
                    s = s.Replace("%%barcodeno%%", (cnt - 1).ToString());
                    s = s.Replace("%%trData%%", sb.ToString());
                    //s = s.Replace("%%nofiles%%", t.ToString());
                    s = s + BuildPrint();
                }
            }

            if (m >= t)
            {
                s = s.Replace(BuildPrint(), "");
            }

            cBarCodeCount = (cnt - 1).ToString();
            if (j != null)
            {
                s = s.Replace("%%nofiles%%", j.Count().ToString());
            }

            return s;
        }

        private string BuildPrint()
        {
            string html = string.Empty;
            StringBuilder sb = new StringBuilder();
            sb.Append(@"<table border=""1"" style='width:100%'>");
            sb.Append(@"  <tr>");
            sb.Append(@"	<td>");
            sb.Append(@"      <table style=""width:100%;"">");
            sb.Append(@"       <tr>");
            sb.Append(@"         <td>");
            sb.Append(@"          <div class=""container row"" style=""height: 100%; display: block;"">");
            sb.Append(@"          <div style=""width: 30 %; float: left; display: inline-block"">");
            sb.Append(@"             <img alt="""" src=""../Content/images/sassa_logoSmall.jpg"" width=""200px"" />");
            sb.Append(@"          </div>");
            sb.Append(@"          <div style=""width:45%; display:inline-block; height:100px; text-align:center; padding-top: 10px;"">");
            sb.Append(@"            <label class=""h2"">Box Coversheet</label>");
            sb.Append(@"            <br />Box no:%%boxno%%<br />No of Files:%%nofiles%%");
            sb.Append(@"          </div>");
            //sb.Append(@"        <br /><br />");
            //sb.Append(@"        <br /><br />");
            //sb.Append(@"        No of Files: ");
            sb.Append(@"          <span runat=""server"" id=""txtNoFiles""></span><br />");
            sb.Append(@"          <span runat=""server"" id=""txtBoxNo""></span><br />");
            sb.Append(@"          <div class=""divDateTime""></div>");
            sb.Append(@"         </td>");
            sb.Append(@"         <td>");
            sb.Append(@"           <div id=""barcodecontainer""  style=""width: 20%; float: right; margin-right: 5px; margin-top: 10px"">");
            sb.Append(@"             <div style=""float: right");
            sb.Append(@"               <img runat=""server"" style=""top: 20px; position: relative;"" class=""barcode"" id=""barcode%%barcodeno%%"" alt=""%%boxno%%"" />");
            sb.Append(@"             </div>");
            sb.Append(@"           </div>");
            sb.Append(@"         </td>");
            sb.Append(@"       </tr>");
            sb.Append(@"     </table>");
            sb.Append(@"   </td>");
            sb.Append(@" </tr>");
            sb.Append(@"</table>");

            sb.Append(@"<table style=""width:100%;"">");
            sb.Append(@"    <tr>");
            sb.Append(@"		<td colspan=""6""> ");
            sb.Append(@"		    <section class=""contact"">");
            sb.Append(@"				<div>");
            sb.Append(@"					<table cellspacing = ""0"" cellpadding=""4"" id=""MainContent_batchGridView"" style=""color:#333333;width:100%;border-collapse:collapse;height: 100%; width: 100%;"">");
            sb.Append(@"						<tr style=""color:White;background-color:#507CD1;font-weight:bold;"">");
            sb.Append(@"							<th scope=""col"">CLM Unique Code</th>");
            sb.Append(@"							<th scope=""col"">BRM File #</th>");
            sb.Append(@"							<th scope=""col"">ID #</th>");
            sb.Append(@"                            <th scope=""col"">Name and Surname</th>");
            sb.Append(@"							<th scope=""col"">Grant Type</th>");
            sb.Append(@"						</tr>");
            sb.Append(@"%%trData%%");
            sb.Append(@"                  </table>");
            sb.Append(@"				</div>");
            sb.Append(@"            </section>");
            sb.Append(@"		</td>");
            sb.Append(@"    </tr>");
            sb.Append(@"</table>");
            sb.Append(@"<p style='page-break-after:always'/>");

            string boxNo = "";
            if (Session["BoxNo"] != null && Session["BoxNo"].ToString() != string.Empty)
            {
                boxNo = Session["BoxNo"].ToString();
            }
            else
            {
                var qs = Request.QueryString["boxNo"].ToString();
                if (qs.Contains("boxNo"))
                {
                    Request.QueryString["boxNo"].ToString();
                }
                else
                {
                    var mss = "Browser Session was inactive for too long: Lost the box number, You might have to Login again, before you can try this again.";
                    ClientScript.RegisterStartupScript(Page.GetType(), "close", "alert('" + mss + "');window.close();", true);
                }
            }
            // ClientScript.RegisterStartupScript(Page.GetType(), "showit", "alert('" + boxNo + "');", true);
            html = sb.ToString();
            html = html.Replace("%%boxno%%", boxNo);
            //nofiles

            return html;
        }

        private void DoMethod()
        {
            string boxNo = string.Empty;
            bool nodata = false;

            if (Session["BoxNo"] != null && Session["BoxNo"].ToString() != string.Empty)
            {
                boxNo = Session["BoxNo"].ToString();
            }
            else
            {
                if (Request.QueryString["BoxNo"].ToString() != null && Request.QueryString["BoxNo"].ToString() != string.Empty)
                {
                    boxNo = Request.QueryString["BoxNo"].ToString();
                }
                else
                {
                    //lost session variables
                    nodata = true;
                }
            }
            if (nodata)
            {
                ClientScript.RegisterStartupScript(Page.GetType(), "nodate", "alert('The session died, or no box number was available.');", true);
            }
            else
            {
                cMyValue = boxNo;
                barcode.Alt = boxNo;
                txtBoxNo.InnerText = boxNo;
                boxGridView.SelectMethod = "GetAllFilesByBoxNo";
                cPrintDiv = Build();
            }
        }

        #endregion Private Methods
    }
}
