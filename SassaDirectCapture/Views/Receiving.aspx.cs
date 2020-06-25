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
    public partial class Receiving : SassaPage
    {
        #region Public Fields

        //public SASSA_Authentication authObject = new SASSA_Authentication();

        public string myPostBack = "N";

        #endregion Public Fields

        #region Private Fields

        private Entities en = new Entities();

        #endregion Private Fields

        #region Protected Methods

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
                        deliveryGridView.DataSource = DT;
                        deliveryGridView.DataBind();
                        btnTakeDelivery.Visible = true;
                        lblDeliverSelected.Text = "Batches selected for Delivery: <b>" + DT.Rows.Count + "</b>";
                    }
                    else
                    {
                        deliveryGridView.DataSource = null;
                        deliveryGridView.DataBind();
                        btnTakeDelivery.Visible = false;
                        lblDeliverSelected.Text = "No Selected Batches";
                    }
                }
                catch (Exception ex)
                {
                    lblDeliveryError.Text = ex.Message;
                    divDeliveryError.Visible = true;
                }
            }
            ClientScript.RegisterStartupScript(Page.GetType(), "focus1", "window.document.getElementById('txtDeliverySearch').focus();", true);
        }

        protected void btnDeliverySearch_Click(object sender, EventArgs e)
        {
            //lblDeliverySuccess.Text = "Trace:54";
            //ClientScript.RegisterStartupScript(Page.GetType(), "clicked", "alert('code behind btnDeliverySearch_Click');", true);

            divDeliveryError.Visible = false;
            divDeliverySuccess.Visible = false;
            SearchDeliveryBatchNo();
        }

        protected void btnHiddenIncomingSearch_Click(object sender, EventArgs e)
        {
            // to refresh this grid

            divReceiveError.Visible = false;
            divReceiveSuccess.Visible = false;
            SearchIncomingBatchNo();
        }

        protected void btnHiddenReceiveSearch_Click(object sender, EventArgs e)
        {
            // to refresh this grid

            divReceiveError.Visible = false;
            divReceiveSuccess.Visible = false;
            SearchReceivingBatchNo();
        }

        //------------------------------------------
        protected void btnTakeDelivery_Click(object sender, EventArgs e)
        {
            try
            {
                if (Session["dtTable"] != null)
                {
                    DataTable DT = (DataTable)Session["dtTable"];
                    if (DT.Rows.Count > 0)
                    {
                        deliveryGridView.DataSource = DT;
                        deliveryGridView.DataBind();
                        btnTakeDelivery.Visible = true;
                        lblDeliverSelected.Text = "Batches selected for Delivery: <b>" + DT.Rows.Count + "</b>";
                    }
                    else
                    {
                        deliveryGridView.DataSource = null;
                        deliveryGridView.DataBind();
                        btnTakeDelivery.Visible = false;
                        lblDeliverSelected.Text = "";
                    }
                    ScriptManager.RegisterStartupScript(this, GetType(), "openCourierSignPage", "openCourierSignPage();", true);
                }
                else
                {
                    lblDeliveryError.Text = "Error occured reading the receipt list";
                    divDeliveryError.Visible = true;
                }
            }
            catch (Exception ex)
            {
                lblDeliveryError.Text = ex.Message;
                divDeliveryError.Visible = true;
            }
        }

        protected void btnTakeDeliveryReset_Click(object sender, EventArgs e)
        {
            lblDeliveryError.Text = "";
            divDeliveryError.Visible = false;
            divDeliverySuccess.Visible = false;

            Session["dtTable"] = null;
            deliveryGridView.DataSource = null;
            deliveryGridView.DataBind();

            btnTakeDelivery.Visible = false;
            lblDeliverSelected.Text = "";
        }

        protected void deliveryGridView_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            deliveryGridView.PageIndex = e.NewPageIndex;
            SearchDeliveryBatchNo();
        }

        protected void deliveryGridView_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            // to remove this file from delivery grid - back into incoming grid

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

                        lblDeliverySuccess.Text = "Batch <b>" + strBatchNo + "</b> successfully removed from the receipt list";
                        divDeliverySuccess.Visible = true;

                        //if (DT.Rows.Count > 0)
                        //{
                        deliveryGridView.DataSource = DT;
                        deliveryGridView.DataBind();
                        btnTakeDelivery.Visible = true;
                        lblDeliverSelected.Text = "Batches selected for Delivery: <b>" + DT.Rows.Count + "</b>";
                        //}
                        //else
                        //{
                        //    deliveryGridView.DataSource = null;
                        //    deliveryGridView.DataBind();
                        //    btnAcceptDeliveryDeliver.Visible = false;
                        //    lblDeliverSelected.Text = "No Selected Batches";
                        //}
                    }
                    else
                    {
                        //deliveryGridView.DataSource = null;
                        //deliveryGridView.DataBind();
                        //btnAcceptDeliveryDeliver.Visible = false;
                        //lblDeliverSelected.Text = "No Selected Batches";
                    }
                }
            }
            catch (Exception ex)
            {
                clearMessages();
                lblDeliveryError.Text = ex.Message;
                divDeliveryError.Visible = true;
            }
            // txtSearch2.Focus();
        }

        protected void incomingGridView_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            incomingGridView.PageIndex = e.NewPageIndex;
            SearchIncomingBatchNo();
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

                myPostBack = "N";
                Page.Header.Title = "Receiving";
                if ((Session["dtTable"]) != null)
                {
                    Session["dtTable"] = null;
                }
                if ((Session["batchNrs"]) != null)
                {
                    Session["batchNrs"] = null;
                }
                if ((Session["BoxNo"]) == null)
                {
                    Session["BoxNo"] = "";
                }
                SearchIncomingBatchNo();
                SearchDeliveryBatchNo();
                txtDeliverySearch.Focus();
            }
        }

        protected void receiveGridView_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            receiveGridView.PageIndex = e.NewPageIndex;
            SearchReceivingBatchNo();
        }

        #endregion Protected Methods

        #region Private Methods

        private void clearMessages()
        {
            lblIncomingError.Text = string.Empty;
            divIncomingError.Visible = false;

            lblDeliveryError.Text = string.Empty;
            divDeliveryError.Visible = false;
            lblDeliverySuccess.Text = string.Empty;
            divDeliverySuccess.Visible = false;

            lblReceiveError.Text = string.Empty;
            divReceiveError.Visible = false;
            lblReceiveSuccess.Text = string.Empty;
            divReceiveSuccess.Visible = false;
        }

        private void SearchDeliveryBatchNo()
        {
            //lblDeliverySuccess.Text = lblDeliverySuccess.Text + ";62";
            try
            {
                //lblDeliverySuccess.Text = lblDeliverySuccess.Text + ";65";

                using (Entities context = new Entities())
                {
                    //lblDeliverySuccess.Text = lblDeliverySuccess.Text + ";69";

                    DataTable DT = null;
                    //lblDeliverySuccess.Text = lblDeliverySuccess.Text + ";72";

                    if (Session["dtTable"] != null)
                    {
                        //lblDeliverySuccess.Text = lblDeliverySuccess.Text + ";76";
                        DT = (DataTable)Session["dtTable"];
                    }
                    else
                    {
                        //lblDeliverySuccess.Text = lblDeliverySuccess.Text + ";81";

                        DT = new DataTable();
                        DT.Columns.Add("BATCH_NO", typeof(string));
                        DT.Columns.Add("UPDATED_DATE", typeof(string));
                        DT.Columns.Add("WAYBILL_NO", typeof(string));
                        DT.Columns.Add("COURIER_NAME", typeof(string));
                        DT.Columns.Add("NO_OF_FILES", typeof(int));

                        DataColumn[] columns = new DataColumn[1];
                        columns[0] = DT.Columns["BATCH_NO"];
                        DT.PrimaryKey = columns;
                        //lblDeliverySuccess.Text = lblDeliverySuccess.Text + ";93";
                    }

                    string batchNrValue = txtDeliverySearch.Text;

                    if (string.IsNullOrEmpty(batchNrValue))
                    {
                        if (myPostBack == "Y")
                        {
                            lblDeliveryError.Text = "Please enter a batch number";
                            divDeliveryError.Visible = true;
                        }
                    }
                    else
                    {
                        //lblDeliverySuccess.Text = lblDeliverySuccess.Text + ";104";
                        try
                        {
                            //lblDeliverySuccess.Text = lblDeliverySuccess.Text + ";107";
                            decimal outValue = 0.0M;
                            if (Decimal.TryParse(batchNrValue, out outValue))
                            {
                                //lblDeliverySuccess.Text = lblDeliverySuccess.Text + ";111";
                                var office = UserSession.Office.OfficeId;
                                //lblDeliverySuccess.Text = lblDeliverySuccess.Text + ";113";

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
                                //lblDeliverySuccess.Text = lblDeliverySuccess.Text + ";136";

                                List<BatchEntity> be = query.ToList();
                                //lblDeliverySuccess.Text = lblDeliverySuccess.Text + ";139";

                                foreach (var item in be)
                                {
                                    int k = en.DC_FILE.Count(c => c.BATCH_NO == item.BATCH_NO);
                                    item.NO_OF_FILES = k;
                                }
                                //lblDeliverySuccess.Text = lblDeliverySuccess.Text + ";146";

                                if (be.Count > 0)
                                {
                                    //lblDeliverySuccess.Text = lblDeliverySuccess.Text + ";150";

                                    if (be.Count >= 100)
                                    {
                                        lblDeliveryError.Text = "More than 100 possible matches were found.  Please change the search value to limit the number of results.";
                                        divDeliveryError.Visible = true;
                                    }
                                    //lblDeliverySuccess.Text = lblDeliverySuccess.Text + ";157";

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
                                            ////lblDeliverySuccess.Text = lblDeliverySuccess.Text + ";171";
                                            lblDeliverySuccess.Text = "Batch <b>" + value.BATCH_NO + "</b> successfully added to receipt";
                                            divDeliverySuccess.Visible = true;
                                        }
                                        else
                                        {
                                            lblDeliveryError.Text = "The searched batch is already in the list";
                                            divDeliveryError.Visible = true;
                                        }
                                    }
                                    //lblDeliverySuccess.Text = lblDeliverySuccess.Text + ";181";
                                }
                                else
                                {
                                    lblDeliveryError.Text = "No results were found for the entered batch number";
                                    divDeliveryError.Visible = true;
                                }
                            }
                            //lblDeliverySuccess.Text = lblDeliverySuccess.Text + ";190";

                            Session["dtTable"] = DT;

                            if (DT.Rows.Count > 0)
                            {
                                //lblDeliverySuccess.Text = lblDeliverySuccess.Text + ";196";

                                deliveryGridView.DataSource = DT;
                                deliveryGridView.DataBind();
                                btnTakeDelivery.Visible = true;
                                lblDeliverSelected.Text = "Batches selected for Delivery: <b>" + DT.Rows.Count + "</b>";
                            }
                            else
                            {
                                //lblDeliverySuccess.Text = lblDeliverySuccess.Text + ";205";

                                btnTakeDelivery.Visible = false;
                                deliveryGridView.DataSource = null;
                                deliveryGridView.DataBind();
                                lblDeliverSelected.Text = "";
                            }
                            //lblDeliverySuccess.Text = lblDeliverySuccess.Text + ";212";
                        }
                        catch (Exception ex)
                        {
                            //lblDeliverySuccess.Text = lblDeliverySuccess.Text + ";216";

                            btnTakeDelivery.Visible = false;
                            lblDeliverSelected.Text = "";
                            lblDeliveryError.Text = ex.Message;
                            divDeliveryError.Visible = true;
                        }
                    }
                }
            }
            catch (Exception x)
            {
                lblDeliveryError.Text = x.Message;
                lblDeliveryError.Text += " " + x.InnerException;
                divDeliveryError.Visible = true;
                divDeliverySuccess.Visible = false;
            }
            txtDeliverySearch.Text = "";
            txtDeliverySearch.Focus();
        }

        //---------- populate grids ----------------
        //Todo: Johan This method does not record updated_BY_AD
        private void SearchIncomingBatchNo()
        {
            try
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

                                    where (b.BATCH_STATUS.ToLower() == "transport")
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
                            lblIncomingError.Text = "No batches available.";
                            divIncomingError.Visible = true;
                        }

                        if (DT.Rows.Count > 0)
                        {
                            incomingGridView.DataSource = DT;
                            incomingGridView.DataBind();
                        }
                        else
                        {
                            incomingGridView.DataSource = null;
                            incomingGridView.DataBind();
                        }
                    }
                    catch (Exception ex)
                    {
                        lblIncomingError.Text = ex.Message;
                        divIncomingError.Visible = true;
                    }
                }
            }
            catch (Exception e)
            {
                lblIncomingError.Text = e.Message;
                divIncomingError.Visible = true;
            }
        }

        //Todo: Johan This method does not impliment Updated_By_AD
        private void SearchReceivingBatchNo()
        {
            try
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
                ClientScript.RegisterStartupScript(Page.GetType(), "focus2", "window.document.getElementById('txtSearch2').focus();", true);
            }
            catch (Exception e)
            {
                lblReceiveError.Text = e.Message;
                divReceiveError.Visible = true;
            }
            ClientScript.RegisterStartupScript(Page.GetType(), "focus2", "window.document.getElementById('txtSearch2').focus();", true);
        }

        #endregion Private Methods
    }
}