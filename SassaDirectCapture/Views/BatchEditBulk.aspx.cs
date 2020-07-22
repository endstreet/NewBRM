using SASSADirectCapture.EntityModels;
using SASSADirectCapture.Sassa;
using System;
using System.Linq;
using System.Web;
using System.Web.UI;

namespace SASSADirectCapture.Views
{
    public partial class BatchEditBulk : SassaPage
    {
        #region Public Fields

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
            //ScriptManager.RegisterStartupScript(this, GetType(), "hideLoadingMsg", "myECM.hidePleaseWait();", true);

            string w = txtWayBillNo.Text;
            string c = txtCourierName.Text;

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
                    en.DC_ACTIVITY.Add(util.CreateActivity("Batching", "Update batch"));
                    en.SaveChanges();

                    string gridToUpdate = Request.QueryString["grd"].ToString();

                    if (gridToUpdate != string.Empty && gridToUpdate == "CURRENT")
                    {
                        ClientScript.RegisterStartupScript(Page.GetType(), "save", "window.opener.UpdateCurrentGrid();", true);
                    }
                    else if (gridToUpdate != string.Empty && gridToUpdate == "CLOSED")
                    {
                        ClientScript.RegisterStartupScript(Page.GetType(), "save", "window.opener.UpdateClosedGrid();", true);
                    }
                    else if (gridToUpdate != string.Empty && gridToUpdate == "SUBMITTED")
                    {
                        ClientScript.RegisterStartupScript(Page.GetType(), "save", "window.opener.UpdateCourierGrid();", true);
                    }

                    btnClose_Click(sender, e);
                }
                catch (Exception)
                {
                    ClientScript.RegisterStartupScript(Page.GetType(), "error", "alert('Unable to save data')", true);
                }
            }

            btnClose_Click(sender, e);
            //ClientScript.RegisterStartupScript(Page.GetType(), "close", "window.close()", true);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                //Get the username of the user that is logged in from session.
                if (Request.QueryString.Count > 0)
                {
                    if (Request.QueryString["batchNo"] != null && Request.QueryString["wayBillNo"] != null && Request.QueryString["courierName"] != null && Request.QueryString["grd"] != null)
                    {
                        pHeading.InnerText = "Please confirm:";
                        txtWayBillNo.Text = Request.QueryString["wayBillNo"];
                        txtCourierName.Text = Request.QueryString["courierName"];
                        txtBatchNo.Text = Request.QueryString["batchNo"];
                        HttpContext.Current.Session["courier"] = Request.QueryString["courierName"];
                        HttpContext.Current.Session["workorder"] = Request.QueryString["wayBillNo"];
                    }
                }
            }
        }

        #endregion Protected Methods
    }
}