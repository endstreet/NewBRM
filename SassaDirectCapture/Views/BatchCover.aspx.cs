using QRCoder;
using SASSADirectCapture.EntityModels;
using SASSADirectCapture.Sassa;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SASSADirectCapture.Views
{
    public partial class BatchCover : SassaPage
    {
        #region Public Fields

        //public SASSA_Authentication authObject = new SASSA_Authentication();

        public string cBarCodeCount = string.Empty;

        public string cBatchPrintedAlready = string.Empty;

        public string cMyValue = string.Empty;

        public string cPrintDiv = string.Empty;

        #endregion Public Fields

        #region Private Fields

        private Entities en = new Entities();

        #endregion Private Fields

        #region Public Methods

        public IQueryable<FileEntity> GetAllFilesByBatchNo()
        {
            decimal batchNo = 0.0M;
            if (Decimal.TryParse(Request.QueryString["batchNo"].ToString(), out batchNo))
            {
                var x = en.DC_FILE.Where(bn => bn.BATCH_NO == batchNo).OrderBy(f => f.UNQ_FILE_NO)
                    .Select(f => new FileEntity
                    {
                        UNQ_FILE_NO = f.UNQ_FILE_NO,
                        BATCH_NO = f.BATCH_NO,
                        OFFICE_NAME = f.DC_LOCAL_OFFICE.OFFICE_NAME,
                        REGION_NAME = f.DC_LOCAL_OFFICE.DC_REGION.REGION_NAME,
                        APPLICANT_NO = f.APPLICANT_NO,
                        GRANT_TYPE_NAME = f.DC_GRANT_TYPE.TYPE_NAME,
                        GRANT_TYPE = f.GRANT_TYPE,
                        //TRANS_TYPE = f.TRANS_TYPE,
                        DOCS_PRESENT = f.DOCS_PRESENT,
                        APP_DATE = "",
                        TRANS_DATE = f.TRANS_DATE,
                        UPDATED_BY = f.UPDATED_BY,
                        UPDATED_DATE = f.UPDATED_DATE,
                        BATCH_ADD_DATE = f.BATCH_ADD_DATE,
                        FILE_STATUS = f.FILE_STATUS,
                        FIRST_NAME = f.USER_FIRSTNAME,
                        LAST_NAME = f.USER_LASTNAME,
                        FILE_COMMENT = f.FILE_COMMENT,
                        CLM_UNIQUE_CODE = f.UNQ_FILE_NO,
                        BRM_BARCODE = f.BRM_BARCODE,
                        APPLICATION_STATUS = f.APPLICATION_STATUS,
                        CHILD_ID_NO = f.CHILD_ID_NO
                    });

                List<FileEntity> fe = new List<FileEntity>();
                foreach (var item in x)
                {
                    // format APP_DATE
                    if (item.TRANS_DATE != null)
                    {
                        string myString = item.TRANS_DATE.ToString();
                        int waar = myString.IndexOf('/');
                        string myMM = myString.Substring(0, waar);
                        myString = myString.Substring(waar + 1);

                        waar = myString.IndexOf('/');
                        string myDD = myString.Substring(0, waar);
                        myString = myString.Substring(waar + 1);

                        waar = myString.IndexOf(' ');
                        string myYYYY = myString.Substring(0, waar);
                        if (myMM.Length == 1) { myMM = "0" + myMM; }
                        if (myDD.Length == 1) { myDD = "0" + myDD; }
                        item.APP_DATE = myYYYY + "-" + myMM + "-" + myDD;
                    }

                    //get the document descriptions

                    List<string> docItems = new List<string>();
                    if (item.DOCS_PRESENT != null)
                    {
                        List<string> s = item.DOCS_PRESENT.Split(';').ToList();
                        foreach (var str in s)
                        {
                            decimal k = Convert.ToDecimal(str.Trim());
                            var t = en.DC_DOCUMENT_TYPE.Where(d => d.TYPE_ID == k).Select(p => p.TYPE_NAME).FirstOrDefault();
                            docItems.Add(t);
                        }
                    }
                    var j = string.Join(",", docItems);

                    FileEntity f = item;
                    f.DOCS_PRESENT = j;

                    //var o = en.SOCPENIDs.Where(ss => ss.PENSION_NO == f.APPLICANT_NO);
                    //if (o != null)
                    //{
                    //    var pp = o.FirstOrDefault();
                    //    if (pp != null)
                    //    {
                    //        f.FULL_NAME = pp.NAME.Trim() + " " + pp.SURNAME.Trim();
                    //    }
                    //}

                    f.FULL_NAME = f.FIRST_NAME + " " + f.LAST_NAME;

                    fe.Add(f);
                }

                txtNrOfApplicants.Text = fe.Count.ToString();

                return fe.AsQueryable();
            }
            else
            {
                return null;
            }
        }

        #endregion Public Methods

        #region Protected Methods

        protected void batchGridView_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            batchGridView.PageIndex = e.NewPageIndex;
            batchGridView.SelectMethod = "GetAllFilesByBatchNo";
            DoMethod();
        }

        protected void batchGridView_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            decimal batchNo = 0.0M;
            Decimal.TryParse(Request.QueryString["batchNo"].ToString(), out batchNo);
            var z = UserSession.Office.OfficeId;
            DC_BATCH batch = en.DC_BATCH
                .Where(b => b.BATCH_NO == batchNo)
                .Where(oid => oid.OFFICE_ID == z)
                .FirstOrDefault();

            bool bBatchIsCurrent = batch.BATCH_CURRENT.ToLower() == "y";

            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                string BRMBarCode = e.Row.Cells[1].Text;
                string unqno = e.Row.Cells[0].Text;
                string pensionNo = e.Row.Cells[2].Text;
                string fullname = e.Row.Cells[3].Text;
                string grantname = e.Row.Cells[4].Text;
                e.Row.FindControl("plBarcode").Controls.Add(doQRCode(BRMBarCode, unqno, pensionNo, fullname, grantname));

                if (bBatchIsCurrent)
                {
                    e.Row.Cells[10].Style["display"] = "";
                    e.Row.Cells[11].Style["display"] = "";
                }
                else
                {
                    e.Row.Cells[10].Style["display"] = "none";
                    e.Row.Cells[11].Style["display"] = "none";
                }
            }
            else if (e.Row.RowType == DataControlRowType.Header)
            {
                if (bBatchIsCurrent)
                {
                    e.Row.Cells[10].Style["display"] = "";
                    e.Row.Cells[11].Style["display"] = "";
                }
                else
                {
                    e.Row.Cells[10].Style["display"] = "none";
                    e.Row.Cells[11].Style["display"] = "none";
                }
            }
        }

        protected void btnPrint_Click(object sender, EventArgs e)//Previously btnPrintClose_Click
        {
            decimal batchNo = 0.0M;
            Decimal.TryParse(Request.QueryString["batchNo"].ToString(), out batchNo);
            var z = UserSession.Office.OfficeId;

            string sUserLogin = UserSession.SamName;

            string sActiveLoginName;
            sActiveLoginName = sUserLogin;

            DC_BATCH batch = en.DC_BATCH
                .Where(b => b.BATCH_NO == batchNo)
                .Where(oid => oid.OFFICE_ID == z)
                .FirstOrDefault();
            if (batch != null && batch.BATCH_CURRENT.ToLower() == "y") //If the user refreshes the Batch coversheet, it shouldn't create a duplicate current batch
            {
                batch.BATCH_CURRENT = "N";
                batch.BATCH_STATUS = "Closed"; // Changed from Sent to Closed
                batch.UPDATED_DATE = DateTime.Now;
                batch.UPDATED_BY_AD = sActiveLoginName;
                batch.UPDATED_BY = 0;


                try
                {
                    en.DC_ACTIVITY.Add(util.CreateActivity("Batching", "Print batch (Close)"));
                    en.SaveChanges();

                    DC_BATCH b = new DC_BATCH();
                    b.BATCH_STATUS = "Open";
                    b.BATCH_CURRENT = "Y";
                    b.OFFICE_ID = UserSession.Office.OfficeId;
                    b.UPDATED_DATE = DateTime.Now;
                    b.UPDATED_BY_AD = sActiveLoginName;
                    b.UPDATED_BY = 0;

                    b.REG_TYPE = batch.REG_TYPE;
                    en.DC_ACTIVITY.Add(util.CreateActivity("Batching", "New batch (Open)"));
                    en.DC_BATCH.Add(b);
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
        }

        protected void btnReprint_Click(object sender, EventArgs e)
        {
            ClientScript.RegisterStartupScript(Page.GetType(), "close", "window.close()", true);
        }

        protected void lbtnRemove_Click(object sender, EventArgs e)
        {
            using (Entities context = new Entities())
            {
                try
                {
                    Decimal dBatchNo = Decimal.Parse(Request.QueryString["batchNo"].ToString());
                    string sUnqFileNo = ((GridViewRow)((LinkButton)sender).Parent.Parent).Cells[0].Text;

                    DC_FILE file = context.DC_FILE.Where(f => f.UNQ_FILE_NO == sUnqFileNo
                                                    && f.BATCH_NO == dBatchNo)
                                             .OrderByDescending(g => g.UPDATED_DATE)
                                             .FirstOrDefault();

                    if (file != null)
                    {
                        file.BATCH_NO = 0;//Set to Temporary Batch 0
                        context.DC_ACTIVITY.Add(util.CreateActivity("Batching", "Remove item from batch"));
                        context.SaveChanges();

                        batchGridView.SelectMethod = "GetAllFilesByBatchNo";
                        batchGridView.DataBind();
                    }
                }
                catch (Exception ex)
                {
                    lblError.Text = ex.Message;
                    divError.Visible = true;
                }
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            //if (!IsPostBack)
            //{
            //    //Get the username of the user that is logged in from session.
            //    string authenticatedUsername = this.authObject.AuthenticateCSAdminUser();

            //    //If no session values are found , redirect to the login screen
            //    if (authenticatedUsername == string.Empty)
            //    {
            //        this.authObject.RedirectToLoginPage();
            //    }
            //}

            Page.Title = string.Empty;
            DoMethod();
        }

        #endregion Protected Methods

        #region Private Methods

        private string Build()
        {
            const int noOfRecordsToBreakAfter = 11; //15;
            string s = string.Empty;

            var j = GetAllFilesByBatchNo();
            var t = j.Count();
            var cnt = 1;
            var m = (cnt - 1) * noOfRecordsToBreakAfter;

            s = s + BuildPrint();

            ClientScript.RegisterStartupScript(Page.GetType(), "BEFORE", "alert(" + s + ");", true);

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
                    //< img src = "#" class="2Dbar" alt='F1200788 KZN000000438' width="52" height="52" />

                    StringBuilder sb = new StringBuilder();
                    foreach (var item in k)
                    {
                        sb.Append(@"<tr>");
                        sb.Append(@"    <td>" + item.CLM_UNIQUE_CODE + "</td>");
                        sb.Append(@"    <td>" + item.BRM_BARCODE + "</td>");
                        sb.Append(@"	<td>" + item.APPLICANT_NO + "</td>");
                        sb.Append(@"    <td>" + item.FULL_NAME + "</td>");
                        sb.Append(@"	<td>" + item.GRANT_TYPE_NAME + "</td>");
                        sb.Append(@"	<td>" + Convert.ToDateTime(item.TRANS_DATE).ToString("yyyy/MM/dd") + "</td>");
                        sb.Append(@"	<td>" + item.APPLICATION_STATUS + "</td>");
                        sb.Append(@"	<td><img src = ""#"" class=""2Dbar"" alt='");
                        //sb.Append(item.BRM_BARCODE + " " + item.CLM_UNIQUE_CODE);
                        sb.Append(item.BRM_BARCODE + "\t" + item.CLM_UNIQUE_CODE);
                        sb.Append(@"' width=""52"" height=""52"" /></td>");
                        sb.Append(@"</tr>");
                    }
                    s = s.Replace("%%barcodeno%%", (cnt - 1).ToString());
                    s = s.Replace("%%trData%%", sb.ToString());

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
                s = s.Replace("%%nrofapplicants%%", j.Count().ToString());
            }
            ClientScript.RegisterStartupScript(Page.GetType(), "AFTER", "alert(" + s + ");", true);

            return s;
        }

        private string BuildPrint()
        {
            string html = string.Empty;
            StringBuilder sb = new StringBuilder();
            sb.Append(@"<table border=""1"" cellpadding=""0"" cellspacing=""0"" style='width:100%;'>");
            sb.Append(@"  <tr>");
            sb.Append(@"    <td>");
            sb.Append(@"      <table cellpadding=""0"" cellspacing=""0"" style=""width:100%;"">");
            sb.Append(@"        <tr>");
            sb.Append(@"          <td>");

            sb.Append(@"      <table cellpadding=""0"" cellspacing=""0"" style=""width:100%;"">");
            sb.Append(@"        <tr>");
            sb.Append(@"          <td>");
            sb.Append(@"            <div class=""container row"" style=""height: 100%; width:100%; display: block;"">");
            sb.Append(@"                <div style="" float: left; display: inline-block"">");
            sb.Append(@"                    <img alt="""" src=""../Content/images/sassa_logoSmall.jpg"" width=""200"" />");
            sb.Append(@"                </div>");

            sb.Append(@"          </td>");
            sb.Append(@"          <td>");

            sb.Append(@"                <div style="" display: inline-block; height: 100px; text-align: center; padding-top: 10px;"">");
            sb.Append(@"                    <label class=""h2"">Batch Coversheet</label>");
            sb.Append(@"                </div>");
            sb.Append(@"          </td>");
            sb.Append(@"          <td>");
            sb.Append(@"                <div id=""barcodecontainer"" style="" float: right; margin-right: 15px"">");
            sb.Append(@"                    <div style=""float: right"">");
            sb.Append(@"                        <div class=""divDateTime"">");
            sb.Append(@"                        </div>");
            sb.Append(@"                        <img runat=""server"" style=""top: 50px; position: relative;"" class=""barcode"" id=""barcode%%barcodeno%%"" alt=""%%batchno%%"" /></div>");

            sb.Append(@"                    </div>");
            sb.Append(@"                </div>");
            sb.Append(@"        </tr>");
            sb.Append(@"      </table>");

            sb.Append(@"            </div>");

            sb.Append(@"          </td>");
            sb.Append(@"        </tr>");
            sb.Append(@"      </table>");
            sb.Append(@"    </td>");
            sb.Append(@"  </tr>");
            sb.Append(@"</table>");

            sb.Append(@"<table cellpadding=""0"" cellspacing=""0"" style=""width:100%;"">");
            sb.Append(@"  <tr>");
            sb.Append(@"    <td style=""text-align:right"">Batch No:</td>");
            sb.Append(@"    <td>%%batchno%%</td>");
            sb.Append(@"    <td style=""text-align:right"">TDW Batch Order No:</td>");
            sb.Append(@"    <td>%%waybillno%%</td>");
            sb.Append(@"    <td>&nbsp;</td>");
            sb.Append(@"    <td>&nbsp;</td>");
            sb.Append(@"  </tr>");
            sb.Append(@"  <tr>");
            sb.Append(@"    <td style=""text-align:right"">Local Office:</td>");
            sb.Append(@"    <td>%%localoffice%%</td>");
            sb.Append(@"    <td style=""text-align:right"">Courier Name:</td>");
            sb.Append(@"    <td>%%couriername%%</td>");
            sb.Append(@"    <td style=""text-align:right"">No of Files:</td>");
            sb.Append(@"    <td>%%nrofapplicants%%</td>");
            sb.Append(@"  </tr>");
            //  for QR barcode: CLM Unique Code | BRM File no  | ID no   | Name and Surname | Grant Type | Application Date
            sb.Append(@"  <tr>");
            sb.Append(@"    <td colspan=""6""> ");
            sb.Append(@"      <section class=""contact"">");
            sb.Append(@"        <div>");
            sb.Append(@"          <table cellspacing = ""0"" cellpadding=""2"" id=""MainContent_batchGridView"" style=""color:#333333;width:100%;border-collapse:collapse;height: 100%; width: 100%;"">");
            sb.Append(@"            <tr style=""color:White;background-color:#507CD1;font-weight:bold;"">");
            sb.Append(@"              <th scope=""col"">CLM Unique Code</th>");
            sb.Append(@"              <th scope=""col"">BRM File No</th>");
            sb.Append(@"              <th scope=""col"">ID #</th>");
            sb.Append(@"              <th scope=""col"">Name and Surname</th>");
            sb.Append(@"              <th scope=""col"">Grant Type</th>");
            sb.Append(@"              <th scope = ""col"" >Application Date</ th >");
            sb.Append(@"              <th scope = ""col"" >Registry Type</ th >");
            sb.Append(@"              <th scope = ""col"" > </ th >");
            sb.Append(@"            </tr>");
            sb.Append(@"%%trData%%");
            sb.Append(@"          </table>");
            sb.Append(@"        </div>");
            sb.Append(@"      </section>");
            sb.Append(@"    </td>");
            sb.Append(@"  </tr>");
            sb.Append(@"</table>");

            sb.Append(@"<p style='page-break-after:always'/>");

            string batchNo = Request.QueryString["batchNo"].ToString();
            decimal outValue = 0.0M;
            if (Decimal.TryParse(batchNo, out outValue))
            {
                var z = UserSession.Office.OfficeId;
                var x = en.DC_BATCH
                    .Where(bn => bn.BATCH_NO == outValue)
                    .Where(oid => oid.OFFICE_ID == z)
                    .OrderBy(bn => bn.BATCH_NO)
                    .Select(
                       bn => new BatchEntity
                       {
                           BATCH_NO = bn.BATCH_NO,
                           OFFICE_NAME = bn.DC_LOCAL_OFFICE.OFFICE_NAME,
                           UPDATED_BY = bn.UPDATED_BY,
                           UPDATED_DATE = bn.UPDATED_DATE,
                           BATCH_CURRENT = bn.BATCH_CURRENT,
                           BATCH_STATUS = bn.BATCH_STATUS,
                           BATCH_COMMENT = bn.BATCH_COMMENT,
                           WAYBILL_NO = bn.WAYBILL_NO,
                           COURIER_NAME = bn.COURIER_NAME,
                           WAYBILL_DATE = bn.WAYBILL_DATE
                       }).FirstOrDefault();

                if (x != null)
                {
                    html = sb.ToString();
                    html = html.Replace("%%batchno%%", batchNo);
                    html = html.Replace("%%waybillno%%", x.WAYBILL_NO);
                    html = html.Replace("%%localoffice%%", x.OFFICE_NAME);
                    html = html.Replace("%%couriername%%", x.COURIER_NAME);
                }
            }

            return html;
        }

        private void DoMethod()
        {
            //btnPrintClose.Visible = false;

            string batchNo = string.Empty;
            if (Request.QueryString.Count > 0)
            {
                batchNo = Request.QueryString["batchNo"].ToString();
                cMyValue = Request.QueryString["batchNo"].ToString();
                //if (cMyValue.Length == 1)
                //{
                //    cMyValue = "00" + cMyValue;
                //}
                //if (cMyValue.Length == 2)
                //{
                //    cMyValue = "0" + cMyValue;
                //}
                barcode.Alt = cMyValue;

                txtBatchNo.Text = cMyValue;
            }

            batchGridView.SelectMethod = "GetAllFilesByBatchNo";

            decimal outValue = 0.0M;

            try
            {
                if (Decimal.TryParse(batchNo, out outValue))
                {
                    var z = UserSession.Office.OfficeId;
                    var x = en.DC_BATCH
                        .Where(bn => bn.BATCH_NO == outValue)
                        .Where(oid => oid.OFFICE_ID == z)
                        .OrderBy(bn => bn.BATCH_NO)
                        .Select(
                           bn => new BatchEntity
                           {
                               BATCH_NO = bn.BATCH_NO,
                               OFFICE_NAME = bn.DC_LOCAL_OFFICE.OFFICE_NAME,
                               UPDATED_BY = bn.UPDATED_BY,
                               UPDATED_DATE = bn.UPDATED_DATE,
                               BATCH_CURRENT = bn.BATCH_CURRENT,
                               BATCH_STATUS = bn.BATCH_STATUS,
                               BATCH_COMMENT = bn.BATCH_COMMENT,
                               WAYBILL_NO = bn.WAYBILL_NO,
                               COURIER_NAME = bn.COURIER_NAME,
                               WAYBILL_DATE = bn.WAYBILL_DATE
                           }).FirstOrDefault();

                    if (x != null)
                    {
                        txtWaybillNo.Text = x.WAYBILL_NO;
                        txtLocalOffice.Text = x.OFFICE_NAME;
                        txtCourierName.Text = x.COURIER_NAME;
                        cBatchPrintedAlready = x.BATCH_CURRENT.ToLower().Trim() == "y" ? true.ToString().ToLower() : false.ToString().ToLower();
                    }
                }
                divError.Visible = false;
            }
            catch (Exception ex)
            {
                lblError.Text = ex.Message.ToString();
                lblError.Text += " - ";
                lblError.Text += ex.InnerException;
                divError.Visible = true;
            }
            //showHideViewLink();

            cPrintDiv = Build();
        }

        private System.Web.UI.WebControls.Image doQRCode(string BRMBarCode, string CLMno, string pensionNo, string fullname, string grantname)
        {
            string barcodestring = BRMBarCode + "\t" + CLMno + "\t" + pensionNo + "\t" + fullname + "\t" + grantname;
            string code = barcodestring;

            QRCodeGenerator qrGenerator = new QRCodeGenerator();
            QRCode qrCode = new QRCode(qrGenerator.CreateQrCode(code, QRCodeGenerator.ECCLevel.Q));
            System.Web.UI.WebControls.Image imgBarCode = new System.Web.UI.WebControls.Image();
            imgBarCode.Height = 100;
            imgBarCode.Width = 100;
            using (Bitmap bitMap = qrCode.GetGraphic(20))
            {
                using (MemoryStream ms = new MemoryStream())
                {
                    bitMap.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
                    byte[] byteImage = ms.ToArray();
                    imgBarCode.ImageUrl = "data:image/png;base64," + Convert.ToBase64String(byteImage);
                }
                //plBarCode.Controls.Add(imgBarCode);
            }

            return imgBarCode;
        }

        #endregion Private Methods
    }
}