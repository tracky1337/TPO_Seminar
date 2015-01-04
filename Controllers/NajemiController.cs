using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Newtonsoft.Json;
using TPO_Seminar.Models;

namespace TPO_Seminar.Controllers
{
    public class NajemiController : Controller
    {
        //
        // GET: /Najemi/

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult GetSubjectProviders(int? subjectId)
        {
            if (subjectId.HasValue)
            {
                using (var model = new UserContext())
                {
                    var providers =
                        model.SubjectRoles.Where(el => el.SubjectId==subjectId).Select(
                            el =>
                                new ResultSubjectProvider()
                                {
                                    Instructor =
                                        el.Instruktors.UserProfile.UserName + " (" + el.Instruktors.Podjetje + ")",
                                    PricePerHour = el.PricePerHour,
                                    InstructorId = el.InstructorId
                                }).OrderBy(el => el.PricePerHour).ThenBy(el => el.Instructor).ToList();
                    return
                        Content(
                            JsonConvert.SerializeObject(new JsonEventSubjectProviders()
                            {
                                success = 1,
                                result = providers
                            }));
                }
            }
            return Content("");
        }


        public ActionResult Instruktor(int? instructorId, int? subjectId)
        {
            if (instructorId.HasValue)
            {
                

            }
            return View();
        }
    }
}
