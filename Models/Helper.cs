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
                foreach (var subject in db.Subjects.ToList())
                {
                    yield return new SelectListItem() {Text = subject.SubjectName, Value = subject.Id.ToString()};
                }
            }
        } 


    }
}
