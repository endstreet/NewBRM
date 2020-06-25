using SASSADirectCapture.BL;
using SASSADirectCapture.Services;
using System.Web;
using System.Web.UI;

namespace SASSADirectCapture.Sassa
{
    public class SassaPage : Page
    {
        public static UserSession UserSession   // property
        {
            get { return (UserSession)HttpContext.Current.Session["us"]; }   // get method
            set
            {
                HttpContext.Current.Session["us"] = value;
                _util = null;
            }  // set method
        }
        private static BLUtility _util;
        public static BLUtility util
        {
            get
            {
                if (_util != null) return _util;
                return new BLUtility((UserSession)HttpContext.Current.Session["us"]);
            }
        }
    }
}