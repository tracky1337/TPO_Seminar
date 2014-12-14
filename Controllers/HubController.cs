using Microsoft.AspNet.SignalR;

namespace TPO_Seminar.SignalR
{
    public class HubController : Hub
    {

        public void Draw(int y, int x, double lineThickness)
        {
            //var hubContext = GlobalHost.ConnectionManager.GetHubContext<HubController>();
            //hubContext.Clients.All.drawPoint(y, x, levelThickness, lineThickness);
            Clients.Others.drawPoint(y, x, lineThickness);
        }

        public void Test(string message)
        {
            Clients.Others.sendMessage(message);
        }
    }
}
