using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using SASSADirectCapture.EntityModels;
using SASSADirectCapture.BL;

namespace SASSADirectCapture.Views
{
    public partial class PrintBoxPickList : System.Web.UI.Page
    {
        Entities en = new Entities();
        BLUtility util = new BLUtility();
        private SASSA_Authentication authObject = new SASSA_Authentication();

        protected void Page_Load(object sender, EventArgs e)
        {
            //Dont need to handle authentication as this is done on the master page load.
            if (!IsPostBack)
            {
                //string whichgrid = Request.QueryString["type"].ToString();
                string whichgrid = Request.QueryString["picklist"].ToString();
                string picklistno = Request.QueryString["picklistno"].ToString();
                string regid = Session["CSUserOfficeRegion"].ToString();
                string username = Session["CSUsername"].ToString();
                string region = string.Empty;
                Page.Title = string.Empty;
                if (regid != null)
                {
                    region += new BLUtility().getRegion("name", regid);
                }

                if (whichgrid=="BOX")
                {
                    lblHead.Text = whichgrid + " File Picklist for " + region + " - Date:" +DateTime.Now.ToString();
                    BoxlistGridView.SelectMethod = "GetBoxRequestPickList()";
                }
                //else
                //{
                //    lblHead.Text = whichgrid + " Personal Picklists for " + username.ToUpper() + " - Date:" + DateTime.Now.ToString();
                //    PicklistGridView.SelectMethod = "FindPicklists()";
                //}
                //string strscript = "hideShowDiv('"+whichgrid+"')";
                //ClientScript.RegisterStartupScript(Page.GetType(), "ShowDiv", strscript, true);
            }
        }
        public IQueryable<DC_BOXPICKLIST> GetBoxRequestPickList()
        {
            string picklistno = Request.QueryString["picklistno"].ToString();

            IQueryable <DC_BOXPICKLIST> query;
            try
            {
                query =
                    from p in en.DC_BOXPICKLIST 
                    where p.UNQ_PICKLIST == picklistno
                    orderby p.BIN_NUMBER, p.BOX_NUMBER
                    select new DC_BOXPICKLIST
                    {
                        UNQ_PICKLIST = p.UNQ_PICKLIST,
                        REGION_ID = p.REGION_ID,
                        REGISTRY_TYPE = p.REGISTRY_TYPE,
                        USERID = p.USERID,
                        PICKLIST_DATE = p.PICKLIST_DATE,
                        PICKLIST_STATUS = p.PICKLIST_STATUS,
                        UNQ_NO = p.UNQ_NO,
                        BIN_NUMBER = p.BIN_NUMBER,
                        BOX_NUMBER = p.BOX_NUMBER,
                        BOX_RECEIVED = p.BOX_RECEIVED,
                        BOX_COMPLETED = p.BOX_COMPLETED,
                        ARCHIVE_YEAR = p.ARCHIVE_YEAR
                    };
            }
            catch (Exception ex)
            {
                return null;
            }

            return query;

                    ////ARCHIVE_YEAR VARCHAR2(4)
                    ////BOX_COMPLETED   CHAR(1)
                    ////BOX_RECEIVED    CHAR(1)
                    ////BOX_NUMBER  VARCHAR2(10)
                    ////BIN_NUMBER  VARCHAR2(10)
                    ////UNQ_NO  VARCHAR2(20)
                    ////PICKLIST_STATUS CHAR(1)
                    ////PICKLIST_DATE   DATE
                    ////USERID  NUMBER
                    ////REGISTRY_TYPE   CHAR(1)
                    ////REGION_ID   VARCHAR2(10)
                    ////UNQ_PICKLIST    VARCHAR2(20)
        }

        protected void btnPrint_Click(object sender, EventArgs e)
        {
            ClientScript.RegisterStartupScript(Page.GetType(), "print", "printlist();", true);            
        }
    }
}
