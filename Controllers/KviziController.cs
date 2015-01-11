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
using System.Data.Entity;

namespace TPO_Seminar.Controllers
{
    public class KviziController : Controller
    {
        //
        // GET: /Kvizi/
        public ActionResult SolveKviz()
        {
            return View();
        }

        public ActionResult EditKviz()
        {
            return View();
        }

        public ActionResult NewKviz(int? InstructorId, int? SubjectId)
        {
            return View();
        }
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult CreateQuiz(int SubjectRolesId, string QuizName, string QuizData)
        {

            if (ModelState.IsValid)
            {
                var quiz = new Quizzes();

                using (UserContext usr = new UserContext())
                {
                    quiz = new Quizzes()
                    {
                        SubjectRolesId = SubjectRolesId,
                        QuizName = QuizName,
                        QuizData = QuizData
                    };
                }

                using (UserContext db = new UserContext())
                {
                    db.Quizzes.Add(quiz);
                    db.SaveChanges();
                }
            }

            return RedirectToAction("Index", "Kvizi");
        }

        [HttpPost]
        public ActionResult DodajQuiz(int? SubjectRolesId)
        {
            if (SubjectRolesId.HasValue)
            {

                UserContext db = new UserContext();
                var subjectRole = db.SubjectRoles.Find(SubjectRolesId);

                ViewData["SubjectRolesId"] = subjectRole;

            }

            return View("NewKviz");
        }

        [HttpPost]
        public ActionResult EditQuiz(int? QuizId)
        {
            if (QuizId.HasValue)
            {

                UserContext db = new UserContext();
                var quiz = db.Quizzes.Find(QuizId);

                ViewData["QuizId"] = quiz;
                return View("EditKviz");
            }

            return View("Index");
        }

        [HttpPost]
        public ActionResult SolveQuiz(int? QuizId)
        {
            if (QuizId.HasValue)
            {

                UserContext db = new UserContext();
                var quiz = db.Quizzes.Find(QuizId);

                ViewData["QuizId"] = quiz;
                return View("SolveKviz");
            }

            return View("Index");
        }

        [HttpPost]
        public ActionResult Update(int QuizId, string QuizName, string QuizData)
        {

            if (ModelState.IsValid)
            {
                Quizzes quiz;

                using (UserContext usr = new UserContext())
                {
                    quiz = usr.Quizzes.Find(QuizId);

                }

                if (quiz != null)
                {
                    quiz.QuizName = QuizName;
                    quiz.QuizData = QuizData;
                }

                using (UserContext db = new UserContext())
                {
                    db.Entry(quiz).State = EntityState.Modified;
                    db.SaveChanges();
                }
            }

            return RedirectToAction("Index", "Kvizi");
        }

        [HttpPost]
        public ActionResult DeleteQuiz(int? QuizId)
        {
            if (QuizId.HasValue)
            {
                using (UserContext db = new UserContext())
                {
                    var quiz = db.Quizzes.Find(QuizId);
                    db.Quizzes.Remove(quiz);
                    db.SaveChanges();
                }

            }

            return View("Index");
        }

    }
}
