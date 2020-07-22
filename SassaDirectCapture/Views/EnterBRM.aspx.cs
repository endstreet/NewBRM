using SASSADirectCapture.EntityModels;
using SASSADirectCapture.Sassa;
//using SASSADirectCapture.Services;
using System;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SASSADirectCapture.Views
{
    public partial class EnterBRM : SassaPage
    {
        //public SASSA_Authentication authObject = new SASSA_Authentication();
        private Entities en = new Entities();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                hf_BRM_BARCODE.Value = GetBRMBarcode();


                // Get the username of the user that is logged in from session.
                //string authenticatedUsername = Usersession.SamName;

                //If no session values are found, redirect to the login screen
                //if (authenticatedUsername == string.Empty)
                //{
                //    authObject.RedirectToLoginPage();
                //}

                HttpContext.Current.Session["BRM"] = "";

                ////Validate query strings
                //ValidateParams();

                //Set focus on BRMBarcode text field.
                txtBRMBarcode.Focus();

                ddlLCType.DataSource = util.getLCTypes();
                ddlLCType.DataBind();
            }
        }

        protected void ValidateParams()
        {
            string boxAudit = Request.QueryString["boxaudit"];
            string pensionNo = Request.QueryString["pensionNo"];
            string batching = Request.QueryString["batching"];
            //string trans = Request.QueryString["trans"];
            string grant = Request.QueryString["gt"];
            string appdate = Request.QueryString["appdate"];
            string mymess = "The following fields were not filled in: ";
            bool mistake = true;

            //Make sure that the pension number / id number is available
            if (!string.IsNullOrEmpty(pensionNo))
            {
                //Check that transaction type is available
                //if (trans.Length > 0)
                //{
                //    mistake = false;
                //}
                //else
                //{
                //    mymess += "[Transaction Type] ";
                //}

                //If this comes from the Box Audit function
                if (!string.IsNullOrEmpty(boxAudit))
                {
                    if (boxAudit.ToUpper() == "Y")
                    {
                        mistake = false;
                    }
                }
                //If this is from the Applicant search function, check the batch
                else if (string.IsNullOrEmpty(batching))
                {
                    mymess += "[Batching Indicator] ";
                }
                else if (string.IsNullOrEmpty(grant))
                {
                    mymess += "[Grant Type] ";
                }
                else if (string.IsNullOrEmpty(appdate))
                {
                    mymess += "[Application Date] ";
                }
            }
            else
            {
                mymess += "[ID number] ";
            }

            if (mistake)
            {
                ClientScript.RegisterStartupScript(Page.GetType(), "close", "alert('" + mymess + "');window.close();", true);
            }
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            //ClientScript.RegisterStartupScript(Page.GetType(), "close", "window.close();", true);

            string boxAudit = Request.QueryString["boxaudit"];
            if (!string.IsNullOrEmpty(boxAudit) && boxAudit.ToUpper() == "Y")
            {
                ClientScript.RegisterStartupScript(Page.GetType(), "SetBRM", "window.opener.document.getElementById('MainContent_btnRevertBRM').click(); window.close();", true);
            }
            else
            {
                ClientScript.RegisterStartupScript(Page.GetType(), "Close", "window.close();", true);
            }
        }

        private string GetBRMBarcode()
        {
            string brmBARCODE = string.Empty;
            string pensionNo = Request.QueryString["pensionNo"];
            string granttype = Request.QueryString["gt"];
            string sChildID = Request.QueryString["ChildID"];

            using (Entities context = new Entities())
            {
                DC_FILE f = context.DC_FILE.Where(k => k.APPLICANT_NO == pensionNo
                                                      && k.GRANT_TYPE == granttype
                                                      && ((granttype != "C" && granttype != "5") || (sChildID == null ? k.CHILD_ID_NO == null : k.CHILD_ID_NO == sChildID))
                                                  )
                                                  .FirstOrDefault();


                brmBARCODE = f?.BRM_BARCODE ?? string.Empty;
            }

            return brmBARCODE;
        }

        protected void btnUpdateBRM_Click(object sender, EventArgs e)
        {
            string barCode = txtBRMBarcode.Text.Trim().ToUpper();
            try
            {

                string pensionNo = Request.QueryString["pensionNo"];
                string boxAudit = Request.QueryString["boxaudit"];
                string boxNo = Request.QueryString["boxNo"];
                string batching = Request.QueryString["batching"];
                string trans = Request.QueryString["trans"];
                string grantname = Request.QueryString["gn"];
                string granttype = Request.QueryString["gt"];
                string appdate = Request.QueryString["appdate"];
                string SRDNo = Request.QueryString["SRDNo"];
                string tempBatch = Request.QueryString["tempBatch"];
                string sMGMerge = Request.QueryString["MGMerge"]; //Multi-grant Merge
                string sChildID = Request.QueryString["ChildID"];
                string sIsReview = rbReview.Checked ? "Y" : "N";
                string sLCType = rbLC.Checked ? ddlLCType.SelectedValue : "";

                hf_PENSION_NUMBER.Value = pensionNo;
                hf_BATCHING.Value = batching;
                hf_TRANSACTION.Value = trans;
                hf_BOX_AUDIT.Value = boxAudit;
                hf_BOX_NUMBER.Value = boxNo;
                hf_GRANT_NAME.Value = grantname;
                hf_APPLICATION_DATE.Value = appdate;
                hf_GRANT_TYPE.Value = granttype;
                hf_SRD_NUMBER.Value = SRDNo;
                hf_TEMP_BATCH.Value = tempBatch;
                hf_CHILD_ID.Value = sChildID;
                hf_IS_REVIEW.Value = sIsReview;
                hf_LC_TYPE.Value = sLCType;

                if (string.IsNullOrEmpty(barCode))
                {
                    ClientScript.RegisterStartupScript(Page.GetType(), "ignore", "alert('Please scan or enter the BRM Barcode.');", true);
                }
                else if (util.checkBRMExists(barCode))
                {
                    ClientScript.RegisterStartupScript(Page.GetType(), "BRMAlreadyUsed", "alert('Please scan or enter a different BRM Barcode,\\n" + txtBRMBarcode.Text + " is already in use.'); WebForm_AutoFocus('" + txtBRMBarcode.ClientID + "');", true);
                }
                else
                {
                    Session["BRM"] = barCode;
                    //HiddenField4.Value = Session["BRM"].ToString();
                    //If this comes from the Box Audit function
                    if (!string.IsNullOrEmpty(boxAudit) && boxAudit.ToUpper() == "Y")
                    {
                        //ClientScript.RegisterStartupScript(Page.GetType(), "localfileCover", "window.opener.openFileCover('" + pensionNo + "', 'Y', '" + boxNo + "', 'N', '" + trans + "', '" + txtBRMBarcode.Text.Trim().ToUpper() + "'); window.close();", true);
                        //ClientScript.RegisterStartupScript(Page.GetType(), "localfileCover", "window.opener.openFileCover('" + pensionNo + "', 'Y', '" + boxNo + "', 'N', '" + txtBRMBarcode.Text.Trim().ToUpper() + "'); window.close();", true);
                        ClientScript.RegisterStartupScript(Page.GetType(), "SetBRM", "CheckBRMUsedOnParentPage('" + barCode + "', '" + txtBRMBarcode.ClientID + "', '" + sMGMerge + "'); window.opener.SetBRMNumber(); window.close();", true);
                    }
                    else if (!string.IsNullOrEmpty(sMGMerge) && sMGMerge.ToUpper() == "Y")
                    {
                        ClientScript.RegisterStartupScript(Page.GetType(), "SetBRM", "CheckBRMUsedOnParentPage('" + barCode + "', '" + txtBRMBarcode.ClientID + "', '" + sMGMerge + "'); openFileCover('" + pensionNo + "','" + grantname + "','" + granttype + "', '" + appdate + "'); window.close();", true);
                    }
                    else
                    {
                        ClientScript.RegisterStartupScript(Page.GetType(), "fileCover", "openFileCover('" + pensionNo + "','" + grantname + "','" + granttype + "', '" + appdate + "'); window.close();", true);
                    }
                    //btnCancel_Click(sender, e);
                }
            }
            catch (Exception ex)
            {
                TextBox1.Text = ex.Message + " " + ex.InnerException + " " + ex.Source;
            }
        }

        protected void rbApplicationType_CheckedChanged(object sender, EventArgs e)
        {
            if (((RadioButton)sender).ID == "rbLCHidden")
            {
                lblLCType.Visible = true;
                ddlLCType.Visible = true;
            }
            else
            {
                lblLCType.Visible = false;
                ddlLCType.Visible = false;
            }
        }
    }
}
