using Microsoft.AspNet.SignalR;

namespace SASSADirectCapture.Hub
{
    public class ProgressHub : Microsoft.AspNet.SignalR.Hub
    {
        public static void SendMessage(string groupName, string msg, string value)
        {
            var hubContext = GlobalHost.ConnectionManager.GetHubContext<ProgressHub>();
            //hubContext.Clients.All.sendNotification(string.Format(msg), string.Format(value));
            hubContext.Clients.Group(groupName).sendNotification(string.Format(msg), string.Format(value));
        }

        public void addUserToRegionGroup(string form)
        {
            Groups.Add(Context.ConnectionId, form);
        }
    }
}