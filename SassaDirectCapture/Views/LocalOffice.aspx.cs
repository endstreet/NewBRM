using System;
using System.Linq.Expressions;
using System.Web;
using System.Web.UI;
using SASSADirectCapture.BL;
using SASSADirectCapture.Sassa;

namespace SASSADirectCapture.Views
{
    public partial class LocalOffice : SassaPage
    {

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {

                ddlRegion.DataSource = util.getRegions();
                ddlRegion.DataBind();
            }
        }

        protected void ddlRegion_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlRegion.SelectedValue != string.Empty)
            {
                ddlLocalOffice.DataSource = util.getLocalOffices(ddlRegion.SelectedValue);
                ddlLocalOffice.DataBind();
            }
            else
            {
                ddlLocalOffice.DataSource = null;
                ddlLocalOffice.DataBind();
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            //lblError.Text = Usersession.SamName + " " + ddlLocalOffice.SelectedValue;
            divError.Visible = false;

            try
            {
                if (!ddlLocalOffice.SelectedValue.IsNumeric()) throw new Exception("Invalid office selected.");
                Usersession.IsIntitialized = false;
                Usersession = Usersession;
                util.updateUserLocalOffice( ddlLocalOffice.SelectedValue);
                ScriptManager.RegisterStartupScript(this, GetType(), "closeFancyBox", "parent.jQuery.fancybox.close();", true);

            }
             catch (Exception ex)
            {
                //lblError.Text = ex.Message;
                lblError.Text += ex.Message;
                divError.Visible = true;
            }
            
        }
    }
}
