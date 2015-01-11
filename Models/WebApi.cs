using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TPO_Seminar.Models
{
    class WebApi
    {
    }

    public class ProfesorItem
    {
        public string CompanyName { get; set; }
        public int InstructionsCount { get; set; }
        public string AvgRating { get; set; }
    }

    public class ProfesorResponse
    {
        public IEnumerable<ProfesorItem> Profesorji { get; set; } 
    }
    public class DogodekRequest
    {
        //1 so prihajajoci
        //2 so pretekli
        public int TipDogodka { get; set; }
        public int StudentId { get; set; }
    }

    public class Dogodek
    {
        public string SubjectName { get; set; }
        public DateTime OrderDate { get; set; }
    }

    public class DogodekResponse
    {
        public IEnumerable<Dogodek> Dogodki;
    }

    public class Register
    {
        public string UserName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string School { get; set; }
        public int BirthYear { get; set; }
    }

    public class Login
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }

    public class LoginResponse
    {
        public int Success { get; set; }
        public int Id { get; set; }
        public int StudentId { get; set; }
        public string ErrorMessage { get; set; }
    }
}
