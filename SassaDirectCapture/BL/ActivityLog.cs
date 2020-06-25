using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SASSADirectCapture.EntityModels;

namespace SASSADirectCapture.BL
{
    public static class ActivityLog
    {
        #region Public Methods

        public static DC_ACTIVITY Create(string Area, string Activity)
        {
            DC_ACTIVITY activity = null;
            SASSA_Authentication authObject = new SASSA_Authentication();
            BLUtility util = new BLUtility();

            int UserId = authObject.getUserID();
            string UserName = authObject.getUserLogin();
            if (UserId != -1)
            {
                var LookupName = util.getUserFullName(UserId.ToString());

                UserName = LookupName == "unknown" ? UserName : LookupName;
            }

            int OfficeId = int.Parse(util.getSessionLocalOfficeId());

            activity = new DC_ACTIVITY { OFFICE_ID = OfficeId, USERID = UserId, USERNAME = UserName, AREA = Area, ACTIVITY = Activity, RESULT = "OK" };
            activity.ACTIVITY_DATE = DateTime.Now;

            return activity;
        }

        #endregion Public Methods
    }
}