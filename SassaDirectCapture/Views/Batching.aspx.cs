using SASSADirectCapture.EntityModels;
using SASSADirectCapture.Sassa;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

//---------------
using System.Xml;

namespace SASSADirectCapture.Views
{
    public partial class Batching : SassaPage
    {

        #region Private Fields

        private Entities en = new Entities();

        #endregion Private Fields

        #region Public Methods

        public void GetAllFilesForBatchXML(int batchno, string waybill, string courier)
        {
            divClosedSuccess.Visible = false;
            lblClosedSuccess.Text = "";

            DateTime myDateTime = DateTime.Now;
            string fileDate = myDateTime.ToString("yyyyMMddHHmmss");
            //myDateTime.Year + myDateTime.Month + myDateTime.Day + "_" + myDateTime.Hour + "H" + myDateTime.Minute + "_" + myDateTime.Second;

            string XML_Path = System.Web.Configuration.WebConfigurationManager.AppSettings["LO_XML_Outgoing"].ToString();
            string Fname = batchno.ToString() + "_" + waybill + "_" + fileDate + ".xml";
            string XMLFilePath = XML_Path + "\\" + Fname;

            DirectoryInfo objDir = new DirectoryInfo(XML_Path);
            if (!objDir.Exists)
            {
                lblClosedError.Text = "Folder for XML does not exist [" + XML_Path + "]<br />";
                lblClosedError.Text += "File [" + XMLFilePath + "] could not be created.";
                divClosedError.Visible = true;
                return;
            }
            else
            {
                lblClosedSuccess.Text = "Folder existed OK - [" + lblClosedSuccess.Text + XML_Path + "]<br />";
                divClosedSuccess.Visible = true;
                lblClosedError.Text = "";
                divClosedError.Visible = false;
            }

            string lo = HttpContext.Current.Session["CSUserOfficeName"].ToString();
            string loid = HttpContext.Current.Session["CSUserOfficeID"].ToString();

            XmlWriter xmlWriter = XmlWriter.Create(XMLFilePath);
            xmlWriter.WriteStartDocument();
            xmlWriter.WriteStartElement("SASSA_BATCH");
            //xmlWriter.WriteAttributeString("XML_FILE", XMLFilePath);
            xmlWriter.WriteAttributeString("BATCH_NO", batchno.ToString());
            xmlWriter.WriteAttributeString("WORK_ORDER", waybill);
            xmlWriter.WriteAttributeString("COURIER", courier);
            xmlWriter.WriteAttributeString("DATE_SUBMITTED", fileDate);
            xmlWriter.WriteAttributeString("SASSA_OFFICE", lo);
            xmlWriter.WriteAttributeString("SASSA_OFFICE_ID", loid);
            xmlWriter.WriteAttributeString("INSTRUCTION", "COLLECT BATCH");
            xmlWriter.WriteAttributeString("BATCH_COMMENT", "");

            xmlWriter.WriteStartElement("FILES");

            var x = en.DC_FILE.Where(bn => bn.BATCH_NO == batchno).OrderBy(f => f.UNQ_FILE_NO)
            .Select(f => new FileEntity
            {
                UNQ_FILE_NO = f.UNQ_FILE_NO,
                BATCH_NO = f.BATCH_NO,
                OFFICE_NAME = f.DC_LOCAL_OFFICE.OFFICE_NAME,
                REGION_NAME = f.DC_LOCAL_OFFICE.DC_REGION.REGION_NAME,
                APPLICANT_NO = f.APPLICANT_NO,
                GRANT_TYPE_NAME = f.DC_GRANT_TYPE.TYPE_NAME,
                //TRANS_TYPE = f.TRANS_TYPE,
                DOCS_PRESENT = f.DOCS_PRESENT,
                TRANS_DATE = f.TRANS_DATE,
                UPDATED_BY = f.UPDATED_BY,
                UPDATED_DATE = f.UPDATED_DATE,
                BATCH_ADD_DATE = f.BATCH_ADD_DATE,
                FILE_STATUS = f.FILE_STATUS,
                FIRST_NAME = f.USER_FIRSTNAME,
                LAST_NAME = f.USER_LASTNAME,
                FILE_COMMENT = f.FILE_COMMENT,
                CLM_UNIQUE_CODE = f.UNQ_FILE_NO,
                BRM_BARCODE = f.BRM_BARCODE
            });

            int myCnt = 0;
            List<FileEntity> fe = new List<FileEntity>();

            xmlWriter.WriteAttributeString("NO_OF_FILES", fe.Count().ToString());

            foreach (var item in x)
            {
                myCnt++;
                FileEntity f = item;
                xmlWriter.WriteStartElement("FILE");
                xmlWriter.WriteAttributeString("UNQ_FILE_NO", f.UNQ_FILE_NO);
                xmlWriter.WriteAttributeString("CLM_UNIQUE_CODE", f.CLM_UNIQUE_CODE);
                xmlWriter.WriteAttributeString("BRM_FILE_NO", f.BRM_BARCODE);
                xmlWriter.WriteAttributeString("RSA_ID_NO", f.APPLICANT_NO);
                xmlWriter.WriteAttributeString("FULLNAMES", f.FIRST_NAME);
                xmlWriter.WriteAttributeString("LASTNAME", f.LAST_NAME);
                xmlWriter.WriteAttributeString("GRANT_TYPE", f.GRANT_TYPE_NAME);
                xmlWriter.WriteAttributeString("REGION_NAME", f.REGION_NAME);
                xmlWriter.WriteAttributeString("OFFICE_NAME", f.OFFICE_NAME);
                xmlWriter.WriteAttributeString("COLLECT_FROM", lo + "[" + loid + "]");
                xmlWriter.WriteEndElement();
            }

            xmlWriter.WriteEndElement();
            xmlWriter.WriteEndElement();
            xmlWriter.WriteEndDocument();
            xmlWriter.Close();
            lblClosedSuccess.Text = "XML file created OK - [" + lblClosedSuccess.Text + XMLFilePath + "]<br />";
        }

        #endregion Public Methods

        #region Protected Methods

        protected void btnBulkChange_Click(object sender, EventArgs e)
        {
            divBulkError.Visible = false;
            divBulkSuccess.Visible = false;

            string sFail = "";
            string sSuccess = "";

            if (hfBulkChangeBatchNos.Value != "")
            {
                foreach (string sBatchNo in hfBulkChangeBatchNos.Value.Split(','))
                {
                    int iBatchNo = int.Parse(sBatchNo);
                    string sWaybill = inputUpdateWayBillNo.Value;
                    string sCourier = inputUpdateCourierName.Value;

                    bool bOK = util.updateBatchDetails(iBatchNo, sWaybill, sCourier);
                    if (!bOK)
                    {
                        sFail += (sFail.Length == 0 ? iBatchNo.ToString() : (", " + iBatchNo.ToString()));
                    }
                    else
                    {
                        sSuccess += (sSuccess.Length == 0 ? iBatchNo.ToString() : (", " + iBatchNo.ToString()));
                    }
                }
            }

            if (sFail.Length != 0)
            {
                lblBulkError.Text = "Batch Updates Failed: " + sFail;
                divBulkError.Visible = true;
            }

            if (sSuccess.Length != 0)
            {
                lblBulkSuccess.Text = "Batches Updated: " + sSuccess;
                divBulkSuccess.Visible = true;
            }

            courierBatchGridView.SelectMethod = "FindCourierBatches";
            courierBatchGridView.DataBind();
            updPnlCourier.Update();
        }

        protected void CheckBox1_CheckedChanged(object sender, EventArgs e)
        {
            lblBulkError.Text = "";
            divBulkError.Visible = false;
            lblBulkSuccess.Text = "";
            divBulkSuccess.Visible = false;

            //hiddenUpdateBatch.Value = "";
            CheckBox cb = (CheckBox)sender;
            if (cb.Checked)
            {
                string batchid = cb.Attributes["batchid"];
                int batchno = int.Parse(batchid);
                string waybill4All = txtUpdateWayBillNo.Text;
                string courier4All = txtUpdateCourierName.Text;

                bool oneOK = util.updateBatchDetails(batchno, waybill4All, courier4All);
                if (!oneOK)
                {
                    lblBulkError.Text = "Update Failed for batch :" + batchid + " Batch Order: " + waybill4All + " Courier:" + courier4All;
                    divBulkError.Visible = true;
                }
                else
                {
                    lblBulkSuccess.Text = "Batch Updated :" + batchid + " Batch Order: " + waybill4All + " Courier:" + courier4All;
                    divBulkSuccess.Visible = true;
                }
            }
            //GridViewRow Grow = (GridViewRow)cb.NamingContainer;
        }

        protected void chkSelect_CheckedChanged(object sender, EventArgs e)
        {
            ClosedCheckChange(false);
        }

        protected void chkSelect3_CheckedChanged(object sender, EventArgs e)
        {
            string waybill = txtUpdateWayBillNo.Text;
            string courier = txtUpdateCourierName.Text;

            if ((waybill.Length == 0) && (courier.Length == 0))
            {
                lblBulkError.Text += "You did not fill in either the [Batch Order Number] or the [Courier Name] to use for the bulk update. Please fill in and try again.";
                divBulkError.Visible = true;
            }
            else
            {
                ClientScript.RegisterStartupScript(Page.GetType(), "xxx", "alert('xxx');", true);
                //CheckBox cb = (CheckBox)sender;
                ////GridViewRow Grow = (GridViewRow)cb.NamingContainer;

                //string batchid = cb.Attributes["batchid"];
                //int batchno = int.Parse(batchid);

                //if (string.IsNullOrEmpty(batchid))
                //{
                //    lblBulkError.Text += "<br />Batch number is empty";
                //    divBulkError.Visible = true;
                //    return;
                //}

                //if (cb.Checked)
                //{
                //    updateColumns(batchno, waybill, courier, cb.Checked);
                //    divBulkError.Visible = true;    //      should be false
                //    //ClientScript.RegisterStartupScript(Page.GetType(), "save", "setBulkValues(" + batchno + ");", true);
                //}
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                //If page is loaded the first time, default to the Current Tab and load the data.
                Page.Header.Title = "Batching";
                currentBatchGridView.SelectMethod = "GetCurrentBatch";
                currentBatchGridView.DataBind();
                HttpContext.Current.Session["batchNrs"] = null;
                HttpContext.Current.Session["dtTable"] = null;
            }
            if (HttpContext.Current.Session["courier"] != null)
            {
                txtUpdateCourierName.Text = HttpContext.Current.Session["courier"].ToString();
            }
            if (HttpContext.Current.Session["workorder"] != null)
            {
                txtUpdateWayBillNo.Text = HttpContext.Current.Session["workorder"].ToString();
            }
        }

        #endregion Protected Methods

        #region GET data for Gridviews

        public IQueryable<BatchEntity> FindClosedBatch()
        {
            try
            {
                var x = UserSession.Office.OfficeId;
                var query = en.DC_BATCH
                    .Where(oid => oid.OFFICE_ID == x)
                    .Where(st => st.BATCH_STATUS.ToLower() == "closed")
                    .OrderBy(bn => bn.BATCH_NO)
                    .Select(
                       bn => new BatchEntity
                       {
                           BATCH_NO = bn.BATCH_NO,
                           OFFICE_NAME = bn.DC_LOCAL_OFFICE.OFFICE_NAME,
                           UPDATED_NAME = bn.UPDATED_BY_AD,
                           UPDATED_BY = bn.UPDATED_BY,
                           UPDATED_DATE = bn.UPDATED_DATE,
                           BATCH_STATUS = bn.BATCH_STATUS,
                           BATCH_COMMENT = bn.BATCH_COMMENT,
                           WAYBILL_NO = bn.WAYBILL_NO,
                           COURIER_NAME = bn.COURIER_NAME,
                           WAYBILL_DATE = bn.WAYBILL_DATE
                       }).AsQueryable();

                if (query.Any())
                {
                    foreach (BatchEntity item in query)
                    {
                        if (item.UPDATED_BY > 0)
                        {
                            var LookupName = util.getUserFullName(item.UPDATED_BY.ToString());

                            item.UPDATED_NAME = LookupName == "unknown" ? item.UPDATED_NAME : LookupName;
                        }
                    }
                }
                else
                {
                    lblClosedError.Text = "No closed batches available.";
                    divClosedError.Visible = true;
                    return null;
                }
                return query;
            }
            catch (Exception ex)
            {
                lblClosedError.Text = ex.Message;
                divClosedError.Visible = true;
                return null;
            }
        }

        public IQueryable<BatchEntity> FindCourierBatches()
        {
            try
            {
                var x = UserSession.Office.OfficeId;
                var query = en.DC_BATCH
                    .Where(oid => oid.OFFICE_ID == x)
                    .Where(st => st.BATCH_STATUS.ToLower() == "transport")
                    .OrderBy(bn => bn.BATCH_NO)
                    .Select(
                       bn => new BatchEntity
                       {
                           BATCH_NO = bn.BATCH_NO,
                           OFFICE_NAME = bn.DC_LOCAL_OFFICE.OFFICE_NAME,
                           UPDATED_NAME = bn.UPDATED_BY_AD,
                           UPDATED_BY = bn.UPDATED_BY,
                           UPDATED_DATE = bn.UPDATED_DATE,
                           BATCH_STATUS = bn.BATCH_STATUS,
                           BATCH_COMMENT = bn.BATCH_COMMENT,
                           WAYBILL_NO = bn.WAYBILL_NO,
                           COURIER_NAME = bn.COURIER_NAME,
                           WAYBILL_DATE = bn.WAYBILL_DATE
                       }).AsQueryable();

                if (query.Any())
                {
                    foreach (BatchEntity item in query)
                    {
                        if (item.UPDATED_BY > 0)
                        {
                            var LookupName = util.getUserFullName(item.UPDATED_BY.ToString());

                            item.UPDATED_NAME = LookupName == "unknown" ? item.UPDATED_NAME : LookupName;
                        }
                    }
                }
                else
                {
                    lblTransportError.Text = "No batches in transport.";
                    divTransportError.Visible = true;
                    return null;
                }
                return query;
            }
            catch (Exception ex)
            {
                lblTransportError.Text = ex.Message;
                divTransportError.Visible = true;
                return null;
            }
        }

        public IQueryable<BatchEntity> GetCurrentBatch()
        {
            try
            {
                //If there is no current batch for the specific local office, quickly create one!
                //if (NoOfBatchesForCurrentOffice() == 0)
                //{
                //    //CreateBatchForOffice();
                //    return new List<BatchEntity>().AsQueryable();
                //}

                var x = UserSession.Office.OfficeId;

                var query = en.DC_BATCH
                    .Where(bn => bn.BATCH_CURRENT.ToLower().Trim() == "y")
                    .Where(oid => oid.OFFICE_ID == x)
                    .OrderBy(bn => bn.BATCH_NO)
                    .Select(
                           bn => new BatchEntity
                           {
                               BATCH_NO = bn.BATCH_NO,
                               OFFICE_NAME = bn.DC_LOCAL_OFFICE.OFFICE_NAME,
                               UPDATED_BY = bn.UPDATED_BY,
                               UPDATED_NAME = bn.UPDATED_BY_AD,
                               UPDATED_DATE = bn.UPDATED_DATE,
                               BATCH_STATUS = bn.BATCH_STATUS,
                               BATCH_COMMENT = bn.BATCH_COMMENT,
                               WAYBILL_NO = bn.WAYBILL_NO,
                               COURIER_NAME = bn.COURIER_NAME,
                               WAYBILL_DATE = bn.WAYBILL_DATE
                           }).AsQueryable();

                //if (query.Any())
                //{
                //Johan 14 May 2019 This loop can be remove if we run a script to populate all the missing Updated _by_ad fields with
                //util.getUserFullName(b.UPDATED_BY.ToString()); This field is now populated with this value by default
                //It is not necessary to create and return a new list here.. we can just update the query values directly...
                //List<BatchEntity> fe = new List<BatchEntity>();
                foreach (BatchEntity item in query)
                {
                    if (item.UPDATED_BY > 0)
                    {
                        var LookupName = util.getUserFullName(item.UPDATED_BY.ToString());

                        item.UPDATED_NAME = LookupName == "unknown" ? item.UPDATED_NAME : LookupName;
                    }
                }

                //return fe.AsQueryable();
                //-------------------------------------------------------

                //}

                return query;
            }
            catch (Exception ex)
            {
                lblCurrentError.Text = ex.Message;
                divCurrentError.Visible = true;
                return null;
            }
        }

        private int NoOfBatchesForCurrentOffice()
        {
            var x = UserSession.Office.OfficeId;
            return en.DC_BATCH.Count(oid => oid.OFFICE_ID == x);
        }

        #endregion GET data for Gridviews

        protected void updateColumns(int batchno, string waybill, string courier, bool check)
        {
            try
            {
                var x = en.DC_BATCH.Where(f => f.BATCH_NO == batchno).FirstOrDefault();

                if (x == null)
                {
                    return;
                }

                if (check)
                {
                    x.COURIER_NAME = courier;
                    x.WAYBILL_NO = waybill;
                    x.WAYBILL_DATE = System.DateTime.Now;
                }
                en.DC_ACTIVITY.Add(util.CreateActivity("Batching", "Update batch"));
                en.SaveChanges();

                //ScriptManager.RegisterStartupScript(Page, Page.GetType(), "reportBack", "UpdateCheckBox(true);", true);
                //lblSuccess.Text = fileID + " successfully updated.";
                //lblError.Text = "";
            }
            catch
            {
                //ScriptManager.RegisterStartupScript(Page, Page.GetType(), "reportBack", "UpdateCheckBox(false);", true);
                //lblError.Text = fileID + " could not be updated.";
                //lblError.Text = "";
            }
        }

        private void ClosedCheckChange(bool reset)
        {
            try
            {
                int count = 0;
                foreach (GridViewRow row in closedBatchGridView.Rows)
                {
                    if (row.RowType == DataControlRowType.DataRow)
                    {
                        CheckBox chkRow = (row.Cells[0].FindControl("chkSelect") as CheckBox);
                        if (chkRow.Checked)
                        {
                            //If this action is from clicking the 'Closed' tab, we need to clear the checkboxes.
                            if (reset)
                            {
                                chkRow.Checked = false;
                            }
                            else
                            {
                                count++;
                            }
                        }
                    }
                }
                if (count > 0)
                {
                    lblClosedSelected.Text = "Batches selected for Submission: <b>" + count + "</b>";
                    btnDispatchCourier.Visible = true;
                }
                else
                {
                    lblClosedSelected.Text = "";
                    btnDispatchCourier.Visible = false;
                }
            }
            catch (Exception ex)
            {
                lblClosedError.Text = ex.Message;
                divClosedError.Visible = true;
            }
            showHideSupervisorCheckbox();
        }

        //Johan removed this as all these batches were empty and orphaned
        private void CreateBatchForOffice()
        {
            DC_BATCH b = new DC_BATCH();
            b.BATCH_STATUS = "Open";
            b.BATCH_CURRENT = "Y";
            b.OFFICE_ID = UserSession.Office.OfficeId;
            b.UPDATED_DATE = DateTime.Now;


            b.UPDATED_BY_AD = UserSession.SamName;

            en.DC_BATCH.Add(b);
            en.DC_ACTIVITY.Add(util.CreateActivity("Batching", "Create batch"));
            en.SaveChanges();
        }

        #region CLICK events

        //protected void btnBulkEdit_Click(object sender, EventArgs e)
        protected void btnBulkEdit_Click(object sender, EventArgs e)
        {
            //for each checked tickbox update the Workorder no and Courier name;
            //int count = 0;

            lblBulkError.Text = "";
            divBulkError.Visible = false;
            lblBulkSuccess.Text = "";
            divBulkSuccess.Visible = true;

            //Button btn = (Button)sender;
            //string batchstr = btn.Text;

            string batchstr = txtThisBatch.Text;
            ScriptManager.RegisterStartupScript(this, GetType(), "cb1", "alert(" + batchstr + ");", true);

            string waybill4All = txtUpdateWayBillNo.Text;
            string courier4All = txtUpdateCourierName.Text;
            ScriptManager.RegisterStartupScript(this, GetType(), "whatdata", "alert('" + waybill4All + "," + courier4All + "," + batchstr + "');", true);

            if (waybill4All.Length == 0)
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "nodata", "alert('You did not fill in a Batch Order number, please do so for the bulk Update');", true);
                ScriptManager.RegisterStartupScript(this, GetType(), "wyscourier", "hideShowDiv('btnCourier');", true);
            }
            else if (courier4All.Length == 0)
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "nodata", "alert('You did not fill in any Courier Name, please do so for the bulk Update');", true);
                ScriptManager.RegisterStartupScript(this, GetType(), "wyscourier", "hideShowDiv('btnCourier');", true);
            }
            else if (batchstr.Length == 0)
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "nodata", "alert('You did not fill in Batch number, please do so for the bulk Update');", true);
                ScriptManager.RegisterStartupScript(this, GetType(), "wyscourier", "hideShowDiv('btnCourier');", true);
            }
            else
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "cb2", "alert('code behind2');", true);

                lblBulkSuccess.Text += "DATA FILLED IN <br />";
                try
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "nodata", "alert('checked');", true);

                    int batchno = int.Parse(batchstr);
                    lblBulkSuccess.Text += "batch no converted <br />";

                    bool oneOK = util.updateBatchDetails(batchno, waybill4All, courier4All);
                    if (!oneOK)
                    {
                        lblBulkError.Text += "Batch No:" + batchno.ToString() + " Failed.<br /> ";
                        divBulkError.Visible = true;
                    }
                    else
                    {
                        lblBulkSuccess.Text += "Batch No:" + batchno.ToString() + " Updated.<br /> ";
                        divBulkSuccess.Visible = true;
                    }
                }
                catch (Exception bex)
                {
                    lblBulkError.Text += "EXCEPTION:" + bex.Message + " <br /> ";
                    lblBulkError.Text += "EXCEPTION:" + bex.InnerException + " <br /> ";
                    divBulkError.Visible = true;
                }
                ScriptManager.RegisterStartupScript(this, GetType(), "wyscourier", "hideShowDiv('btnCourier');", true);
            }
        }

        protected void btnCurrentBatch_Click(object sender, EventArgs e)
        {
            currentBatchGridView.SelectMethod = "GetCurrentBatch";
        }

        protected void btnDispatchCourier_Click(object sender, EventArgs e)
        {
            // this is probably where we need to create XML file t send to TDW....

            lblClosedError.Text = "";
            divClosedError.Visible = false;

            List<decimal> batchNrs = new List<decimal>();
            HttpContext.Current.Session["batchNrs"] = null;

            try
            {
                foreach (GridViewRow row in closedBatchGridView.Rows)
                {
                    if (row.RowType == DataControlRowType.DataRow)
                    {
                        CheckBox chkRow = (row.Cells[0].FindControl("chkSelect") as CheckBox);
                        if (chkRow.Checked)
                        {
                            int num = row.DataItemIndex;// - (closedBatchGridView.PageSize * closedBatchGridView.PageIndex);
                            int batchno = int.Parse(closedBatchGridView.DataKeys[num].Values["BATCH_NO"].ToString());
                            string waybill = closedBatchGridView.DataKeys[num].Values["WAYBILL_NO"] != null ? closedBatchGridView.DataKeys[num].Values["WAYBILL_NO"].ToString() : "";
                            string courier = closedBatchGridView.DataKeys[num].Values["COURIER_NAME"] != null ? closedBatchGridView.DataKeys[num].Values["COURIER_NAME"].ToString() : "";

                            batchNrs.Add((decimal)batchno);
                            // write XML FILES for each batch
                            GetAllFilesForBatchXML(batchno, waybill, courier);
                        }
                    }
                }

                HttpContext.Current.Session["batchNrs"] = batchNrs;
                ScriptManager.RegisterStartupScript(this, GetType(), "openCourierSignPage", "openCourierSignPage();", true);
            }
            catch (Exception ex)
            {
                lblClosedError.Text = ex.Message;
                divClosedError.Visible = true;
            }
        }

        protected void btnSearchBatchNo_Click(object sender, EventArgs e)
        {
            try
            {
                string batch = txtSearchBarcode.Text;

                if (batch != string.Empty)
                {
                    int count = 0;
                    foreach (GridViewRow row in closedBatchGridView.Rows)
                    {
                        if (row.RowType == DataControlRowType.DataRow)
                        {
                            CheckBox chkRow = (row.Cells[0].FindControl("chkSelect") as CheckBox);
                            if (chkRow.Checked)
                            {
                                count++;
                            }

                            // added
                            if (UserSession.GetRole().Equals("Y"))
                            {
                                chkRow.Visible = true;
                            }
                            else
                            {
                                chkRow.Visible = false;
                            }
                            // end added

                            //If the batch number searched for matches the row, check it
                            if (row.Cells[1].Text == batch)
                            {
                                if (!chkRow.Checked)
                                {
                                    chkRow.Checked = true;
                                    count++;
                                }
                                else
                                {
                                    lblClosedError.Text = "Batch already in the list for transport";
                                    divClosedError.Visible = true;
                                }
                            }
                        }
                    }

                    if (count > 0)
                    {
                        lblClosedSelected.Text = "Batches selected for Transport: <b>" + count + "</b>";
                        btnDispatchCourier.Visible = true;
                    }
                    else
                    {
                        lblClosedSelected.Text = "";
                        btnDispatchCourier.Visible = false;
                    }
                }
                txtSearchBarcode.Text = string.Empty;
            }
            catch (Exception ex)
            {
                txtSearchBarcode.Text = string.Empty;
                lblClosedError.Text = ex.Message;
                divClosedError.Visible = true;
            }
            //showHideSupervisorCheckbox();
        }

        protected void btnSearchClosed_Click(object sender, EventArgs e)
        {
            //Forces the Closed tab to refresh the gridview when clicked.
            //This is required when closing a batch on the 'Current' tab and then clicking on the 'Closed' tab.
            closedBatchGridView.SelectMethod = "FindClosedBatch";
            showHideSupervisorCheckbox();

            ClosedCheckChange(false);
        }

        protected void btnSearchClosedReset_Click(object sender, EventArgs e)
        {
            //Forces the Closed tab to refresh the gridview when clicked.
            //This is required when closing a batch on the 'Current' tab and then clicking on the 'Closed' tab.
            closedBatchGridView.SelectMethod = "FindClosedBatch";
            showHideSupervisorCheckbox();

            ClosedCheckChange(true);
        }

        protected void btnSearchCourier_Click(object sender, EventArgs e)
        {
            courierBatchGridView.SelectMethod = "FindCourierBatches";
            courierBatchGridView.DataBind();
            updPnlCourier.Update();

            if (HttpContext.Current.Session["courier"] != null && HttpContext.Current.Session["workorder"] != null && HttpContext.Current.Session["courier"].ToString() != string.Empty && HttpContext.Current.Session["workorder"].ToString() != string.Empty)
            {
                string co = HttpContext.Current.Session["courier"].ToString();
                string wo = HttpContext.Current.Session["workorder"].ToString();
                txtUpdateCourierName.Text = co;
                txtUpdateWayBillNo.Text = wo;
                string strRepop = "window.opener.RepopValues('" + co + "', '" + wo + "');";
                //ScriptManager.RegisterStartupScript(this, GetType(), "repop1", "alert(strRepop);", true);
                ScriptManager.RegisterStartupScript(this, GetType(), "repop2", strRepop, true);
            }
        }

        #endregion CLICK events

        #region Page Index Change Events

        protected void closedBatchGridView_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            closedBatchGridView.PageIndex = e.NewPageIndex;
            //closedBatchGridView.SelectMethod = "FindClosedBatch";
        }

        protected void courierBatchGridView_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            courierBatchGridView.PageIndex = e.NewPageIndex;
            courierBatchGridView.DataSource = FindCourierBatches().ToList();
            courierBatchGridView.DataBind();
            updPnlCourier.Update();
        }

        protected void currentBatchGridView_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            currentBatchGridView.PageIndex = e.NewPageIndex;
            currentBatchGridView.DataSource = GetCurrentBatch().ToList();
            currentBatchGridView.DataBind();
        }

        #endregion Page Index Change Events

        private void showHideSupervisorCheckbox()
        {
            //If the user is a supervisor set the checkboxes to visible.

            foreach (GridViewRow row in closedBatchGridView.Rows)
            {
                if (row.RowType == DataControlRowType.DataRow)
                {
                    CheckBox chkRow = (row.Cells[0].FindControl("chkSelect") as CheckBox);
                    string sCSUserRole = UserSession.GetRole();
                    if (sCSUserRole == "Y" || sCSUserRole == "T")
                    {
                        chkRow.Visible = true;
                    }
                    else
                    {
                        chkRow.Visible = false;
                    }
                }
            }
        }
    }
}