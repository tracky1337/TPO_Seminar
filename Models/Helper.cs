using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace TPO_Seminar.Models
{
    public static class Helper
    {
        public static IEnumerable<SelectListItem> GetSubjects()
        {
            using (CustomModels db = new CustomModels())
            {
                if (!db.Subjects.Any())
                {
                    db.Subjects.Add(new Subjects() { SubjectName = "Matematika" });
                    db.Subjects.Add(new Subjects() { SubjectName = "Fizika" });
                    db.Subjects.Add(new Subjects() { SubjectName = "Angleščina" });
                    db.Subjects.Add(new Subjects() { SubjectName = "Programiranje - Java" });
                    db.Subjects.Add(new Subjects() { SubjectName = "Tehnologija programske opreme" });
                    db.Subjects.Add(new Subjects() { SubjectName = "Športna vzgoja" });
                    db.SaveChanges();
                }
                foreach (var subject in db.Subjects.ToList())
                {
                    yield return new SelectListItem() {Text = subject.SubjectName, Value = subject.Id.ToString()};
                }
            }
        } 


    }
}
