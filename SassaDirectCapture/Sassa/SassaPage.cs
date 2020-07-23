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
            }  // set method
        }
        public static BLUtility util
        {
            get
            {
                return new BLUtility(Usersession);
            }
        }
    }
}
