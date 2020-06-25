using SASSADirectCapture.EntityModels;
using SASSADirectCapture.Sassa;
using System;
using System.Linq;
using System.Web.UI;

namespace SASSADirectCapture.Views
{
    public partial class FileEdit : SassaPage
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
            string myUpperBC = txtBRM_BARCODE.Text.ToUpper();

            if (util.checkBRMExists(myUpperBC))
            {
                ClientScript.RegisterStartupScript(Page.GetType(), "BRMAlreadyUsed", "alert('Please scan or enter a different BRM Barcode,\\n" + myUpperBC + " is already in use.');", true);
            }
            else
            {
                DC_FILE myfile = en.DC_FILE
                    .Where(f => f.UNQ_FILE_NO == txtUNQ_FILE_NO.Text)
                    .FirstOrDefault();

                if (myfile != null)
                {
                    myfile.BRM_BARCODE = myUpperBC;

                    try
                    {
                        en.DC_ACTIVITY.Add(util.CreateActivity("Files", "Update BRM Barcode"));
                        en.SaveChanges();

                        //ClientScript.RegisterStartupScript(Page.GetType(), "refresh", "window.opener.refresh", true);
                        //ClientScript.RegisterStartupScript(Page.GetType(), "save", "alert('Updated successfully. Refresh to see changes.');window.close()", true);
                        //string gridToUpdate = Request.QueryString["grd"].ToString();

                        //if (gridToUpdate != string.Empty && gridToUpdate == "CURRENT")
                        //{
                        //    ClientScript.RegisterStartupScript(Page.GetType(), "save", "window.opener.UpdateCurrentGrid();window.close()", true);
                        //}
                        //else if (gridToUpdate != string.Empty && gridToUpdate == "CLOSED")
                        //{
                        //    ClientScript.RegisterStartupScript(Page.GetType(), "save", "window.opener.UpdateClosedGrid();window.close()", true);
                        //}
                        //ClientScript.RegisterStartupScript(Page.GetType(), "reload", "window.opener.UpdateGrid();", true);
                        ClientScript.RegisterStartupScript(Page.GetType(), "reload", "window.opener.location.reload();", true);
                        ClientScript.RegisterStartupScript(Page.GetType(), "close", "window.close();", true);
                    }
                    catch (Exception)
                    {
                        ClientScript.RegisterStartupScript(Page.GetType(), "error", "alert('Unable to save data');", true);
                    }
                }
                else
                {
                    ClientScript.RegisterStartupScript(Page.GetType(), "error", "alert('Nothing to update');", true);
                }
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Request.QueryString.Count > 0)
                {
                    if (Request.QueryString["fn"] != null)
                    {
                        pFooter.Visible = true;
                        btnSave.Visible = false;
                    }
                    else
                    {
                        txtUNQ_FILE_NO.Text = Request.QueryString["FileNo"].ToString();
                        txtBRM_BARCODE.Text = "";
                    }

                    if (Request.QueryString["brmBC"] != null)
                    {
                        curBRM.Text = "Current BRM File number :" + Request.QueryString["brmBC"].ToString();
                    }
                }
            }
            txtBRM_BARCODE.Focus();
        }

        #endregion Protected Methods
    }
}