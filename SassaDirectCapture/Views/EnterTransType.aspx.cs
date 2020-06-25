using SASSADirectCapture.Sassa;
using System;
using System.Web.UI;

namespace SASSADirectCapture.Views
{
    public partial class EnterTransType : SassaPage
    {

        protected void Page_Load(object sender, EventArgs e)
        {
            //Validate query strings
            ValidateParams();
        }

        protected void ValidateParams()
        {
            string pensionNo = Request.QueryString["IdNo"];
            string boxNo = Request.QueryString["boxNo"];

            string mymess = "The following field values are missing: ";
            bool mistake = false;

            //Make sure that the pension number / id number is available
            if (string.IsNullOrEmpty(pensionNo))
            {
                mymess += "[ID number] ";
                mistake = true;
            }
            if (string.IsNullOrEmpty(boxNo))
            {
                mymess += "[MIS Box No] ";
                mistake = true;
            }

            if (mistake)
            {
                ClientScript.RegisterStartupScript(Page.GetType(), "close", "alert(" + mymess + ");window.close();", true);
            }
        }

        protected void ddlServiceType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(ddlServiceType.SelectedValue))
            {
                ddlTransactionType.Enabled = true;
                ddlTransactionType.DataSource = util.getTransactionTypesByService(ddlServiceType.SelectedValue);
                ddlTransactionType.DataBind();
            }
            else
            {
                ddlTransactionType.Enabled = false;
            }
            ddlTransactionType.SelectedIndex = 0;
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            ClientScript.RegisterStartupScript(Page.GetType(), "close", "window.close()", true);
        }

        protected void btnUpdateTransType_Click(object sender, EventArgs e)
        {
            string transType = ddlTransactionType.SelectedValue;

            if (string.IsNullOrEmpty(transType))
            {
                ClientScript.RegisterStartupScript(Page.GetType(), "ignore", "alert('Please select a Transaction Type');", true);
            }
            else
            {
                string pensionNo = Request.QueryString["IdNo"];
                string boxNo = Request.QueryString["boxNo"];
                ClientScript.RegisterStartupScript(Page.GetType(), "setTransType", "window.opener.openBRMForm('" + pensionNo + "', '" + boxNo + "', '" + transType + "'); window.close(); ", true);
            }
        }
    }
}