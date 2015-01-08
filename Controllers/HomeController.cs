using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;

namespace TPO_Seminar.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            ViewBag.Message = "Modify this template to jump-start your ASP.NET MVC application.";

            return View();
        }


        [HttpPost]
        public ActionResult SendMail(string sender, string message)
        {
            try
            {
                var body = "Od:" + sender + "\nSporocilo:" + message;
                SmtpClient client = new SmtpClient("smtp.gmail.com", 587)
                {
                    EnableSsl = true,
                    Credentials = new NetworkCredential("tposeminar@gmail.com", "seminar123")
                };
                client.Send("tposeminar@gmail.com", "tposeminar@gmail.com", "Sporočilo iz strani", body);
            }catch { }
            return Content("");
        }

        public ActionResult Profesorji()
        {
            return View();
        }
    }
}
