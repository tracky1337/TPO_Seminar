using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Newtonsoft.Json;
using TPO_Seminar.Models;
using WebMatrix.WebData;

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
                        model.SubjectRoles.Where(el => el.SubjectId == subjectId).Select(
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
            return View();
        }

        public ActionResult DeleteOrder(int? Id)
        {
            using (var model = new UserContext())
            {
                var order = model.Orders.Find(Id);
                if (order == null) return RedirectToAction("Dogodek", "Najemi");

                model.Orders.Remove(order);
                model.SaveChanges();

                return RedirectToAction("Dogodek", "Najemi");
            }
        }

        public ActionResult Dogodek()
        {
            using (var model = new UserContext())
            {
                var student = model.Students.FirstOrDefault(el => el.UserProfileId == WebSecurity.CurrentUserId);
                if (student == null) return View(-1);
                return View(student.Id);
            }
        }

        public ActionResult Kosarica()
        {
            var model = new UserContext();

            var student = model.Students.FirstOrDefault(el => el.UserProfileId == WebSecurity.CurrentUserId);
            if (student == null) return View(new List<Orders>().AsEnumerable());
            var items =
                model.Orders.Where(el => el.StudentId == student.Id && el.Approved == false)
                    .OrderByDescending(el => el.OrderDate)
                    .ThenBy(el => el.Subjects.SubjectName).AsEnumerable();
            return View(items);
        }

        public ActionResult DeleteItemCart(int? id)
        {
            using (var model = new UserContext())
            {
                var order = model.Orders.Find(id);
                if (order == null) return RedirectToAction("Kosarica", "Najemi");
                model.Orders.Remove(order);
                model.SaveChanges();
            }
            return RedirectToAction("Kosarica", "Najemi");
        }

        public string GetInActiveItems()
        {
            using (var model = new UserContext())
            {
                var student = model.Students.FirstOrDefault(el => el.UserProfileId == WebSecurity.CurrentUserId);
                if (student == null) return "0";
                return model.Orders.Count(el => el.Approved == false && el.StudentId == student.Id).ToString();
            }
        }

        public ActionResult CartStatus()
        {
            return Content(GetInActiveItems());
        }

        [HttpPost]
        public ActionResult AddItemToCard(int? instructorId, int? subjectId, string orderDate, int? available)
        {
            if (instructorId.HasValue && subjectId.HasValue)
            {

                using (var model = new UserContext())
                {
                    var date = DateTime.Parse(orderDate);
                    var studentId = model.Students.FirstOrDefault(el => el.UserProfileId == WebSecurity.CurrentUserId);
                    if (studentId == null) return Content(GetInActiveItems());
                    var order = new Orders();
                    if (available == 1)
                    {
                        order = new Orders()
                        {
                            InstructorId = instructorId.Value,
                            StudentId = studentId.Id,
                            SubjectId = subjectId.Value,
                            OrderDate = date
                        };
                        model.Orders.Add(order);
                        model.SaveChanges();
                    }
                    else
                    {
                        order =
                            model.Orders.FirstOrDefault(
                                el =>
                                    el.InstructorId == instructorId.Value && el.StudentId == studentId.Id &&
                                    el.SubjectId == subjectId.Value && el.OrderDate == date);
                        if (order != null)
                        {
                            //delete order
                            model.Orders.Remove(order);
                            model.SaveChanges();
                        }
                    }

                }

            }

            return Content(GetInActiveItems());
        }

        [HttpPost]
        public ActionResult Rate(int? orderId, int? rating)
        {
            if (orderId.HasValue && rating.HasValue)
            {
                using (var model = new UserContext())
                {
                    var order = model.Orders.Find(orderId);
                    if (order == null) return Content("");
                    var ratingEntity = new Ratings()
                    {
                        InstructorId = order.InstructorId,
                        StudentId = order.StudentId,
                        OrderId = order.Id,
                        Rating = rating.Value
                    };
                    model.Ratings.Add(ratingEntity);
                    model.SaveChanges();
                }
            }
            return Content("");
        }

        public ActionResult Profesorji()
        {
            return View();
        }


    }
}
