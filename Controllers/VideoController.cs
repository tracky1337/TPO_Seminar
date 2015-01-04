using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TPO_Seminar.Models;

namespace TPO_Seminar.Controllers
{
    public class VoicePassage
    {
        public string Title { get; set; }
        public string FileName { get; set; }
        public HttpPostedFileBase Recording { get; set; }
    }
    public class VideoController : Controller
    {
        //
        // GET: /Video/

        public ActionResult Host()
        {
            return View();
        }

        public ActionResult ViewInstruction()
        {
            return View();
        }

        [HttpPost]
        public string Upload(string blob)
        {
            using (var entity = new UserContext())
            {
                var blobElement = new Blobs() {Blob = blob};
                entity.Blobs.Add(blobElement);
                entity.SaveChanges();
                return blobElement.Id.ToString();
            }
        }



    }
}
