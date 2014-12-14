using Microsoft.AspNet.SignalR;

namespace TPO_Seminar.SignalR
{
    public class HubController : Hub
    {

        public void Draw(int y, int x, int levelThickness, int lineThickness)
        {
            var hubContext = GlobalHost.ConnectionManager.GetHubContext<HubController>();
            hubContext.Clients.All.drawPoint(y, x, levelThickness, lineThickness);
            //Clients.Others
        }

    }
}
