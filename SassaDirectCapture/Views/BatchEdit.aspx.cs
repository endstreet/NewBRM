using SASSADirectCapture.EntityModels;
using SASSADirectCapture.Sassa;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;

namespace SASSADirectCapture.Views
{
    public partial class BatchEdit : SassaPage
    {
        #region Public Fields

        //public SASSA_Authentication authObject = new SASSA_Authentication();

        #endregion Public Fields

        #region Private Fields

        private Entities en = new Entities();

        #endregion Private Fields

        #region Protected Methods

        protected void btnClose_Click(object sender, EventArgs e)
        {
            ClientScript.RegisterStartupScript(Page.GetType(), "close", "window.close();", true);
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            var z = Usersession.Office.OfficeId;
            decimal batchNo = 0.0M;
            Decimal.TryParse(Request.QueryString["batchNo"].ToString(), out batchNo);
            DC_BATCH batch = en.DC_BATCH
                .Where(b => b.BATCH_NO == batchNo)
                .Where(oid => oid.OFFICE_ID == z)
                .FirstOrDefault();

            if (batch != null)
            {
                batch.WAYBILL_NO = txtWayBillNo.Text;
                batch.COURIER_NAME = txtCourierName.Text;
                batch.WAYBILL_DATE = System.DateTime.Now;

                try
                {

                    en.DC_ACTIVITY.Add(util.CreateActivity("Batching", "Save batch"));
                    en.SaveChanges();

                    string gridToUpdate = Request.QueryString["grd"].ToString();

                    if (gridToUpdate != string.Empty && gridToUpdate == "CURRENT")
                    {
                        ClientScript.RegisterStartupScript(Page.GetType(), "save1", "window.opener.UpdateCurrentGrid();", true);
                    }
                    else if (gridToUpdate != string.Empty && gridToUpdate == "CLOSED")
                    {
                        ClientScript.RegisterStartupScript(Page.GetType(), "save2", "window.opener.UpdateClosedGrid();", true);
                    }
                    else if (gridToUpdate != string.Empty && gridToUpdate == "SUBMITTED")
                    {
                        ClientScript.RegisterStartupScript(Page.GetType(), "save3", "window.opener.UpdateCourierGrid();", true);
                    }
                    ClientScript.RegisterStartupScript(Page.GetType(), "close", "window.close();", true);
                }
                catch (Exception)
                {
                    ClientScript.RegisterStartupScript(Page.GetType(), "error", "alert('Unable to save data')", true);
                }
            }
        }

        protected void btnUpdateAll_Click(object sender, EventArgs e)
        {
            updateCourierDetails("ALL");
        }

        protected void btnUpdateMissing_Click(object sender, EventArgs e)
        {
            updateCourierDetails("MISSING");
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                //Get the username of the user that is logged in from session.
                string authenticatedUsername = Usersession.SamName;

                //If no session values are found , redirect to the login screen
                //if (authenticatedUsername == string.Empty)
                //{
                //    this.authObject.RedirectToLoginPage();
                //}
                //else 
                if (Request.QueryString.Count > 0)
                {
                    if (Request.QueryString["bc"] != null && Request.QueryString["bc"].ToString() == "y")
                    {
                        pHeading.InnerText = "Missing courier details for selected batch(es)";
                        pFooter.Visible = true;
                        btnSave.Visible = false;
                        btnUpdateAll.Visible = true;
                        btnUpdateMissing.Visible = true;
                    }
                    else
                    {
                        txtWayBillNo.Text = Request.QueryString["wayBillNo"].ToString();
                        txtCourierName.Text = Request.QueryString["courierName"].ToString();
                        if ((txtCourierName.Text == "") || (txtCourierName.Text == null))
                        {
                            txtCourierName.Text = "TDW";
                        }
                    }
                }
            }
        }

        protected void updateCourierDetails(string missingAll)
        {
            List<decimal> batchNrs = Session["batchNrs"] != null ? (List<decimal>)Session["batchNrs"] : null;

            try
            {
                var z = Usersession.Office.OfficeId;

                foreach (decimal batchno in batchNrs)
                {
                    DC_BATCH batch = en.DC_BATCH
                        .Where(b => b.BATCH_NO == batchno)
                        .Where(oid => oid.OFFICE_ID == z)
                        .FirstOrDefault();

                    if (batch != null)
                    {
                        if (missingAll == "MISSING")
                        {
                            if (batch.WAYBILL_NO == null || batch.WAYBILL_NO == string.Empty)
                            {
                                batch.WAYBILL_NO = txtWayBillNo.Text;
                            }
                            if (batch.COURIER_NAME == null || batch.COURIER_NAME == string.Empty)
                            {
                                batch.COURIER_NAME = txtCourierName.Text;
                            }
                            if (batch.WAYBILL_DATE == null)
                            {
                                batch.WAYBILL_DATE = System.DateTime.Now;
                            }
                        }
                        else if (missingAll == "ALL")
                        {
                            batch.WAYBILL_NO = txtWayBillNo.Text;
                            batch.COURIER_NAME = txtCourierName.Text;
                            batch.WAYBILL_DATE = System.DateTime.Now;
                        }
                        en.DC_ACTIVITY.Add(util.CreateActivity("Batching", "Update batch"));
                        en.SaveChanges();
                    }
                }

                ClientScript.RegisterStartupScript(Page.GetType(), "save", "window.opener.refresh;window.opener.UpdateClosedGrid();window.close()", true);
            }
            catch (Exception)
            {
                ClientScript.RegisterStartupScript(Page.GetType(), "error", "alert('Unable to save data')", true);
            }
        }

        #endregion Protected Methods
    }
}