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
    public partial class TransportCover : SassaPage
    {
        #region Public Fields

        // public SASSA_Authentication authObject = new SASSA_Authentication();

        #endregion Public Fields

        #region Private Fields

        private List<decimal> batches = new List<decimal>();

        #endregion Private Fields

        #region Protected Methods

        protected void btnPrintClose_Click(object sender, EventArgs e)
        {
            //var localoffice = UserSession.Office.OfficeId;
            //int userId = new SASSA_Authentication().getUserID();
            //string sUserLogin = new SASSA_Authentication().getUserLogin();

            using (Entities en = new Entities())
            {
                foreach (GridViewRow row in batchGridView.Rows)
                {
                    int num = row.DataItemIndex - (batchGridView.PageSize * batchGridView.PageIndex);
                    decimal batchno = decimal.Parse(batchGridView.DataKeys[num].Values["BATCH_NO"].ToString());

                    DC_BATCH batch = en.DC_BATCH.Find(batchno);

                    if (batch != null)
                    {
                        if (hiddenReceiptType.Value == "Transport")
                        {
                            batch.BATCH_STATUS = "Transport"; // Transport Receipt = Transport, Delivery Receipt = Delivery
                        }
                        else if (hiddenReceiptType.Value == "Delivery")
                        {
                            batch.BATCH_STATUS = "Delivered"; // Transport Receipt = Transport, Delivery Receipt = Delivery
                        }

                        batch.UPDATED_DATE = DateTime.Now;

                        batch.UPDATED_BY_AD = UserSession.SamName;

                    }
                }

                try
                {
                    en.DC_ACTIVITY.Add(util.CreateActivity("Transport", "Cover Printed - Status Update"));
                    en.SaveChanges();

                    divError.Visible = false;
                    divSuccess.Visible = true;
                    lblSuccess.Text = "Batch(es) successfully updated.";

                    ClientScript.RegisterStartupScript(Page.GetType(), "save", "printpage('Y');", true);
                    //ClientScript.RegisterStartupScript(Page.GetType(), "save", "printpage('Y'); try {window.opener.UpdateDeliveryGridReset();} catch (Exception) {}", true);
                    //ClientScript.RegisterStartupScript(Page.GetType(), "save", "printpage('Y'); try {window.opener.UpdateClosedGridReset();} catch (Exception) {} try {window.opener.UpdateDeliveryGridReset();} catch (Exception) {}", true);
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
            if (!IsPostBack)
            {
                //Get the username of the user that is logged in from session.


                if (Session["batchNrs"] != null)
                {
                    batches = (List<decimal>)Session["batchNrs"];
                    hiddenReceiptType.Value = "Transport";
                    loadTransportData();
                }
                else if (Session["dtTable"] != null)
                {
                    lblHeading.InnerText = "Delivery Receipt";
                    hiddenReceiptType.Value = "Delivery";
                    loadDeliveryData();
                }
            }
        }

        #endregion Protected Methods

        #region Private Methods

        private void loadDeliveryData()
        {
            int mycount = 0;
            string datavalue;
            try
            {
                DataTable DT = (DataTable)Session["dtTable"];
                batchGridView.DataSource = DT;
                batchGridView.DataBind();
                lblTotalBatches.Text = DT.Rows.Count.ToString();
                foreach (DataRow dr in DT.Rows)
                {
                    datavalue = dr["NO_OF_FILES"].ToString();
                    mycount += int.Parse(datavalue);
                }
                lblTotalFiles.Text = mycount.ToString();
            }
            catch (Exception ex)
            {
                lblError.Text = ex.Message;
                divError.Visible = true;
            }
        }

        private void loadTransportData()
        {
            int totalBatches = 0;
            int totalFiles = 0;

            using (Entities en = new Entities())
            {
                using (DataTable DT = new DataTable())
                {
                    DT.Columns.Add("batch_no", typeof(string));
                    DT.Columns.Add("NO_OF_FILES", typeof(string));
                    DT.Columns.Add("courier_name", typeof(string));
                    DT.Columns.Add("waybill_no", typeof(string));

                    try
                    {
                        var query = from btch in en.DC_BATCH
                                    where batches.Contains(btch.BATCH_NO)

                                    select new BatchEntity
                                    {
                                        BATCH_NO = btch.BATCH_NO,
                                        COURIER_NAME = btch.COURIER_NAME,
                                        WAYBILL_NO = btch.WAYBILL_NO
                                    };

                        List<BatchEntity> be = query.ToList();
                        foreach (var item in be)
                        {
                            int k = en.DC_FILE.Count(c => c.BATCH_NO == item.BATCH_NO);
                            item.NO_OF_FILES = k;

                            //For our totals to display on the page.
                            totalBatches++;
                            totalFiles += k;
                        }

                        if (be.Count > 0)
                        {
                            foreach (BatchEntity value in be.OrderBy(x => x.BATCH_NO))
                            {
                                DataRow dr = DT.NewRow();
                                dr["batch_no"] = value.BATCH_NO;
                                dr["NO_OF_FILES"] = value.NO_OF_FILES;
                                dr["courier_name"] = value.COURIER_NAME;
                                dr["waybill_no"] = value.WAYBILL_NO;

                                DT.Rows.Add(dr);
                            }
                        }

                        batchGridView.DataSource = DT;
                        batchGridView.DataBind();

                        lblTotalBatches.Text = totalBatches.ToString();
                        lblTotalFiles.Text = totalFiles.ToString();
                    }
                    catch (Exception ex)
                    {
                        lblError.Text = ex.Message;
                        divError.Visible = true;
                    }
                    lblTotalBatches.Text = totalBatches.ToString();
                    lblTotalFiles.Text = totalFiles.ToString();
                }
            }
        }

        #endregion Private Methods
    }
}