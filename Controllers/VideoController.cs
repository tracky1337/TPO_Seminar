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

        [HttpPost]
        public ActionResult Host(int? Id)
        {
            if (Id.HasValue)
            {
                return View(Id);
            }
            return View(-1);
        }

        [HttpPost]
        public ActionResult ViewInstruction(int? Id)
        {
            if (Id.HasValue)
            {
                return View(Id);
            }
            return View(-1);
        }

        [HttpPost]
        public string Upload(string blob)
        {
            using (var entity = new UserContext())
            {
                var blobElement = new Blobs() { Blob = blob };
                entity.Blobs.Add(blobElement);
                entity.SaveChanges();
                return blobElement.Id.ToString();
            }
        }



    }
}
