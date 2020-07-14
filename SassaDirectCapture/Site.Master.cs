using SASSADirectCapture.BL;
using SASSADirectCapture.Services;
using System;
using System.DirectoryServices.AccountManagement;
using System.Web;
using System.Web.UI;

namespace SASSADirectCapture
{
    public partial class SiteMaster : MasterPage
    {
        #region Private Fields

        string version = string.Empty;

        UserSession us;

        //private SASSA_Authentication authObject = new SASSA_Authentication();

        private BLUtility util;

        #endregion Private Fields

        #region Protected Methods

        protected void checkUserOffice()
        {
            //Set user logged-in label on heading
            lblUsername.Text = us.Name;
            lblUserRole.Text = us.GetRole();
            txthiddenRegion.Text = us.Office.RegionId;
            txthiddenRegionCode.Text = us.Office.RegionCode;
            lblLocalOffice.Text = us.Office.RegionCode + " - " + us.Office.OfficeName;

            liRsWeb.Visible = true;
            liFD.Visible = true;
            //Show Menu
            switch (us.Office.OfficeType)
            {
                case "LO":
                    liAppSearch.Visible = true;            // to be replaced by fileprep
                    //Comment out the following two lines to temporarily stop using batching.
                    liBatching.Visible = true;
                    liScan.Visible = true;
                    liFileRequestOut.Visible = true;
                    //liReports.Visible = true;
                    break;

                case "RMC":
                    liRMCFileCap.Visible = true;
                    liReceiving.Visible = true;
                    liQC.Visible = true;
                    liBoxAudit.Visible = true;
                    liFileRequestOut.Visible = true;
                    //liReports.Visible = true;
                    liFileRequestIn.Visible = true;

                    break;

                case "SC":
                    liReceiving.Visible = true;
                    //liReports.Visible = true;
                    liFileRequestIn.Visible = true;
                    break;

                default:
                    break;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            us = (UserSession)Session["us"];
            if (!us.IsIntitialized)
            {
                us = new UserSession();

                PrincipalContext ctx = new PrincipalContext(ContextType.Domain, "SASSA");
                UserPrincipal user = UserPrincipal.FindByIdentity(ctx, HttpContext.Current.Request.LogonUserIdentity.Name);
                //string xx = System.Security.Principal.WindowsIdentity.GetCurrent().Name;
                //string xy=  HttpContext.Current.Request.LogonUserIdentity.Name;
                //UserPrincipal user = UserPrincipal.Current;

                us.SamName = user.SamAccountName;
                us.Name = user.Name;
                us.Surname = user.Surname;
                us.AdName = user.UserPrincipalName;
                Session["us"] = us;
                util = new BLUtility(us);
                bool bIsInGroups = util.setUserGroup(us.SamName);/* "SASSA\\VelemseniM"*/
                if (!bIsInGroups)
                {
                    HttpContext.Current.Response.Redirect("~/Default.aspx");
                }
                util.getLocalOffice();
                Session["us"] = util.UserSession;
            }
            checkUserOffice();
        }

        #endregion Protected Methods
    }
}
