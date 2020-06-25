using Microsoft.Owin;
using Owin;

[assembly: OwinStartup(typeof(SASSADirectCapture.Startup))]

namespace SASSADirectCapture
{
    public class Startup
    {

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic")]
        public void Configuration(IAppBuilder app)
        {
            // Any connection or hub wire up and configuration should go here
            app.MapSignalR();
        }
    }
}