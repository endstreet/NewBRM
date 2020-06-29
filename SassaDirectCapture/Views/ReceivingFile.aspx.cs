using SASSADirectCapture.EntityModels;
using SASSADirectCapture.Sassa;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SASSADirectCapture.Views
{
    public partial class ReceivingFile : SassaPage
    {
        #region Private Fields

        //private SASSA_Authentication authObject = new SASSA_Authentication();

        private Entities en = new Entities();

        #endregion Private Fields

        #region Protected Methods

        protected void batchGridView_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            fileGridView.PageIndex = e.NewPageIndex;
            SearchBatchNo();
        }

        protected void btnSearchFileNo_Click(object sender, EventArgs e)
        {
            lblError.Text = "";
            divError.Visible = false;

            try
            {
                //string fileno = txtSearchBarcode.Text;
                string brmno = txtSearchBarcode.Text.Trim().ToUpper();

                if (brmno != string.Empty)
                {
                    bool bBarcodeFound = false;
                    foreach (GridViewRow row in fileGridView.Rows)
                    {
                        if (row.RowType == DataControlRowType.DataRow)
                        {
                            CheckBox chkRow = (row.Cells[0].FindControl("CheckBox1") as CheckBox);
                            string brmCell = row.Cells[5].Text.Trim().ToUpper();

                            //ClientScript.RegisterStartupScript(Page.GetType(), "updategrid", "alert(brmno:'"+ brmno + "');", true);
                            //ClientScript.RegisterStartupScript(Page.GetType(), "updategrid", "alert('Cells[5]:" + row.Cells[5].Text + "');", true);

                            //if (brmCell == brmno)
                            //{
                            //    if (!chkRow.Checked)
                            //    {
                            //        chkRow.Checked = true;
                            //        row.Cells[6].Text = txtBoxNo.Text.ToUpper();
                            //        updateFileStatus(brmno, true);
                            //    }
                            //    else
                            //    {
                            //        lblError.Text = "Batch already in the list for transport";
                            //        divError.Visible = true;
                            //    }
                            //}
                            if (brmCell == brmno)
                            {
                                bBarcodeFound = true;
                                //lblError.Text = "";
                                //divError.Visible = false;
                                if (!chkRow.Checked)
                                {
                                    //string abc = row.Cells[0].Text;
                                    //abc = row.Cells[1].Text;
                                    //abc = row.Cells[2].Text;
                                    //abc = row.Cells[3].Text;
                                    //abc = row.Cells[4].Text;
                                    //abc = row.Cells[5].Text;
                                    //abc = row.Cells[6].Text;
                                    //abc = row.Cells[7].Text;
                                    //abc = row.Cells[8].Text;
                                    //abc = row.Cells[9].Text;
                                    if (txtBoxType.Text.ToUpper().Trim() == row.Cells[7].Text.ToUpper().Trim())
                                    {
                                        if (txtAYear.Text == "" || txtAYear.Text == row.Cells[8].Text)
                                        {
                                            chkRow.Checked = true;
                                            row.Cells[6].Text = txtBoxNo.Text.ToUpper();
                                            updateFileStatus(brmno, true);
                                        }
                                        else
                                        {
                                            lblError.Text = "File archive year does not match box archive year.";
                                            divError.Visible = true;
                                        }
                                    }
                                    else
                                    {
                                        //ClientScript.RegisterStartupScript(Page.GetType(), "wrongtype", "alert('Wrong Registry Type');", true);
                                        lblError.Text = "Wrong Registry Type";
                                        divError.Visible = true;
                                    }
                                }
                                else
                                {
                                    // ClientScript.RegisterStartupScript(Page.GetType(), "alreadyinbatch", "alert('Batch already in the list for transport');", true);
                                    //lblError.Text = "Already in the list for transport";
                                    lblError.Text = "File already in box";
                                    divError.Visible = true;
                                }
                            }
                            //else
                            //{
                            //    //ClientScript.RegisterStartupScript(Page.GetType(), "notonpage", "alert('Barcode not on this page');", true);
                            //    lblError.Text = brmno + " - Barcode not on this page";
                            //    divError.Visible = true;
                            //}
                        }
                    }

                    if (!bBarcodeFound)
                    {
                        //ClientScript.RegisterStartupScript(Page.GetType(), "notonpage", "alert('Barcode not on this page');", true);
                        lblError.Text = brmno + " - Barcode not on this page";
                        divError.Visible = true;
                    }
                }
                txtSearchBarcode.Text = string.Empty;
            }
            catch (Exception ex)
            {
                txtSearchBarcode.Text = string.Empty;
                lblError.Text = lblError.Text + ex.Message;
                divError.Visible = true;
            }
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            DC_BATCH batch = GetCurrentBatch();
            if (batch == null)
            {
                return;
            }

            batch.BATCH_STATUS = ddlBatchStatus.SelectedValue;

            try
            {
                //If batch is marked as completed, ensure all files are received or comment is added at least.
                if (batch.BATCH_STATUS == "Completed")
                {
                    var query = GetAllFilesByBatchNo();

                    if (query.Any())
                    {
                        foreach (FileEntity value in query.OrderBy(x => x.UNQ_FILE_NO))
                        {
                            if (!value.FILE_STATUS_COMPLETED && (value.FILE_COMMENT == null || value.FILE_COMMENT == string.Empty))
                            {
                                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "reportBack", "UpdateBatch(false);", true);
                                lblError.Text = "Batch status could not be updated to " + batch.BATCH_STATUS + ". All files must either have a comment or be marked as received.";
                                divError.Visible = true;
                                lblSuccess.Text = "";

                                //If the batch was previously updated as completed and a user tries to delete a comment or uncheck a file as being received.
                                batch.BATCH_STATUS = "Received";
                                en.DC_ACTIVITY.Add(util.CreateActivity("Receiving", "Completed Batch"));
                                en.SaveChanges();
                                return;
                            }
                        }
                    }
                }

                en.SaveChanges();

                if (batch.BATCH_STATUS == "Completed")
                {
                    ScriptManager.RegisterStartupScript(Page, Page.GetType(), "SuccessAll", "SuccessAll();", true);
                }
                else
                {
                    ScriptManager.RegisterStartupScript(Page, Page.GetType(), "reportBack1", "UpdateBatch(true);", true);
                    lblSuccess.Text = "Batch status successfully updated to " + batch.BATCH_STATUS;
                    lblError.Text = "";
                    divError.Visible = false;
                    //btnClose.Visible = true;
                }
            }
            catch
            {
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "reportBack", "UpdateBatch(false);", true);
                lblError.Text = "Batch status could not be updated to " + batch.BATCH_STATUS;
                lblSuccess.Text = "";
                divError.Visible = true;
            }
        }

        protected void btnUnBox_Click(object sender, EventArgs e)
        {
            // get the brm no or file number and uncheck and change data in dc file
            // do the reverse of scan
            //string FID = e.CommandArgument.ToString();
            //string FID = e.ToString();

            Button mybutt = (Button)sender;
            string FID = mybutt.CommandArgument.ToString();

            if (FID != null)
            {
                try
                {
                    var x = en.DC_FILE
                            .Where(f => f.UNQ_FILE_NO == FID).FirstOrDefault();

                    if (x == null)
                    {
                        return;
                    }

                    x.FILE_STATUS = "Pending";
                    x.TDW_BOXNO = "";
                    x.UPDATED_DATE = DateTime.Now;

                    x.UPDATED_BY_AD = UserSession.SamName;

                    en.DC_ACTIVITY.Add(util.CreateActivity("Receiving", "Unbox files (Pending)"));
                    en.SaveChanges();

                    if (FID != string.Empty)
                    {
                        foreach (GridViewRow row in fileGridView.Rows)
                        {
                            if (row.RowType == DataControlRowType.DataRow)
                            {
                                CheckBox chkRow = (row.Cells[0].FindControl("CheckBox1") as CheckBox);
                                string clmCell = row.Cells[1].Text.Trim().ToUpper();

                                if (clmCell == FID)
                                {
                                    chkRow.Checked = false;
                                    row.Cells[6].Text = "";
                                }
                            }
                        }
                    }
                    ScriptManager.RegisterStartupScript(Page, Page.GetType(), "reportBack", "UpdateCheckBox(true);", true);
                    lblSuccess.Text = FID + " successfully unboxed.";
                    lblError.Text = "";
                    divError.Visible = false;
                }
                catch
                {
                    ScriptManager.RegisterStartupScript(Page, Page.GetType(), "reportBack", "UpdateCheckBox(false);", true);
                    lblError.Text = FID + " could not be unboxed.";
                    lblSuccess.Text = "";
                    divError.Visible = true;
                }
            }
            else
            {
                lblError.Text = FID + " was not passed.";
                lblSuccess.Text = "";
                divError.Visible = true;
            }
            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "reportBack", "UpdateGrid();", true);
        }

        protected void CheckBox1_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox cb = (CheckBox)sender;
            //string fileId = cb.Attributes["fileId"];
            string brmno = cb.Attributes["brmno"];
            string ayear = cb.Attributes["ayear"];

            if ((txtAYear.Text != ayear) && (txtAYear.Text != ""))
            {
                //lblError.Text = "Archive Year of the file should be the same a the box archive year";
                lblError.Text = "An incorrect Archive Year is captured for BRM File number " + brmno;
                lblError.Visible = true;
                return;
            }
            else
            {
                lblError.Visible = false;
            }

            if (string.IsNullOrEmpty(brmno))
            {
                return;
            }

            //updateFileStatus(fileId, cb.Checked);
            updateFileStatus(brmno, cb.Checked);

            ClientScript.RegisterStartupScript(Page.GetType(), "updategrid", "UpdateGrid();", true);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            //Get the username of the user that is logged in from session.
            //labJustSaying.Text = "";
            //If no session values are found , redirect to the login screen

            string myBoxno = "";
            string myBoxType = "";
            if ((Session["BoxNo"] != null) && (Session["BoxType"] != null))
            {
                if ((Session["BoxNo"].ToString() != string.Empty) && (Session["BoxType"].ToString() != string.Empty))
                {
                    labJustSaying.Text = "Please confirm the Box number and Registry Type.";
                    myBoxno = Session["BoxNo"].ToString();
                    myBoxType = Session["BoxType"].ToString();
                    txtBoxNo.Text = myBoxno;
                    txtBoxType.Text = myBoxType;
                    hfBoxTypeID.Value = Session["BoxTypeID"].ToString();
                    txtAYear.Text = Session["ArchiveYear"] == null ? "" : Session["ArchiveYear"].ToString();
                    Page.Header.Title = "Receiving File";
                    SetLabels();
                    SetBatchStatus();
                    SearchBatchNo();
                }
                else
                {
                    ScriptManager.RegisterStartupScript(Page, Page.GetType(), "getBox1", "getBox();", true);
                }
            }
            else
            {
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "getBox2", "getBox();", true);
            }
        }

        //protected void CheckBox1_CheckedChanged(object sender, EventArgs e)
        //{
        //    //string myBoxno = "";
        //    //if ((Session["BoxNo"] != null) && (Session["BoxNo"].ToString() != string.Empty))
        //    //{
        //    //    myBoxno = Session["BoxNo"].ToString();
        //    //    txtBoxNo.Text = myBoxno;
        protected void updateFileStatus(string brmno, bool check)
        {
            //var x = en.DC_FILE
            //        .Where(bn => bn.BATCH_NO == batchNo)
            //        .OrderBy(f => f.UNQ_FILE_NO)
            //        .Select(f => new FileEntity

            decimal batchNo = 0.0M;
            batchNo = Decimal.Parse(lblBatchNo.Text);

            try
            {
                var x = en.DC_FILE
                        .Where(bn => bn.BATCH_NO == batchNo)
                        .Where(f => f.BRM_BARCODE == brmno).FirstOrDefault();

                if (x == null)
                {
                    return;
                }

                if (check)
                {
                    x.FILE_STATUS = "Completed";
                    x.TDW_BOXNO = txtBoxNo.Text.ToUpper();
                    x.TDW_BOX_TYPE_ID = hfBoxTypeID.Value == "" ? (Decimal?)null : Decimal.Parse(hfBoxTypeID.Value);
                    x.TDW_BOX_ARCHIVE_YEAR = txtAYear.Text;
                }
                else
                {
                    x.FILE_STATUS = "Pending";
                    x.TDW_BOXNO = "";
                    x.TDW_BOX_TYPE_ID = null;
                    x.TDW_BOX_ARCHIVE_YEAR = "";
                }

                x.UPDATED_DATE = DateTime.Now;


                x.UPDATED_BY_AD = UserSession.SamName;

                en.DC_ACTIVITY.Add(util.CreateActivity("Receiving", "Update File Status(Completed/Pending)"));
                en.SaveChanges();

                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "reportBack", "UpdateCheckBox(true);", true);
                lblSuccess.Text = brmno + " successfully updated.";
                lblError.Text = "";
                divError.Visible = false;
            }
            catch
            {
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "reportBack", "UpdateCheckBox(false);", true);
                lblError.Text = brmno + " could not be updated.";
                lblSuccess.Text = "";
                divError.Visible = true;
            }
        }

        #endregion Protected Methods

        #region Private Methods

        private IQueryable<FileEntity> GetAllFilesByBatchNo()
        {
            decimal batchNo = 0.0M;
            if (Decimal.TryParse(Request.QueryString["batchNo"].ToString(), out batchNo))
            {
                //var x = en.DC_FILE
                //    .Where(bn => bn.BATCH_NO == batchNo)
                //    .OrderBy(f => f.UNQ_FILE_NO)
                //    .Select(f => new FileEntity
                //    {
                //        UNQ_FILE_NO = f.UNQ_FILE_NO,
                //        REGION_NAME = f.DC_LOCAL_OFFICE.DC_REGION.REGION_NAME,
                //        APPLICANT_NO = f.APPLICANT_NO,
                //        GRANT_TYPE_NAME = f.DC_GRANT_TYPE.TYPE_NAME,
                //        FILE_COMMENT = f.FILE_COMMENT,
                //        FILE_STATUS = f.FILE_STATUS,
                //        FIRST_NAME = f.USER_FIRSTNAME,
                //        LAST_NAME = f.USER_LASTNAME,
                //        TRANS_TYPE = f.TRANS_TYPE,
                //        CLM_UNIQUE_CODE = f.UNQ_FILE_NO,
                //        BRM_BARCODE = f.BRM_BARCODE,
                //        TDW_BOXNO = f.TDW_BOXNO,
                //        APPLICATION_STATUS = f.APPLICATION_STATUS
                //    });

                var x = en.DC_FILE
                   .Where(bn => bn.BATCH_NO == batchNo)
                   .OrderBy(f => f.UNQ_FILE_NO)
                   .Select(f => new FileEntity
                   {
                       UNQ_FILE_NO = f.UNQ_FILE_NO,
                       REGION_NAME = f.DC_LOCAL_OFFICE.DC_REGION.REGION_NAME,
                       APPLICANT_NO = f.APPLICANT_NO,
                       GRANT_TYPE_NAME = f.DC_GRANT_TYPE.TYPE_NAME,
                       FILE_COMMENT = f.FILE_COMMENT,
                       FILE_STATUS = f.FILE_STATUS,
                       FIRST_NAME = f.USER_FIRSTNAME,
                       LAST_NAME = f.USER_LASTNAME,
                       CLM_UNIQUE_CODE = f.UNQ_FILE_NO,
                       BRM_BARCODE = f.BRM_BARCODE,
                       TDW_BOXNO = f.TDW_BOXNO,
                       APPLICATION_STATUS = f.APPLICATION_STATUS,
                       APP_DATE_DT = f.TRANS_DATE,
                       ARCHIVE_YEAR = f.ARCHIVE_YEAR
                   });

                List<FileEntity> fe = new List<FileEntity>();
                foreach (var item in x)
                {
                    FileEntity f = item;
                    //var o = en.SOCPENIDs.Where(ss => ss.PENSION_NO == f.APPLICANT_NO);
                    //if (o != null)
                    //{
                    //    var pp = o.FirstOrDefault();
                    //    if (pp != null)
                    //    {
                    //        f.FULL_NAME = pp.NAME.Trim() + " " + pp.SURNAME.Trim();
                    //    }
                    //}

                    //f.FULL_NAME = f.FIRST_NAME + " " + f.LAST_NAME;

                    if (f.FILE_STATUS != null)
                    {
                        f.FILE_STATUS_COMPLETED = f.FILE_STATUS.Trim().ToLower().Equals("completed");
                    }

                    fe.Add(f);
                }

                return fe.AsQueryable();
            }
            else
            {
                return null;
            }
        }

        private DC_BATCH GetCurrentBatch()
        {
            string batchNrValue = Request.QueryString["batchNo"].ToString();
            var z = UserSession.Office.OfficeId;
            decimal outValue = 0.0M;
            if (Decimal.TryParse(batchNrValue, out outValue))
            {
                return en.DC_BATCH
                    .Where(c => c.BATCH_NO == outValue)
                    //.Where(oid => oid.OFFICE_ID == z)
                    .FirstOrDefault();
            }

            return null;
        }

        private void SearchBatchNo()
        {
            using (Entities context = new Entities())
            {
                using (DataTable DT = new DataTable())
                {
                    DT.Columns.Add("UNQ_FILE_NO", typeof(string));
                    DT.Columns.Add("CLM_UNIQUE_CODE", typeof(string));
                    DT.Columns.Add("REGION_NAME", typeof(string));
                    DT.Columns.Add("FULL_NAME", typeof(string));
                    DT.Columns.Add("GRANT_TYPE_NAME", typeof(string));
                    DT.Columns.Add("FILE_COMMENT", typeof(string));
                    //DT.Columns.Add("TRANS_TYPE", typeof(string));
                    DT.Columns.Add("FILE_STATUS_COMPLETED", typeof(bool));
                    DT.Columns.Add("BRM_BARCODE", typeof(string));
                    DT.Columns.Add("TDW_BOXNO", typeof(string));
                    DT.Columns.Add("APPLICANT_NO", typeof(string));
                    DT.Columns.Add("APPLICATION_STATUS", typeof(string));
                    DT.Columns.Add("APP_DATE", typeof(string));
                    DT.Columns.Add("ARCHIVE_YEAR", typeof(string));
                    DT.Columns.Add("CHILD_ID_NO", typeof(string));

                    string batchNrValue = Request.QueryString["batchNo"].ToString();

                    try
                    {
                        decimal outValue = 0.0M;
                        if (Decimal.TryParse(batchNrValue, out outValue))
                        {
                            var query = GetAllFilesByBatchNo();

                            if (query.Any())
                            {
                                foreach (FileEntity value in query.OrderBy(x => x.UNQ_FILE_NO))
                                {
                                    DataRow dr = DT.NewRow();
                                    dr["UNQ_FILE_NO"] = value.UNQ_FILE_NO;
                                    dr["CLM_UNIQUE_CODE"] = value.CLM_UNIQUE_CODE;
                                    dr["REGION_NAME"] = value.REGION_NAME;
                                    dr["FULL_NAME"] = value.FULL_NAME;
                                    dr["GRANT_TYPE_NAME"] = value.GRANT_TYPE_NAME;
                                    //dr["TRANS_TYPE"] = value.TRANS_TYPE.ToString();
                                    dr["FILE_COMMENT"] = value.FILE_COMMENT;
                                    dr["FILE_STATUS_COMPLETED"] = value.FILE_STATUS_COMPLETED;
                                    dr["BRM_BARCODE"] = value.BRM_BARCODE;
                                    dr["APPLICANT_NO"] = value.APPLICANT_NO;
                                    dr["TDW_BOXNO"] = value.TDW_BOXNO;
                                    dr["APPLICATION_STATUS"] = value.APPLICATION_STATUS;
                                    dr["APP_DATE"] = value.APP_DATE_DT.ToString();
                                    dr["ARCHIVE_YEAR"] = value.APPLICATION_STATUS == "MAIN" ? "" : value.ARCHIVE_YEAR;
                                    dr["CHILD_ID_NO"] = value.CHILD_ID_NO;
                                    DT.Rows.Add(dr);
                                }
                            }
                        }

                        fileGridView.DataSource = DT;
                        fileGridView.DataBind();
                    }
                    catch (Exception ex)
                    {
                        lblError.Text = "SearchBatchNo() - failure " + ex.InnerException;
                        divError.Visible = true;
                    }
                }
            }
        }

        private void SetBatchStatus()
        {
            DC_BATCH batch = GetCurrentBatch();
            if (batch == null)
            {
                return;
            }

            var s = batch.BATCH_STATUS;
            var j = ddlBatchStatus.Items.FindByValue(s);
            var p = ddlBatchStatus.SelectedIndex;
            if ((j != null) && (p < 0))
            {
                j.Selected = true;
            }
        }

        private void SetLabels()
        {
            DC_BATCH batch = GetCurrentBatch();
            if (batch == null)
            {
                return;
            }

            lblBatchNo.Text = batch.BATCH_NO.ToString();
            lblBatchSentDate.Text = batch.UPDATED_DATE.ToString();
            lblCourierName.Text = batch.COURIER_NAME;
            lblWayBillNo.Text = batch.WAYBILL_NO;
        }

        #endregion Private Methods
    }
}
