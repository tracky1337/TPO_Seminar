using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using System.Web.Security;
using Newtonsoft.Json;
using TPO_Seminar.Models;
using WebMatrix.WebData;

namespace TPO_Seminar.Controllers
{

    public class ProfesorController : ApiController
    {
        public string Get()
        {
            try
            {
                using (var model = new UserContext())
                {
                    var profesorji =
                        model.Database.SqlQuery<Profesor>("select * from vw_PregledProfesorjev")
                            .Select(
                                el =>
                                    new ProfesorItem()
                                    {
                                        CompanyName = el.Podjetje,
                                        AvgRating = el.AvgOcena,
                                        InstructionsCount = el.CountOfOrders
                                    })
                            .ToList();
                    return JsonConvert.SerializeObject(new ProfesorResponse(){Profesorji=profesorji});
                }
            }
            catch
            {
                return JsonConvert.SerializeObject(new ProfesorResponse() {Profesorji = new List<ProfesorItem>()});
            }

        }
    }
    public class DogodekController : ApiController
    {
        public string Post([FromBody] string value)
        {
            try
            {
                var obj = JsonConvert.DeserializeObject<DogodekRequest>(value);
                using (var model = new UserContext())
                {
                    var studentId = obj.StudentId;
                    var currDate = DateTime.Now.AddMinutes(-10);
                    if (obj.TipDogodka == 1)
                    {
                        var events =
                            model.Orders.Where(
                                el => el.Approved && el.StudentId == studentId && el.OrderDate <= currDate)
                                .Select(
                                    el =>
                                        new Dogodek() {OrderDate = el.OrderDate, SubjectName = el.Subjects.SubjectName})
                                .ToList();
                        return JsonConvert.SerializeObject(new DogodekResponse() {Dogodki = events});
                    }
                    else
                    {
                        var events =
                            model.Orders.Where(el => el.Approved && el.StudentId == studentId && el.OrderDate > currDate)
                                .Select(
                                    el =>
                                        new Dogodek() {OrderDate = el.OrderDate, SubjectName = el.Subjects.SubjectName})
                                .ToList();
                        return JsonConvert.SerializeObject(new DogodekResponse() {Dogodki = events});
                    }
                }
            }
            catch
            {
                return JsonConvert.SerializeObject(new DogodekResponse() {Dogodki = new List<Dogodek>()});
            }
        }
    }

    public class RegisterController : ApiController
    {

        public string Post([FromBody] string value)
        {
            var errorMessage = "";
            try
            {
                var obj = JsonConvert.DeserializeObject<Register>(value);
                using (var model = new UserContext())
                {
                    var userExists = model.UserProfiles.Any(el => el.UserName == obj.UserName);
                    if (!userExists)
                    {
                        WebSecurity.CreateUserAndAccount(obj.UserName, obj.Password);
                        var user = model.UserProfiles.FirstOrDefault(el => el.UserName == obj.UserName);

                        var userDatas = new UserDatas()
                        {
                            Email = obj.Email,
                            LastName = obj.LastName,
                            Name = obj.FirstName,
                            UserName = obj.UserName,
                            UserProfileId = user.UserId,
                            Password = obj.Password
                        };
                        model.UserDatas.Add(userDatas);
                        model.SaveChanges();

                        Roles.AddUserToRole(userDatas.UserName, "Ucenec");

                        var student = new Students()
                        {
                            LetoRojstva = obj.BirthYear,
                            Sola = obj.School,
                            UserProfileId = user.UserId
                        };
                        model.Students.Add(student);
                        model.SaveChanges();


                        var loginResponse = new LoginResponse()
                        {
                            Id = user.UserId,
                            Success = 1,
                            StudentId = student.Id
                        };


                        return JsonConvert.SerializeObject(loginResponse);
                    }
                    errorMessage = "Uporabnik že obstaja";
                }
            }
            catch (Exception er)
            {

                errorMessage = "Napaka pri registraciji";
            }
            return JsonConvert.SerializeObject(new LoginResponse() { Id = -1, StudentId = -1, Success = 0, ErrorMessage = errorMessage });
        }
    }
    public class LoginController : ApiController
    {
        public string Post([FromBody]string value)
        {
            var errorMessage = "";
            try
            {
                var obj = JsonConvert.DeserializeObject<Login>(value);
                var login = WebSecurity.Login(obj.Username, obj.Password);
                if (login)
                {
                    using (var model = new UserContext())
                    {
                        var user = model.UserProfiles.FirstOrDefault(el => el.UserName == ((Login)obj).Username);
                        var student = model.Students.FirstOrDefault(el => el.UserProfileId == user.UserId);
                        if (student != null)
                        {
                            var loginResponse = new LoginResponse()
                            {
                                Id = user.UserId,
                                Success = 1,
                                StudentId = student.Id
                            };

                            return JsonConvert.SerializeObject(loginResponse);

                        }
                        errorMessage = "Aplikacija je samo za učence";
                    }
                }
                else
                {
                    errorMessage = "Napačno geslo / up. ime";
                }
            }
            catch
            {
                errorMessage = "Napaka pri povezavi s strežnikom";
            }
            return JsonConvert.SerializeObject(new LoginResponse() { Id = -1, StudentId = -1, Success = 0, ErrorMessage = errorMessage });
        }

    }
    public class WebApiController : ApiController
    {
        #region inactive
        // GET api/<controller>
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/<controller>/5
        public string Get(int id)
        {
            return "value";
        }




        // PUT api/<controller>/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/<controller>/5
        public void Delete(int id)
        {
        }
        #endregion

    }
}