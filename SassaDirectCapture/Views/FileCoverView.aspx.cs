using SASSADirectCapture.EntityModels;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SASSADirectCapture.BL;

namespace SASSADirectCapture.Views
{
    public partial class FileCoverView : Page
    {
        private BLUtility util = new BLUtility();
        private SASSA_Authentication authObject = new SASSA_Authentication();
        private Entities en = new Entities();

        string _schema = "";

        private string schema
        {
            get
            {
                if (_schema == string.Empty)
                    _schema = System.Configuration.ConfigurationManager.AppSettings["schema"].ToString();
                return _schema;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            //Get the username of the user that is logged in from session.
            string authenticatedUsername = authObject.AuthenticateCSAdminUser();

            //If no session values are found , redirect to the login screen
            if (authenticatedUsername == string.Empty)
            {
                authObject.RedirectToLoginPage();
            }
            else
            {
                populateDataforSheet();
            }
        }

        private void populateDataforSheet()
        {
            Session["applicantFile"] = null;
            if (Request.QueryString["BRM"] != null)
            {
                Session["BRM"] = Request.QueryString["BRM"];
            }

            // FOR BATCHING: =>
            //http://localhost:34186/Views/FileCover.aspx?
            //PensionNo = 7907110695086
            //&boxaudit =
            //&boxNo =
            //&batching = y
            //&trans = 67
            //&brmBC = jhgfdsa
            //&grant = Child%20Support%20Grant
            //&appdate = 20130513

            string pensionNo = Request.QueryString["PensionNo"];
            string addToBatch = Request.QueryString["Batching"];
            string boxAudit = Request.QueryString["boxaudit"];
            string addToBox = Request.QueryString["boxing"] != null ? Request.QueryString["boxing"] : "Y";
            string boxNo = Request.QueryString["boxNo"];
            string transactionType = Request.QueryString["trans"] != null ? Request.QueryString["trans"] : "";
            string BRMBarCode = Request.QueryString["BRM"] != null ? Request.QueryString["BRM"] : Session["BRM"].ToString();
            string grantname = Request.QueryString["gn"];
            string granttype = Request.QueryString["gt"];
            string appdate = Request.QueryString["appdate"];
            hiddenBarcode.Value = BRMBarCode;
            txtBRM.InnerText = BRMBarCode;

            //Mandatory queryStrings:
            //  PensionNo + grant + appdate (unique no for batch process)
            //  PensionNo (id number for box audit)

            //  Either Batching or boxaudit to determine process
            //  BRMBarCode

            if (!string.IsNullOrEmpty(pensionNo) && !string.IsNullOrEmpty(granttype) && !string.IsNullOrEmpty(appdate) &&
                ((!string.IsNullOrEmpty(addToBatch) && (addToBatch.ToUpper().Contains("Y") || addToBatch.ToUpper().Contains("N"))) ||
                !string.IsNullOrEmpty(boxAudit) && (boxAudit.ToUpper().Contains("Y"))) &&
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
                    int transType = 1;
                    Dictionary<string, string> dictGrantTypes = util.getGrantTypes();
                        //}
                        //else
                        //{
                        //    transType = (int)filePrep.TRANS_TYPE;
                        //}
                    //Hide the appropriate print button
                    btnPrint.Visible = addToBatch.ToUpper() == "Y" ? true : false;
                    btnReprint.Visible = addToBatch.ToUpper() == "N" ? true : false;

                    //Get Applicant Details from SOCPENGRANTS
                    filePrep = getFilePrepDataSOCPEN(pensionNo, granttype, appdate, addToBatch.ToUpper());
                    if (filePrep != null)
                    {
                        //New feature - transaction type per grant documents - only on new additions to batch!!!

                        //if (addToBatch.ToUpper() == "Y")
                        //{
                        //    int.TryParse(transactionType, out transType);
                            
                        if (transactionType != null)
                        {
                            filePrep.TRANS_TYPE = int.Parse(transactionType);
                            transType = int.Parse(transactionType);
                        }
                        else
                        {
                            filePrep.TRANS_TYPE = transType;
                        }
                        filePrep.TRANS_NAME = getTransActionName(transType);

                        divTransType.InnerText = filePrep.TRANS_NAME;
                        divTransDate.InnerText = filePrep.TRANS_DATE;


                        divTransDate.InnerText = appdate;   // Request.QueryString["appdate"];
                        //string latestGrantType = granttype;     // Request.QueryString["grant"];

                        filePrep.GRANT_TYPE = granttype;
                        filePrep.GRANT_NAME = grantname;
                        //if (dictGrantTypes.ContainsKey(latestGrantType))
                        //{
                        //filePrep.GRANT_NAME = dictGrantTypes[latestGrantType];
                        //}
                        string MYAPPLICANT = filePrep.APPLICANT_NO;
                    }

                    if (filePrep != null && !string.IsNullOrEmpty(filePrep.APPLICANT_NO))
                    {
                        //Set all the fields on the form
                        //Summary section details
                        //-------------------------------------------------------------------------
                        divUserName.InnerText = filePrep.FIRSTNAME + " " + filePrep.LASTNAME;
                        divSocpenRef.InnerText = filePrep.APPLICANT_NO;
                        divIDNo.InnerText = filePrep.APPLICANT_NO;
                        divGrantName.InnerText = filePrep.GRANT_NAME;

                        divRegion.InnerText = filePrep.REGION_CODE + " - " + filePrep.REGION_NAME;
                        divIDNoBarcode.Alt = filePrep.APPLICANT_NO;

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
                            //Session["BRM"] = BRMBarCode.ToUpper();
                            divBRMBarCode.Alt = BRMBarCode.ToUpper();
                            txtBRM.InnerText = BRMBarCode.ToUpper();

                            btnPrint.Visible = false;
                            btnReprint.Visible = false;
                        }
                        else
                        {
                            //Session["BRM"] = BRMBarCode.ToUpper();
                            divBRMBarCode.Alt = BRMBarCode.ToUpper();
                            txtBRM.InnerText = BRMBarCode.ToUpper();
                        }

                        if (!string.IsNullOrEmpty(filePrep.UNQ_FILE_NO))
                        {
                            divFileUnqBarcode.Alt = filePrep.UNQ_FILE_NO;
                        }

                        //Check if application status is filled in and check relevant checkbox.
                        if (!string.IsNullOrEmpty(filePrep.APPLICATION_STATUS))
                        {
                            UpdateRegistryTypeCheckbox(filePrep.APPLICATION_STATUS);
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
                    int transType = 1;
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

                    //Get Applicant Details from MISLIVELINKBRM for the Box Audit Function
                    applicant = getApplicantDataMIS(pensionNo, boxNo, exists);

                    if (!string.IsNullOrEmpty(transactionType) && int.TryParse(transactionType, out transType))
                    {
                        applicant.TRANS_TYPE = transType;
                        applicant.TRANS_NAME = getTransActionName(transType);
                    }

                    divTransType.InnerText = applicant.TRANS_NAME;

                    if ((divTransDate.InnerText == null) || (divTransDate.InnerText == ""))
                    {
                        string mydate = applicant.TRANS_DATE_DATE.HasValue ? applicant.TRANS_DATE_DATE.ToString() : string.IsNullOrEmpty(applicant.TRANS_DATE) ? "" : applicant.TRANS_DATE;
                        int mypos = mydate.IndexOf(" ");
                        if (mypos > 0)
                        {
                            mydate = mydate.Substring(0, mypos);
                        }
                        divTransDate.InnerText = mydate;
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
                        divIDNo.InnerText = applicant.APPLICANT_NO;
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

        private Applicant getApplicantDataMIS(string idNo, string MISBoxNo, bool exists)
        {
            Applicant applicant = new Applicant();

            string myRegionID = util.getSessionLocalOfficeRegion();

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
                                                        from MISLIVELINKBRM mis
                                                        left outer join SOCPENBRM spn on spn.PENSION_NO = mis.ID_NUMBER
                                                        inner join DC_REGION rg on mis.REGION_ID = rg.REGION_ID 
                                                        left outer join DC_GRANT_TYPE gt on gt.TYPE_ID = mis.GRANT_TYPE
                                                        where mis.ID_NUMBER = '" + idNo + "' and mis.BOX_NUMBER = '" + MISBoxNo + "' and mis.REGION_ID = '" + myRegionID + "'");

                        foreach (Applicant value in query)
                        {
                            applicant = value;
                            //applicant.GRANT_TYPE1 = BoxFileActions.convertMISGrantTypeToSocpen(value.GRANT_TYPE1);

                            // MIS Grant types are not the same as the SOCPEN grant types we are storing - need to remap.
                            string grantName = "";
                            if (grantTypes.TryGetValue(applicant.GRANT_TYPE1, out grantName))
                            {
                                applicant.GRANT_NAME = grantName;
                            }

                            break;
                        }
                    }
                    //If this file was requested as part of a reprint, get record from DC_FILE and not MISLIVELINKBRM
                    else
                    {
                        var query = from file in context.DC_FILE
                                    join grt in context.DC_GRANT_TYPE on file.GRANT_TYPE equals grt.TYPE_ID
                                    join region in context.DC_REGION on file.REGION_ID equals region.REGION_ID
                                    join trans in context.DC_TRANSACTION_TYPE on file.TRANS_TYPE equals trans.TYPE_ID
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
                                        TRANS_TYPE = file.TRANS_TYPE,
                                        TRANS_NAME = trans.TYPE_NAME,
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
                                                        where spn.PENSION_NO = '" + pensionNo + "'");

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
                                    join trans in context.DC_TRANSACTION_TYPE on file.TRANS_TYPE equals trans.TYPE_ID
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
                                        TRANS_TYPE = file.TRANS_TYPE,
                                        TRANS_NAME = trans.TYPE_NAME,
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

        private ApplicantGrants getFilePrepDataSOCPEN(string pensionNo, string granttype, string appdate, string newFileAdded)
        {
            ApplicantGrants FilePrepData = new ApplicantGrants();
            try
            {
                using (Entities context = new Entities())
                {
                    //If this file was added to a new batch
                    if (newFileAdded == "Y")
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
                                                                spn.DOCS_PRESENT as DOCS_PRESENT
                                                        from SOCPENGRANTS spn
                                                        inner join DC_REGION rg on spn.PROVINCE = rg.REGION_ID 
                                                        where spn.PENSION_NO = '" + pensionNo + "' and spn.GRANT_TYPE = '" + granttype + "' and spn.APP_DATE = '" + appdate + "' order by spn.APP_DATE desc");
                       
                        foreach (ApplicantGrants value in query)
                        {
                            FilePrepData = value;
                            break;
                        }
                    }
                    //If this file was requested as part of a reprint, get record from audit and not SOCPEN
                    else
                    {
                        string mydate = appdate+" 12:00:00 AM";
                          var query = from file in context.DC_FILE
                                    join grt in context.DC_GRANT_TYPE on file.GRANT_TYPE equals grt.TYPE_ID
                                    join region in context.DC_REGION on file.REGION_ID equals region.REGION_ID
                                    join trans in context.DC_TRANSACTION_TYPE on file.TRANS_TYPE equals trans.TYPE_ID
                                    where file.APPLICANT_NO == pensionNo && file.GRANT_TYPE == granttype 
                                    && file.TRANS_DATE == DateTime.Parse(mydate)

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
                                        TRANS_TYPE = file.TRANS_TYPE,
                                        TRANS_NAME = trans.TYPE_NAME,
                                        BRM_BARCODE = file.BRM_BARCODE
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
                lblMsg.Text = ex.Message;
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
            catch (Exception ex)
            {

            }

            return transactionName;
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
            if (!checkApprove.Checked && !checkReject.Checked && !checkReview.Checked)
            {
                lblMsg.Text = "Mark the application as <b>Main</b>, <b>Archive</b> or <b>Review</b> before printing.";
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
            if(txtProcess.Value == "batching")
            {
                ApplicantGrants fileprep = (ApplicantGrants)Session["applicantFile"];
                if (fileprep != null)
                {
                    try
                    {
                        using (Entities context = new Entities())
                        {
                            DC_FILE file = new DC_FILE();
                            string localOffice = util.getSessionLocalOfficeId();
                            decimal batchNo = -1;

                            //If this is the Batching function, use the batching process
                            if (string.IsNullOrEmpty(fileprep.MIS_BOXNO))
                            {
                                var query = from b in context.DC_BATCH
                                    .Where(bn => bn.BATCH_CURRENT.ToLower().Trim() == "y")
                                    .Where(oid => oid.OFFICE_ID == localOffice)
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

                                //If there is no current batch for the local office, create one.
                                if (batchNo == -1)
                                {
                                    batchNo = CreateBatchForOffice();
                                }

                                if (batchNo > -1)
                                {
                                    file.BATCH_NO = batchNo;
                                    file.BATCH_ADD_DATE = DateTime.Now;
                                    file.TRANS_TYPE = fileprep.TRANS_TYPE;
                                }
                            }

                            file.APPLICANT_NO = fileprep.APPLICANT_NO;
                            file.DOCS_PRESENT = docsSelected;
                            file.GRANT_TYPE = fileprep.GRANT_TYPE;
                            file.REGION_ID = fileprep.REGION_ID;
                            file.OFFICE_ID = localOffice; // Get office id from logged-in user.
                            file.UPDATED_DATE = DateTime.Now;
                            file.UPDATED_BY = authObject.getUserID();
                            file.USER_FIRSTNAME = fileprep.FIRSTNAME;
                            file.USER_LASTNAME = fileprep.LASTNAME;
                            file.APPLICATION_STATUS = getCheckboxStatus();
                            file.BRM_BARCODE = txtBRM.InnerText;
                            file.TRANS_DATE = DateTime.ParseExact(fileprep.APP_DATE.Trim(), "yyyyMMdd", null);

                            context.DC_FILE.Add(file);

                            context.SaveChanges();

                            divFileUnqBarcode.Alt = file.UNQ_FILE_NO;
                            unq_ref = file.UNQ_FILE_NO;
                            batchno = file.BATCH_NO.ToString();
                            boxno = file.MIS_BOXNO;

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
                            fullErrorMessage = ex.Message;
                            fullErrorMessage += " InnerException:" + ex.InnerException;
                        }

                        lblMsg.Text = fullErrorMessage;
                        divError.Visible = true;

                    }
                    catch (Exception ex)
                    {
                        lblMsg.Text = ex.Message;
                        lblMsg.Text += " InnerException:" + ex.InnerException;
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
                            string localOffice = util.getSessionLocalOfficeId();
                            decimal batchNo = -1;

                            //If this is not the Box Audit function, use the batching process
                            if (string.IsNullOrEmpty(applicant.MIS_BOXNO))
                            {
                                var query = from b in context.DC_BATCH
                                    .Where(bn => bn.BATCH_CURRENT.ToLower().Trim() == "y")
                                    .Where(oid => oid.OFFICE_ID == localOffice)
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

                                //If there is no current batch for the local office, create one.
                                if (batchNo == -1)
                                {
                                    batchNo = CreateBatchForOffice();
                                }

                                if (batchNo > -1)
                                {
                                    file.BATCH_NO = batchNo;
                                    file.BATCH_ADD_DATE = DateTime.Now;
                                    file.TRANS_TYPE = applicant.TRANS_TYPE;
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
                                    file.TRANS_TYPE = x.TRANS_TYPE.HasValue ? x.TRANS_TYPE : applicant.TRANS_TYPE; //Get the transaction type first before removing the old record.
                                    file.TRANS_DATE = x.TRANS_DATE;
                                    context.DC_FILE.Remove(x);
                                }
                                else
                                {
                                    file.TRANS_TYPE = applicant.TRANS_TYPE; //Get the transaction type first before removing the old record.
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
                            file.UPDATED_BY = authObject.getUserID();
                            file.USER_FIRSTNAME = applicant.FIRSTNAME;
                            file.USER_LASTNAME = applicant.LASTNAME;
                            file.APPLICATION_STATUS = getCheckboxStatus();
                            file.BRM_BARCODE = txtBRM.InnerText;
                            //file.TRANS_DATE = DateTime.ParseExact(applicant.TRANS_DATE.Trim(), "yyyy-MM-dd", null); TODO

                            context.DC_FILE.Add(file);

                            context.SaveChanges();

                            divFileUnqBarcode.Alt = file.UNQ_FILE_NO;
                            unq_ref = file.UNQ_FILE_NO;
                            batchno = file.BATCH_NO.ToString();
                            boxno = file.MIS_BOXNO;

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
                            fullErrorMessage = ex.Message;
                            fullErrorMessage += " InnerException:" + ex.InnerException;
                        }

                        lblMsg.Text = fullErrorMessage;
                        divError.Visible = true;

                    }
                    catch (Exception ex)
                    {
                        lblMsg.Text = ex.Message;
                        lblMsg.Text += " InnerException:" + ex.InnerException;
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
                    lblSuccess.Text = "File added to batch <b>" + batchno + "</b> successfully. The unique file number is <b>" + unq_ref + "</b>";
                }
                else
                {
                    lblSuccess.Text = "MIS File audited successfully. The unique file number is <b>" + unq_ref + "</b>";
                }

                ScriptManager.RegisterStartupScript(this, GetType(), "print", "printpage('Y'); try { window.opener.updateGrid(); } catch (err) { } ", true);
            }
        }

        protected string getCheckboxStatus()
        {
            string status = string.Empty;

            if (checkApprove.Checked)
            {
                status = "MAIN";
            }
            else if (checkReject.Checked)
            {
                status = "ARCHIVE";
            }
            else if (checkReview.Checked)
            {
                status = "LOOSE CORRESPONDENCE";
            }

            return status;
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

        private decimal CreateBatchForOffice()
        {
            DC_BATCH b = new DC_BATCH();
            b.BATCH_STATUS = "Open";
            b.BATCH_CURRENT = "Y";
            b.OFFICE_ID = new BLUtility().getSessionLocalOfficeId();
            b.UPDATED_BY = new SASSA_Authentication().getUserID();
            b.UPDATED_DATE = DateTime.Now;

            en.DC_BATCH.Add(b);
            en.SaveChanges();

            return b.BATCH_NO;
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

        private void UpdateRegistryTypeCheckbox(string appStatus)
        {
            switch (appStatus)
            {
                case "MAIN": checkApprove.Checked = true; break;
                case "ARCHIVE": checkReject.Checked = true; break;
                case "LOOSE CORRESPONDENCE": checkReview.Checked = true; break;
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

        private List<int> getSOCPENDocs(string idNo, string grantype)
        {
            try
            {
                using (Entities ent = new Entities())
                {
                    IEnumerable<int> query = ent.Database.SqlQuery<int>
                                                                       (@"select DOC_NO_IN_IN
                                                                            from SOCPENDOCSs 
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
            catch (Exception ex)
            {
                return new List<int>();
            }
        }
    }
}