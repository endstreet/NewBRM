using SASSADirectCapture.BL;
using SASSADirectCapture.EntityModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SASSADirectCapture.Views
{
    public partial class PickList : System.Web.UI.Page
    {
        public SASSA_Authentication authObject = new SASSA_Authentication();
        Entities en = new Entities();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                //Get the username of the user that is logged in from session.
                string authenticatedUsername = this.authObject.AuthenticateCSAdminUser();

                //If no session values are found , redirect to the login screen
                if (authenticatedUsername == string.Empty)
                {
                    this.authObject.RedirectToLoginPage();
                }
            }
            
            Page.Title = string.Empty;
            fileGridView.SelectMethod = "GetFileRequestHistory";
        }

        public IQueryable<FileRequest> GetFileRequestHistory()
        {
            var x = new BLUtility().getSessionLocalOfficeId();
            string shortdate = "";
            // Retrieve all files that have been submitted by local offices in the same region as the RMC office.
            // Only show files that have not been updated or closed.

            IQueryable<FileRequest> query;
            try
            {
                query =
                    from fr in en.DC_FILE_REQUEST
                    join region in en.DC_REGION on fr.REGION_ID equals region.REGION_ID
                    join lo in en.DC_LOCAL_OFFICE on fr.REQUESTED_OFFICE_ID equals lo.OFFICE_ID
                    join lou in en.DC_LOCAL_OFFICE on lo.REGION_ID equals lou.REGION_ID
                    join c1 in en.DC_REQ_CATEGORY on fr.REQ_CATEGORY equals c1.CATEGORY_ID
                    join c2 in en.DC_REQ_CATEGORY_TYPE on fr.REQ_CATEGORY_TYPE equals c2.TYPE_ID
                    where fr.SCANNED_DATE == null 
                    && fr.PICKLIST_STATUS == "In Progress" 
                    && fr.CLOSED_DATE == null
                    && lou.OFFICE_ID == x 

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
                        REGION_ID = fr.REGION_ID,
                        REQUESTED_DATE = fr.REQUESTED_DATE,
                        REQUESTED_OFFICE_NAME = lo.OFFICE_NAME,
                        RELATED_MIS_FILE_NO = fr.RELATED_MIS_FILE_NO,
                        REGION_NAME = region.REGION_NAME,
                        PICKLIST_TYPE = fr.PICKLIST_TYPE,
                        PICKLIST_STATUS = fr.PICKLIST_STATUS,
                        PICKED_BY = fr.PICKED_BY,
                        PICKLIST_NO = fr.PICKLIST_NO,
                        SCANNED_PHYSICAL_IND = fr.SCANNED_PHYSICAL_IND,
                        REQUEST_CAT_ID = fr.REQ_CATEGORY,
                        REQUEST_CAT_TYPE_ID = fr.REQ_CATEGORY_TYPE,
                        REQUEST_CAT_DETAIL = fr.REQ_CATEGORY_DETAIL,
                        CATEGORY_DESCR = c1.CATEGORY_DESCR,
                        TYPE_DESCR = c2.TYPE_DESCR
                    };
            }
            catch (Exception ex)
            {
                return null;
            }

            return query;
        }

        //protected void fileGridView_PageIndexChanging(object sender, GridViewPageEventArgs e)
        //{
        //    fileGridView.PageIndex = e.NewPageIndex;
        //    fileGridView.SelectMethod = "GetFileRequestHistory";
        //}

        protected void hiddenBtnData_Click(object sender, EventArgs e)
        {
            //Button is clicked via javascript to reload the grid after a successful update on a file.
            fileGridView.SelectMethod = "GetFileRequestHistory";
        }
    }
}


