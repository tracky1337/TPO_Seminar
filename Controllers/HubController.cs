using System;
using System.Linq;
using Microsoft.AspNet.SignalR;
using TPO_Seminar.Models;
using WebMatrix.WebData;

namespace TPO_Seminar.SignalR
{

    public class HubController : Hub
    {
        public void Draw(int y, int x, string fill, string size)
        {
            Clients.Others.drawPoint(y, x,fill,size);
        }

        public void SendVoiceMessage(string message)
        {
            string x = "";
            using (var entity = new CustomModels())
            {
                int id = Convert.ToInt32(message);
                var blob = entity.Blobs.FirstOrDefault(b => b.Id == id);
                Clients.Others.sendVoiceMessage(blob.Blob);
            }
        }

        public void SendMessage(string message)
        {
            var username = WebSecurity.CurrentUserName;
            var date = DateTime.Now.ToString("d MMMM, H:mm");
            var messageCaller =  "<li class=\"right clearfix\"><span class=\"chat-img pull-right\">    <img src=\"http://placehold.it/50/FA6F57/fff&text=ME\" alt=\"User Avatar\" class=\"img-circle\" /></span>    <div class=\"chat-body clearfix\">        <div class=\"header\">            <small class=\" text-muted\"><span class=\"glyphicon glyphicon-time\"></span>"+date+"</small>            <strong class=\"pull-right primary-font\">" + username + "</strong>        </div>        <p>            " + message + "        </p>    </div></li>";
            var messageOthers =  "<li class=\"left clearfix\"><span class=\"chat-img pull-left\">    <img src=\"http://placehold.it/50/55C1E7/fff&text=U\" alt=\"User Avatar\" class=\"img-circle\" />   </span>    <div class=\"chat-body clearfix\">        <div class=\"header\">            <strong class=\"primary-font\">"+username+"</strong> <small class=\"pull-right text-muted\">                <span class=\"glyphicon glyphicon-time\"></span>"+date+"</small>        </div>        <p>            "+message+"            </p>    </div></li>";
            Clients.Caller.sendMessage(messageCaller);
            Clients.Others.sendMessage(messageOthers);
        }

    }
}
