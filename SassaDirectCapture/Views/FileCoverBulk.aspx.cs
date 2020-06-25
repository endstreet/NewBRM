using QRCoder;
using SASSADirectCapture.EntityModels;
using SASSADirectCapture.Sassa;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SASSADirectCapture.Views
{
    public partial class FileCoverBulk : SassaPage
    {
        #region Private Fields

        private readonly Table tblDocAssets = null;

        private readonly Table tblDocGeneral = null;

        private readonly Table tblDocIncome = null;

        //private SASSA_Authentication authObject = new SASSA_Authentication();

        private Entities en = new Entities();

        #endregion Private Fields

        #region Public Methods

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

        protected void btnPrint_Click(object sender, EventArgs e)
        {
            //ScriptManager.RegisterStartupScript(this, GetType(), "hideLoadingMsg", "myECM.hidePleaseWait();", true);

            ////lblMsg.Text = string.Empty;
            ////divError.Visible = false;
            //string batchno = string.Empty;
            //string boxno = string.Empty;
            //string unq_ref = string.Empty;

            ////Check if application status is checked
            //if (!chkREGTYPE_MAIN.Checked && !chkREGTYPE_ARCHIVE.Checked && !chkREGTYPE_LC.Checked)
            //{
            //    lblMsg.Text = "Mark the application as <b>Main</b>, <b>Archive</b> and/or <b>Loose Correspondence</b> before printing.";
            //    divError.Visible = true;
            //    return;
            //}
            //if (!chkREGTYPE_MAIN.Checked && !chkREGTYPE_ARCHIVE.Checked && chkREGTYPE_LC.Checked)
            //{
            //    lblMsg.Text = "Invalid selection : For Loose Correspondence, mark the application as <b>Main and Loose Correspondence</b> or <b>Archive and Loose Correspondence</b> before printing.";
            //    divError.Visible = true;
            //    return;
            //}
            ////Get all the checked critical documents
            //string docsSelected = string.Empty;
            //int checkedCount = 0;
            //var checkboxes = new List<Control>();
            //FindControlsRecursive(this.divCriticalDocs, typeof(CheckBox), ref checkboxes);

            //foreach (Control c in checkboxes)
            //{
            //    if (c is CheckBox)
            //    {
            //        if (((CheckBox)c).Checked)
            //        {
            //            string docid = c.ID.Replace("chk_", "");
            //            docsSelected += docid + ";";
            //            checkedCount++;
            //        }
            //    }
            //}

            //if (checkedCount == 0)
            //{
            //    lblMsg.Text = "None of the documents were checked.  Please select all the appropriate documents";
            //    divError.Visible = true;
            //    return;
            //}

            //if (docsSelected != string.Empty)
            //{
            //    docsSelected = docsSelected.TrimEnd(';');
            //}

            ////Upload this applicant file into DC_FILE for the current batch.
            //if (txtProcess.Value == "batching")
            //{
            //    ApplicantGrants fileprep = (ApplicantGrants)Session["applicantFile"];
            //    if (fileprep != null)
            //    {
            //        try
            //        {
            //            using (Entities context = new Entities())
            //            {
            //                DC_FILE file = new DC_FILE();
            //                string localOffice = UserSession.Office.OfficeId;
            //                decimal batchNo = -1;

            //                //If this is the Batching function, use the batching process
            //                if (string.IsNullOrEmpty(fileprep.MIS_BOXNO))
            //                {
            //                    var query = from b in context.DC_BATCH
            //                        .Where(bn => bn.BATCH_CURRENT.ToLower().Trim() == "y")
            //                        .Where(oid => oid.OFFICE_ID == localOffice)
            //                                select b;
            //                    try
            //                    {
            //                        foreach (DC_BATCH s in query)
            //                        {
            //                            batchNo = s.BATCH_NO;
            //                            break;
            //                        }
            //                    }
            //                    catch (Exception ex)
            //                    {
            //                        lblMsg.Text = "There was a problem retrieving the current batch: " + ex.Message;
            //                        //lblMsg.Text += " InnerException:" + ex.InnerException;
            //                        divError.Visible = true;
            //                    }

            //                    //If there is no current batch for the local office, create one.
            //                    if (batchNo == -1)
            //                    {
            //                        batchNo = CreateBatchForOffice();
            //                    }

            //                    if (batchNo > -1)
            //                    {
            //                        file.BATCH_NO = batchNo;
            //                        file.BATCH_ADD_DATE = DateTime.Now;
            //                        file.TRANS_TYPE = 0;
            //                    }
            //                }

            //                file.APPLICANT_NO = fileprep.APPLICANT_NO;
            //                file.DOCS_PRESENT = docsSelected;
            //                file.GRANT_TYPE = fileprep.GRANT_TYPE;
            //                file.REGION_ID = fileprep.REGION_ID;
            //                file.OFFICE_ID = localOffice; // Get office id from logged-in user.
            //                file.UPDATED_DATE = DateTime.Now;
            //                file.UPDATED_BY = authObject.getUserID();
            //                file.USER_FIRSTNAME = fileprep.FIRSTNAME;
            //                file.USER_LASTNAME = fileprep.LASTNAME;
            //                file.APPLICATION_STATUS = getCheckboxStatus();
            //                file.BRM_BARCODE = txtBRM.InnerText;
            //                file.TRANS_DATE = DateTime.ParseExact(fileprep.APP_DATE.Trim(), "yyyyMMdd", null);

            //                context.DC_FILE.Add(file);

            //                context.SaveChanges();

            //                divFileUnqBarcode.Alt = file.UNQ_FILE_NO;
            //                unq_ref = file.UNQ_FILE_NO;
            //                batchno = file.BATCH_NO.ToString();
            //                boxno = file.MIS_BOXNO;

            //            }
            //        }
            //        catch (DbEntityValidationException ex)
            //        {
            //            // Retrieve the error messages as a list of strings.
            //            var errorMessages = ex.EntityValidationErrors
            //                    .SelectMany(x => x.ValidationErrors)
            //                    .Select(x => x.ErrorMessage);

            //            // Join the list to a single string.
            //            var fullErrorMessage = string.Join("; " + System.Environment.NewLine, errorMessages);

            //            if (fullErrorMessage.Length < 1)
            //            {
            //                fullErrorMessage = ex.Message;
            //                fullErrorMessage += " InnerException:" + ex.InnerException;
            //            }

            //            lblMsg.Text = fullErrorMessage;
            //            divError.Visible = true;

            //        }
            //        catch (Exception ex)
            //        {
            //            lblMsg.Text = ex.Message;
            //            lblMsg.Text += " InnerException:" + ex.InnerException;
            //            divError.Visible = true;
            //        }
            //    }
            //}
            //else
            //{
            //    Applicant applicant = (Applicant)Session["applicantFile"];
            //    if (applicant != null)
            //    {
            //        try
            //        {
            //            using (Entities context = new Entities())
            //            {
            //                DC_FILE file = new DC_FILE();
            //                string localOffice = UserSession.Office.OfficeId;
            //                decimal batchNo = -1;

            //                //If this is not the Box Audit function, use the batching process
            //                if (string.IsNullOrEmpty(applicant.MIS_BOXNO))
            //                {
            //                    var query = from b in context.DC_BATCH
            //                        .Where(bn => bn.BATCH_CURRENT.ToLower().Trim() == "y")
            //                        .Where(oid => oid.OFFICE_ID == localOffice)
            //                                select b;
            //                    try
            //                    {
            //                        foreach (DC_BATCH s in query)
            //                        {
            //                            batchNo = s.BATCH_NO;
            //                            break;
            //                        }
            //                    }
            //                    catch (Exception ex)
            //                    {
            //                        lblMsg.Text = "There was a problem retrieving the current batch: " + ex.Message;
            //                        //lblMsg.Text += " InnerException:" + ex.InnerException;
            //                        divError.Visible = true;
            //                    }

            //                    //If there is no current batch for the local office, create one.
            //                    if (batchNo == -1)
            //                    {
            //                        batchNo = CreateBatchForOffice();
            //                    }

            //                    if (batchNo > -1)
            //                    {
            //                        file.BATCH_NO = batchNo;
            //                        file.BATCH_ADD_DATE = DateTime.Now;
            //                        file.TRANS_TYPE = 0;
            //                    }
            //                }
            //                else
            //                {
            //                    //First check if the file already exists in DC_FILE (if user updates missing checkbox first, this might happen)
            //                    var x = context.DC_FILE
            //                            .Where(f => f.APPLICANT_NO == applicant.APPLICANT_NO && f.MIS_BOXNO == applicant.MIS_BOXNO && f.GRANT_TYPE == applicant.GRANT_TYPE1).FirstOrDefault();

            //                    if (x != null)
            //                    {
            //                        file.FILE_STATUS = x.FILE_STATUS; //Get the file status first before removing the old record.
            //                        file.TRANS_TYPE = 0; //x.TRANS_TYPE.HasValue ? x.TRANS_TYPE : applicant.TRANS_TYPE; //Get the transaction type first before removing the old record.
            //                        file.TRANS_DATE = x.TRANS_DATE;
            //                        context.DC_FILE.Remove(x);
            //                    }
            //                    else
            //                    {
            //                        file.TRANS_TYPE = 0; // applicant.TRANS_TYPE; //Get the transaction type first before removing the old record.
            //                    }

            //                    if (!file.TRANS_DATE.HasValue && !string.IsNullOrEmpty(applicant.TRANS_DATE))
            //                    {
            //                        string mydate = applicant.TRANS_DATE;
            //                        int mypos = mydate.IndexOf(" ");
            //                        if (mypos > 0)
            //                        {
            //                            mydate = mydate.Substring(0, mypos);
            //                            file.TRANS_DATE = DateTime.ParseExact(mydate, "yyyy-MM-dd", null);
            //                        }
            //                    }

            //                    file.MIS_BOXNO = applicant.MIS_BOXNO;
            //                    file.MIS_BOX_DATE = applicant.MIS_BOX_DATE;

            //                }

            //                file.APPLICANT_NO = applicant.APPLICANT_NO;
            //                file.DOCS_PRESENT = docsSelected;
            //                file.GRANT_TYPE = applicant.GRANT_TYPE1;
            //                file.REGION_ID = applicant.REGION_ID;
            //                file.OFFICE_ID = localOffice; // Get office id from logged-in user.
            //                file.UPDATED_DATE = DateTime.Now;
            //                file.UPDATED_BY = authObject.getUserID();
            //                file.USER_FIRSTNAME = applicant.FIRSTNAME;
            //                file.USER_LASTNAME = applicant.LASTNAME;
            //                file.APPLICATION_STATUS = getCheckboxStatus();
            //                file.BRM_BARCODE = txtBRM.InnerText;
            //                //file.TRANS_DATE = DateTime.ParseExact(applicant.TRANS_DATE.Trim(), "yyyy-MM-dd", null); TODO

            //                context.DC_FILE.Add(file);

            //                context.SaveChanges();

            //                divFileUnqBarcode.Alt = file.UNQ_FILE_NO;
            //                unq_ref = file.UNQ_FILE_NO;
            //                batchno = file.BATCH_NO.ToString();
            //                boxno = file.MIS_BOXNO;

            //            }
            //        }
            //        catch (DbEntityValidationException ex)
            //        {
            //            // Retrieve the error messages as a list of strings.
            //            var errorMessages = ex.EntityValidationErrors
            //                    .SelectMany(x => x.ValidationErrors)
            //                    .Select(x => x.ErrorMessage);

            //            // Join the list to a single string.
            //            var fullErrorMessage = string.Join("; " + System.Environment.NewLine, errorMessages);

            //            if (fullErrorMessage.Length < 1)
            //            {
            //                fullErrorMessage = ex.Message;
            //                fullErrorMessage += " InnerException:" + ex.InnerException;
            //            }

            //            lblMsg.Text = fullErrorMessage;
            //            divError.Visible = true;

            //        }
            //        catch (Exception ex)
            //        {
            //            lblMsg.Text = ex.Message;
            //            lblMsg.Text += " InnerException:" + ex.InnerException;
            //            divError.Visible = true;
            //        }
            //    }
            //}

            ////If successful with everything - print page.
            //if (lblMsg.Text == string.Empty)
            //{
            //    divError.Visible = false;
            //    divSuccess.Visible = true;
            //    if (string.IsNullOrEmpty(boxno))
            //    {
            //        lblSuccess.Text = "File added to batch <b>" + batchno + "</b> successfully. The unique file number is <b>" + unq_ref + "</b>";
            //    }
            //    else
            //    {
            //        lblSuccess.Text = "MIS File audited successfully. The unique file number is <b>" + unq_ref + "</b>";
            //    }

            //    ScriptManager.RegisterStartupScript(this, GetType(), "print", "printpage('Y'); try { window.opener.updateGrid(); } catch (err) { } ", true);
            //}
        }

        protected string getCheckboxStatus()
        {
            string status = string.Empty;

            //if (chkREGTYPE_MAIN.Checked)
            //{
            //    if (chkREGTYPE_LC.Checked)
            //    {
            //        status = "LC-MAIN";
            //    }
            //    else
            //    {
            //        status = "MAIN";
            //    }
            //}
            //else if (chkREGTYPE_ARCHIVE.Checked)
            //{
            //    if (chkREGTYPE_LC.Checked)
            //    {
            //        status = "LC-ARCHIVE";
            //    }
            //    else
            //    {
            //        status = "ARCHIVE";
            //    }
            //}
            return status;
        }

        protected void GetFileData(string batch, string boxNo, string sTDWBoxNo)
        {
            int batchNo = int.Parse(batch);
            string myRegionID = UserSession.Office.RegionId;

            //divError.Visible = false;
            //lblMsg.Text = string.Empty;

            // DATA REQUIRED FOR LISTVIEW:
            // DO NOT SELECT THE FILES WITH STATUS "DESTROY"

            //MIS File Number
            //Approved - Main
            //Rejected – Archive

            //Non - Compliant
            //Transfer
            //Legal
            //Fraud
            //Debtors

            //User Name & Surname
            //SocPen REF no
            //ID No
            //Grant Name
            //Region
            //Application Date

            //divIDNoBarcode
            //divFileUnqBarcode

            //------
            //General Particulars
            //Particulars of Income
            //Particulars of Assets
            //------

            Dictionary<string, string> grantTypes = util.getGrantTypes();

            List<MISBoxFiles> boxFiles = new List<MISBoxFiles>();

            using (Entities context = new Entities())
            {
                try
                {
                    //IEnumerable<MISBoxFiles> query = context.Database.SqlQuery<MISBoxFiles>
                    IEnumerable<MISBoxFiles> query;
                    if (string.IsNullOrEmpty(sTDWBoxNo))
                    {
                        if (myRegionID == "2")//Eastern Cape
                        {
                            query = context.Database.SqlQuery<MISBoxFiles>
                            (@"select DISTINCT
                                        mis.POSITN as POSITION,
                                        f.UNQ_FILE_NO,
                                        f.FILE_NUMBER,
                                        f.BRM_BARCODE as BRM_NO,
                                        f.APPLICATION_STATUS as REGISTRY_TYPE,
                                        f.ARCHIVE_YEAR,
                                        f.NON_COMPLIANT,
                                        f.TRANSFERRED,
                                        f.EXCLUSIONS,
                                        f.USER_FIRSTNAME AS NAME,
                                        f.USER_LASTNAME AS SURNAME,
                                        f.APPLICANT_NO AS ID_NUMBER,
                                        f.GRANT_TYPE,
                                        r.REGION_NAME,
                                        g.TYPE_NAME AS GRANT_NAME,
                                        f.FILE_STATUS,
                                        f.TRANSFERRED,
                                        f.DOCS_PRESENT,
                                        f.TEMP_BOX_NO,
                                        f.PRINT_ORDER
                                from SS_APPLICATION mis
                                left outer join DC_FILE f on f.FILE_NUMBER = TRIM(mis.FORM_TYPE)||TRIM(mis.FORM_NUMBER)
                                inner join DC_REGION r on '2' = r.REGION_ID
                                left outer join DC_GRANT_TYPE g on g.TYPE_ID = F.GRANT_TYPE
                                where f.MIS_BOXNO = '" + boxNo + "' and f.BATCH_NO = '" + batchNo + "' and f.APPLICATION_STATUS != 'DESTROY' and f.MISSING != 'Y' and f.GRANT_TYPE != '4'"
                                    //+ " order by cast(NVL(REPLACE(mis.POSITN, 'NULL', '0'), '0') as number), cast(f.APPLICANT_NO as number) ");
                                    + " order by f.PRINT_ORDER ");
                            //from DC_FILE f
                            //inner join DC_REGION r on f.REGION_ID = r.REGION_ID
                            //left outer join DC_GRANT_TYPE g on g.TYPE_ID = F.GRANT_TYPE
                            //where f.MIS_BOXNO = '" + boxNo + "' and f.BATCH_NO = '" + batchNo + "' and f.APPLICATION_STATUS != 'DESTROY' and f.MISSING != 'Y' ");
                        }
                        else
                        {
                            query = context.Database.SqlQuery<MISBoxFiles>
                            (@"select DISTINCT
                                        mis.POSITION,
                                        f.UNQ_FILE_NO,
                                        f.FILE_NUMBER,
                                        f.BRM_BARCODE as BRM_NO,
                                        f.APPLICATION_STATUS as REGISTRY_TYPE,
                                        f.ARCHIVE_YEAR,
                                        f.NON_COMPLIANT,
                                        f.TRANSFERRED,
                                        f.EXCLUSIONS,
                                        f.USER_FIRSTNAME AS NAME,
                                        f.USER_LASTNAME AS SURNAME,
                                        f.APPLICANT_NO AS ID_NUMBER,
                                        f.GRANT_TYPE,
                                        r.REGION_NAME,
                                        g.TYPE_NAME AS GRANT_NAME,
                                        f.FILE_STATUS,
                                        f.TRANSFERRED,
                                        f.DOCS_PRESENT,
                                        f.TEMP_BOX_NO,
                                        f.PRINT_ORDER
                                from MIS_LIVELINK_TBL mis
                                left outer join DC_FILE f on f.FILE_NUMBER = mis.FILE_NUMBER
                                inner join DC_REGION r on f.REGION_ID = r.REGION_ID
                                left outer join DC_GRANT_TYPE g on g.TYPE_ID = F.GRANT_TYPE
                                where f.MIS_BOXNO = '" + boxNo + "' and f.BATCH_NO = '" + batchNo + "' and f.APPLICATION_STATUS != 'DESTROY' and f.MISSING != 'Y' "
                                    //+ " order by cast(mis.POSITION as number), cast(f.APPLICANT_NO as number) ");
                                    + " order by f.PRINT_ORDER ");
                        }
                    }
                    else
                    {
                        query = context.Database.SqlQuery<MISBoxFiles>
                            (@"select DISTINCT
                                        f.UNQ_FILE_NO,
                                        f.FILE_NUMBER,
                                        f.BRM_BARCODE as BRM_NO,
                                        f.APPLICATION_STATUS as REGISTRY_TYPE,
                                        f.ARCHIVE_YEAR,
                                        f.NON_COMPLIANT,
                                        f.TRANSFERRED,
                                        f.EXCLUSIONS,
                                        f.USER_FIRSTNAME AS NAME,
                                        f.USER_LASTNAME AS SURNAME,
                                        f.APPLICANT_NO AS ID_NUMBER,
                                        f.GRANT_TYPE,
                                        r.REGION_NAME,
                                        g.TYPE_NAME AS GRANT_NAME,
                                        f.FILE_STATUS,
                                        f.TRANSFERRED,
                                        f.DOCS_PRESENT,
                                        f.TEMP_BOX_NO,
                                        f.PRINT_ORDER
                                from DC_FILE f
                                inner join DC_REGION r on f.REGION_ID = r.REGION_ID
                                left outer join DC_GRANT_TYPE g on g.TYPE_ID = F.GRANT_TYPE
                                where f.TDW_BOXNO = '" + sTDWBoxNo + "'"
                                + " order by f.PRINT_ORDER ");
                    }

                    //if (query.Any())
                    //{
                    //instead of just returning the listview, go through each record and 'clean' it before displaying/using it.

                    foreach (MISBoxFiles value in query)
                    {
                        if (value.REGISTRY_TYPE == "MAIN") { value.CHECK_REGTYPE_MAIN = true; } else { value.CHECK_REGTYPE_MAIN = false; }
                        if (value.REGISTRY_TYPE == "ARCHIVE") { value.CHECK_REGTYPE_ARCHIVE = true; } else { value.CHECK_REGTYPE_ARCHIVE = false; }
                        if (value.NON_COMPLIANT == "Y") { value.CHECK_STATUS_NON_COMPLIANT = value.CHECK_NON_COMPLIANT = true; } else { value.CHECK_STATUS_NON_COMPLIANT = value.CHECK_NON_COMPLIANT = false; }
                        if (value.TRANSFERRED == "Y") { value.CHECK_STATUS_TRANSFERRED = value.CHECK_TRANSFERRED = true; } else { value.CHECK_STATUS_TRANSFERRED = value.CHECK_TRANSFERRED = false; }
                        if (value.EXCLUSIONS == "Legal") { value.CHECK_STATUS_LEGAL = true; } else { value.CHECK_STATUS_LEGAL = false; }
                        if (value.EXCLUSIONS == "Fraud") { value.CHECK_STATUS_FRAUD = true; } else { value.CHECK_STATUS_FRAUD = false; }
                        if (value.EXCLUSIONS == "Debtors") { value.CHECK_STATUS_DEBTORS = true; } else { value.CHECK_STATUS_DEBTORS = false; }
                        if (value.ARCHIVE_YEAR != null) { value.ARCHIVE_YEAR = "(" + value.ARCHIVE_YEAR + ")"; }

                        switch (value.REGISTRY_TYPE)
                        {
                            case "ARCHIVE":
                                {
                                    if (value.GRANT_TYPE == "C")
                                    {
                                        var result = context.Database.SqlQuery<SOCPEN_CHILD_STATUS>($"SELECT CAST(PENSION_NO AS VARCHAR2(30)) AS PENSION_NO, LPAD(CAST(ID_NO AS VARCHAR2(30)), 13, '0') AS ID_NO, STATUS_CODE, STATUS_DATE  FROM SASSA.SOCPEN_P12_CHILDREN WHERE PENSION_NO = {value.ID_NUMBER} AND STATUS_CODE = '2'");
                                        value.ARCHIVE_YEAR = result.FirstOrDefault()?.STATUS_DATE.ToString("yyyy-MM-dd").Substring(0, 4);
                                    }
                                    break;
                                }
                            default:
                                break;
                        }

                        MISBoxFiles newFile = new MISBoxFiles()
                        {
                            UNQ_FILE_NO = value.UNQ_FILE_NO,
                            BRM_NO = value.BRM_NO,
                            FILE_NUMBER = value.FILE_NUMBER,
                            REGTYPE_MAIN = value.REGTYPE_MAIN,
                            REGTYPE_ARCHIVE = value.REGTYPE_ARCHIVE,
                            ARCHIVE_YEAR = value.ARCHIVE_YEAR,
                            STATUS_NON_COMPLIANT = value.STATUS_NON_COMPLIANT,
                            STATUS_TRANSFERRED = value.STATUS_TRANSFERRED,
                            STATUS_LEGAL = value.STATUS_LEGAL,
                            STATUS_FRAUD = value.STATUS_FRAUD,
                            STATUS_DEBTORS = value.STATUS_DEBTORS,
                            NAME = value.NAME,
                            SURNAME = value.SURNAME,
                            ID_NUMBER = value.ID_NUMBER,
                            TEMP_BOX_NO = value.TEMP_BOX_NO,
                            DOCS_PRESENT = value.DOCS_PRESENT,
                            GRANT_NAME = value.GRANT_NAME,
                            GRANT_TYPE = value.GRANT_TYPE,
                            REGION_NAME = value.REGION_NAME,
                            COVER_DATE = DateTime.Now.ToString(),
                            CHECK_NON_COMPLIANT = value.CHECK_NON_COMPLIANT,
                            CHECK_REGTYPE_MAIN = value.CHECK_REGTYPE_MAIN,
                            CHECK_REGTYPE_ARCHIVE = value.CHECK_REGTYPE_ARCHIVE,
                            CHECK_TRANSFERRED = value.CHECK_TRANSFERRED,
                            CHECK_STATUS_TRANSFERRED = value.CHECK_STATUS_TRANSFERRED,
                            CHECK_STATUS_NON_COMPLIANT = value.CHECK_STATUS_NON_COMPLIANT,
                            CHECK_STATUS_LEGAL = value.CHECK_STATUS_LEGAL,
                            CHECK_STATUS_FRAUD = value.CHECK_STATUS_FRAUD,
                            CHECK_STATUS_DEBTORS = value.CHECK_STATUS_DEBTORS
                        };
                        boxFiles.Add(newFile);

                        // do the tick boxes for this grant type
                        //xxx

                        //LoadCriticalDocumentSection(value.GRANT_TYPE, newFile.DOCS_PRESENT);
                    }
                    //}

                    ListView1.DataSource = boxFiles;
                    ListView1.DataBind();
                    return;
                }
                catch (Exception ex)
                {
                    Label1.Text = ex.Message + "|" + ex.InnerException;
                    div3.Visible = true;
                }
            }
        }

        protected void ListView1_ItemDataBound(object sender, ListViewItemEventArgs e)
        {
            if (e.Item.ItemType == ListViewItemType.DataItem)
            {
                ListViewItem lvi = e.Item;
                string BRMBarCode = ((Label)lvi.FindControl("Label4")).Text;
                string unqno = ((Label)lvi.FindControl("Label3")).Text;
                string pensionNo = ((Label)lvi.FindControl("Label8")).Text;
                string fullname = ((Label)lvi.FindControl("Label6")).Text;
                string grantname = ((Label)lvi.FindControl("Label9")).Text;

                ((PlaceHolder)e.Item.FindControl("phQR")).Controls.Add(getQRCode(BRMBarCode, unqno, pensionNo, fullname, grantname));
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            ////Get the username of the user that is logged in from session.
            //string authenticatedUsername = authObject.AuthenticateCSAdminUser();

            ////If no session values are found , redirect to the login screen
            //if (authenticatedUsername == string.Empty)
            //{
            //    authObject.RedirectToLoginPage();
            //}
            //else
            //{
            string boxNo = Request.QueryString["BoxNo"];
            string batchNo = Request.QueryString["BatchNo"];
            string sTDWBoxNo = Request.QueryString["TDWBoxNo"];

            if (string.IsNullOrEmpty(boxNo) || string.IsNullOrEmpty(batchNo))
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "noboxorbatch", "alert('Parameter error on boxno or batchno');", true);
                ScriptManager.RegisterStartupScript(this, GetType(), "noboxorbatch", "window.close();", true);
            }

            GetFileData(batchNo, boxNo, sTDWBoxNo);
            // }
        }

        #endregion Protected Methods

        #region Private Methods

        private decimal CreateBatchForOffice()
        {
            DC_BATCH b = new DC_BATCH();
            b.BATCH_STATUS = "Open";
            b.BATCH_CURRENT = "Y";
            b.OFFICE_ID = UserSession.Office.OfficeId;
            b.UPDATED_DATE = DateTime.Now;
            b.UPDATED_BY_AD = UserSession.SamName;

            en.DC_BATCH.Add(b);
            en.DC_ACTIVITY.Add(util.CreateActivity("Files", "Add Open Batch"));
            en.SaveChanges();

            return b.BATCH_NO;
        }

        private Applicant getApplicantDataMIS(string idNo, string MISBoxNo, bool exists)
        {
            //Applicant applicant = new Applicant();

            //string myRegionID = util.BLUtility().UserSession.Office.RegionCode;

            //Dictionary<string, string> grantTypes = util.getGrantTypes();

            //try
            //{
            //    using (Entities context = new Entities())
            //    {
            //        if (!exists)
            //        {
            //            IEnumerable<Applicant> query = context.Database.SqlQuery<Applicant>
            //                                       (@"select mis.ID_NUMBER as APPLICANT_NO,
            //                                                   mis.NAME as FIRSTNAME,
            //                                                   mis.SURNAME as LASTNAME,
            //                                                   mis.GRANT_TYPE as GRANT_TYPE1,
            //                                                   gt.TYPE_NAME as GRANT_NAME,
            //                                                   rg.REGION_ID,
            //                                                   rg.REGION_CODE as REGION_CODE,
            //                                                   rg.REGION_NAME as REGION_NAME,
            //                                                   mis.APP_DATE as TRANS_DATE,
            //                                                   spn.DOCS_PRESENT as DOCS_PRESENT
            //                                            from MISNATIONALs mis
            //                                            left outer join SOCPENBRM spn on spn.PENSION_NO = mis.ID_NUMBER
            //                                            inner join DC_REGION rg on mis.REGION_ID = rg.REGION_ID
            //                                            left outer join DC_GRANT_TYPE gt on gt.TYPE_ID = mis.GRANT_TYPE
            //                                            where mis.ID_NUMBER = '" + idNo + "' and mis.BOX_NUMBER = '" + MISBoxNo + "' and mis.REGION_ID = '" + myRegionID + "'");

            //            foreach (Applicant value in query)
            //            {
            //                applicant = value;
            //                applicant.GRANT_TYPE1 = BoxFileActions.convertMISGrantTypeToSocpen(value.GRANT_TYPE1);

            //                // MIS Grant types are not the same as the SOCPEN grant types we are storing - need to remap.
            //                string grantName = "";
            //                if (grantTypes.TryGetValue(applicant.GRANT_TYPE1, out grantName))
            //                {
            //                    applicant.GRANT_NAME = grantName;
            //                }

            //                break;
            //            }
            //        }
            //        //If this file was requested as part of a reprint, get record from DC_FILE and not MISNATIONAL
            //        else
            //        {
            //            //var query = from file in context.DC_FILE
            //            //            join grt in context.DC_GRANT_TYPE on file.GRANT_TYPE equals grt.TYPE_ID
            //            //            join region in context.DC_REGION on file.REGION_ID equals region.REGION_ID
            //            //            join trans in context.DC_TRANSACTION_TYPE on file.TRANS_TYPE equals trans.TYPE_ID
            //            //            where file.APPLICANT_NO == idNo //This pension no is actually the unq code
            //            //            && file.MIS_BOXNO == MISBoxNo
            //            //            && file.REGION_ID == myRegionID

            //            var query = from file in context.DC_FILE
            //                        join grt in context.DC_GRANT_TYPE on file.GRANT_TYPE equals grt.TYPE_ID
            //                        join region in context.DC_REGION on file.REGION_ID equals region.REGION_ID
            //                        where file.APPLICANT_NO == idNo //This pension no is actually the unq code
            //                        && file.MIS_BOXNO == MISBoxNo
            //                        && file.REGION_ID == myRegionID

            //                        select new Applicant
            //                        {
            //                            UNQ_FILE_NO = file.UNQ_FILE_NO,
            //                            APPLICANT_NO = file.APPLICANT_NO,
            //                            FIRSTNAME = file.USER_FIRSTNAME,
            //                            LASTNAME = file.USER_LASTNAME,
            //                            GRANT_TYPE1 = file.GRANT_TYPE,
            //                            GRANT_NAME = grt.TYPE_NAME,
            //                            REGION_ID = region.REGION_ID,
            //                            REGION_CODE = region.REGION_CODE,
            //                            REGION_NAME = region.REGION_NAME,
            //                            TRANS_DATE_DATE = file.TRANS_DATE, //TRANS_DATE_DATE = file.TRANS_DATE,
            //                            DOCS_PRESENT = file.DOCS_PRESENT,
            //                            APPLICATION_STATUS = file.APPLICATION_STATUS,
            //                            TRANS_TYPE = 0,
            //                            //TRANS_NAME = trans.TYPE_NAME,
            //                            BRM_BARCODE = file.BRM_BARCODE,
            //                            MIS_BOX_DATE = file.MIS_BOX_DATE
            //                        };

            //            foreach (Applicant value in query)
            //            {
            //                applicant = value;
            //                break;
            //            }

            //        }
            //    }
            //}
            //catch (Exception ex)
            //{
            //    lblMsg.Text = ex.Message;
            //    divError.Visible = true;
            //    return null;
            //}

            //return applicant;
            return null;
        }

        private ApplicantGrants getFilePrepDataSOCPEN(string pensionNo, string granttype, string appdate, string newFileAdded, string CLMno)
        {
            //string appdatedigits = appdate.Replace("/", "");

            //ApplicantGrants FilePrepData = new ApplicantGrants();
            //try
            //{
            //    using (Entities context = new Entities())
            //    {
            //        //If this file was added to a new batch
            //        if (newFileAdded == "Y")
            //        {
            //            IEnumerable<ApplicantGrants> query = context.Database.SqlQuery<ApplicantGrants>
            //                                            (@"select spn.PENSION_NO as APPLICANT_NO,
            //                                                    spn.NAME as FIRSTNAME,
            //                                                    spn.SURNAME as LASTNAME,
            //                                                    spn.GRANT_TYPE,
            //                                                    spn.APP_DATE,
            //                                                    rg.REGION_ID as REGION_ID,
            //                                                    rg.REGION_CODE as REGION_CODE,
            //                                                    rg.REGION_NAME as REGION_NAME,
            //                                                    spn.APP_DATE as TRANS_DATE,
            //                                                    spn.DOCS_PRESENT as DOCS_PRESENT,
            //                                                    spn.STATUS_DATE as STATUS_DATE,
            //                                                    spn.PRIM_STATUS as PRIM_STATUS,
            //                                                    spn.SEC_STATUS as SEC_STATUS
            //                                            from SOCPENGRANTS spn
            //                                            inner join DC_REGION rg on spn.PROVINCE = rg.REGION_ID
            //                                            where spn.PENSION_NO = '" + pensionNo + "' and spn.GRANT_TYPE = '" + granttype + "' and spn.APP_DATE = '" + appdatedigits + "' order by spn.APP_DATE desc");

            //            foreach (ApplicantGrants value in query)
            //            {
            //                FilePrepData = value;
            //                break;
            //            }
            //        }
            //        //If this file was requested as part of a reprint, get record from audit and not SOCPEN
            //        else
            //        {
            //            //var query = from file in context.DC_FILE
            //            //            join grt in context.DC_GRANT_TYPE on file.GRANT_TYPE equals grt.TYPE_ID
            //            //            join region in context.DC_REGION on file.REGION_ID equals region.REGION_ID
            //            //            join trans in context.DC_TRANSACTION_TYPE on file.TRANS_TYPE equals trans.TYPE_ID
            //            //            where file.UNQ_FILE_NO == CLMno

            //            var query = from file in context.DC_FILE
            //                        join grt in context.DC_GRANT_TYPE on file.GRANT_TYPE equals grt.TYPE_ID
            //                        join region in context.DC_REGION on file.REGION_ID equals region.REGION_ID
            //                        where file.UNQ_FILE_NO == CLMno

            //                        select new ApplicantGrants
            //                        {
            //                            UNQ_FILE_NO = file.UNQ_FILE_NO,
            //                            APPLICANT_NO = file.APPLICANT_NO,
            //                            FIRSTNAME = file.USER_FIRSTNAME,
            //                            LASTNAME = file.USER_LASTNAME,
            //                            GRANT_TYPE = file.GRANT_TYPE,
            //                            GRANT_NAME = grt.TYPE_NAME,
            //                            REGION_ID = region.REGION_ID,
            //                            REGION_CODE = region.REGION_CODE,
            //                            REGION_NAME = region.REGION_NAME,
            //                            TRANS_DATE_DATE = file.TRANS_DATE,
            //                            DOCS_PRESENT = file.DOCS_PRESENT,
            //                            APPLICATION_STATUS = file.APPLICATION_STATUS,
            //                            TRANS_TYPE = 0,
            //                            //TRANS_NAME = trans.TYPE_NAME,
            //                            BRM_BARCODE = file.BRM_BARCODE
            //                        };

            //            //string a = query.ToString();
            //            foreach (ApplicantGrants value in query)
            //            {
            //                FilePrepData = value;
            //                break;
            //            }
            //        }
            //    }
            //}
            //catch (Exception ex)
            //{
            //    lblMsg.Text = ex.Message + " Innerexception: " + ex.InnerException;
            //    divError.Visible = true;
            //    return null;
            //}

            //return FilePrepData;
            return null;
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
            catch (Exception)
            {
                //lblMsg.Text = ex.Message;
                //divError.Visible = true;
                return null;
            }

            return docs;
        }

        private System.Web.UI.WebControls.Image getQRCode(string BRMBarCode, string CLMno, string pensionNo, string fullname, string grantname)
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
                return imgBarCode;
            }
        }

        private List<int> getSOCPENDocs(string idNo, string grantype)
        {
            try
            {
                using (Entities ent = new Entities())
                {
                    IEnumerable<int> query = ent.Database.SqlQuery<int>
                          (@"select DOC_NO_IN_IN
                                    from SOCPENDOCS
                                    where ID_NO = '" + idNo + "' and GRANT_TYPE = '" + grantype + "'");

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
            ////Get all the required documents for the applicant's grant type
            //List<RequiredDocs> docs = getGrantDocuments(grantype);
            //List<int> docList = null;

            ////Check if docs present is filled in on the socpen data.
            //docList = getSOCPENDocs(idNo, grantype);

            ////Critical Documents
            ////-------------------------------------------------------------------------
            //Table tblGeneral = (Table)tblDocGeneral;
            //Table tblIncome = (Table)tblDocIncome;
            //Table tblAssets = (Table)tblDocAssets;
            //TableRow rowParticular = new TableRow();
            //TableRow rowIncome = new TableRow();
            //TableRow rowAssets = new TableRow();

            //if (docs != null)
            //{
            //    foreach (RequiredDocs doc in docs)
            //    {
            //        //Create Table cell
            //        TableCell cell = new TableCell();
            //        cell.Attributes.Add("style", "width:50%; padding-left:5px; vertical-align:top");
            //        //Create textbox control
            //        CheckBox chkbox = new CheckBox();
            //        chkbox.ID = "chk_" + doc.DOC_ID;
            //        chkbox.Text = doc.DOC_NAME;
            //        chkbox.LabelAttributes.Add("class", "chkboxLabel");

            //        //set checked state if id matches a document in docs-present array
            //        if (docList != null && docList.Count > 0)
            //        {
            //            foreach (int val in docList)
            //            {
            //                try
            //                {
            //                    if (doc.DOC_ID == val)
            //                    {
            //                        chkbox.Checked = true;
            //                    }
            //                }
            //                catch { }
            //            }
            //        }

            //        //add checkbox to cell
            //        cell.Controls.Add(chkbox);

            //        switch (doc.DOC_SECTION)
            //        {
            //            case "General Particulars":
            //                //Only add 2 columns for each row
            //                if (rowParticular.Cells.Count < 2)
            //                {
            //                    rowParticular.Cells.Add(cell);
            //                }
            //                else
            //                {
            //                    tblGeneral.Rows.Add(rowParticular);
            //                    rowParticular = new TableRow();
            //                    rowParticular.Cells.Add(cell);
            //                }
            //                break;
            //            case "Particulars of Income":
            //                //Only add 2 columns for each row
            //                if (rowIncome.Cells.Count < 2)
            //                {
            //                    rowIncome.Cells.Add(cell);
            //                }
            //                else
            //                {
            //                    tblIncome.Rows.Add(rowIncome);
            //                    rowIncome = new TableRow();
            //                    rowIncome.Cells.Add(cell);
            //                }
            //                break;
            //            case "Particulars of Assets":
            //                //Only add 2 columns for each row
            //                if (rowAssets.Cells.Count < 2)
            //                {
            //                    rowAssets.Cells.Add(cell);
            //                }
            //                else
            //                {
            //                    tblAssets.Rows.Add(rowAssets);
            //                    rowAssets = new TableRow();
            //                    rowAssets.Cells.Add(cell);
            //                }
            //                break;
            //        }
            //    }
            //}

            //if (rowParticular.Cells.Count > 0)
            //{
            //    tblGeneral.Rows.Add(rowParticular);
            //}
            //if (rowIncome.Cells.Count > 0)
            //{
            //    tblIncome.Rows.Add(rowIncome);
            //}
            //if (rowAssets.Cells.Count > 0)
            //{
            //    tblAssets.Rows.Add(rowAssets);
            //}
        }

        private void populateDataforSheet()
        {
            //Session["applicantFile"] = null;
            //if (Request.QueryString["BRM"] != null)
            //{
            //    Session["BRM"] = Request.QueryString["BRM"];
            //}

            //// FOR BATCHING: =>
            ////http://localhost:34186/Views/FileCover.aspx?
            ////PensionNo = 7907110695086
            ////&boxaudit =
            ////&boxNo =
            ////&batching = y
            ////&trans = 67
            ////&brmBC = jhgfdsa
            ////&grant = Child%20Support%20Grant
            ////&appdate = 20130513

            //string pensionNo = Request.QueryString["PensionNo"];
            //string CLMno = Request.QueryString["CLM"];
            //string addToBatch = Request.QueryString["Batching"];
            //string boxAudit = Request.QueryString["boxaudit"];
            //string addToBox = Request.QueryString["boxing"] != null ? Request.QueryString["boxing"] : "Y";
            //string boxNo = Request.QueryString["boxNo"];
            //string BRMBarCode = Request.QueryString["BRM"] != null ? Request.QueryString["BRM"] : Session["BRM"].ToString();
            //string grantname = Request.QueryString["gn"];
            //string granttype = Request.QueryString["gt"];
            //string appdate = Request.QueryString["appdate"];
            //txtBRM.InnerText = BRMBarCode;
            ////Run qrbarcode script

            //string barcodestring = BRMBarCode + "\t" + CLMno + "\t" + pensionNo + "\t" + divUserName.InnerText + "\t" + grantname;
            //string code = barcodestring;

            //QRCodeGenerator qrGenerator = new QRCodeGenerator();
            //QRCodeGenerator.QRCode qrCode = qrGenerator.CreateQrCode(code, QRCodeGenerator.ECCLevel.Q);
            //System.Web.UI.WebControls.Image imgBarCode = new System.Web.UI.WebControls.Image();
            //imgBarCode.Height = 100;
            //imgBarCode.Width = 100;
            //using (Bitmap bitMap = qrCode.GetGraphic(20))
            //{
            //    using (MemoryStream ms = new MemoryStream())
            //    {
            //        bitMap.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
            //        byte[] byteImage = ms.ToArray();
            //        imgBarCode.ImageUrl = "data:image/png;base64," + Convert.ToBase64String(byteImage);
            //    }
            //    plBarCode.Controls.Add(imgBarCode);
            //    //findvalues();

            //}
            //// end qr barcode
            ////Mandatory queryStrings:
            ////  PensionNo + grant + appdate (unique no for batch process)
            ////  PensionNo (id number for box audit)

            ////  Either Batching or boxaudit to determine process
            ////  BRMBarCode

            //if (!string.IsNullOrEmpty(pensionNo) &&
            //    (
            //    (!string.IsNullOrEmpty(addToBatch) && (addToBatch.ToUpper().Contains("Y") || addToBatch.ToUpper().Contains("N")))
            //    ||
            //    (!string.IsNullOrEmpty(boxAudit) && (boxAudit.ToUpper().Contains("Y")))
            //    ||
            //    (!string.IsNullOrEmpty(addToBatch) && addToBatch.ToUpper().Contains("N") && !string.IsNullOrEmpty(CLMno))
            //    ) &&
            //    !string.IsNullOrEmpty(BRMBarCode)
            //   )

            ////        if (!string.IsNullOrEmpty(pensionNo) &&
            ////((!string.IsNullOrEmpty(addToBatch) && (addToBatch.ToUpper().Contains("Y") || addToBatch.ToUpper().Contains("N"))) ||
            ////!string.IsNullOrEmpty(boxAudit) && (boxAudit.ToUpper().Contains("Y"))) &&
            ////!string.IsNullOrEmpty(BRMBarCode)
            ////)
            ////        {
            //////if (!string.IsNullOrEmpty(pensionNo) && !string.IsNullOrEmpty(granttype) && !string.IsNullOrEmpty(appdate) &&
            //////((!string.IsNullOrEmpty(addToBatch) && (addToBatch.ToUpper().Contains("Y")
            //////|| addToBatch.ToUpper().Contains("N"))) ||
            //////!string.IsNullOrEmpty(boxAudit) && (boxAudit.ToUpper().Contains("Y"))) && !string.IsNullOrEmpty(BRMBarCode)
            //////)
            //{
            //    //This bool will be used throughout to differentiate between the Applicant Search File process and the Box Audit Function
            //    //This way we can use the same FileCover sheet for 2 different processes.
            //    bool isBoxAuditProcess = boxAudit != null ? boxAudit.ToUpper().Contains("Y") : false;

            //    //If this is the normal Applicant Search function, get data from Socpen, check transaction type and get correct grant type.
            //    if (!isBoxAuditProcess)
            //    {
            //        //  BATCHING =================================================
            //        txtProcess.Value = "batching";
            //        ApplicantGrants filePrep = null;
            //        int transType = 1;
            //        Dictionary<string, string> dictGrantTypes = util.getGrantTypes();
            //        //}
            //        //else
            //        //{
            //        transType = 0; // (int)filePrep.TRANS_TYPE;
            //        //}
            //        //Hide the appropriate print button
            //        btnPrint.Visible = addToBatch.ToUpper() == "Y" ? true : false;
            //        btnReprint.Visible = addToBatch.ToUpper() == "N" ? true : false;

            //        //Get Applicant Details from SOCPENGRANTS
            //        filePrep = getFilePrepDataSOCPEN(pensionNo, granttype, appdate, addToBatch.ToUpper(), CLMno);
            //        if (filePrep != null)
            //        {
            //            //New feature - transaction type per grant documents - only on new additions to batch!!!

            //            //if (addToBatch.ToUpper() == "Y")
            //            //{
            //            //    int.TryParse(transactionType, out transType);

            //            //if (transactionType != null)
            //            //{
            //            //    filePrep.TRANS_TYPE = int.Parse(transactionType);
            //            //    transType = int.Parse(transactionType);
            //            //}
            //            //else
            //            //{
            //            //    filePrep.TRANS_TYPE = transType;
            //            //}
            //            //filePrep.TRANS_NAME = getTransActionName(transType);
            //            //divTransType.InnerText = filePrep.TRANS_NAME;
            //            filePrep.TRANS_TYPE = 0;

            //            if (filePrep.TRANS_DATE != null)
            //            {
            //                string myYYYY = filePrep.TRANS_DATE.Substring(0, 4);
            //                string myMM = filePrep.TRANS_DATE.Substring(4, 2);
            //                string myDD = filePrep.TRANS_DATE.Substring(6, 2);
            //                divTransDate.InnerText = myYYYY + "/" + myMM + "/" + myDD;
            //            }
            //            else
            //            {
            //                if (filePrep.TRANS_DATE_DATE == null)
            //                {
            //                    divTransDate.InnerText = appdate;
            //                }
            //                else
            //                {
            //                    divTransDate.InnerText = filePrep.TRANS_DATE_DATE.ToString();
            //                }
            //            }

            //            //divTransDate.InnerText = filePrep.TRANS_DATE;

            //            filePrep.GRANT_NAME = grantname;
            //            string MYAPPLICANT = filePrep.APPLICANT_NO;
            //        }

            //        if (filePrep != null && !string.IsNullOrEmpty(filePrep.APPLICANT_NO))
            //        {
            //            //Set all the fields on the form
            //            //Summary section details
            //            //-------------------------------------------------------------------------
            //            divUserName.InnerText = filePrep.FIRSTNAME + " " + filePrep.LASTNAME;
            //            divSocpenRef.InnerText = filePrep.APPLICANT_NO;
            //            divIDNo.InnerText = filePrep.APPLICANT_NO;
            //            divGrantName.InnerText = filePrep.GRANT_NAME;

            //            divRegion.InnerText = filePrep.REGION_CODE + " - " + filePrep.REGION_NAME;
            //            divIDNoBarcode.Alt = filePrep.APPLICANT_NO;

            //            if (BRMBarCode == null)
            //            {
            //                if (string.IsNullOrEmpty(txtBRM.InnerText))
            //                {
            //                    if (string.IsNullOrEmpty(filePrep.BRM_BARCODE))
            //                    {
            //                        Page.ClientScript.RegisterStartupScript(this.GetType(), "getBRMBarcodeAndReload", "getBRMBarcodeAndReload()", true);
            //                    }
            //                    else
            //                    {
            //                        Session["BRM"] = filePrep.BRM_BARCODE.ToUpper();
            //                        divBRMBarCode.Alt = filePrep.BRM_BARCODE.ToUpper();
            //                        txtBRM.InnerText = filePrep.BRM_BARCODE.ToUpper();
            //                        BRMBarCode = filePrep.BRM_BARCODE.ToUpper();
            //                        Page.ClientScript.RegisterStartupScript(this.GetType(), "Reload", "location.reload();", true);
            //                    }
            //                }
            //                //Session["BRM"] = BRMBarCode.ToUpper();
            //                divBRMBarCode.Alt = BRMBarCode.ToUpper();
            //                txtBRM.InnerText = BRMBarCode.ToUpper();

            //                btnPrint.Visible = false;
            //                btnReprint.Visible = false;
            //            }
            //            else
            //            {
            //                //Session["BRM"] = BRMBarCode.ToUpper();
            //                divBRMBarCode.Alt = BRMBarCode.ToUpper();
            //                txtBRM.InnerText = BRMBarCode.ToUpper();
            //            }

            //            if (!string.IsNullOrEmpty(filePrep.UNQ_FILE_NO))
            //            {
            //                divFileUnqBarcode.Alt = filePrep.UNQ_FILE_NO;
            //            }

            //            //Check if application status is filled in and check relevant checkbox.
            //            if (string.IsNullOrEmpty(filePrep.APPLICATION_STATUS))
            //            {
            //                string mystatus = "";
            //                //string diff2 = "0";
            //                string checkstatus = filePrep.PRIM_STATUS.Trim() + filePrep.SEC_STATUS.Trim();
            //                //if (checkstatus == "B2" || checkstatus == "A2" || checkstatus == "92")
            //                //{
            //                //    // This is either "MAIN" or "LOOSE CORRESPONDENCE"
            //                //    DateTime myStart;
            //                //    DateTime.TryParseExact(filePrep.APP_DATE.Trim(), "yyyyMMdd", CultureInfo.InvariantCulture, DateTimeStyles.None, out myStart);
            //                //    DateTime myEnd;
            //                //    DateTime.TryParseExact(filePrep.STATUS_DATE.Trim(), "yyyyMMdd", CultureInfo.InvariantCulture, DateTimeStyles.None, out myEnd);
            //                //    double mydiff1 = (double)((myEnd - myStart).TotalDays);
            //                //    if (mydiff1 > 60)
            //                //    {
            //                //        mystatus = "LOOSE CORRESPONDENCE";

            //                //    }
            //                //    else
            //                //    {
            //                //        mystatus = "ARCHIVE";
            //                //    }
            //                //    //lblMsg.Text = mydiff1.ToString();
            //                //    diff2 = mydiff1.ToString();
            //                //}
            //                if (checkstatus == "B2" || checkstatus == "A2" || checkstatus == "92")
            //                {
            //                    mystatus = "MAIN";
            //                }
            //                else
            //                {
            //                    mystatus = "ARCHIVE";
            //                }
            //                //lblMsg.Text = lblMsg.Text + "|" + mystatus;
            //                //divError.Visible = true;
            //                //lblStatus.Text = "Status:" + checkstatus;
            //                //lblStatusDate1.Text = "Application Date:" + filePrep.APP_DATE.Trim();
            //                //lblStatusDate2.Text = "Status Date:" + filePrep.STATUS_DATE.Trim();
            //                //lblDays.Text = diff2 + " Days Between";

            //                switch (mystatus)
            //                {
            //                    case "MAIN": chkREGTYPE_MAIN.Checked = true; break;
            //                    case "ARCHIVE": chkREGTYPE_ARCHIVE.Checked = true; break;
            //                        //case "LOOSE CORRESPONDENCE": checkReview.Checked = true; break;
            //                }

            //            }
            //            else
            //            {
            //                UpdateRegistryTypeCheckbox(filePrep.APPLICATION_STATUS);
            //            }

            //            //If this is via a reprint, get docs from our tables, else SOCPEN
            //            if ((!isBoxAuditProcess && addToBatch.ToUpper() == "N") ||
            //                (isBoxAuditProcess && addToBox.Trim().ToUpper() == "N"))
            //            {
            //                //Get all the required documents for the applicant's grant type and display in the 3 sections on the form
            //                LoadCriticalDocumentSection(filePrep.GRANT_TYPE, filePrep.DOCS_PRESENT);
            //            }
            //            else
            //            {
            //                LoadCriticalDocumentSocpen(filePrep.APPLICANT_NO, filePrep.GRANT_TYPE);
            //            }

            //            Session["applicantFile"] = filePrep;
            //        }

            //        else
            //        {
            //            lblMsg.Text = "File data not found.";
            //            divError.Visible = true;
            //            btnPrint.Visible = false;
            //            btnReprint.Visible = false;
            //            return;
            //        }
            //    }
            //    else
            //    {
            //        //  BOX AUDIT =================================================
            //        txtProcess.Value = "box audit";

            //        Applicant applicant = null;
            //        //int transType = 1;
            //        Dictionary<string, string> dictGrantTypes = util.getGrantTypes();

            //        //Hide the appropriate print button
            //        btnPrint.Visible = addToBox.ToUpper() == "Y" ? true : false;
            //        btnReprint.Visible = addToBox.ToUpper() == "N" ? true : false;
            //        bool exists = false;

            //        //If this is via a reprint, get data from dc_file
            //        if (!string.IsNullOrEmpty(addToBox) && addToBox.Trim().ToUpper() == "N")
            //        {
            //            exists = true;
            //        }

            //        //Get Applicant Details from MISNATIONAL for the Box Audit Function
            //        applicant = getApplicantDataMIS(pensionNo, boxNo, exists);

            //        //if (!string.IsNullOrEmpty(transactionType) && int.TryParse(transactionType, out transType))
            //        //{
            //        //    applicant.TRANS_TYPE = transType;
            //        //    applicant.TRANS_NAME = getTransActionName(transType);
            //        //}
            //        //divTransType.InnerText = applicant.TRANS_NAME;
            //        applicant.TRANS_TYPE = 0;

            //        if ((divTransDate.InnerText == null) || (divTransDate.InnerText == ""))
            //        {
            //            string mydate = applicant.TRANS_DATE_DATE.HasValue ? applicant.TRANS_DATE_DATE.ToString() : string.IsNullOrEmpty(applicant.TRANS_DATE) ? "" : applicant.TRANS_DATE;
            //            int mypos = mydate.IndexOf(" ");
            //            if (mypos > 0)
            //            {
            //                mydate = mydate.Substring(0, mypos);
            //            }
            //            divTransDate.InnerText = mydate;
            //        }

            //        applicant.MIS_BOXNO = boxNo;
            //        applicant.MIS_BOX_DATE = applicant.MIS_BOX_DATE.HasValue ? applicant.MIS_BOX_DATE : DateTime.Now;

            //        //Continue the normal process for generating FileCover sheet.
            //        if (applicant != null && !string.IsNullOrEmpty(applicant.APPLICANT_NO))
            //        {
            //            //Set all the fields on the form
            //            //Summary section details
            //            //-------------------------------------------------------------------------
            //            divUserName.InnerText = applicant.FIRSTNAME + " " + applicant.LASTNAME;
            //            divSocpenRef.InnerText = applicant.APPLICANT_NO;
            //            divIDNo.InnerText = applicant.APPLICANT_NO;
            //            divGrantName.InnerText = applicant.GRANT_NAME;

            //            divRegion.InnerText = applicant.REGION_CODE + " - " + applicant.REGION_NAME;
            //            divIDNoBarcode.Alt = applicant.APPLICANT_NO;

            //            if (BRMBarCode == null)
            //            {
            //                if (string.IsNullOrEmpty(txtBRM.InnerText))
            //                {
            //                    if (string.IsNullOrEmpty(applicant.BRM_BARCODE))
            //                    {
            //                        Page.ClientScript.RegisterStartupScript(this.GetType(), "getBRMBarcodeAndReload", "getBRMBarcodeAndReload()", true);
            //                    }
            //                    else
            //                    {
            //                        divBRMBarCode.Alt = applicant.BRM_BARCODE.ToUpper();
            //                        txtBRM.InnerText = applicant.BRM_BARCODE.ToUpper();
            //                        BRMBarCode = applicant.BRM_BARCODE.ToUpper();
            //                    }
            //                }
            //                divBRMBarCode.Alt = BRMBarCode.ToUpper();
            //                txtBRM.InnerText = BRMBarCode.ToUpper();

            //                btnPrint.Visible = false;
            //                btnReprint.Visible = false;
            //            }
            //            else
            //            {
            //                divBRMBarCode.Alt = BRMBarCode.ToUpper();
            //                txtBRM.InnerText = BRMBarCode.ToUpper();
            //            }

            //            if (!string.IsNullOrEmpty(applicant.UNQ_FILE_NO))
            //            {
            //                divFileUnqBarcode.Alt = applicant.UNQ_FILE_NO;
            //            }

            //            //Check if application status is filled in and check relevant checkbox.
            //            if (!string.IsNullOrEmpty(applicant.APPLICATION_STATUS))
            //            {
            //                UpdateRegistryTypeCheckbox(applicant.APPLICATION_STATUS);
            //            }

            //            //If this is via a reprint, get docs from our tables, else SOCPEN
            //            if ((!isBoxAuditProcess && addToBatch.ToUpper() == "N") ||
            //                (isBoxAuditProcess && addToBox.Trim().ToUpper() == "N"))
            //            {
            //                //Get all the required documents for the applicant's grant type and display in the 3 sections on the form
            //                LoadCriticalDocumentSection(applicant.GRANT_TYPE1, applicant.DOCS_PRESENT);
            //            }
            //            else
            //            {
            //                LoadCriticalDocumentSocpen(applicant.APPLICANT_NO, applicant.GRANT_TYPE1);
            //            }

            //            Session["applicantFile"] = applicant;

            //        }
            //        else
            //        {
            //            lblMsg.Text = "File data not found.";
            //            divError.Visible = true;
            //            btnPrint.Visible = false;
            //            btnReprint.Visible = false;
            //            return;
            //        }
            //    }
            //}
            //else
            //{
            //    lblMsg.Text = "Invalid parameters provided.";
            //    divError.Visible = true;
            //    btnPrint.Visible = false;
            //    btnReprint.Visible = false;
            //    return;
            //}
        }

        private void UpdateRegistryTypeCheckbox(string appStatus)
        {
            //switch (appStatus)
            //{
            //    case "MAIN":
            //        chkREGTYPE_MAIN.Checked = true;
            //        break;

            //    case "ARCHIVE":
            //        chkREGTYPE_ARCHIVE.Checked = true;
            //        break;

            //    case "LC-MAIN":
            //        chkREGTYPE_LC.Checked = true;
            //        chkREGTYPE_MAIN.Checked = true;
            //        break;

            //    case "LC-ARCHIVE":
            //        chkREGTYPE_LC.Checked = true;
            //        chkREGTYPE_ARCHIVE.Checked = true;
            //        break;
            //}
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