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
        public ActionResult Predmeti()
        {
            return View();
        }
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Delete(int? ServiceId)
        {
            if (ServiceId.HasValue)
            {
                using (var db = new UserContext())
                {
                    var service = db.Services.Find(ServiceId);
                    db.Services.Remove(service);
                    db.SaveChanges();
                }
            }

            return View("Index");
        }

        [HttpPost]
        public ActionResult DeleteSubjectRole(int? Id)
        {
            if (Id.HasValue)
            {
                using (var db = new UserContext())
                {
                    var subjectRole = db.SubjectRoles.Find(Id.Value);
                    if (subjectRole != null)
                    {
                        db.SubjectRoles.Remove(subjectRole);
                        db.SaveChanges();
                    }
                }
            }
            return View("Predmeti");

        }

        [HttpPost]
        public ActionResult DodajStoritev(int? SubjectId, string PricePerHour)
        {
            if (!SubjectId.HasValue || PricePerHour.Length == 0) return View("Predmeti");
            using (var model = new UserContext())
            {
                var price = Convert.ToDecimal(PricePerHour.Replace('.', ','));
                var instructor = model.Instruktors.FirstOrDefault(inst => inst.UserProfileId == WebSecurity.CurrentUserId);
                if (instructor != null)
                {
                    var instructorId = instructor.Id;
                    var subjectRole = new SubjectRoles()
                    {
                        InstructorId = instructorId,
                        SubjectId = SubjectId.Value,
                        PricePerHour = price
                    };
                    model.SubjectRoles.Add(subjectRole);
                    model.SaveChanges();

                }
            }
            return View("Predmeti");
        }

        [HttpPost]
        public ActionResult Index(int SubjectId)
        {
            if (ModelState.IsValid)
            {
                var service = new Services();

                using (var model = new UserContext())
                {
                    service = new Services()
                    {
                        Active = true,
                        CreationDate = DateTime.Now,
                        InstructorId = model.Instruktors.Where(inst => inst.UserProfileId == WebSecurity.CurrentUserId).FirstOrDefault().Id,
                        SubjectId = SubjectId
                    };
                    model.Services.Add(service);
                    model.SaveChanges();
                }



            }
            return View();
        }

    }
}
