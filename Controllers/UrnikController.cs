using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects;
using System.Globalization;
using System.Linq;
using System.Web.Helpers;
using System.Web.Mvc;
using Newtonsoft.Json;
using TPO_Seminar.Models;
using WebMatrix.WebData;

namespace TPO_Seminar.Controllers
{
    public class UrnikController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Seznam(long from, long to, string browser_timezone)
        {

            ResultDay res = new ResultDay()
            {
                hour = "15"
            };
            JsonEventDay e = new JsonEventDay() { success = 1, result = new List<ResultDay>() { res } };
            return Content(JsonConvert.SerializeObject(e), "application/json");
        }

        public ActionResult GetMonthlyAvailability(string date, int? instructorId)
        {
            using (var model = new UserContext())
            {
                if (!instructorId.HasValue)
                {
                    var instructor = model.Instruktors.FirstOrDefault(
                        user => user.UserProfileId == WebSecurity.CurrentUserId);
                    if (instructor != null)
                        instructorId = instructor.Id;
                    else
                        return Json("", JsonRequestBehavior.AllowGet);
                }

                var dt = DateTime.ParseExact(date, "yyyy-MM-dd", CultureInfo.CurrentCulture);

                var list =
                    model.Schedules.Where(
                        el =>
                            el.InstruktorId == instructorId && el.ScheduleDate.Year == dt.Year &&
                            el.ScheduleDate.Month == dt.Month && el.Available)
                        .Select(
                        el => new ResultMonth() { month = el.ScheduleDate.Year + "-" + (el.ScheduleDate.Month.ToString().Length == 1 ? "0" + el.ScheduleDate.Month.ToString() : el.ScheduleDate.Month.ToString()) + "-" + (el.ScheduleDate.Day.ToString().Length == 1 ? "0" + el.ScheduleDate.Day.ToString() : el.ScheduleDate.Day.ToString()) }).Distinct().ToList();

                var e = new JsonEventMonth() { success = 1, result = list };
                return Content(JsonConvert.SerializeObject(e), "application/json");
            }

        }

        public ActionResult GetAvailability(string date, int? instructorId)
        {
            using (var model = new UserContext())
            {
                if (!instructorId.HasValue)
                {
                    var instructor = model.Instruktors.FirstOrDefault(
    user => user.UserProfileId == WebSecurity.CurrentUserId);
                    if (instructor != null)
                        instructorId = instructor.Id;
                    else
                        return Json("", JsonRequestBehavior.AllowGet);
                }

                var dt = DateTime.ParseExact(date, "yyyy-MM-dd", CultureInfo.CurrentCulture);
                var list =
                    model.Schedules.Where(
                        el =>
                            el.InstruktorId == instructorId.Value && el.ScheduleDate.Year == dt.Year &&
                            el.ScheduleDate.Month == dt.Month && el.ScheduleDate.Day == dt.Day && el.Available)
                        .Select(el => new ResultDay() { hour = el.ScheduleDate.Hour.ToString() })
                        .ToList();

                //remove instructors that are busy
                var busyInstructorHours = model.Orders.Where(el => el.Approved && el.InstructorId == instructorId)
                    .Select(el => new ResultDay() {hour = el.OrderDate.Hour.ToString()})
                    .Distinct()
                    .ToList();

                foreach (var item in busyInstructorHours)
                {
                    var listItem = list.FirstOrDefault(el => el.hour == item.hour);
                    if (listItem == null) continue;

                    list.Remove(listItem);
                }


                JsonEventDay e = new JsonEventDay() { success = 1, result = list };
                return Content(JsonConvert.SerializeObject(e), "application/json");

            }

        }

        /*public ActionResult GetAvailability(string date)
        {
            int? instructorId = null;
            using (var model = new UserContext())
            {

                var instructor = model.Instruktors.FirstOrDefault(
user => user.UserProfileId == WebSecurity.CurrentUserId);
                if (instructor != null)
                    instructorId = instructor.Id;
                else
                    return Json("", JsonRequestBehavior.AllowGet);


                var dt = DateTime.ParseExact(date, "yyyy-MM-dd", CultureInfo.CurrentCulture);
                var list =
                    model.Schedules.Where(
                        el =>
                            el.InstruktorId == instructorId.Value && el.ScheduleDate.Year == dt.Year &&
                            el.ScheduleDate.Month == dt.Month && el.ScheduleDate.Day == dt.Day && el.Available)
                        .Select(el => new ResultDay() { hour = el.ScheduleDate.Hour.ToString() })
                        .ToList();
                JsonEventDay e = new JsonEventDay() { success = 1, result = list };
                return Content(JsonConvert.SerializeObject(e), "application/json");

            }

        }*/



        [HttpPost]
        public void SetAvailability(int hour, int available, string date)
        {
            int? instructorId = null;
            using (var model = new UserContext())
            {
                var instructor = model.Instruktors.FirstOrDefault(
                    user => user.UserProfileId == WebSecurity.CurrentUserId);
                if (instructor != null)
                    instructorId = instructor.Id;
                else
                    return;

                var dt = DateTime.ParseExact(date, "yyyy-MM-dd", CultureInfo.CurrentCulture).AddHours(hour);
                var entry = model.Schedules.FirstOrDefault(en => en.ScheduleDate == dt && en.InstruktorId == instructorId.Value);
                var availability = Convert.ToBoolean(available);
                if (entry != null)
                {
                    entry.Available = availability;
                    model.SaveChanges();
                }
                else
                {
                    //entry doesn't exist
                    entry = new Schedules() { InstruktorId = instructorId.Value, ScheduleDate = dt, Available = availability };
                    model.Schedules.Add(entry);
                    model.SaveChanges();
                }
            }
        }
    }
}