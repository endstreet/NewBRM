using SASSADirectCapture.EntityModels;
using SASSADirectCapture.Sassa;
using System;
using System.Linq;
using System.Web.UI;

namespace SASSADirectCapture.Views
{
    public partial class ReceivingEdit : SassaPage
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
            string fileNo = Request.QueryString["fileNo"].ToString();
            string mymess = "";

            DC_FILE file = en.DC_FILE.Where(b => b.UNQ_FILE_NO == fileNo).FirstOrDefault();
            if (file != null)
            {
                file.FILE_COMMENT = txtComment.Text;

                try
                {
                    en.DC_ACTIVITY.Add(util.CreateActivity("Receiving", "Save File Comment"));
                    en.SaveChanges();

                    ClientScript.RegisterStartupScript(Page.GetType(), "save", "window.opener.UpdateGrid();window.close();", true);
                }
                catch (Exception ex)
                {
                    mymess = ex.Message;
                    ClientScript.RegisterStartupScript(Page.GetType(), "error", "alert('" + mymess + " - Unable to save data');", true);
                }
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if ((Request.QueryString.Count > 0) && (Request.QueryString.ToString().Contains("comment")))
                {
                    txtComment.Text = Request.QueryString["comment"].ToString();
                }
            }
        }

        #endregion Protected Methods
    }
}