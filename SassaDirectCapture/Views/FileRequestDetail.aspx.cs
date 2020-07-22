using SASSADirectCapture.EntityModels;
using SASSADirectCapture.Sassa;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;

namespace SASSADirectCapture.Views
{
    public partial class FileRequestDetail : SassaPage
    {

        #region Page Load

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Request.QueryString.Count > 0)
                {
                    ddlReqCategory.DataSource = util.getRequestCategory();
                    ddlReqCategory.DataBind();

                    ddlStakeholder.DataBind();
                    ddlStakeholder.Enabled = false;

                    loadFieldsfromRecord(Request.QueryString["fileNo"].ToString());
                }

                //TODO validation on querystrings
                ClientScript.RegisterStartupScript(Page.GetType(), "setButtons", "enableDisableSave();", true);
            }
        }

        #endregion Page Load

        #region Load Data from Database

        protected void loadFieldsfromRecord(string unqFileNo)
        {
            using (Entities en = new Entities())
            {
                try
                {
                    var query = from fr in en.DC_FILE_REQUEST
                                join region in en.DC_REGION on fr.REGION_ID equals region.REGION_ID
                                join grt in en.DC_GRANT_TYPE on fr.GRANT_TYPE equals grt.TYPE_ID into grt1
                                from grant in grt1.DefaultIfEmpty()
                                join lo in en.DC_LOCAL_OFFICE on fr.REQUESTED_OFFICE_ID equals lo.OFFICE_ID
                                join k in en.KUAFs on fr.SCANNED_BY equals k.ID into k1
                                from scanners in k1.DefaultIfEmpty()
                                where fr.UNQ_FILE_NO == unqFileNo

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
                                    RELATED_MIS_FILE_NO = fr.RELATED_MIS_FILE_NO,
                                    NAME = fr.NAME,
                                    SURNAME = fr.SURNAME,
                                    GRANT_NAME = grant.TYPE_NAME,
                                    APPLICATION_DATE = fr.APP_DATE.Value,
                                    REQUESTED_DATE = fr.REQUESTED_DATE,
                                    REQUESTED_OFFICE_NAME = lo.OFFICE_NAME,
                                    SCANNED_PHYSICAL_IND = fr.SCANNED_PHYSICAL_IND,
                                    REQUEST_CAT_ID = fr.REQ_CATEGORY,
                                    REQUEST_CAT_TYPE_ID = fr.REQ_CATEGORY_TYPE,
                                    REQUEST_CAT_DETAIL = fr.REQ_CATEGORY_DETAIL,
                                    SCANNED_BY = fr.SCANNED_BY,
                                    SCANNER_NAME = scanners.FIRSTNAME + " " + scanners.LASTNAME,
                                    SCANNED_DATE = fr.SCANNED_DATE,
                                    SENT_TDW = fr.SENT_TDW,
                                    RECEIVED_TDW = fr.RECEIVED_TDW,
                                    FILE_RETRIEVED = fr.FILE_RETRIEVED,
                                    SEND_TO_REQUESTOR = fr.SEND_TO_REQUESTOR,
                                    PICKLIST_STATUS = fr.PICKLIST_STATUS,
                                    REGION_NAME = region.REGION_NAME,
                                    STAKEHOLDER_ID = fr.STAKEHOLDER,
                                    SERV_BY = fr.SERV_BY
                                };

                    if (query.Any())
                    {
                        foreach (FileRequest fileReq in query)
                        {
                            hfStatus.Value = fileReq.PICKLIST_STATUS;

                            txtHiddenUnqFileNo.Text = fileReq.UNQ_FILE_NO;
                            txtIDNo.Text = fileReq.ID_NO;
                            txtFileNo.Text = fileReq.MIS_FILE_NO;
                            txtName.Text = fileReq.NAME;
                            txtSurname.Text = fileReq.SURNAME;
                            txtRelatedMISNo.Text = fileReq.RELATED_MIS_FILE_NO;
                            txtRegion.Text = fileReq.REGION_NAME;

                            txtBRMno.Text = fileReq.BRM_BARCODE;
                            txtBin.Text = fileReq.BIN_ID;
                            txtBox.Text = fileReq.BOX_NUMBER;
                            txtPosition.Text = fileReq.POSITION;
                            txtTDWBox.Text = fileReq.TDW_BOXNO;
                            txtServBy.Text = fileReq.SERV_BY;

                            //check boxes

                            chkSentToTDW.Checked = fileReq.SENT_TDW == "Y" ? true : false;
                            chkReceivedTDW.Checked = fileReq.RECEIVED_TDW == "Y" ? true : false;
                            chkRetrieved.Checked = fileReq.FILE_RETRIEVED == "Y" ? true : false;
                            chkSent.Checked = fileReq.SEND_TO_REQUESTOR == "Y" ? true : false;
                            if ((fileReq.SCANNED_DATE != null) && (fileReq.SCANNED_BY != null))
                            {
                                chkScanned.Checked = true;
                            }
                            else
                            {
                                chkScanned.Checked = false;
                            }
                            optRadioScanned.Checked = fileReq.SCANNED_PHYSICAL_IND == "S" ? true : false;
                            optRadioPhysical.Checked = fileReq.SCANNED_PHYSICAL_IND == "P" ? true : false;

                            //txtAppDate.Text = fileReq.APPLICATION_DATE.HasValue ? fileReq.APPLICATION_DATE.Value.ToString("yyyy/MM/dd") : "";
                            txtDetail.Text = fileReq.REQUEST_CAT_DETAIL;

                            //drop down lists

                            ddlReqCategory.SelectedValue = fileReq.REQUEST_CAT_ID.HasValue ? fileReq.REQUEST_CAT_ID.ToString() : "";

                            //if (ddlReqCategory.SelectedValue != "")
                            //{
                            //    ddlReqCategoryType.DataSource = util.getRequestCategoryType(int.Parse(ddlReqCategory.SelectedValue));
                            //}
                            //else
                            //{
                            //    Dictionary<string, string> dict = new Dictionary<string, string>();
                            //    dict.Add(string.Empty, "<--Please Select-->");
                            //    ddlReqCategoryType.DataSource = dict;
                            //}

                            //ddlReqCategoryType.DataBind();

                            LoadCategoryType();
                            ddlReqCategoryType.SelectedValue = fileReq.REQUEST_CAT_TYPE_ID.HasValue ? fileReq.REQUEST_CAT_TYPE_ID.ToString() : "";

                            LoadStakeHolders();
                            ddlStakeholder.SelectedValue = fileReq.STAKEHOLDER_ID.HasValue ? fileReq.STAKEHOLDER_ID.ToString() : "";

                            headerInfo.InnerHtml = "The following file has been requested by the <b>" + fileReq.REQUESTED_OFFICE_NAME + "</b> local office.";

                            //if ((fileReq.CLOSED_DATE == null) || (fileReq.CLOSED_BY == null))
                            //{
                            //    string isCompleted = "N";
                            //    //SCENARIO TDW
                            //    if ((chkSentToTDW.Checked) && (chkReceivedTDW.Checked) && (chkScanned.Checked) && (chkSent.Checked))
                            //    {
                            //        fileReq.CLOSED_DATE = DateTime.Now;
                            //        fileReq.CLOSED_BY = authObject.getUserID();
                            //        fileReq.PICKLIST_STATUS = "Completed";
                            //        btnSave.Text = "Complete";
                            //        isCompleted = "Y";
                            //    }

                            //    // SCENARIO RMC
                            //    if ((chkRetrieved.Checked) && (chkScanned.Checked) && (chkSent.Checked) && (!chkSentToTDW.Checked))
                            //    {
                            //        fileReq.CLOSED_DATE = DateTime.Now;
                            //        fileReq.CLOSED_BY = authObject.getUserID();
                            //        fileReq.PICKLIST_STATUS = "Completed";
                            //        btnSave.Text = "Complete";
                            //        isCompleted = "Y";
                            //    }
                            //    if (isCompleted == "N")
                            //    {
                            //        btnSave.Text = "Accept";
                            //        btnSave.Enabled = true;
                            //        btnSave.CssClass += " active";
                            //    }
                            //}
                            //else
                            //{
                            //    btnSave.Text = "Accept";
                            //    btnSave.Enabled = false;
                            //    //btnSave.CssClass += " active";
                            //}

                            //if (fileReq.SCANNED_BY != null) //&& ((fileReq.FILE_RETRIEVED == "Y")||(fileReq.RECEIVED_TDW == "Y")) && (fileReq.SEND_TO_REQUESTOR == "Y"))
                            //{
                            //    chkScanned.Checked = true;
                            //    if ((int)fileReq.SCANNED_BY == authObject.getUserID())
                            //    {
                            //        btnSave.Text = "Complete";
                            //        headerInfo.InnerHtml += "<br/>The file has been picked by <b>yourself</b> for processing";
                            //    }
                            //    else
                            //    {
                            //        btnSave.Enabled = false;
                            //        btnSave.Visible = false;
                            //        //divScanCheck.Visible = false;

                            //        if (fileReq.SCANNER_NAME != string.Empty)
                            //        {
                            //            headerInfo.InnerHtml += "<br/>The file has been picked by <b>" + fileReq.SCANNER_NAME + "</b> for processing";
                            //        }
                            //    }
                            //}
                            //else
                            //{
                            //    chkScanned.Checked = false;
                            //    btnSave.Text = "Accept";
                            //    btnSave.Enabled = true;
                            //    btnSave.CssClass += " active";

                            //    //divScanCheck.Visible = false;
                            //}

                            break; //There can only be one record for that unique file no
                        }

                        //lblComplete.Text = optRadioScanned.Checked ? "Scanned to Livelink?" : "Ready for Collection?";
                    }
                }
                catch (Exception ex)
                {
                    lblMsg.Text = ex.Message;
                    divError.Style.Add("display", "");
                    return;
                }
            }
        }

        private void LoadCategoryType()
        {
            if (ddlReqCategory.SelectedValue != "")
            {
                ddlReqCategoryType.DataSource = util.getRequestCategoryType(int.Parse(ddlReqCategory.SelectedValue));
            }
            else
            {
                Dictionary<string, string> dict = new Dictionary<string, string>();
                dict.Add(string.Empty, "<--Please Select-->");
                ddlReqCategoryType.DataSource = dict;
            }

            ddlReqCategoryType.DataBind();
        }

        private void LoadStakeHolders()
        {
            if (ddlReqCategory.SelectedValue != "")
            {
                ddlStakeholder.DataSource = util.getDepartmentStakeholders(int.Parse(ddlReqCategory.SelectedValue));
            }
            else
            {
                Dictionary<string, string> dict = new Dictionary<string, string>();
                dict.Add(string.Empty, "<--Please Select-->");
                ddlStakeholder.DataSource = dict;
            }

            ddlStakeholder.DataBind();
        }

        #endregion Load Data from Database

        #region Button Click Events

        protected void btnClose_Click(object sender, EventArgs e)
        {
            ClientScript.RegisterStartupScript(Page.GetType(), "close", "window.opener.UpdateMessage(" +
                                                                                    "'close', " +
                                                                                    "\"\", " +
                                                                                    "\"\", " +
                                                                                    "\"\"); " +
                                                                                    "window.close()", true);
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            using (Entities en = new Entities())
            {
                try
                {
                    string acceptFile = "picked";
                    string errMess = String.Empty;
                    bool bUseADAuth = bool.Parse(System.Web.Configuration.WebConfigurationManager.AppSettings["UseADAuth"].ToString());

                    DC_FILE_REQUEST fileReq = en.DC_FILE_REQUEST.Find(txtHiddenUnqFileNo.Text);

                    if (fileReq != null)
                    {
                        if (hfBtnVal.Value == "Complete")
                        {
                            //if ((!chkRetrieved.Checked)&&(!chkReceivedTDW.Checked))
                            //{
                            //    errMess = "* The file is neither received from TDW not retrieved fom RMC. ";
                            //}
                            //if ((!chkScanned.Checked) || (!chkSent.Checked))
                            //{
                            //    errMess += "* The file must be scanned and sent to the requestor. ";
                            //}
                            //if ((chkSentToTDW.Checked) && (!chkReceivedTDW.Checked))
                            //{
                            //    errMess += "* The file was requested from TDW, but not received. ";
                            //}
                            //if (!errMess.Equals(null))
                            //{
                            //    errMess += "- CANNOT COMPLETE.";
                            //    ClientScript.RegisterStartupScript(Page.GetType(), "error1", "window.opener.UpdateMessage(" +
                            //                                                        "'false', " +
                            //                                                        "\"" + errMess + "\", " +
                            //                                                        "\"" + txtIDNo.Text + "\", " +
                            //                                                        "\"" + txtFileNo.Text + "\"); " +
                            //                                                        "window.close()", true);
                            //    return;
                            //}

                            if ((fileReq.SCANNED_DATE == null) || (fileReq.SCANNED_BY == null))
                            {
                                if (chkScanned.Checked)
                                {
                                    fileReq.SCANNED_DATE = DateTime.Now;

                                    fileReq.SCANNED_BY_AD = Usersession.SamName;

                                }
                            }

                            ////SCENARIO TDW
                            //if ((chkSentToTDW.Checked) && (chkReceivedTDW.Checked) && (chkScanned.Checked) && (chkSent.Checked))
                            //{
                            //    fileReq.CLOSED_DATE = DateTime.Now;
                            //    fileReq.CLOSED_BY = authObject.getUserID();
                            //    fileReq.PICKLIST_STATUS = "Completed";
                            //    acceptFile = "completed";
                            //}

                            //// SCENARIO RMC
                            //if ((chkRetrieved.Checked) && (chkScanned.Checked) && (chkSent.Checked) && (!chkSentToTDW.Checked))
                            //{
                            //    fileReq.CLOSED_DATE = DateTime.Now;
                            //    fileReq.CLOSED_BY = authObject.getUserID();
                            //    fileReq.PICKLIST_STATUS = "Completed";
                            //    acceptFile = "completed";
                            //}

                            fileReq.CLOSED_DATE = DateTime.Now;


                            fileReq.CLOSED_BY_AD = Usersession.SamName;


                            fileReq.PICKLIST_STATUS = "Completed";
                            acceptFile = "completed";

                            fileReq.SENT_TDW = chkSentToTDW.Checked ? "Y" : "N";
                            fileReq.RECEIVED_TDW = chkReceivedTDW.Checked ? "Y" : "N";
                            fileReq.FILE_RETRIEVED = chkRetrieved.Checked ? "Y" : "N";
                            fileReq.SEND_TO_REQUESTOR = chkSent.Checked ? "Y" : "N";

                            DC_FILE file = en.DC_FILE.Find(txtHiddenUnqFileNo.Text);

                            if (file != null)
                            {
                                file.REGION_ID_FROM = file.REGION_ID;
                                file.REGION_ID = fileReq.REGION_ID;
                            }
                            en.DC_ACTIVITY.Add(util.CreateActivity("FileRequest", "Update file and Request"));
                            en.SaveChanges();

                            ClientScript.RegisterStartupScript(Page.GetType(), "saveC", "window.opener.UpdateMessage(" +
                                                                                        "'true', " +
                                                                                        "\"File <b>" + fileReq.MIS_FILE_NO + "</b> with ID No: <b>" + fileReq.ID_NO + "</b> successfully " + acceptFile + "\", " +
                                                                                        "\"" + fileReq.ID_NO + "\", " +
                                                                                        "\"" + fileReq.MIS_FILE_NO + "\"); " +
                                                                                        "window.close()", true);
                        }
                        else
                        {
                            if (hfBtnVal.Value == "Accept" || hfBtnVal.Value == "Update")
                            {
                                fileReq.PICKLIST_STATUS = "In Progress";

                                if ((fileReq.SCANNED_DATE == null) || (fileReq.SCANNED_BY == null))
                                {
                                    if (chkScanned.Checked)
                                    {
                                        fileReq.SCANNED_DATE = DateTime.Now;

                                        fileReq.SCANNED_BY_AD = Usersession.SamName;

                                    }
                                }

                                ////SCENARIO TDW
                                //if ((chkSentToTDW.Checked) && (chkReceivedTDW.Checked) && (chkScanned.Checked) && (chkSent.Checked))
                                //{
                                //    fileReq.CLOSED_DATE = DateTime.Now;
                                //    fileReq.CLOSED_BY = authObject.getUserID();
                                //    fileReq.PICKLIST_STATUS = "Completed";
                                //    acceptFile = "completed";
                                //}

                                //// SCENARIO RMC
                                //if ((chkRetrieved.Checked) && (chkScanned.Checked) && (chkSent.Checked) && (!chkSentToTDW.Checked))
                                //{
                                //    fileReq.CLOSED_DATE = DateTime.Now;
                                //    fileReq.CLOSED_BY = authObject.getUserID();
                                //    fileReq.PICKLIST_STATUS = "Completed";
                                //    acceptFile = "completed";
                                //}

                                if (hfBtnVal.Value == "Update")
                                {
                                    acceptFile = "updated";
                                }

                                if (!chkScanned.Checked)
                                {
                                    fileReq.SCANNED_DATE = null;
                                    fileReq.SCANNED_BY = null;
                                }

                                fileReq.SENT_TDW = chkSentToTDW.Checked ? "Y" : "N";
                                fileReq.RECEIVED_TDW = chkReceivedTDW.Checked ? "Y" : "N";
                                fileReq.FILE_RETRIEVED = chkRetrieved.Checked ? "Y" : "N";
                                fileReq.SEND_TO_REQUESTOR = chkSent.Checked ? "Y" : "N";
                                en.DC_ACTIVITY.Add(util.CreateActivity("FileRequest", "Update File Request"));
                                en.SaveChanges();

                                ClientScript.RegisterStartupScript(Page.GetType(), "saveA", "window.opener.UpdateMessage(" +
                                                                                        "'true', " +
                                                                                        "\"File <b>" + fileReq.MIS_FILE_NO + "</b> with ID No: <b>" + fileReq.ID_NO + "</b> successfully " + acceptFile + "\", " +
                                                                                        "\"" + fileReq.ID_NO + "\", " +
                                                                                        "\"" + fileReq.MIS_FILE_NO + "\"); " +
                                                                                        "window.close()", true);
                            }
                            else
                            {
                                ClientScript.RegisterStartupScript(Page.GetType(), "invalidbutton", "alert('button has invalid value.');");
                            }
                        }
                    }
                    else
                    {
                        ClientScript.RegisterStartupScript(Page.GetType(), "error2", "window.opener.UpdateMessage(" +
                                                                                    "'false', " +
                                                                                    "\"Error updating file, can't find record\", " +
                                                                                    "\"" + txtIDNo.Text + "\", " +
                                                                                    "\"" + txtFileNo.Text + "\"); " +
                                                                                    "window.close()", true);
                    }
                }
                catch (Exception ex)
                {
                    ClientScript.RegisterStartupScript(Page.GetType(), "error3", "window.opener.UpdateMessage(" +
                                                                                    "'false', " +
                                                                                    "\"" + ex.Message + "\", " +
                                                                                    "\"" + txtIDNo.Text + "\", " +
                                                                                    "\"" + txtFileNo.Text + "\"); " +
                                                                                    "window.close()", true);
                }
            }
        }

        #endregion Button Click Events

        #region Protected Methods

        protected void ddlReqCategory_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadCategoryType();
            LoadStakeHolders();
        }

        #endregion Protected Methods
    }
}