using SASSADirectCapture.EntityModels;
using SASSADirectCapture.Hub;
using SASSADirectCapture.Sassa;
using SASSADirectCapture.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SASSADirectCapture.Views
{
    public partial class FileRequestEdit : SassaPage
    {
        #region Public Fields

        //public SASSA_Authentication authObject = new SASSA_Authentication();

        #endregion Public Fields

        #region Private Fields

        private string _schema = "";

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

        #region Protected Methods

        protected void closeCancelRequest()
        {
            if (txtHiddenUnqFileNo.Text.Trim() != string.Empty)
            {
                using (Entities en = new Entities())
                {
                    try
                    {
                        DC_FILE_REQUEST fileReq = en.DC_FILE_REQUEST.Find(txtHiddenUnqFileNo.Text);

                        if (fileReq != null)
                        {
                            fileReq.CLOSED_DATE = DateTime.Now;

                            fileReq.CLOSED_BY_AD = UserSession.SamName;

                            en.DC_ACTIVITY.Add(util.CreateActivity("FileRequest", "Close/Cancel File Request"));
                            en.SaveChanges();
                        }
                        ClientScript.RegisterStartupScript(Page.GetType(), "save", "window.opener.UpdateMessage(" +
                                                                                    "'true', " +
                                                                                    "\"File <b>" + fileReq.MIS_FILE_NO + "</b> with ID No: <b>" + fileReq.ID_NO + "</b> successfully closed\", " +
                                                                                    "\"" + fileReq.ID_NO + "\", " +
                                                                                    "\"" + fileReq.MIS_FILE_NO + "\"); " +
                                                                                    "window.close()", true);
                    }
                    catch (Exception ex)
                    {
                        ClientScript.RegisterStartupScript(Page.GetType(), "error", "window.opener.UpdateMessage(" +
                                                                                    "'false', " +
                                                                                    "\"" + ex.Message + "\", " +
                                                                                    "\"" + txtIDNo.Text + "\", " +
                                                                                    "\"" + txtFileNo.Text + "\"); " +
                                                                                    "window.close()", true);
                    }
                }
            }
        }

        protected void ddlReqCategory_SelectedIndexChanged(object sender, EventArgs e)
        {
            lblMsg.Text = "";
            divError.Style.Add("display", "none");

            if (((DropDownList)sender).SelectedValue != string.Empty)
            {
                ddlReqCategoryType.DataSource = util.getRequestCategoryType(int.Parse(((DropDownList)sender).SelectedValue));
                ddlReqCategoryType.DataBind();
                ddlReqCategoryType.Enabled = true;

                ddlStakeholder.DataSource = util.getDepartmentStakeholders(int.Parse(((DropDownList)sender).SelectedValue));
                ddlStakeholder.DataBind();
                ddlStakeholder.Enabled = true;
            }
            else
            {
                Dictionary<string, string> dict = new Dictionary<string, string>();
                dict.Add(string.Empty, "<--Please Select-->");
                ddlReqCategoryType.DataSource = dict;
                ddlReqCategoryType.DataBind();
                ddlReqCategoryType.Enabled = false;

                ddlStakeholder.DataSource = dict;
                ddlStakeholder.DataBind();
                ddlStakeholder.Enabled = false;
            }
        }

        protected void ddlReqCategoryType_SelectedIndexChanged(object sender, EventArgs e)
        {
            lblMsg.Text = "";
            divError.Style.Add("display", "none");
        }

        protected void ddlStakeholder_SelectedIndexChanged(object sender, EventArgs e)
        {
            lblMsg.Text = "";
            divError.Style.Add("display", "none");
        }

        protected string getAllRelatedMISNumbers(string idNumber, string fileNumber)
        {
            string misNumbers = string.Empty;
            try
            {
                using (Entities context = new Entities())
                {
                    // this is now a table not a view
                    IEnumerable<string> query = context.Database.SqlQuery<string>
                             (@"select spn.FILE_NUMBER
                                from MISNATIONALs spn
                                where spn.ID_NUMBER = '" + idNumber + "' and spn.FILE_NUMBER != '" + fileNumber + "'");

                    //IEnumerable<string> query = context.Database.SqlQuery<string>
                    //                (@"select spn.FILE_NUMBER
                    //                    from " + schema + @".MISLIVELINKBRM spn
                    //                    where spn.ID_NUMBER = @p0 and spn.FILE_NUMBER != @p1", idNumber, fileNumber);

                    //var query = from spn in context.MIS_LIVELINK
                    //            where spn.ID_NUMBER == idNumber
                    //            && spn.FILE_NUMBER != fileNumber
                    //            select spn.FILE_NUMBER;

                    if (query.Any())
                    {
                        foreach (string value in query)
                        {
                            misNumbers += value + ";";
                        }

                        misNumbers = misNumbers.TrimEnd(';');
                    }
                }
            }
            catch (Exception ex)
            {
                lblMsg.Text = ex.Message;
                lblMsg.Text += " InnerException:" + ex.InnerException;
                divError.Style.Add("display", "");
            }
            return misNumbers;
        }

        protected void loadFieldsfromRecord(string unqFileNo)
        {
            btnSave.Visible = false;
            btnCancel.Visible = false;
            btnComplete.Visible = false;
            txtDetail.Enabled = false;
            ddlReqCategory.Enabled = false;
            ddlReqCategoryType.Enabled = false;
            ddlStakeholder.Enabled = false;
            optRadioScanned.Disabled = true;
            optRadioPhysical.Disabled = true;

            ddlReqCategory.DataSource = util.getRequestCategory();
            ddlReqCategory.DataBind();

            using (Entities en = new Entities())
            {
                try
                {
                    var query = from fr in en.DC_FILE_REQUEST
                                join region in en.DC_REGION on fr.REGION_ID equals region.REGION_ID
                                join grt in en.DC_GRANT_TYPE on fr.GRANT_TYPE equals grt.TYPE_ID into grt1
                                from grant in grt1.DefaultIfEmpty()
                                join k in en.KUAFs on fr.SCANNED_BY equals k.ID into k1
                                from scanners in k1.DefaultIfEmpty()
                                where fr.UNQ_FILE_NO == unqFileNo

                                select new FileRequest
                                {
                                    UNQ_FILE_NO = fr.UNQ_FILE_NO,
                                    ID_NO = fr.ID_NO,
                                    MIS_FILE_NO = fr.MIS_FILE_NO,
                                    NAME = fr.NAME,
                                    SURNAME = fr.SURNAME,
                                    REGION_ID = fr.REGION_ID,
                                    REQUEST_CAT_ID = fr.REQ_CATEGORY,
                                    REQUEST_CAT_TYPE_ID = fr.REQ_CATEGORY_TYPE,
                                    REQUEST_CAT_DETAIL = fr.REQ_CATEGORY_DETAIL,
                                    REGION_NAME = region.REGION_NAME,
                                    GRANT_TYPE = fr.GRANT_TYPE,
                                    GRANT_NAME = grant.TYPE_NAME,
                                    REQUESTED_BY = fr.REQUESTED_BY,
                                    REQUESTED_DATE = fr.REQUESTED_DATE,
                                    SCANNED_DATE = fr.SCANNED_DATE,
                                    SCANNED_BY = fr.SCANNED_BY,
                                    SCANNER_NAME = scanners.FIRSTNAME + " " + scanners.LASTNAME,
                                    CLOSED_DATE = fr.CLOSED_DATE,
                                    CLOSED_BY = fr.CLOSED_BY,
                                    SCANNED_PHYSICAL_IND = fr.SCANNED_PHYSICAL_IND,
                                    BRM_BARCODE = fr.BRM_BARCODE,
                                    BIN_ID = fr.BIN_ID,
                                    BOX_NUMBER = fr.BOX_NUMBER,
                                    POSITION = fr.POSITION,
                                    TDW_BOXNO = fr.TDW_BOXNO,
                                    SERV_BY = fr.SERV_BY,
                                    APPLICATION_STATUS = fr.APPLICATION_STATUS
                                };

                    if (query.Any())
                    {
                        foreach (FileRequest fileReq in query)
                        {
                            txtIDNo.Text = fileReq.ID_NO;
                            txtFileNo.Text = fileReq.MIS_FILE_NO;
                            txtName.Text = fileReq.NAME;
                            txtSurname.Text = fileReq.SURNAME;
                            txtRegion.Text = fileReq.REGION_NAME;
                            txtHiddenUnqFileNo.Text = fileReq.UNQ_FILE_NO;
                            txtBRM.Text = fileReq.BRM_BARCODE;
                            txtBin.Text = fileReq.BIN_ID;
                            txtBox.Text = fileReq.BOX_NUMBER;
                            txtPosition.Text = fileReq.POSITION;
                            txtTDWBox.Text = fileReq.TDW_BOXNO;
                            txtServBy.Text = fileReq.SERV_BY;
                            hfAppStatus.Value = fileReq.APPLICATION_STATUS;

                            //txtGrant.Text = fileReq.GRANT_NAME;
                            //txtGrantID.Text = fileReq.GRANT_TYPE;
                            //txtAppDate.Text = fileReq.APP_DATE;
                            txtDetail.Text = fileReq.REQUEST_CAT_DETAIL;
                            ddlReqCategory.SelectedValue = fileReq.REQUEST_CAT_ID.HasValue ? fileReq.REQUEST_CAT_ID.ToString() : "";

                            if (ddlReqCategory.SelectedValue != "")
                            {
                                ddlReqCategoryType.DataSource = util.getRequestCategoryType(int.Parse(ddlReqCategory.SelectedValue));
                                ddlStakeholder.DataSource = util.getDepartmentStakeholders(int.Parse(ddlReqCategory.SelectedValue));
                            }
                            else
                            {
                                Dictionary<string, string> dict = new Dictionary<string, string>();
                                dict.Add(string.Empty, "<--Please Select-->");
                                ddlReqCategoryType.DataSource = dict;
                                ddlStakeholder.DataSource = dict;
                            }

                            ddlReqCategoryType.DataBind();
                            ddlStakeholder.DataBind();

                            ddlReqCategoryType.SelectedValue = fileReq.REQUEST_CAT_TYPE_ID.HasValue ? fileReq.REQUEST_CAT_TYPE_ID.ToString() : "";
                            ddlStakeholder.SelectedValue = fileReq.STAKEHOLDER_ID.HasValue ? fileReq.STAKEHOLDER_ID.ToString() : "";
                            optRadioScanned.Checked = fileReq.SCANNED_PHYSICAL_IND == "S" ? true : false;
                            optRadioPhysical.Checked = fileReq.SCANNED_PHYSICAL_IND == "P" ? true : false;

                            if (fileReq.SCANNED_DATE != null)
                            {
                                headerText.InnerText = "Complete File Request";
                                headerInfo.InnerHtml = "This file has been processed by <b>" + fileReq.SCANNER_NAME + "</b> on <b>" + fileReq.SCANNED_DATE + "</b><br/>Click on <b>Complete Request</b> to close this request.";
                                btnComplete.Visible = true;
                            }
                            else
                            {
                                headerText.InnerText = "Cancel File Request";
                                headerInfo.InnerHtml = "This file has not been processed by the RMC yet.<br/>Click on <b>Cancel Request</b> to cancel this request.";
                                btnCancel.Visible = true;
                            }

                            break; //There can only be one record for that unique file no
                        }
                    }
                }
                catch (Exception ex)
                {
                    lblMsg.Text = ex.Message;
                    divError.Style.Add("display", "");
                }
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                //Get the username of the user that is logged in from session.
                string authenticatedUsername = UserSession.SamName;

                //If no session values are found , redirect to the login screen

                if (Request.QueryString.Count > 0)
                {
                    //http://localhost:34186/Views/FileRequestEdit.aspx?
                    //'status=1&idNo=' + idNumber;
                    //'&fileNo=' + fileNumber;
                    //'&name=' + name;
                    //'&surname=' + surname;
                    //'&regionID=' + regionID;
                    //'&region=' + region;
                    //'&BRM=' + BRM_Number;
                    //'&Bin=' + MISBin;
                    //'&Box=' + MISBox;
                    //'&Pos=' + MISPos;
                    //'&TDW=' + TDWBoxno;

                    txtServBy.Text = UserSession.Name == null ? "" : UserSession.Name;
                    if (Request.QueryString["status"].ToString() == "1")
                    {
                        txtIDNo.Text = Request.QueryString["idNo"].ToString();
                        txtFileNo.Text = Request.QueryString["fileNo"].ToString();
                        txtName.Text = Request.QueryString["name"].ToString();
                        txtSurname.Text = Request.QueryString["surname"].ToString();
                        txtRegion.Text = Request.QueryString["region"].ToString();
                        txtRegionID.Text = Request.QueryString["regionID"].ToString();

                        txtBRM.Text = Request.QueryString["BRM"].ToString();
                        txtBin.Text = Request.QueryString["Bin"].ToString();
                        txtBox.Text = Request.QueryString["Box"].ToString();
                        txtPosition.Text = Request.QueryString["Pos"].ToString();
                        txtTDWBox.Text = Request.QueryString["TDW"].ToString();
                        hfAppStatus.Value = Request.QueryString["AppStatus"].ToString();

                        //txtGrant.Text = Request.QueryString["grant"].ToString();
                        //txtAppDate.Text = Request.QueryString["appdate"].ToString();
                        //txtGrantID.Text = Request.QueryString["grantID"].ToString();

                        ddlReqCategory.DataSource = util.getRequestCategory();
                        ddlReqCategory.DataBind();

                        Dictionary<string, string> dict = new Dictionary<string, string>();
                        dict.Add(string.Empty, "<--Please Select-->");

                        ddlReqCategoryType.DataSource = dict;
                        ddlReqCategoryType.DataBind();
                        ddlReqCategoryType.Enabled = false;

                        ddlStakeholder.DataSource = dict;
                        ddlStakeholder.DataBind();
                        ddlStakeholder.Enabled = false;

                        btnSave.Visible = true;
                        btnCancel.Visible = false;
                        btnComplete.Visible = false;
                    }
                    else if (Request.QueryString["status"].ToString() == "2")
                    {
                        loadFieldsfromRecord(Request.QueryString["fileNo"].ToString());
                    }
                }
                //TODO validation on querystrings
            }
        }

        #endregion Protected Methods

        #region Button Click Events

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            closeCancelRequest();
        }

        protected void btnClose_Click(object sender, EventArgs e)
        {
            ClientScript.RegisterStartupScript(Page.GetType(), "close", "window.opener.UpdateMessage(" +
                                                                                   "'close', " +
                                                                                   "\"\", " +
                                                                                   "\"\", " +
                                                                                   "\"\"); " +
                                                                                   "window.close()", true);
        }

        protected void btnComplete_Click(object sender, EventArgs e)
        {
            closeCancelRequest();
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            if (ddlReqCategoryType.SelectedIndex < 1 || ddlStakeholder.SelectedIndex < 1)
            {
                lblMsg.Text = "Please select a department, stakeholder and file request reason, before requesting this file.";
                divError.Style.Add("display", "");
                return;
            }

            using (Entities en = new Entities())
            {
                DC_FILE_REQUEST fileReq = new DC_FILE_REQUEST();

                try
                {
                    fileReq.ID_NO = txtIDNo.Text;
                    fileReq.MIS_FILE_NO = txtFileNo.Text;
                    fileReq.NAME = txtName.Text;
                    fileReq.SURNAME = txtSurname.Text;
                    fileReq.REGION_ID = txtRegionID.Text;
                    fileReq.REGION_ID_TO = UserSession.Office.RegionId;
                    fileReq.REQUESTED_OFFICE_ID = UserSession.Office.OfficeId;
                    fileReq.REQUESTED_DATE = DateTime.Now;


                    fileReq.REQUESTED_BY_AD = UserSession.SamName;


                    fileReq.RELATED_MIS_FILE_NO = getAllRelatedMISNumbers(txtIDNo.Text, txtFileNo.Text);
                    fileReq.REQ_CATEGORY = int.Parse(ddlReqCategory.SelectedValue);
                    fileReq.STAKEHOLDER = int.Parse(ddlStakeholder.SelectedValue);
                    fileReq.REQ_CATEGORY_TYPE = int.Parse(ddlReqCategoryType.SelectedValue);
                    fileReq.REQ_CATEGORY_DETAIL = txtDetail.Text.Trim();
                    //fileReq.GRANT_TYPE = txtGrantID.Text;
                    //fileReq.APP_DATE = txtAppDate.Text;
                    fileReq.SCANNED_PHYSICAL_IND = optRadioScanned.Checked ? "S" : "P";

                    fileReq.BRM_BARCODE = txtBRM.Text;
                    fileReq.BIN_ID = txtBin.Text;
                    fileReq.BOX_NUMBER = txtBox.Text;
                    fileReq.POSITION = txtPosition.Text;
                    fileReq.TDW_BOXNO = txtTDWBox.Text;

                    fileReq.SERV_BY = txtServBy.Text;

                    fileReq.APPLICATION_STATUS = hfAppStatus.Value;

                    en.DC_FILE_REQUEST.Add(fileReq);
                    en.DC_ACTIVITY.Add(util.CreateActivity("FileRequest", "Add File Request"));
                    en.SaveChanges();

                    ClientScript.RegisterStartupScript(Page.GetType(), "save", "window.opener.UpdateMessage(" +
                                                                                    "'true', " +
                                                                                    "\"File <b>" + fileReq.MIS_FILE_NO + "</b> with ID No: <b>" + fileReq.ID_NO + "</b> successfully requested\", " +
                                                                                    "\"" + fileReq.ID_NO + "\", " +
                                                                                    "\"" + fileReq.MIS_FILE_NO + "\"); " +
                                                                                    "window.close()", true);
                    //Todo:this may need to be regionCode
                    ProgressHub.SendMessage(UserSession.Office.RegionId, "New File Request Added", "15");
                }
                catch (Exception ex)
                {
                    ClientScript.RegisterStartupScript(Page.GetType(), "error", "window.opener.UpdateMessage(" +
                                                                                    "'false', " +
                                                                                    "\"" + ex.Message + "\", " +
                                                                                    "\"" + txtIDNo.Text + "\", " +
                                                                                    "\"" + txtFileNo.Text + "\"); " +
                                                                                    "window.close()", true);
                }
            }
        }

        #endregion Button Click Events
    }
}
