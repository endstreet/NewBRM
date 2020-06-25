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
    public partial class QCheck : SassaPage
    {
        #region Private Fields

        private Entities en = new Entities();

        #endregion Private Fields

        #region Protected Methods

        protected void batchGridView_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            batchGridView.PageIndex = e.NewPageIndex;
            SearchTransportBatchNo();
        }

        protected void batchGridView_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                if (e.CommandName == "lnkRemove")
                {
                    clearMessages();
                    string strBatchNo = Convert.ToString(e.CommandArgument.ToString());
                    if (Session["dtTable"] != null)
                    {
                        DataTable DT = (DataTable)Session["dtTable"];

                        DataRow row = DT.Rows.Find(strBatchNo);
                        DT.Rows.Remove(row);
                        Session["dtTable"] = DT;

                        lblSuccess.Text = "Batch <b>" + strBatchNo + "</b> successfully removed from the receipt list";
                        divSuccess.Visible = true;

                        if (DT.Rows.Count > 0)
                        {
                            batchGridView.DataSource = DT;
                            batchGridView.DataBind();
                            btnTransportDeliver.Visible = true;
                            lblDeliverSelected.Text = "Batches selected for Delivery: <b>" + DT.Rows.Count + "</b>";
                        }
                        else
                        {
                            batchGridView.DataSource = null;
                            batchGridView.DataBind();
                            btnTransportDeliver.Visible = false;
                            lblDeliverSelected.Text = "";
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                clearMessages();
                lblMsg.Text = ex.Message;
                divError.Visible = true;
            }
        }

        protected void btnDeliverReset_Click(object sender, EventArgs e)
        {
            Session["dtTable"] = null;
            lblMsg.Text = "";
            divSuccess.Visible = false;
            divError.Visible = false;
            batchGridView.DataSource = null;
            batchGridView.DataBind();
            btnTransportDeliver.Visible = false;
            lblDeliverSelected.Text = "";
        }

        protected void btnDeliverShow_Click(object sender, EventArgs e)
        {
            clearMessages();

            if (Session["dtTable"] != null)
            {
                try
                {
                    DataTable DT = (DataTable)Session["dtTable"];
                    if (DT.Rows.Count > 0)
                    {
                        batchGridView.DataSource = DT;
                        batchGridView.DataBind();
                        btnTransportDeliver.Visible = true;
                        lblDeliverSelected.Text = "Batches selected for Delivery: <b>" + DT.Rows.Count + "</b>";
                    }
                    else
                    {
                        batchGridView.DataSource = null;
                        batchGridView.DataBind();
                        btnTransportDeliver.Visible = false;
                        lblDeliverSelected.Text = "";
                    }
                }
                catch (Exception ex)
                {
                    lblMsg.Text = ex.Message;
                    divError.Visible = true;
                }
            }
        }

        protected void btnHiddenReceiveSearch_Click(object sender, EventArgs e)
        {
            divReceiveError.Visible = false;
            divReceiveSuccess.Visible = false;
            SearchDeliveredBatchNo();
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            divError.Visible = false;
            divSuccess.Visible = false;
            SearchTransportBatchNo();
        }

        protected void btnTransportDeliver_Click(object sender, EventArgs e)
        {
            try
            {
                if (Session["dtTable"] != null)
                {
                    DataTable DT = (DataTable)Session["dtTable"];
                    if (DT.Rows.Count > 0)
                    {
                        batchGridView.DataSource = DT;
                        batchGridView.DataBind();
                        btnTransportDeliver.Visible = true;
                        lblDeliverSelected.Text = "Batches selected for Delivery: <b>" + DT.Rows.Count + "</b>";
                    }
                    else
                    {
                        batchGridView.DataSource = null;
                        batchGridView.DataBind();
                        btnTransportDeliver.Visible = false;
                        lblDeliverSelected.Text = "";
                    }
                    ScriptManager.RegisterStartupScript(this, GetType(), "openCourierSignPage", "openCourierSignPage();", true);
                }
                else
                {
                    lblMsg.Text = "Error occured reading the receipt list";
                    divError.Visible = true;
                }
            }
            catch (Exception ex)
            {
                lblMsg.Text = ex.Message;
                divError.Visible = true;
            }
        }

        protected void lnkRemoveBatch_Click(object sender, EventArgs e)
        {
            GridViewRow grdrow = (GridViewRow)((LinkButton)sender).NamingContainer;
            string batchNo = grdrow.Cells[0].Text;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Page.Header.Title = "Receiving";
                Session["dtTable"] = null;
                Session["batchNrs"] = null;
            }
        }

        protected void receiveGridView_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            receiveGridView.PageIndex = e.NewPageIndex;
            SearchDeliveredBatchNo();
        }

        #endregion Protected Methods

        #region Private Methods

        private void clearMessages()
        {
            lblMsg.Text = string.Empty;
            divError.Visible = false;

            lblSuccess.Text = string.Empty;
            divSuccess.Visible = false;
        }

        private void SearchDeliveredBatchNo()
        {
            using (Entities context = new Entities())
            {
                DataTable DT = new DataTable();
                DT.Columns.Add("BATCH_NO", typeof(string));
                DT.Columns.Add("BATCH_STATUS", typeof(string));
                DT.Columns.Add("UPDATED_DATE", typeof(string));
                DT.Columns.Add("WAYBILL_NO", typeof(string));
                DT.Columns.Add("COURIER_NAME", typeof(string));
                DT.Columns.Add("NO_OF_FILES", typeof(int));

                DataColumn[] columns = new DataColumn[1];
                columns[0] = DT.Columns["BATCH_NO"];
                DT.PrimaryKey = columns;

                try
                {
                    var office = UserSession.Office.OfficeId;

                    var query = from b in context.DC_BATCH
                                join lo in context.DC_LOCAL_OFFICE on b.OFFICE_ID equals lo.OFFICE_ID
                                join r in context.DC_LOCAL_OFFICE on lo.REGION_ID equals r.REGION_ID

                                where (b.BATCH_STATUS.ToLower() == "delivered" || b.BATCH_STATUS.ToLower() == "received")
                                && r.OFFICE_ID == office

                                orderby b.BATCH_NO

                                select new BatchEntity
                                {
                                    BATCH_NO = b.BATCH_NO,
                                    BATCH_STATUS = b.BATCH_STATUS,
                                    OFFICE_NAME = b.DC_LOCAL_OFFICE.OFFICE_NAME,
                                    UPDATED_BY = b.UPDATED_BY,
                                    UPDATED_DATE = b.UPDATED_DATE,
                                    BATCH_COMMENT = b.BATCH_COMMENT,
                                    WAYBILL_NO = b.WAYBILL_NO,
                                    COURIER_NAME = b.COURIER_NAME,
                                    WAYBILL_DATE = b.WAYBILL_DATE
                                };

                    List<BatchEntity> be = query.ToList();
                    foreach (var item in be)
                    {
                        int k = en.DC_FILE.Count(c => c.BATCH_NO == item.BATCH_NO);
                        item.NO_OF_FILES = k;
                    }

                    if (be.Count > 0)
                    {
                        foreach (BatchEntity value in be.OrderBy(x => x.BATCH_NO))
                        {
                            DataRow dr = DT.NewRow();
                            dr["BATCH_NO"] = value.BATCH_NO;
                            dr["BATCH_STATUS"] = value.BATCH_STATUS;
                            dr["UPDATED_DATE"] = value.UPDATED_DATE;
                            dr["WAYBILL_NO"] = value.WAYBILL_NO;
                            dr["COURIER_NAME"] = value.COURIER_NAME;
                            dr["NO_OF_FILES"] = value.NO_OF_FILES;

                            DT.Rows.Add(dr);
                        }
                    }
                    else
                    {
                        lblReceiveError.Text = "No batches available.";
                        divReceiveError.Visible = true;
                    }

                    if (DT.Rows.Count > 0)
                    {
                        receiveGridView.DataSource = DT;
                        receiveGridView.DataBind();
                    }
                    else
                    {
                        receiveGridView.DataSource = null;
                        receiveGridView.DataBind();
                    }
                }
                catch (Exception ex)
                {
                    lblReceiveError.Text = ex.Message;
                    divReceiveError.Visible = true;
                }
            }
        }

        //Todo: Johan This method does not impliment Updated by ad for some reason...
        private void SearchTransportBatchNo()
        {
            using (Entities context = new Entities())
            {
                DataTable DT = null;

                if (Session["dtTable"] != null)
                {
                    DT = (DataTable)Session["dtTable"];
                }
                else
                {
                    DT = new DataTable();
                    DT.Columns.Add("BATCH_NO", typeof(string));
                    DT.Columns.Add("UPDATED_DATE", typeof(string));
                    DT.Columns.Add("WAYBILL_NO", typeof(string));
                    DT.Columns.Add("COURIER_NAME", typeof(string));
                    DT.Columns.Add("NO_OF_FILES", typeof(int));

                    DataColumn[] columns = new DataColumn[1];
                    columns[0] = DT.Columns["BATCH_NO"];
                    DT.PrimaryKey = columns;
                }

                string batchNrValue = txtSearch.Text;

                if (string.IsNullOrEmpty(batchNrValue))
                {
                    lblMsg.Text = "Please enter a batch number";
                    divError.Visible = true;
                }

                try
                {
                    decimal outValue = 0.0M;
                    if (Decimal.TryParse(batchNrValue, out outValue))
                    {
                        var office = UserSession.Office.OfficeId;

                        var query = from b in context.DC_BATCH
                                    join lo in context.DC_LOCAL_OFFICE on b.OFFICE_ID equals lo.OFFICE_ID
                                    join r in context.DC_LOCAL_OFFICE on lo.REGION_ID equals r.REGION_ID

                                    where b.BATCH_NO == outValue
                                    && (b.BATCH_STATUS.ToLower() == "transport")
                                    && r.OFFICE_ID == office

                                    orderby b.BATCH_NO

                                    select new BatchEntity
                                    {
                                        BATCH_NO = b.BATCH_NO,
                                        OFFICE_NAME = b.DC_LOCAL_OFFICE.OFFICE_NAME,
                                        UPDATED_BY = b.UPDATED_BY,
                                        UPDATED_DATE = b.UPDATED_DATE,
                                        BATCH_COMMENT = b.BATCH_COMMENT,
                                        WAYBILL_NO = b.WAYBILL_NO,
                                        COURIER_NAME = b.COURIER_NAME,
                                        WAYBILL_DATE = b.WAYBILL_DATE
                                    };

                        List<BatchEntity> be = query.ToList();
                        foreach (var item in be)
                        {
                            int k = en.DC_FILE.Count(c => c.BATCH_NO == item.BATCH_NO);
                            item.NO_OF_FILES = k;
                        }

                        if (be.Count > 0)
                        {
                            if (be.Count >= 100)
                            {
                                lblMsg.Text = "More than 100 possible matches were found.  Please change the search value to limit the number of results.";
                                divError.Visible = true;
                            }

                            foreach (BatchEntity value in be.OrderBy(x => x.BATCH_NO))
                            {
                                DataRow dr = DT.NewRow();
                                dr["BATCH_NO"] = value.BATCH_NO;
                                dr["UPDATED_DATE"] = value.UPDATED_DATE;
                                dr["WAYBILL_NO"] = value.WAYBILL_NO;
                                dr["COURIER_NAME"] = value.COURIER_NAME;
                                dr["NO_OF_FILES"] = value.NO_OF_FILES;

                                if (DT.Rows.Find(value.BATCH_NO) == null)
                                {
                                    DT.Rows.Add(dr);

                                    lblSuccess.Text = "Batch <b>" + value.BATCH_NO + "</b> successfully added to receipt";
                                    divSuccess.Visible = true;
                                }
                                else
                                {
                                    lblMsg.Text = "The searched batch is already in the list";
                                    divError.Visible = true;
                                }
                            }
                        }
                        else
                        {
                            lblMsg.Text = "No results were found for the entered batch number";
                            divError.Visible = true;
                        }
                    }

                    Session["dtTable"] = DT;

                    if (DT.Rows.Count > 0)
                    {
                        batchGridView.DataSource = DT;
                        batchGridView.DataBind();
                        btnTransportDeliver.Visible = true;
                        lblDeliverSelected.Text = "Batches selected for Delivery: <b>" + DT.Rows.Count + "</b>";
                    }
                    else
                    {
                        btnTransportDeliver.Visible = false;
                        batchGridView.DataSource = null;
                        batchGridView.DataBind();
                        lblDeliverSelected.Text = "";
                    }
                }
                catch (Exception ex)
                {
                    btnTransportDeliver.Visible = false;
                    lblDeliverSelected.Text = "";
                    lblMsg.Text = ex.Message;
                    divError.Visible = true;
                }
            }
        }

        #endregion Private Methods
    }
}