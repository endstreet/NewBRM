using QRCoder;
using SASSADirectCapture.BL;
using SASSADirectCapture.EntityModels;
using SASSADirectCapture.Sassa;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SASSADirectCapture.Views
{
    public partial class FileCover : SassaPage
    {
        #region Private Fields

        private string _schema = "";

        //private SASSA_Authentication authObject = new SASSA_Authentication();

        private Entities en = new Entities();

        #endregion Private Fields

        #region Private Properties

        private string schema
        {
            get
            {
                if (_schema == string.Empty)
                    _schema = System.Configuration.ConfigurationManager.AppSettings["schema"].ToString();
                return _schema;
            }
        }

        #endregion Private Properties

        #region Public Methods

        public string createBRMFile(string brmno, string granttype,string id)
        {
            string localOffice = Usersession.Office.OfficeId;
            string localRegion = Usersession.Office.RegionId;
            decimal batchNo = -1;
            string unqfileno = "";

            using (Entities context = new Entities())
            {
                //If there is no current batch for the local office, use the temporary batch for now.
                if (batchNo == -1)
                {
                    batchNo = 0;
                }

                DC_FILE f = en.DC_FILE.Where(k => k.BRM_BARCODE == brmno)
                    .FirstOrDefault();
                if (f == null)
                {
                    //DC_FILE file = new DC_FILE();
                    //file.BRM_BARCODE = brmno;
                    //file.BATCH_ADD_DATE = DateTime.Now;
                    //file.TRANS_TYPE = 0;
                    //file.BATCH_NO = batchNo;
                    //file.GRANT_TYPE = granttype;
                    //file.OFFICE_ID = localOffice;
                    //file.REGION_ID = localRegion;
                    //context.Configuration.AutoDetectChangesEnabled = false;
                    //context.Configuration.ValidateOnSaveEnabled = false;
                    //context.DC_FILE.Add(file);
                    //context.SaveChanges();
                    //unqfileno = file.UNQ_FILE_NO;

                    string currentDate = DateTime.Parse(DateTime.Now.ToString()).ToString("dd-MMM-yyyy", System.Globalization.CultureInfo.InvariantCulture);

                    string sql = @"INSERT INTO DC_FILE
                            (APPLICANT_NO, BRM_BARCODE, BATCH_ADD_DATE,TRANS_TYPE,BATCH_NO,GRANT_TYPE,OFFICE_ID,REGION_ID)
                            VALUES
                            ('" + id + "','" + brmno + "', '" + currentDate + "',0, " + batchNo + ",'" + granttype + "'," + localOffice + "," + localRegion + ")";

                    int noOfRowInserted = context.Database.ExecuteSqlCommand(sql);

                    DC_FILE dc = en.DC_FILE.Where(k => k.BRM_BARCODE == brmno)
                    .FirstOrDefault();
                    unqfileno = dc.UNQ_FILE_NO;
                }
                else { unqfileno = f.UNQ_FILE_NO; }
            }
            return unqfileno;
        }

        public void FindControlsRecursive(Control root, Type type, ref List<Control> list)
        {
            if (root.Controls.Count != 0)
            {
                foreach (Control c in root.Controls)
                {
                    if (c.GetType() == type)
                        list.Add(c);
                    else if (c.HasControls())
                        FindControlsRecursive(c, type, ref list);
                }
            }
        }

        #endregion Public Methods

        #region Protected Methods

        protected void BtnClose_Click(object sender, EventArgs e)
        {
            string BRMBarCode = Request.QueryString["BRM"] ?? Session["BRM"].ToString();

            foreach (var file in en.DC_FILE
                .Where(dc_file => dc_file.BRM_BARCODE == BRMBarCode
                    && dc_file.APPLICANT_NO == null
                    && dc_file.UPDATED_DATE == null)
                )
            {
                en.DC_FILE.Remove(file);
            }
            en.DC_ACTIVITY.Add(util.CreateActivity("Files", "Cover/Close (remove for barcode)"));
            en.SaveChanges();

            ClientScript.RegisterStartupScript(Page.GetType(), "fileCoverClose", "refocusWindowClose();", true);
        }

        protected void btnPrint_Click(object sender, EventArgs e)
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "hideLoadingMsg", "myECM.hidePleaseWait();", true);

            lblMsg.Text = string.Empty;
            divError.Visible = false;
            string batchno = string.Empty;
            string boxno = string.Empty;
            string unq_ref = string.Empty;

            //Check if application status is checked
            if (!checkApprove.Checked && !checkReject.Checked && !checkLC.Checked)
            {
                lblMsg.Text = "Mark the application as <b>Main</b>, <b>Archive</b> and/or <b>Loose Correspondence</b> before printing.";
                divError.Visible = true;
                return;
            }
            if (!checkApprove.Checked && !checkReject.Checked && checkLC.Checked)
            {
                lblMsg.Text = "Invalid selection : For Loose Correspondence, mark the application as <b>Main and Loose Correspondence</b> or <b>Archive and Loose Correspondence</b> before printing.";
                divError.Visible = true;
                return;
            }
            //Get all the checked critical documents
            string docsSelected = string.Empty;
            int checkedCount = 0;
            var checkboxes = new List<Control>();
            FindControlsRecursive(this.divCriticalDocs, typeof(CheckBox), ref checkboxes);

            foreach (Control c in checkboxes)
            {
                if (c is CheckBox)
                {
                    if (((CheckBox)c).Checked)
                    {
                        string docid = c.ID.Replace("chk_", "");
                        docsSelected += docid + ";";
                        checkedCount++;
                    }
                }
            }

            if (checkedCount == 0)
            {
                lblMsg.Text = "None of the documents were checked.  Please select all the appropriate documents";
                divError.Visible = true;
                return;
            }

            if (docsSelected != string.Empty)
            {
                docsSelected = docsSelected.TrimEnd(';');
            }

            //Upload this applicant file into DC_FILE for the current batch.
            if (txtProcess.Value == "batching")
            {
                ApplicantGrants fileprep = (ApplicantGrants)Session["applicantFile"];
                if (fileprep != null)
                {
                    try
                    {
                        using (Entities context = new Entities())
                        {
                            DC_FILE file = context.DC_FILE.Where(f => f.UNQ_FILE_NO == txtCLM.InnerText).FirstOrDefault();

                            string localOffice = Usersession.Office.OfficeId;
                            decimal batchNo = -1;

                            //If this is the Batching function, use the batching process
                            if (string.IsNullOrEmpty(fileprep.MIS_BOXNO))
                            {
                                //var authenticationMethod = bool.Parse(System.Web.Configuration.WebConfigurationManager.AppSettings["UseADAuth"].ToString());
                                var ADUser = Usersession.SamName;
                                //var CSUser = new SASSA_Authentication().getUserID();

                                string sRegType = "";
                                if (checkApprove.Checked)
                                {
                                    if (checkLC.Checked)
                                    {
                                        sRegType = "LC-MAIN";
                                    }
                                    else
                                    {
                                        sRegType = "MAIN";
                                    }
                                }
                                else if (checkReject.Checked)
                                {
                                    if (checkLC.Checked)
                                    {
                                        sRegType = "LC-ARCHIVE";
                                    }
                                    else
                                    {
                                        sRegType = "ARCHIVE";
                                    }
                                }

                                var query = from b in context.DC_BATCH
                                    .Where(bn => bn.BATCH_CURRENT.ToLower().Trim() == "y")
                                    .Where(oid => oid.OFFICE_ID == localOffice)
                                    .Where(bs => bs.BATCH_STATUS.ToUpper().Trim() != "BULK")
                                    .Where(batch => true
                                        ? batch.UPDATED_BY_AD == ADUser
                                        : batch.UPDATED_BY == 0)
                                    .Where(rt => rt.REG_TYPE == sRegType)
                                            select b;
                                try
                                {
                                    foreach (DC_BATCH s in query)
                                    {
                                        batchNo = s.BATCH_NO;
                                        break;
                                    }
                                }
                                catch (Exception ex)
                                {
                                    lblMsg.Text = "There was a problem retrieving the current batch: " + ex.Message;
                                    //lblMsg.Text += " InnerException:" + ex.InnerException;
                                    divError.Visible = true;
                                }

                                //Use the temp batch
                                if (Request.QueryString["tempBatch"] != null && Request.QueryString["tempBatch"].ToString() == "Y")
                                {
                                    batchNo = 0;
                                }
                                else if (batchNo == 0)
                                {
                                    //Should be moved from temporary batch into a proper batch
                                    batchNo = -1;
                                }

                                //If there is no current batch for the local office, create one.
                                if (batchNo == -1)
                                {
                                    batchNo = CreateBatchForOffice(sRegType);
                                }

                                if (batchNo > -1)
                                {
                                    file.BATCH_NO = batchNo;
                                    file.BATCH_ADD_DATE = DateTime.Now;
                                    file.TRANS_TYPE = 0;
                                }
                            }

                            file.APPLICANT_NO = fileprep.APPLICANT_NO;
                            file.DOCS_PRESENT = docsSelected;
                            file.GRANT_TYPE = fileprep.GRANT_TYPE;
                            file.REGION_ID = fileprep.REGION_ID;
                            file.OFFICE_ID = localOffice; // Get office id from logged-in user.
                            file.UPDATED_DATE = DateTime.Now;
                            file.USER_FIRSTNAME = fileprep.FIRSTNAME;
                            file.USER_LASTNAME = fileprep.LASTNAME;
                            file.APPLICATION_STATUS = getCheckboxStatus();
                            file.BRM_BARCODE = txtBRM.InnerText;
                            file.TRANS_DATE = fileprep.APP_DATE == null ? null : (DateTime?)DateTime.ParseExact(fileprep.APP_DATE.Replace("/", "").Trim(), "yyyyMMdd", null);
                            file.UNQ_FILE_NO = txtCLM.InnerText;
                            file.SRD_NO = Request.QueryString["SRDNo"];
                            file.ARCHIVE_YEAR = (fileprep.STATUS_DATE == null || fileprep.STATUS_DATE == "") ? "" : fileprep.STATUS_DATE.Substring(0, 4);
                            file.CHILD_ID_NO = Request.QueryString["ChildID"];
                            file.ISREVIEW = checkReview.Checked ? "Y" : "N";
                            file.LASTREVIEWDATE = hfReviewDate.Value == "" ? null : (DateTime?)DateTime.Parse(hfReviewDate.Value);
                            file.LCTYPE = hfLCType.Value == "" ? null : (Decimal?)Decimal.Parse(hfLCType.Value);

                            file.UPDATED_BY_AD = Usersession.SamName;


                            divFileUnqBarcode.Alt = file.UNQ_FILE_NO;
                            unq_ref = file.UNQ_FILE_NO;
                            batchno = file.BATCH_NO.ToString();
                            boxno = file.MIS_BOXNO;

                            context.DC_FILE.Add(file);
                            context.DC_ACTIVITY.Add(util.CreateActivity("Files", "Print Cover/Close"));
                            context.SaveChanges();
                        }
                    }
                    catch (DbEntityValidationException ex)
                    {
                        // Retrieve the error messages as a list of strings.
                        var errorMessages = ex.EntityValidationErrors
                                .SelectMany(x => x.ValidationErrors)
                                .Select(x => x.ErrorMessage);

                        // Join the list to a single string.
                        var fullErrorMessage = string.Join("; " + System.Environment.NewLine, errorMessages);

                        if (fullErrorMessage.Length < 1)
                        {
                            fullErrorMessage = ex.ToString();
                        }

                        lblMsg.Text = fullErrorMessage;
                        divError.Visible = true;
                    }
                    catch (Exception ex)
                    {
                        lblMsg.Text = ex.ToString();
                        divError.Visible = true;
                    }
                }
            }
            else
            {
                Applicant applicant = (Applicant)Session["applicantFile"];
                if (applicant != null)
                {
                    try
                    {
                        using (Entities context = new Entities())
                        {
                            DC_FILE file = new DC_FILE();
                            string localOffice = Usersession.Office.OfficeId;
                            decimal batchNo = -1;

                            //If this is not the Box Audit function, use the batching process
                            if (string.IsNullOrEmpty(applicant.MIS_BOXNO))
                            {
                                //var authenticationMethod = bool.Parse(System.Web.Configuration.WebConfigurationManager.AppSettings["UseADAuth"].ToString());
                                var ADUser = Usersession.SamName;
                                //var CSUser = new SASSA_Authentication().getUserID();

                                string sRegType = "";
                                if (checkApprove.Checked)
                                {
                                    if (checkLC.Checked)
                                    {
                                        sRegType = "LC-MAIN";
                                    }
                                    else
                                    {
                                        sRegType = "MAIN";
                                    }
                                }
                                else if (checkReject.Checked)
                                {
                                    if (checkLC.Checked)
                                    {
                                        sRegType = "LC-ARCHIVE";
                                    }
                                    else
                                    {
                                        sRegType = "ARCHIVE";
                                    }
                                }

                                var query = from b in context.DC_BATCH
                                    .Where(bn => bn.BATCH_CURRENT.ToLower().Trim() == "y")
                                    .Where(oid => oid.OFFICE_ID == localOffice)
                                    .Where(bs => bs.BATCH_STATUS.ToUpper().Trim() != "BULK")
                                    .Where(batch => true
                                        ? batch.UPDATED_BY_AD == ADUser
                                        : batch.UPDATED_BY == 0)
                                    .Where(rt => rt.REG_TYPE == sRegType)
                                            select b;
                                try
                                {
                                    foreach (DC_BATCH s in query)
                                    {
                                        batchNo = s.BATCH_NO;
                                        break;
                                    }
                                }
                                catch (Exception ex)
                                {
                                    lblMsg.Text = "There was a problem retrieving the current batch: " + ex.Message;
                                    //lblMsg.Text += " InnerException:" + ex.InnerException;
                                    divError.Visible = true;
                                }

                                //Use the temp batch
                                if (Request.QueryString["tempBatch"] != null && Request.QueryString["tempBatch"].ToString() == "Y")
                                {
                                    batchNo = 0;
                                }
                                else if (batchNo == 0)
                                {
                                    //Should be moved from temporary batch into a proper batch
                                    batchNo = -1;
                                }

                                //If there is no current batch for the local office, create one.
                                if (batchNo == -1)
                                {
                                    batchNo = CreateBatchForOffice(sRegType);
                                }

                                if (batchNo > -1)
                                {
                                    file.BATCH_NO = batchNo;
                                    file.BATCH_ADD_DATE = DateTime.Now;
                                    file.TRANS_TYPE = 0;
                                }
                            }
                            else
                            {
                                //First check if the file already exists in DC_FILE (if user updates missing checkbox first, this might happen)
                                var x = context.DC_FILE
                                        .Where(f => f.APPLICANT_NO == applicant.APPLICANT_NO && f.MIS_BOXNO == applicant.MIS_BOXNO && f.GRANT_TYPE == applicant.GRANT_TYPE1).FirstOrDefault();

                                if (x != null)
                                {
                                    file.FILE_STATUS = x.FILE_STATUS; //Get the file status first before removing the old record.
                                    file.TRANS_TYPE = 0; //x.TRANS_TYPE.HasValue ? x.TRANS_TYPE : applicant.TRANS_TYPE; //Get the transaction type first before removing the old record.
                                    file.TRANS_DATE = x.TRANS_DATE;
                                    context.DC_FILE.Remove(x);
                                }
                                else
                                {
                                    file.TRANS_TYPE = 0; // applicant.TRANS_TYPE; //Get the transaction type first before removing the old record.
                                }

                                if (!file.TRANS_DATE.HasValue && !string.IsNullOrEmpty(applicant.TRANS_DATE))
                                {
                                    string mydate = applicant.TRANS_DATE;
                                    int mypos = mydate.IndexOf(" ");
                                    if (mypos > 0)
                                    {
                                        mydate = mydate.Substring(0, mypos);
                                        file.TRANS_DATE = DateTime.ParseExact(mydate, "yyyy-MM-dd", null);
                                    }
                                }

                                file.MIS_BOXNO = applicant.MIS_BOXNO;
                                file.MIS_BOX_DATE = applicant.MIS_BOX_DATE;
                            }

                            file.APPLICANT_NO = applicant.APPLICANT_NO;
                            file.DOCS_PRESENT = docsSelected;
                            file.GRANT_TYPE = applicant.GRANT_TYPE1;
                            file.REGION_ID = applicant.REGION_ID;
                            file.OFFICE_ID = localOffice; // Get office id from logged-in user.
                            file.UPDATED_DATE = DateTime.Now;
                            file.USER_FIRSTNAME = applicant.FIRSTNAME;
                            file.USER_LASTNAME = applicant.LASTNAME;
                            file.APPLICATION_STATUS = getCheckboxStatus();
                            file.BRM_BARCODE = txtBRM.InnerText;
                            file.UNQ_FILE_NO = txtCLM.InnerText;
                            file.CHILD_ID_NO = Request.QueryString["ChildID"];
                            file.ISREVIEW = checkReview.Checked ? "Y" : "N";
                            file.LASTREVIEWDATE = hfReviewDate.Value == "" ? null : (DateTime?)DateTime.Parse(hfReviewDate.Value);
                            file.LCTYPE = hfLCType.Value == "" ? null : (Decimal?)Decimal.Parse(hfLCType.Value);


                            file.UPDATED_BY_AD = Usersession.SamName;


                            divFileUnqBarcode.Alt = file.UNQ_FILE_NO;
                            unq_ref = file.UNQ_FILE_NO;
                            batchno = file.BATCH_NO.ToString();
                            boxno = file.MIS_BOXNO;

                            context.DC_FILE.Add(file);
                            en.DC_ACTIVITY.Add(util.CreateActivity("Files", "Print Add File"));
                            context.SaveChanges();
                        }
                    }
                    catch (DbEntityValidationException ex)
                    {
                        // Retrieve the error messages as a list of strings.
                        var errorMessages = ex.EntityValidationErrors
                                .SelectMany(x => x.ValidationErrors)
                                .Select(x => x.ErrorMessage);

                        // Join the list to a single string.
                        var fullErrorMessage = string.Join("; " + System.Environment.NewLine, errorMessages);

                        if (fullErrorMessage.Length < 1)
                        {
                            //fullErrorMessage = ex.Message;
                            //fullErrorMessage += " InnerException:" + ex.InnerException;
                            fullErrorMessage = ex.ToString();
                        }

                        lblMsg.Text = fullErrorMessage;
                        divError.Visible = true;
                    }
                    catch (Exception ex)
                    {
                        //lblMsg.Text = ex.Message;
                        //lblMsg.Text += " InnerException:" + ex.InnerException;
                        lblMsg.Text = ex.ToString();
                        divError.Visible = true;
                    }
                }
            }

            //If successful with everything - print page.
            if (lblMsg.Text == string.Empty)
            {
                divError.Visible = false;
                divSuccess.Visible = true;
                if (string.IsNullOrEmpty(boxno))
                {
                    if (Request.QueryString["tempBatch"] != null && Request.QueryString["tempBatch"].ToString() == "Y")
                    {
                        lblSuccess.Text = "The unique CLM file number <b>" + unq_ref + "</b> has been generated successfully.";
                    }
                    else
                    {
                        lblSuccess.Text = "File added to batch <b>" + batchno + "</b> successfully. The unique file number is <b>" + unq_ref + "</b>";
                    }
                }
                else
                {
                    lblSuccess.Text = "MIS File audited successfully. The unique file number is <b>" + unq_ref + "</b>";
                }

                ScriptManager.RegisterStartupScript(this, GetType(), "print2", " try { window.opener.updateGrid(); } catch (err) { } ", true);
                ScriptManager.RegisterStartupScript(this, GetType(), "print3", " window.print(); ", true);
            }
        }

        protected string getCheckboxStatus()
        {
            string status = string.Empty;

            if (checkApprove.Checked)
            {
                if (checkLC.Checked)
                {
                    status = "LC-MAIN";
                }
                else
                {
                    status = "MAIN";
                }
            }
            else if (checkReject.Checked)
            {
                if (checkLC.Checked)
                {
                    status = "LC-ARCHIVE";
                }
                else
                {
                    status = "ARCHIVE";
                }
            }
            return status;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            //Get the username of the user that is logged in from session.
            string authenticatedUsername = Usersession.SamName;


            if (!IsPostBack)
            {
                string sMGMerge = Request.QueryString["MGMerge"]; //Multi-grant Merge
                hfIsMGMerge.Value = (!string.IsNullOrEmpty(sMGMerge) && sMGMerge.ToUpper() == "Y") ? "True" : "False";
            }

            populateDataforSheet();
        }

        #endregion Protected Methods

        #region Private Methods

        private decimal CreateBatchForOffice(string sRegType)
        {
            string sLocalOfficeID = Usersession.Office.OfficeId;

            DC_BATCH b = new DC_BATCH();
            b.BATCH_STATUS = "Open";
            b.BATCH_CURRENT = "Y";
            b.OFFICE_ID = sLocalOfficeID;
            b.UPDATED_DATE = DateTime.Now;

            if (bool.Parse(System.Web.Configuration.WebConfigurationManager.AppSettings["UseADAuth"].ToString()))
            {
                b.UPDATED_BY_AD = Usersession.SamName;
            }
            //else
            //{
            //    b.UPDATED_BY = new SASSA_Authentication().getUserID();
            //    b.UPDATED_BY_AD = util.getUserFullName(b.UPDATED_BY.ToString());
            //}

            b.REG_TYPE = sRegType;

            en.DC_BATCH.Add(b);
            en.DC_ACTIVITY.Add(util.CreateActivity("Files", "Add Open Batch"));
            en.SaveChanges();

            decimal batchNo = -1;
            using (Entities context = new Entities())
            {
                //var authenticationMethod = bool.Parse(System.Web.Configuration.WebConfigurationManager.AppSettings["UseADAuth"].ToString());
                var ADUser = Usersession.SamName;
                //var CSUser = Usersession.AdName;
                var query = from btc in context.DC_BATCH
                                    .Where(bn => bn.BATCH_CURRENT.ToLower().Trim() == "y")
                                    .Where(oid => oid.OFFICE_ID == sLocalOfficeID)
                                    .Where(bs => bs.BATCH_STATUS.ToUpper().Trim() != "BULK")
                                    .Where(batch => true
                                        ? batch.UPDATED_BY_AD == ADUser
                                        : batch.UPDATED_BY == 0)
                                    .Where(rt => rt.REG_TYPE == sRegType)
                            select btc;
                try
                {
                    foreach (DC_BATCH s in query)
                    {
                        batchNo = s.BATCH_NO;
                        break;
                    }
                }
                catch (Exception ex)
                {
                    lblMsg.Text = "There was a problem retrieving the current batch: " + ex.Message;
                    //lblMsg.Text += " InnerException:" + ex.InnerException;
                    divError.Visible = true;
                }
            }

            return batchNo;
        }

        private void doQRCode(string BRMBarCode, string CLMno, string pensionNo, string fullname, string grantname)
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
                plBarCode.Controls.Add(imgBarCode);
            }
        }

        private Applicant getApplicantDataMIS(string idNo, string MISBoxNo, bool exists)
        {
            Applicant applicant = new Applicant();

            string myRegionID = Usersession.Office.RegionId;

            Dictionary<string, string> grantTypes = util.getGrantTypes();

            try
            {
                using (Entities context = new Entities())
                {
                    if (!exists)
                    {
                        IEnumerable<Applicant> query = context.Database.SqlQuery<Applicant>
                                                   (@"select mis.ID_NUMBER as APPLICANT_NO,
                                                               mis.NAME as FIRSTNAME,
                                                               mis.SURNAME as LASTNAME,
                                                               mis.GRANT_TYPE as GRANT_TYPE1,
                                                               gt.TYPE_NAME as GRANT_NAME,
                                                               rg.REGION_ID,
                                                               rg.REGION_CODE as REGION_CODE,
                                                               rg.REGION_NAME as REGION_NAME,
                                                               mis.APP_DATE as TRANS_DATE,
                                                               spn.DOCS_PRESENT as DOCS_PRESENT
                                                        from MISNATIONALs mis
                                                        left outer join SOCPENBRM spn on mis.ID_NUMBER in (spn.PENSION_NO, spn.OLD_ID1, spn.OLD_ID2, spn.OLD_ID3, spn.OLD_ID4, spn.OLD_ID5, spn.OLD_ID6, spn.OLD_ID7, spn.OLD_ID8, spn.OLD_ID9, spn.OLD_ID10)
                                                        inner join DC_REGION rg on mis.REGION_ID = rg.REGION_ID
                                                        left outer join DC_GRANT_TYPE gt on gt.TYPE_ID = mis.GRANT_TYPE
                                                        where mis.ID_NUMBER = '" + idNo + "' and mis.BOX_NUMBER = '" + MISBoxNo + "' and mis.REGION_ID = '" + myRegionID + "'");
                        //left outer join SOCPENBRM spn on spn.PENSION_NO = mis.ID_NUMBER

                        foreach (Applicant value in query)
                        {
                            applicant = value;
                            applicant.GRANT_TYPE1 = BoxFileActions.convertMISGrantTypeToSocpen(value.GRANT_TYPE1);

                            // MIS Grant types are not the same as the SOCPEN grant types we are storing - need to remap.
                            string grantName = "";
                            if (grantTypes.TryGetValue(applicant.GRANT_TYPE1, out grantName))
                            {
                                applicant.GRANT_NAME = grantName;
                            }

                            break;
                        }
                    }
                    //If this file was requested as part of a reprint, get record from DC_FILE and not MISNATIONAL
                    else
                    {
                        var query = from file in context.DC_FILE
                                    join grt in context.DC_GRANT_TYPE on file.GRANT_TYPE equals grt.TYPE_ID
                                    join region in context.DC_REGION on file.REGION_ID equals region.REGION_ID
                                    where file.APPLICANT_NO == idNo //This pension no is actually the unq code
                                    && file.MIS_BOXNO == MISBoxNo
                                    && file.REGION_ID == myRegionID

                                    select new Applicant
                                    {
                                        UNQ_FILE_NO = file.UNQ_FILE_NO,
                                        APPLICANT_NO = file.APPLICANT_NO,
                                        FIRSTNAME = file.USER_FIRSTNAME,
                                        LASTNAME = file.USER_LASTNAME,
                                        GRANT_TYPE1 = file.GRANT_TYPE,
                                        GRANT_NAME = grt.TYPE_NAME,
                                        REGION_ID = region.REGION_ID,
                                        REGION_CODE = region.REGION_CODE,
                                        REGION_NAME = region.REGION_NAME,
                                        TRANS_DATE_DATE = file.TRANS_DATE, //TRANS_DATE_DATE = file.TRANS_DATE,
                                        DOCS_PRESENT = file.DOCS_PRESENT,
                                        APPLICATION_STATUS = file.APPLICATION_STATUS,
                                        TRANS_TYPE = 0,
                                        //TRANS_NAME = trans.TYPE_NAME,
                                        BRM_BARCODE = file.BRM_BARCODE,
                                        MIS_BOX_DATE = file.MIS_BOX_DATE
                                    };

                        foreach (Applicant value in query)
                        {
                            applicant = value;
                            break;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                lblMsg.Text = ex.Message;
                divError.Visible = true;
                return null;
            }

            return applicant;
        }

        private Applicant getApplicantDataSOCPEN(string pensionNo, string newFileAdded)
        {
            Applicant applicant = new Applicant();
            try
            {
                using (Entities context = new Entities())
                {
                    //If this file was added to a new batch
                    if (newFileAdded == "Y")
                    {
                        IEnumerable<Applicant> query = context.Database.SqlQuery<Applicant>
                                                     (@"select spn.PENSION_NO as APPLICANT_NO,
                                                               spn.NAME as FIRSTNAME,
                                                               spn.SURNAME as LASTNAME,
                                                               spn.GRANT_TYPE1 as GRANT_TYPE1,
                                                               spn.GRANT_TYPE2 as GRANT_TYPE2,
                                                               spn.GRANT_TYPE3 as GRANT_TYPE3,
                                                               spn.GRANT_TYPE4 as GRANT_TYPE4,
                                                               spn.APP_DATE1 as APP_DATE1,
                                                               spn.APP_DATE2 as APP_DATE2,
                                                               spn.APP_DATE3 as APP_DATE3,
                                                               spn.APP_DATE4 as APP_DATE4,
                                                               rg.REGION_ID as REGION_ID,
                                                               rg.REGION_CODE as REGION_CODE,
                                                               rg.REGION_NAME as REGION_NAME,
                                                               spn.APP_DATE1 as TRANS_DATE,
                                                               spn.DOCS_PRESENT as DOCS_PRESENT
                                                        from SOCPENBRM spn
                                                        inner join DC_REGION rg on spn.PROVINCE = rg.REGION_ID
                                                        where spn.PENSION_NO =  '" + pensionNo + "' -- in (spn.PENSION_NO, spn.OLD_ID1, spn.OLD_ID2, spn.OLD_ID3, spn.OLD_ID4, spn.OLD_ID5, spn.OLD_ID6, spn.OLD_ID7, spn.OLD_ID8, spn.OLD_ID9, spn.OLD_ID10)");

                        foreach (Applicant value in query)
                        {
                            applicant = value;
                            break;
                        }
                    }
                    //If this file was requested as part of a reprint, get record from audit and not SOCPEN
                    else
                    {
                        var query = from file in context.DC_FILE
                                    join grt in context.DC_GRANT_TYPE on file.GRANT_TYPE equals grt.TYPE_ID
                                    join region in context.DC_REGION on file.REGION_ID equals region.REGION_ID
                                    where file.UNQ_FILE_NO == pensionNo //This pension no is actually the unq code

                                    select new Applicant
                                    {
                                        UNQ_FILE_NO = file.UNQ_FILE_NO,
                                        APPLICANT_NO = file.APPLICANT_NO,
                                        FIRSTNAME = file.USER_FIRSTNAME,
                                        LASTNAME = file.USER_LASTNAME,
                                        GRANT_TYPE1 = file.GRANT_TYPE,
                                        GRANT_NAME = grt.TYPE_NAME,
                                        REGION_ID = region.REGION_ID,
                                        REGION_CODE = region.REGION_CODE,
                                        REGION_NAME = region.REGION_NAME,
                                        TRANS_DATE_DATE = file.TRANS_DATE, //TRANS_DATE_DATE = file.TRANS_DATE,
                                        DOCS_PRESENT = file.DOCS_PRESENT,
                                        APPLICATION_STATUS = file.APPLICATION_STATUS,
                                        TRANS_TYPE = 0,
                                        BRM_BARCODE = file.BRM_BARCODE
                                    };

                        foreach (Applicant value in query)
                        {
                            applicant = value;
                            break;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                lblMsg.Text = ex.Message;
                divError.Visible = true;
                return null;
            }

            return applicant;
        }

        private ApplicantGrants getFilePrepDataSOCPEN(string pensionNo, string granttype, string appdate, string newFileAdded, string CLMno, string SRDNo, string childID)
        {
            string appdatedigits = appdate.Replace("/", "");

            ApplicantGrants FilePrepData = new ApplicantGrants();
            try
            {
                using (Entities context = new Entities())
                {
                    //If this file was added to a new batch
                    if (newFileAdded == "Y")
                    {
                        if (granttype != "S")
                        {
                            IEnumerable<ApplicantGrants> query = context.Database.SqlQuery<ApplicantGrants>
                                                            (@"select spn.PENSION_NO as APPLICANT_NO,
                                                                spn.NAME as FIRSTNAME,
                                                                spn.SURNAME as LASTNAME,
                                                                spn.GRANT_TYPE,
                                                                spn.APP_DATE,
                                                                rg.REGION_ID as REGION_ID,
                                                                rg.REGION_CODE as REGION_CODE,
                                                                rg.REGION_NAME as REGION_NAME,
                                                                spn.APP_DATE as TRANS_DATE,
                                                                spn.DOCS_PRESENT as DOCS_PRESENT,
                                                                spn.STATUS_DATE as STATUS_DATE,
                                                                spn.PRIM_STATUS as PRIM_STATUS,
                                                                spn.SEC_STATUS as SEC_STATUS,
                                                                c.APPLICATION_DATE as C_APPLICATION_DATE,
                                                                c.STATUS_CODE as C_STATUS_CODE,
                                                                c.STATUS_DATE as C_STATUS_DATE,
                                                                r.DATE_REVIEWED as LASTREVIEWDATE
                                                        from SOCPENGRANTS spn
                                                        inner join DC_REGION rg on spn.PROVINCE = rg.REGION_ID
                                                        left join SASSA.SOCPEN_P12_CHILDREN c on spn.GRANT_TYPE in ('C', '5') and c.PENSION_NO = spn.PENSION_NO and c.GRANT_TYPE = spn.GRANT_TYPE and to_char(c.ID_NO) = '" + childID + @"'
                                                        left join SASSA.SOCPEN_REVIEW r on r.PENSION_NO = spn.PENSION_NO
                                                        where spn.PENSION_NO = '" + pensionNo + @"' -- in (spn.PENSION_NO, spn.OLD_ID1, spn.OLD_ID2, spn.OLD_ID3, spn.OLD_ID4, spn.OLD_ID5, spn.OLD_ID6, spn.OLD_ID7, spn.OLD_ID8, spn.OLD_ID9, spn.OLD_ID10)
                                                        and spn.GRANT_TYPE = '" + granttype + @"'
                                                        " + (appdatedigits == "" ? "and " + ((granttype == "C" || granttype == "5") ? "c.APPLICATION_DATE" : "spn.APP_DATE") + " is null" : "and " + ((granttype == "C" || granttype == "5") ? "((c.APPLICATION_DATE is null and spn.APP_DATE = '" + appdatedigits + "') or c.APPLICATION_DATE = TO_DATE('" + appdate + "', 'YYYY/MM/DD'))" : "spn.APP_DATE = '" + appdatedigits + "'")) + @"
                                                        order by NVL(c.APPLICATION_DATE, to_date(spn.APP_DATE, 'YYYYMMDD')) desc");

                            foreach (ApplicantGrants value in query)
                            {
                                FilePrepData = value;
                                break;
                            }
                        }
                        else
                        {
                            IEnumerable<ApplicantGrants> query = context.Database.SqlQuery<ApplicantGrants>
                                                        (@"select cast(srdben.ID_NO as varchar2(13)) as APPLICANT_NO,
                                                                  srdben.NAME as FIRSTNAME,
                                                                  srdben.SURNAME as LASTNAME,
                                                                  'S' as GRANT_TYPE,
                                                                  to_char(srdtype.APPLICATION_DATE, 'YYYY/MM/DD') as APP_DATE,
                                                                  rg.REGION_ID as REGION_ID,
                                                                  rg.REGION_CODE as REGION_CODE,
                                                                  rg.REGION_NAME as REGION_NAME,
                                                                  srdtype.APPLICATION_STATUS as APPLICATION_STATUS,
                                                                  srdtype.DATE_APPROVED as DATE_APPROVED,
                                                                  r.DATE_REVIEWED as LASTREVIEWDATE
                                                            from SASSA.SOCPEN_SRD_BEN srdben
                                                            left join SASSA.SOCPEN_SRD_TYPE srdtype on srdtype.SOCIAL_RELIEF_NO = srdben.SRD_NO
                                                            inner join DC_REGION rg on rg.REGION_ID = cast(srdben.PROVINCE as NUMBER)
                                                            left join SASSA.SOCPEN_REVIEW r on r.PENSION_NO = srdben.ID_NO
                                                            where cast(srdben.SRD_NO as NUMBER) = cast('" + SRDNo + @"' as NUMBER)
                                                            order by srdtype.APPLICATION_DATE desc");
                            //to_char(srdtype.APPLICATION_DATE, 'YYYY/MM/DD') as TRANS_DATE,

                            foreach (ApplicantGrants value in query)
                            {
                                FilePrepData = value;
                                break;
                            }
                        }
                    }
                    //If this file was requested as part of a reprint, get record from audit and not SOCPEN
                    else
                    {
                        var query = from file in context.DC_FILE
                                    join grt in context.DC_GRANT_TYPE on file.GRANT_TYPE equals grt.TYPE_ID
                                    join region in context.DC_REGION on file.REGION_ID equals region.REGION_ID
                                    where file.UNQ_FILE_NO == CLMno

                                    select new ApplicantGrants
                                    {
                                        UNQ_FILE_NO = file.UNQ_FILE_NO,
                                        APPLICANT_NO = file.APPLICANT_NO,
                                        FIRSTNAME = file.USER_FIRSTNAME,
                                        LASTNAME = file.USER_LASTNAME,
                                        GRANT_TYPE = file.GRANT_TYPE,
                                        GRANT_NAME = grt.TYPE_NAME,
                                        REGION_ID = region.REGION_ID,
                                        REGION_CODE = region.REGION_CODE,
                                        REGION_NAME = region.REGION_NAME,
                                        TRANS_DATE_DATE = file.TRANS_DATE,
                                        DOCS_PRESENT = file.DOCS_PRESENT,
                                        APPLICATION_STATUS = file.APPLICATION_STATUS,
                                        STATUS_DATE = file.ARCHIVE_YEAR,
                                        TRANS_TYPE = 0,
                                        BRM_BARCODE = file.BRM_BARCODE,
                                        ISREVIEW = file.ISREVIEW,
                                        LASTREVIEWDATE = file.LASTREVIEWDATE,
                                        LCTYPE = file.LCTYPE
                                    };

                        foreach (ApplicantGrants value in query)
                        {
                            FilePrepData = value;
                            break;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                //lblMsg.Text = ex.Message + " Innerexception: " + ex.InnerException;
                lblMsg.Text = ex.ToString();
                divError.Visible = true;
                return null;
            }

            return FilePrepData;
        }

        private List<RequiredDocs> getGrantDocuments(string grantType)
        {
            List<RequiredDocs> docs = new List<RequiredDocs>();

            try
            {
                using (Entities context = new Entities())
                {
                    var query = (from reqDocGrant in context.DC_GRANT_DOC_LINK
                                 join reqDoc in context.DC_DOCUMENT_TYPE on reqDocGrant.DOCUMENT_ID equals reqDoc.TYPE_ID
                                 where reqDocGrant.GRANT_ID == grantType && reqDocGrant.CRITICAL_FLAG == "Y"
                                 orderby reqDocGrant.SECTION, reqDoc.TYPE_ID ascending

                                 select new RequiredDocs
                                 {
                                     DOC_ID = reqDoc.TYPE_ID,
                                     DOC_NAME = reqDoc.TYPE_NAME,
                                     DOC_SECTION = reqDocGrant.SECTION,
                                     DOC_CRITICAL = reqDocGrant.CRITICAL_FLAG
                                 }).Distinct();

                    foreach (RequiredDocs value in query)
                    {
                        docs.Add(value);
                    }
                }
            }
            catch (Exception ex)
            {
                lblMsg.Text = ex.Message;
                divError.Visible = true;
                return null;
            }

            return docs;
        }

        private string GetLatestGrantType(Applicant applicant)
        {
            //SET THE CORRECT GRANT TYPE BEFORE STORING RECORD
            //WORK OUT THE CORRECT GRANT TYPE TO USE BASED ON THE RUBBISH DATA IN SOCPENID.
            int latestAppDate = 0;
            string latestGrantType = string.Empty;

            if (applicant.GRANT_TYPE1 != null && applicant.GRANT_TYPE1.Trim() != string.Empty)
            {
                latestGrantType = applicant.GRANT_TYPE1.Trim();
            }
            if (applicant.GRANT_TYPE2 != null && applicant.GRANT_TYPE2.Trim() != string.Empty)
            {
                latestGrantType = applicant.GRANT_TYPE2.Trim();
            }
            if (applicant.GRANT_TYPE3 != null && applicant.GRANT_TYPE3.Trim() != string.Empty)
            {
                latestGrantType = applicant.GRANT_TYPE3.Trim();
            }
            if (applicant.GRANT_TYPE4 != null && applicant.GRANT_TYPE4.Trim() != string.Empty)
            {
                latestGrantType = applicant.GRANT_TYPE4.Trim();
            }

            try
            {
                //Check if APP_DATE4 is the latest
                if (applicant.APP_DATE4 != null && applicant.APP_DATE4 != string.Empty
                    && applicant.GRANT_TYPE4 != null && applicant.GRANT_TYPE4.Trim() != string.Empty)
                {
                    int.TryParse(applicant.APP_DATE4, out latestAppDate);
                    latestGrantType = applicant.GRANT_TYPE4;
                }

                //Check if APP_DATE3 is the latest
                if (applicant.APP_DATE3 != null && applicant.APP_DATE3 != string.Empty
                    && applicant.GRANT_TYPE3 != null && applicant.GRANT_TYPE3.Trim() != string.Empty)
                {
                    if (int.Parse(applicant.APP_DATE3) > latestAppDate)
                    {
                        latestAppDate = int.Parse(applicant.APP_DATE3);
                        latestGrantType = applicant.GRANT_TYPE3;
                    }
                }

                //Check if APP_DATE2 is the latest
                if (applicant.APP_DATE2 != null && applicant.APP_DATE2 != string.Empty
                    && applicant.GRANT_TYPE2 != null && applicant.GRANT_TYPE2.Trim() != string.Empty)
                {
                    if (int.Parse(applicant.APP_DATE2) > latestAppDate)
                    {
                        latestAppDate = int.Parse(applicant.APP_DATE2);
                        latestGrantType = applicant.GRANT_TYPE2;
                    }
                }

                //Check if APP_DATE1 is the latest
                if (applicant.APP_DATE1 != null && applicant.APP_DATE1 != string.Empty
                    && applicant.GRANT_TYPE1 != null && applicant.GRANT_TYPE1.Trim() != string.Empty)
                {
                    if (int.Parse(applicant.APP_DATE1) > latestAppDate)
                    {
                        latestAppDate = int.Parse(applicant.APP_DATE1);
                        latestGrantType = applicant.GRANT_TYPE1;
                    }
                }
            }
            catch { }

            return latestGrantType;
        }

        private List<int> getSOCPENDocs(string idNo, string grantype)
        {
            try
            {
                using (Entities ent = new Entities())
                {
                    IEnumerable<int> query = ent.Database.SqlQuery<int>
                          (@"select DI.DOC_NO_IN AS DOC_NO_IN_IN
                                    from SASSA.SOCPEN_DOW_APPLICATIONS_CHEC01 DAC
                                    LEFT JOIN SASSA.SOCPEN_DOCUMENTS_IN DI ON DI.ADABAS_ISN = DAC.ADABAS_ISN
                                    LEFT JOIN SASSA.SOCPEN_DOC_REL_IN DRI ON DRI.ADABAS_ISN = DI.ADABAS_ISN AND DRI.DPS_PE_SEQ = DI.DPS_PE_SEQ
                                    where ID_NO = '" + idNo + @"'
                                    and GRANT_TYPE = '" + grantype + @"'
                                    and DPS_MU_SEQ = '001'
                                    and DOC_REL_IN = 'Y' ");

                    if (query.Any())
                    {
                        return query.ToList();
                    }
                    else
                    {
                        return new List<int>();
                    }
                }
            }
            catch (Exception)
            {
                return new List<int>();
            }
        }

        private string getTransActionName(int transactionType)
        {
            string transactionName = string.Empty;

            try
            {
                using (Entities context = new Entities())
                {
                    DC_TRANSACTION_TYPE trans = context.DC_TRANSACTION_TYPE.Find(transactionType);

                    if (trans != null)
                    {
                        transactionName = trans.TYPE_NAME;
                    }
                }
            }
            catch (Exception)
            {
            }

            return transactionName;
        }

        private void LoadCriticalDocumentSection(string grantype, string docsPresent)
        {
            //Get all the required documents for the applicant's grant type
            List<RequiredDocs> docs = getGrantDocuments(grantype);
            string[] docPresentArray = null;

            //Check if docs present is filled in on the socpen data.
            docPresentArray = docsPresent != null ? docsPresent.Split(';') : null;

            //Critical Documents
            //-------------------------------------------------------------------------
            Table tblGeneral = (Table)tblDocGeneral;
            Table tblIncome = (Table)tblDocIncome;
            Table tblAssets = (Table)tblDocAssets;
            TableRow rowParticular = new TableRow();
            TableRow rowIncome = new TableRow();
            TableRow rowAssets = new TableRow();

            if (docs != null)
            {
                foreach (RequiredDocs doc in docs)
                {
                    //Create Table cell
                    TableCell cell = new TableCell();
                    cell.Attributes.Add("style", "width:50%; padding-left:5px; vertical-align:top");
                    //Create textbox control
                    CheckBox chkbox = new CheckBox();
                    chkbox.ID = "chk_" + doc.DOC_ID;
                    chkbox.Text = doc.DOC_NAME;
                    chkbox.LabelAttributes.Add("class", "chkboxLabel");

                    //set checked state if id matches a document in docs-present array
                    if (docPresentArray != null && docPresentArray.Length > 0)
                    {
                        foreach (string val in docPresentArray)
                        {
                            try
                            {
                                if (doc.DOC_ID == decimal.Parse(val))
                                {
                                    chkbox.Checked = true;
                                }
                            }
                            catch { }
                        }
                    }

                    //add checkbox to cell
                    cell.Controls.Add(chkbox);

                    switch (doc.DOC_SECTION)
                    {
                        case "General Particulars":
                            //Only add 2 columns for each row
                            if (rowParticular.Cells.Count < 2)
                            {
                                rowParticular.Cells.Add(cell);
                            }
                            else
                            {
                                tblGeneral.Rows.Add(rowParticular);
                                rowParticular = new TableRow();
                                rowParticular.Cells.Add(cell);
                            }
                            break;

                        case "Particulars of Income":
                            //Only add 2 columns for each row
                            if (rowIncome.Cells.Count < 2)
                            {
                                rowIncome.Cells.Add(cell);
                            }
                            else
                            {
                                tblIncome.Rows.Add(rowIncome);
                                rowIncome = new TableRow();
                                rowIncome.Cells.Add(cell);
                            }
                            break;

                        case "Particulars of Assets":
                            //Only add 2 columns for each row
                            if (rowAssets.Cells.Count < 2)
                            {
                                rowAssets.Cells.Add(cell);
                            }
                            else
                            {
                                tblAssets.Rows.Add(rowAssets);
                                rowAssets = new TableRow();
                                rowAssets.Cells.Add(cell);
                            }
                            break;
                    }
                }
            }

            if (rowParticular.Cells.Count > 0)
            {
                tblGeneral.Rows.Add(rowParticular);
            }
            if (rowIncome.Cells.Count > 0)
            {
                tblIncome.Rows.Add(rowIncome);
            }
            if (rowAssets.Cells.Count > 0)
            {
                tblAssets.Rows.Add(rowAssets);
            }
        }

        private void LoadCriticalDocumentSocpen(string idNo, string grantype)
        {
            //Get all the required documents for the applicant's grant type
            List<RequiredDocs> docs = getGrantDocuments(grantype);
            List<int> docList = null;

            //Check if docs present is filled in on the socpen data.
            docList = getSOCPENDocs(idNo, grantype);

            //Critical Documents
            //-------------------------------------------------------------------------
            Table tblGeneral = (Table)tblDocGeneral;
            Table tblIncome = (Table)tblDocIncome;
            Table tblAssets = (Table)tblDocAssets;
            TableRow rowParticular = new TableRow();
            TableRow rowIncome = new TableRow();
            TableRow rowAssets = new TableRow();

            if (docs != null)
            {
                foreach (RequiredDocs doc in docs)
                {
                    //Create Table cell
                    TableCell cell = new TableCell();
                    cell.Attributes.Add("style", "width:50%; padding-left:5px; vertical-align:top");
                    //Create textbox control
                    CheckBox chkbox = new CheckBox();
                    chkbox.ID = "chk_" + doc.DOC_ID;
                    chkbox.Text = doc.DOC_NAME;
                    chkbox.LabelAttributes.Add("class", "chkboxLabel");

                    //set checked state if id matches a document in docs-present array
                    if (docList != null && docList.Count > 0)
                    {
                        foreach (int val in docList)
                        {
                            try
                            {
                                if (doc.DOC_ID == val)
                                {
                                    chkbox.Checked = true;
                                }
                            }
                            catch { }
                        }
                    }

                    //add checkbox to cell
                    cell.Controls.Add(chkbox);

                    switch (doc.DOC_SECTION)
                    {
                        case "General Particulars":
                            //Only add 2 columns for each row
                            if (rowParticular.Cells.Count < 2)
                            {
                                rowParticular.Cells.Add(cell);
                            }
                            else
                            {
                                tblGeneral.Rows.Add(rowParticular);
                                rowParticular = new TableRow();
                                rowParticular.Cells.Add(cell);
                            }
                            break;

                        case "Particulars of Income":
                            //Only add 2 columns for each row
                            if (rowIncome.Cells.Count < 2)
                            {
                                rowIncome.Cells.Add(cell);
                            }
                            else
                            {
                                tblIncome.Rows.Add(rowIncome);
                                rowIncome = new TableRow();
                                rowIncome.Cells.Add(cell);
                            }
                            break;

                        case "Particulars of Assets":
                            //Only add 2 columns for each row
                            if (rowAssets.Cells.Count < 2)
                            {
                                rowAssets.Cells.Add(cell);
                            }
                            else
                            {
                                tblAssets.Rows.Add(rowAssets);
                                rowAssets = new TableRow();
                                rowAssets.Cells.Add(cell);
                            }
                            break;
                    }
                }
            }

            if (rowParticular.Cells.Count > 0)
            {
                tblGeneral.Rows.Add(rowParticular);
            }
            if (rowIncome.Cells.Count > 0)
            {
                tblIncome.Rows.Add(rowIncome);
            }
            if (rowAssets.Cells.Count > 0)
            {
                tblAssets.Rows.Add(rowAssets);
            }
        }

        private void populateDataforSheet()
        {
            Session["applicantFile"] = null;
            if (Request.QueryString["BRM"] != null)
            {
                Session["BRM"] = Request.QueryString["BRM"];
            }
            if(Session["BRM"] == null)
            {
                lblMsg.Text = "No BRM cant continue.";
                divError.Visible = true;
                btnPrint.Visible = false;
                btnReprint.Visible = false;
                return;
            }
            string pensionNo = Request.QueryString["PensionNo"];
            string CLMno = Request.QueryString["CLM"];
            string addToBatch = Request.QueryString["Batching"];
            string boxAudit = Request.QueryString["boxaudit"];
            string addToBox = Request.QueryString["boxing"] != null ? Request.QueryString["boxing"] : "Y";
            string boxNo = Request.QueryString["boxNo"];
            //string transactionType = Request.QueryString["trans"] != null ? Request.QueryString["trans"] : "";
            string BRMBarCode = Session["BRM"].ToString();
            string grantname = Request.QueryString["gn"];
            string granttype = Request.QueryString["gt"];
            string appdate = Request.QueryString["appdate"];
            string SRDNo = Request.QueryString["SRDNo"];
            string sChildID = Request.QueryString["ChildID"];
            string sIsReview = Request.QueryString["IsReview"];
            string sLCType = Request.QueryString["LCType"];
            //hiddenBarcode.Value = BRMBarCode;
            txtBRM.InnerText = BRMBarCode;

            // check if a filecover for this BRM already exists
            bool thisBRMExists = false;
            if (!string.IsNullOrEmpty(BRMBarCode))
            {
                thisBRMExists = util.checkBRMExists(BRMBarCode);
                divBRMBarCode.Alt = BRMBarCode;
            }

            string unqno = "";
            if (!thisBRMExists)
            {
                unqno = createBRMFile(BRMBarCode, granttype,pensionNo);
            }
            else
            {
                unqno = util.getCLM(BRMBarCode);
            }

            txtCLM.InnerText = unqno;
            divFileUnqBarcode.Alt = unqno;
            CLMno = unqno;

            string fullname = "";
            if (granttype == "S")
            {
                divIDNoBarcode.Alt = SRDNo;
                fullname = util.getNameSurnameSRD(SRDNo);
                doQRCode(BRMBarCode, unqno, SRDNo, fullname, grantname);
            }
            else
            {
                divIDNoBarcode.Alt = "abc\tdef";//pensionNo;
                fullname = util.getNameSurname(pensionNo);
                doQRCode(BRMBarCode, unqno, pensionNo, fullname, grantname);
            }

            //REQ0148
            //if (Request.QueryString["tempBatch"] != null && Request.QueryString["tempBatch"].ToString() == "Y")
            //{
            //    btnPrint.Text = "Print";
            //}

            if ((!string.IsNullOrEmpty(pensionNo) || !string.IsNullOrEmpty(SRDNo)) &&
                (
                (!string.IsNullOrEmpty(addToBatch) /*&& (addToBatch.ToUpper().Contains("Y") || addToBatch.ToUpper().Contains("N"))*/)//Commented out reason: Will always be either "Y" or "N" if has value
                ||
                (!string.IsNullOrEmpty(boxAudit) && (boxAudit.ToUpper().Contains("Y")))
                ||
                (!string.IsNullOrEmpty(addToBatch) && addToBatch.ToUpper().Contains("N") && !string.IsNullOrEmpty(CLMno))
                ) &&
                !string.IsNullOrEmpty(BRMBarCode)
               )

            {
                //This bool will be used throughout to differentiate between the Applicant Search File process and the Box Audit Function
                //This way we can use the same FileCover sheet for 2 different processes.
                bool isBoxAuditProcess = boxAudit != null ? boxAudit.ToUpper().Contains("Y") : false;

                //If this is the normal Applicant Search function, get data from Socpen, check transaction type and get correct grant type.
                if (!isBoxAuditProcess)
                {
                    //  BATCHING =================================================
                    txtProcess.Value = "batching";
                    ApplicantGrants filePrep = null;
                    Dictionary<string, string> dictGrantTypes = util.getGrantTypes();

                    //Hide the appropriate print button
                    btnPrint.Visible = addToBatch.ToUpper() == "Y" ? true : false;
                    btnReprint.Visible = addToBatch.ToUpper() == "N" ? true : false;

                    //Get Applicant Details from SOCPENGRANTS
                    filePrep = getFilePrepDataSOCPEN(pensionNo, granttype, appdate, addToBatch.ToUpper(), CLMno, SRDNo, sChildID);
                    if (filePrep != null)
                    {
                        filePrep.TRANS_TYPE = 0;

                        divTransDate.InnerText = "Transaction Date: ";
                        if (granttype == "S")
                        {
                            //SRD doesn't have status date, so we use approval date instead
                            divTransDate.InnerText = "Approval Date: " + (filePrep.DATE_APPROVED == null ? "" : filePrep.DATE_APPROVED.ToString().Substring(0, 10));
                        }
                        else if (filePrep.C_STATUS_DATE != null)//C_APPLICATION_DATE != null)
                        {
                            divTransDate.InnerText += ((DateTime)filePrep.C_STATUS_DATE).ToString("yyyy/MM/dd");//C_APPLICATION_DATE.ToString();
                        }
                        else if (filePrep.STATUS_DATE != null)//TRANS_DATE != null)
                        {
                            string myYYYY = filePrep.STATUS_DATE.Substring(0, 4);//TRANS_DATE.Substring(0, 4);
                            if (filePrep.STATUS_DATE.Length > 4)
                            {
                                string myMM = filePrep.STATUS_DATE.Substring(4, 2);//TRANS_DATE.Substring(4, 2);
                                string myDD = filePrep.STATUS_DATE.Substring(6, 2);//TRANS_DATE.Substring(6, 2);
                                divTransDate.InnerText += myYYYY + "/" + myMM + "/" + myDD;
                            }
                            else
                            {
                                divTransDate.InnerText += myYYYY;
                            }
                        }
                        else
                        {
                            if (filePrep.TRANS_DATE_DATE == null)
                            {
                                divTransDate.InnerText += appdate;
                            }
                            else
                            {
                                divTransDate.InnerText += ((DateTime)filePrep.TRANS_DATE_DATE).ToString("yyyy/MM/dd");
                            }
                        }

                        if (grantname != null)
                        {
                            filePrep.GRANT_NAME = grantname;
                        }

                        string MYAPPLICANT = filePrep.APPLICANT_NO;
                    }

                    if (filePrep != null && (granttype == "S" || !string.IsNullOrEmpty(filePrep.APPLICANT_NO)))
                    {
                        //Set all the fields on the form
                        //Summary section details
                        //-------------------------------------------------------------------------
                        divUserName.InnerText = filePrep.FIRSTNAME + " " + filePrep.LASTNAME;
                        divSocpenRef.InnerText = granttype == "S" ? SRDNo : filePrep.APPLICANT_NO;
                        if (granttype == "S")
                        {
                            divIDContrainer.Style.Add("display", "block");
                            divIDNo.InnerText = filePrep.APPLICANT_NO;
                        }
                        else
                        {
                            divIDContrainer.Style.Add("display", "none");
                        }
                        divGrantName.InnerText = filePrep.GRANT_NAME;

                        divRegion.InnerText = filePrep.REGION_CODE + " - " + filePrep.REGION_NAME;
                        divIDNoBarcode.Alt = granttype == "S" ? SRDNo : filePrep.APPLICANT_NO;

                        if (BRMBarCode == null)
                        {
                            if (string.IsNullOrEmpty(txtBRM.InnerText))
                            {
                                if (string.IsNullOrEmpty(filePrep.BRM_BARCODE))
                                {
                                    Page.ClientScript.RegisterStartupScript(this.GetType(), "getBRMBarcodeAndReload", "getBRMBarcodeAndReload()", true);
                                }
                                else
                                {
                                    Session["BRM"] = filePrep.BRM_BARCODE.ToUpper();
                                    divBRMBarCode.Alt = filePrep.BRM_BARCODE.ToUpper();
                                    txtBRM.InnerText = filePrep.BRM_BARCODE.ToUpper();
                                    BRMBarCode = filePrep.BRM_BARCODE.ToUpper();
                                    Page.ClientScript.RegisterStartupScript(this.GetType(), "Reload", "location.reload();", true);
                                }
                            }
                            divBRMBarCode.Alt = BRMBarCode.ToUpper();
                            txtBRM.InnerText = BRMBarCode.ToUpper();

                            btnPrint.Visible = false;
                            btnReprint.Visible = false;
                        }
                        else
                        {
                            divBRMBarCode.Alt = BRMBarCode.ToUpper();
                            txtBRM.InnerText = BRMBarCode.ToUpper();
                        }

                        if (!string.IsNullOrEmpty(filePrep.UNQ_FILE_NO))
                        {
                            divFileUnqBarcode.Alt = filePrep.UNQ_FILE_NO;
                        }

                        //Check if application status is filled in and check relevant checkbox.
                        if (string.IsNullOrEmpty(filePrep.APPLICATION_STATUS))
                        {
                            string mystatus = "";

                            switch (filePrep.C_STATUS_CODE)
                            {
                                case null:
                                    string checkstatus = filePrep.PRIM_STATUS == null ? "" : filePrep.PRIM_STATUS.Trim() + (filePrep.SEC_STATUS == null ? "" : filePrep.SEC_STATUS.Trim());
                                    if (checkstatus == "B2" || checkstatus == "A2" || checkstatus == "92")
                                    {
                                        mystatus = "MAIN";
                                    }
                                    else
                                    {
                                        mystatus = "ARCHIVE";
                                    }
                                    break;

                                case "1":
                                    mystatus = "MAIN";
                                    break;

                                default:
                                    mystatus = "ARCHIVE";
                                    break;
                            }

                            switch (mystatus)
                            {
                                case "MAIN": checkApprove.Checked = true; break;
                                case "ARCHIVE":
                                    checkReject.Checked = true;
                                    divArchiveYear.InnerText = "0000";
                                    if (filePrep.GRANT_TYPE == "C")
                                    {
                                        var result = en.Database.SqlQuery<SOCPEN_CHILD_STATUS>($"SELECT CAST(PENSION_NO AS VARCHAR2(30)) AS PENSION_NO, LPAD(CAST(ID_NO AS VARCHAR2(30)), 13, '0') AS ID_NO, STATUS_CODE, STATUS_DATE FROM SASSA.SOCPEN_P12_CHILDREN WHERE PENSION_NO = {filePrep.APPLICANT_NO} AND STATUS_CODE = '2'");
                                        divArchiveYear.InnerText = result.FirstOrDefault()?.STATUS_DATE.ToString("yyyy-MM-dd").Substring(0, 4);
                                    }
                                    //divArchiveYear.InnerText = "Archive Year: " + ((filePrep.STATUS_DATE == null || filePrep.STATUS_DATE == "") ? "0000" : filePrep.STATUS_DATE.Substring(0, 4));
                                    break;
                            }
                        }
                        else
                        {
                            if (granttype == "S")
                            {
                                bool bStatus = filePrep.APPLICATION_STATUS == "2";
                                checkApprove.Checked = bStatus;
                                checkReject.Checked = !bStatus;
                            }
                            else
                            {
                                UpdateRegistryTypeCheckbox(filePrep.APPLICATION_STATUS);
                            }
                        }

                        if (checkReject.Checked)
                        {
                            string sTransDateText = divTransDate.InnerText.Substring(divTransDate.InnerText.IndexOf(":") + 2);
                            divArchiveYear.InnerText = (sTransDateText == "" ? "0000" : sTransDateText.Substring(0, 4));
                        }

                        //If this is via a reprint, get docs from our tables, else SOCPEN
                        if ((!isBoxAuditProcess && addToBatch.ToUpper() == "N") ||
                            (isBoxAuditProcess && addToBox.Trim().ToUpper() == "N"))
                        {
                            //Get all the required documents for the applicant's grant type and display in the 3 sections on the form
                            LoadCriticalDocumentSection(filePrep.GRANT_TYPE, filePrep.DOCS_PRESENT);
                        }
                        else
                        {
                            LoadCriticalDocumentSocpen(filePrep.APPLICANT_NO, filePrep.GRANT_TYPE);
                        }

                        //For Review checkbox, querystring values take prcedence if present
                        checkReview.Checked = (sIsReview == "Y" || filePrep.ISREVIEW == "Y");
                        hfReviewDate.Value = filePrep.LASTREVIEWDATE == null ? "" : ((DateTime)filePrep.LASTREVIEWDATE).ToString("yyyy/MM/dd").Substring(0, 10);
                        divReviewDate.InnerText = "Date Last Reviewed: " + hfReviewDate.Value;

                        //For LC checkbox, querystring values take prcedence if present
                        checkLC.Checked = ((sLCType != null && sLCType != "") || filePrep.LCTYPE != null);
                        hfLCType.Value = (sLCType != null && sLCType != "") ? sLCType : (filePrep.LCTYPE != null ? filePrep.LCTYPE.ToString() : "");
                        divLCType.InnerText = "LC Type: " + (hfLCType.Value == "" ? "" : util.getLCTypeDescription(Decimal.Parse(hfLCType.Value)));

                        if (checkLC.Checked)
                        {
                            divLCType.Style.Add("display", "block");
                        }
                        else
                        {
                            divLCType.Style.Add("display", "none");
                        }

                        Session["applicantFile"] = filePrep;
                    }
                    else
                    {
                        lblMsg.Text = "File data not found.";
                        divError.Visible = true;
                        btnPrint.Visible = false;
                        btnReprint.Visible = false;
                        return;
                    }
                }
                else
                {
                    //  BOX AUDIT =================================================
                    txtProcess.Value = "box audit";

                    Applicant applicant = null;
                    //int transType = 1;
                    Dictionary<string, string> dictGrantTypes = util.getGrantTypes();

                    //Hide the appropriate print button
                    btnPrint.Visible = addToBox.ToUpper() == "Y" ? true : false;
                    btnReprint.Visible = addToBox.ToUpper() == "N" ? true : false;
                    bool exists = false;

                    //If this is via a reprint, get data from dc_file
                    if (!string.IsNullOrEmpty(addToBox) && addToBox.Trim().ToUpper() == "N")
                    {
                        exists = true;
                    }

                    //Get Applicant Details from MISNATIONAL for the Box Audit Function
                    applicant = getApplicantDataMIS(pensionNo, boxNo, exists);
                    applicant.TRANS_TYPE = 0;

                    if ((divTransDate.InnerText == null) || (divTransDate.InnerText == ""))
                    {
                        string mydate = applicant.TRANS_DATE_DATE.HasValue ? applicant.TRANS_DATE_DATE.ToString() : string.IsNullOrEmpty(applicant.TRANS_DATE) ? "" : applicant.TRANS_DATE;
                        int mypos = mydate.IndexOf(" ");
                        if (mypos > 0)
                        {
                            mydate = mydate.Substring(0, mypos);
                        }
                        divTransDate.InnerText = "Transaction Date: " + mydate;
                    }

                    applicant.MIS_BOXNO = boxNo;
                    applicant.MIS_BOX_DATE = applicant.MIS_BOX_DATE.HasValue ? applicant.MIS_BOX_DATE : DateTime.Now;

                    //Continue the normal process for generating FileCover sheet.
                    if (applicant != null && !string.IsNullOrEmpty(applicant.APPLICANT_NO))
                    {
                        //Set all the fields on the form
                        //Summary section details
                        //-------------------------------------------------------------------------
                        divUserName.InnerText = applicant.FIRSTNAME + " " + applicant.LASTNAME;
                        divSocpenRef.InnerText = applicant.APPLICANT_NO;
                        //divIDNo.InnerText = applicant.APPLICANT_NO;
                        divGrantName.InnerText = applicant.GRANT_NAME;

                        divRegion.InnerText = applicant.REGION_CODE + " - " + applicant.REGION_NAME;
                        divIDNoBarcode.Alt = applicant.APPLICANT_NO;

                        if (BRMBarCode == null)
                        {
                            if (string.IsNullOrEmpty(txtBRM.InnerText))
                            {
                                if (string.IsNullOrEmpty(applicant.BRM_BARCODE))
                                {
                                    Page.ClientScript.RegisterStartupScript(this.GetType(), "getBRMBarcodeAndReload", "getBRMBarcodeAndReload()", true);
                                }
                                else
                                {
                                    divBRMBarCode.Alt = applicant.BRM_BARCODE.ToUpper();
                                    txtBRM.InnerText = applicant.BRM_BARCODE.ToUpper();
                                    BRMBarCode = applicant.BRM_BARCODE.ToUpper();
                                }
                            }
                            divBRMBarCode.Alt = BRMBarCode.ToUpper();
                            txtBRM.InnerText = BRMBarCode.ToUpper();

                            btnPrint.Visible = false;
                            btnReprint.Visible = false;
                        }
                        else
                        {
                            divBRMBarCode.Alt = BRMBarCode.ToUpper();
                            txtBRM.InnerText = BRMBarCode.ToUpper();
                        }

                        if (!string.IsNullOrEmpty(applicant.UNQ_FILE_NO))
                        {
                            divFileUnqBarcode.Alt = applicant.UNQ_FILE_NO;
                        }

                        //Check if application status is filled in and check relevant checkbox.
                        if (!string.IsNullOrEmpty(applicant.APPLICATION_STATUS))
                        {
                            UpdateRegistryTypeCheckbox(applicant.APPLICATION_STATUS);
                        }

                        //If this is via a reprint, get docs from our tables, else SOCPEN
                        if ((!isBoxAuditProcess && addToBatch.ToUpper() == "N") ||
                            (isBoxAuditProcess && addToBox.Trim().ToUpper() == "N"))
                        {
                            //Get all the required documents for the applicant's grant type and display in the 3 sections on the form
                            LoadCriticalDocumentSection(applicant.GRANT_TYPE1, applicant.DOCS_PRESENT);
                        }
                        else
                        {
                            LoadCriticalDocumentSocpen(applicant.APPLICANT_NO, applicant.GRANT_TYPE1);
                        }

                        Session["applicantFile"] = applicant;
                    }
                    else
                    {
                        lblMsg.Text = "File data not found.";
                        divError.Visible = true;
                        btnPrint.Visible = false;
                        btnReprint.Visible = false;
                        return;
                    }
                }
            }
            else
            {
                lblMsg.Text = "Invalid parameters provided.";
                divError.Visible = true;
                btnPrint.Visible = false;
                btnReprint.Visible = false;
                return;
            }
        }

        private void UpdateRegistryTypeCheckbox(string appStatus)
        {
            switch (appStatus)
            {
                case "MAIN":
                    checkApprove.Checked = true;
                    break;

                case "ARCHIVE":
                    checkReject.Checked = true;
                    break;

                case "LC-MAIN":
                    checkLC.Checked = true;
                    checkApprove.Checked = true;
                    break;

                case "LC-ARCHIVE":
                    checkLC.Checked = true;
                    checkReject.Checked = true;
                    break;
            }
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

        #endregion Private Methods
    }
}
