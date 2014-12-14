using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using System.Web.Services.Description;
using TPO_Seminar.Models;
using WebMatrix.WebData;

namespace TPO_Seminar.Controllers
{
    public class StoritveController : Controller
    {
        //
        // GET: /Storitve/

        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Delete(int? ServiceId)
        {
            if (ServiceId.HasValue)
            {
                using (CustomModels db = new CustomModels())
                {
                    var service = db.Services.Find(ServiceId);
                    db.Services.Remove(service);
                    db.SaveChanges();
                }
            }

            return View("Index");
        }

        [HttpPost]
        public ActionResult Index(int SubjectId)
        {
            if (ModelState.IsValid)
            {
                var service = new Services();
                    
                using (UsersContext usr = new UsersContext())
                {
                    service = new Services()
                    {
                        Active = true,
                        CreationDate = DateTime.Now,
                        InstructorId = usr.Instruktors.Where(inst => inst.UserProfileId == WebSecurity.CurrentUserId).FirstOrDefault().Id,
                        SubjectId = SubjectId
                    };
                }

                using (CustomModels db = new CustomModels())
                {
                    db.Services.Add(service);
                    db.SaveChanges();
                }
            }
            return View();
        }

    }
}
