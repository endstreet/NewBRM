using SASSADirectCapture.EntityModels;
using SASSADirectCapture.Sassa;
using System;
using System.Web.UI;

namespace SASSADirectCapture.Views
{
    public partial class EnterBoxnoAudit : SassaPage
    {

        private Entities en = new Entities();

        protected void Page_Load(object sender, EventArgs e)
        {

            txtBoxBarcode.Focus();
        }

        protected void txtBoxBarcode_TextChanged(object sender, EventArgs e)
        {
            //btnUpdateBox_Click(sender, e);
        }

        protected void btnUpdateBox_Click(object sender, EventArgs e)
        {
            string bc = txtBoxBarcode.Text.Trim().ToUpper();

            if (bc.Length < 3)
            {
                lblTooShort.Visible = true;
                ClientScript.RegisterStartupScript(Page.GetType(), "ignore", "alert('Please scan or enter the Box Barcode');", true);
            }
            else
            {
                Session["BoxNo"] = bc;
                bool bIsRebox = (Request.QueryString["IsRebox"] != null && Request.QueryString["IsRebox"] == "Y");

                if (bIsRebox)
                {
                    ClientScript.RegisterStartupScript(Page.GetType(), "getboxno", "window.opener.document.getElementById('MainContent_txtBoxNo').value='" + bc + "'; window.opener.document.getElementById('MainContent_tdBRMFileToRebox').style.display = ''; window.opener.document.getElementById('MainContent_btnSetReboxFields').click(); window.close();", true);
                    //ClientScript.RegisterStartupScript(Page.GetType(), "setboxno", "window.opener.location.reload();", true);
                    //ClientScript.RegisterStartupScript(Page.GetType(), "close", "window.close();", true);
                }
                else
                {
                    ClientScript.RegisterStartupScript(Page.GetType(), "getboxno", "window.opener.document.getElementById('MainContent_hfTDWBoxNo').value='" + bc + "'; window.opener.PrintBulk(); window.close();", true);
                }
            }
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            ClientScript.RegisterStartupScript(Page.GetType(), "close", "window.close();", true);
        }
    }
}