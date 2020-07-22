using SASSADirectCapture.EntityModels;
using SASSADirectCapture.Sassa;
using System;
using System.Data;
using System.Linq;
using System.Web.UI;

namespace SASSADirectCapture.Views
{
    public partial class FileRequestPickList : SassaPage
    {
        #region Private Fields

        private Entities en = new Entities();

        #endregion Private Fields

        #region Public Methods

        public IQueryable<FileRequest> GetRMCPicklist()
        {
            var x = Usersession.Office.OfficeId;
            string pstatus = "In Progress";
            string region = Usersession.Office.RegionId;

            // Retrieve all files that have been submitted by local offices in the same region as the RMC office.
            // Only show files that have aRE iN Progress and not sent to TDW.

            IQueryable<FileRequest> query;
            try
            {
                query =
                    from fr in en.DC_FILE_REQUEST
                    where fr.SCANNED_DATE == null
                    && fr.CLOSED_DATE == null
                    && fr.REGION_ID == region
                    && fr.SENT_TDW != "Y"
                    && fr.PICKLIST_STATUS == pstatus
                    orderby fr.REQUESTED_DATE, fr.ID_NO

                    select new FileRequest
                    {
                        UNQ_FILE_NO = fr.UNQ_FILE_NO,
                        ID_NO = fr.ID_NO,
                        BRM_BARCODE = fr.BRM_BARCODE,
                        MIS_FILE_NO = fr.MIS_FILE_NO,
                        BIN_ID = fr.BIN_ID,
                        BOX_NUMBER = fr.BOX_NUMBER,
                        POSITION = fr.POSITION,
                        TDW_BOXNO = fr.TDW_BOXNO,
                        NAME = fr.NAME,
                        SURNAME = fr.SURNAME,
                        REQUESTED_DATE = fr.REQUESTED_DATE,
                        RELATED_MIS_FILE_NO = fr.RELATED_MIS_FILE_NO,
                        SCANNED_PHYSICAL_IND = fr.SCANNED_PHYSICAL_IND,
                        REQUEST_CAT_ID = fr.REQ_CATEGORY,
                        REQUEST_CAT_TYPE_ID = fr.REQ_CATEGORY_TYPE,
                        REQUEST_CAT_DETAIL = fr.REQ_CATEGORY_DETAIL,
                    };
            }
            catch (Exception)
            {
                return null;
            }

            return query;
        }

        public IQueryable<FileRequest> GetTDWPicklist()
        {
            var x = Usersession.Office.OfficeId;
            string pstatus = "In Progress";
            string region = Usersession.Office.RegionId;
            // Retrieve all files that have been submitted by local offices in the same region as the RMC office.
            // Only show files that have aRE iN Progress and not sent to TDW.

            IQueryable<FileRequest> query;
            try
            {
                query =
                   from fr in en.DC_FILE_REQUEST
                   where fr.SCANNED_DATE == null
                   && fr.CLOSED_DATE == null
                   && fr.REGION_ID == region
                   && fr.PICKLIST_STATUS == pstatus
                   && fr.SENT_TDW == "Y"
                   orderby fr.REQUESTED_DATE, fr.ID_NO

                   select new FileRequest
                   {
                       UNQ_FILE_NO = fr.UNQ_FILE_NO,
                       ID_NO = fr.ID_NO,
                       BRM_BARCODE = fr.BRM_BARCODE,
                       MIS_FILE_NO = fr.MIS_FILE_NO,
                       BIN_ID = fr.BIN_ID,
                       BOX_NUMBER = fr.BOX_NUMBER,
                       POSITION = fr.POSITION,
                       TDW_BOXNO = fr.TDW_BOXNO,
                       NAME = fr.NAME,
                       SURNAME = fr.SURNAME,
                       REQUESTED_DATE = fr.REQUESTED_DATE,
                       RELATED_MIS_FILE_NO = fr.RELATED_MIS_FILE_NO,
                       SCANNED_PHYSICAL_IND = fr.SCANNED_PHYSICAL_IND,
                       REQUEST_CAT_ID = fr.REQ_CATEGORY,
                       REQUEST_CAT_TYPE_ID = fr.REQ_CATEGORY_TYPE,
                       REQUEST_CAT_DETAIL = fr.REQ_CATEGORY_DETAIL,
                   };
            }
            catch (Exception)
            {
                return null;
            }

            return query;
        }

        #endregion Public Methods

        #region Protected Methods

        protected void btnPrint_Click(object sender, EventArgs e)
        {
            ClientScript.RegisterStartupScript(Page.GetType(), "print", "printlist();", true);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            //Dont need to handle authentication as this is done on the master page load.
            if (!IsPostBack)
            {
                string whichgrid = Request.QueryString["picklist"].ToString();
                //if (whichgrid == "BOX")
                //{
                //    isBoxList = true;
                //}
                //else
                //{
                //string regid = Session["CSUserOfficeRegion"].ToString();
                string region = " for ";

                //if (regid != null)
                //{
                region += Usersession.Office.RegionName;//util.getRegion("name", regid);
                //}

                if (whichgrid == "RMC")
                {
                    fileGridView.SelectMethod = "GetRMCPicklist";
                }
                else
                {
                    fileGridView.SelectMethod = "GetTDWPicklist";
                }

                lblHead.Text = whichgrid + " File Picklist" + region + " - Date:" + DateTime.Now.ToString();
                //}
            }
        }

        #endregion Protected Methods
    }
}
