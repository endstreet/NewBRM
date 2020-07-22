using SASSADirectCapture.BL;
using SASSADirectCapture.Services;
using System.Web;
using System.Web.UI;

namespace SASSADirectCapture.Sassa
{
    public class SassaPage : Page
    {
        public static UserSession Usersession   // property
        {
            // get method
            get
            { 
                
                return (UserSession)HttpContext.Current.Session["us"]; 
            }   
            set
            {
                HttpContext.Current.Session["us"] = value;
                _util = new BLUtility(value);
            }  // set method
        }
        private static BLUtility _util = null;
        public static BLUtility util
        {
            get
            {
                if (_util != null) return _util;
                if (Usersession == null) Usersession = (UserSession)HttpContext.Current.Session["us"];
                 _util = new BLUtility(Usersession);
                return _util;
            }
        }
    }
}
