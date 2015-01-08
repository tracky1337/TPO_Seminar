using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.SignalR;
using TPO_Seminar.Models;
using WebMatrix.WebData;

namespace TPO_Seminar.SignalR
{

    public class HubController : Hub
    {
        public bool IsAllowedToConnect(int orderId)
        {
            using (var model = new UserContext())
            {
                var order = model.Orders.Find(orderId);
                if (order == null || !order.Approved) return false;

                if (Context.User.IsInRole("Instruktor"))
                {
                    var instructor = model.Instruktors.FirstOrDefault(el => el.UserProfileId == WebSecurity.CurrentUserId);
                    if (instructor == null) return false;

                    if (order.InstructorId == instructor.Id) return true;
                }
                else if (Context.User.IsInRole("Ucenec"))
                {
                    var student = model.Students.FirstOrDefault(el => el.UserProfileId == WebSecurity.CurrentUserId);
                    if (student == null) return false;

                    if (order.StudentId == student.Id) return true;
                }
            }
            return false;
        }

        public override Task OnConnected()
        {
            var token = Context.QueryString["token"];
            if (!string.IsNullOrEmpty(token) && IsAllowedToConnect(Convert.ToInt32(token)))
                Groups.Add(Context.ConnectionId, token);
            return base.OnConnected();
        }



        public void Draw(int y, int x, string fill, string size, int orderId)
        {
            Clients.OthersInGroup(orderId.ToString()).drawPoint(y, x, fill, size);
        }

        public void SendVoiceMessage(string message, int orderId)
        {
            using (var entity = new UserContext())
            {
                //retrieve blob
                int id = Convert.ToInt32(message);
                var blob = entity.Blobs.FirstOrDefault(b => b.Id == id);

                Clients.OthersInGroup(orderId.ToString()).sendVoiceMessage(blob.Blob);

                //delete from db
                entity.Blobs.Remove(blob);
                entity.SaveChanges();
            }
        }

        public void SendMessage(string message, int orderId)
        {
            var username = WebSecurity.CurrentUserName;
            var date = DateTime.Now.ToString("d MMMM, H:mm");
            var messageCaller = "<li class=\"right clearfix\"><span class=\"chat-img pull-right\">    <img src=\"http://placehold.it/50/FA6F57/fff&text=ME\" alt=\"User Avatar\" class=\"img-circle\" /></span>    <div class=\"chat-body clearfix\">        <div class=\"header\">            <small class=\" text-muted\"><span class=\"glyphicon glyphicon-time\"></span>" + date + "</small>            <strong class=\"pull-right primary-font\">" + username + "</strong>        </div>        <p>            " + message + "        </p>    </div></li>";
            var messageOthers = "<li class=\"left clearfix\"><span class=\"chat-img pull-left\">    <img src=\"http://placehold.it/50/55C1E7/fff&text=U\" alt=\"User Avatar\" class=\"img-circle\" />   </span>    <div class=\"chat-body clearfix\">        <div class=\"header\">            <strong class=\"primary-font\">" + username + "</strong> <small class=\"pull-right text-muted\">                <span class=\"glyphicon glyphicon-time\"></span>" + date + "</small>        </div>        <p>            " + message + "            </p>    </div></li>";
            Clients.Caller.sendMessage(messageCaller);
            Clients.OthersInGroup(orderId.ToString()).sendMessage(messageOthers);
        }

    }
}
