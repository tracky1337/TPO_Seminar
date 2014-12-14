using Microsoft.Owin;
using Owin;
using TPO_Seminar;

[assembly: OwinStartup(typeof(Startup))]
namespace TPO_Seminar
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            app.MapSignalR();
        }
    }
}

