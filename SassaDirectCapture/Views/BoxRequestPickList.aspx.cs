using SASSADirectCapture.EntityModels;
using SASSADirectCapture.Sassa;
using System;
using System.Data;
using System.Linq;
using System.Web.UI;

namespace SASSADirectCapture.Views
{
    public partial class BoxRequestPickList : SassaPage
    {
        private Entities en = new Entities();

        protected void Page_Load(object sender, EventArgs e)
        {
            //Dont need to handle authentication as this is done on the master page load.
            if (!IsPostBack)
            {
                string whichgrid = Request.QueryString["picklist"].ToString();
                string picklistno = Request.QueryString["picklistno"].ToString();
                string regid = Usersession.Office.RegionId;
                string username = Usersession.Name; //Session["CSUsername"].ToString();
                string region = string.Empty;
                Page.Title = string.Empty;
                //if (regid != null)
                //{
                region = Usersession.Office.RegionName;//util.getRegion("name", regid);
                //}

                if (whichgrid == "BOX")
                {
                    lblHead.Text = "Box Picklist for " + region + " - Date:" + DateTime.Now.ToString();
                }
            }
            //BoxlistGridView.SelectMethod="GetBoxRequestPickList()";
        }

        public IQueryable<MISBoxesPicked> GetBoxRequestPickList()
        {
            string picklistno = Request.QueryString["picklistno"].ToString();

            IQueryable<MISBoxesPicked> query;
            try
            {
                query =
                    from p in en.DC_BOXPICKLIST
                    where p.UNQ_PICKLIST == picklistno
                    orderby p.BIN_NUMBER, p.BOX_NUMBER
                    select new MISBoxesPicked
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
            catch (Exception)
            {
                return null;
            }

            return query;
        }

        protected void btnPrint_Click(object sender, EventArgs e)
        {
            ClientScript.RegisterStartupScript(Page.GetType(), "print", "printlist();", true);
        }
    }
}