using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;

namespace TPO_Seminar.Models
{
    public class UserContext : DbContext
    {
        public UserContext()
            : base("DefaultConnection")
        {
        }

        public DbSet<UserProfile> UserProfiles { get; set; }
        public DbSet<UserDatas> UserDatas { get; set; }
        public DbSet<Instruktors> Instruktors { get; set; }
        public DbSet<Students> Students { get; set; }
        public DbSet<Subjects> Subjects { get; set; }
        public DbSet<Services> Services { get; set; }
        public DbSet<Blobs> Blobs { get; set; }
        public DbSet<Schedules> Schedules { get; set; }
        public DbSet<SubjectRoles> SubjectRoles { get; set; }
        public DbSet<Orders> Orders { get; set; }
        public DbSet<Ratings> Ratings { get; set; }


    }


    [Table("UserProfile")]
    public class UserProfile
    {
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int UserId { get; set; }
        public string UserName { get; set; }

    }

    public class RegisterExternalLoginModel
    {
        [Required]
        [Display(Name = "User name")]
        public string UserName { get; set; }

        public string ExternalLoginData { get; set; }
    }

    public class LocalPasswordModel
    {
        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Trenutno geslo")]
        public string OldPassword { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Novo geslo")]
        public string NewPassword { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Potrdi novo geslo")]
        [Compare("NewPassword", ErrorMessage = "Novo in staro geslo se ne ujemata.")]
        public string ConfirmPassword { get; set; }
    }

    public class LoginModel
    {
        [Required]
        [Display(Name = "Uporabniško ime")]
        public string UserName { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Geslo")]
        public string Password { get; set; }

        [Display(Name = "Zapomni me?")]
        public bool RememberMe { get; set; }
    }

    public class UserDatas
    {
        [Required]
        [Display(Name = "User name")]
        public string UserName { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        public string Email { get; set; }

        public string Name { get; set; }
        public string LastName { get; set; }

        [ForeignKey("UserProfile")]
        public int UserProfileId { get; set; }
        public virtual UserProfile UserProfile { get; set; }

        [Key]
        public int Id { get; set; }
    }

    public class Instruktors
    {
        [ForeignKey("UserProfile")]
        public int UserProfileId { get; set; }
        public virtual UserProfile UserProfile { get; set; }

        [Key]
        public int Id { get; set; }


        public string Podjetje { get; set; }
        public string DavcnaStevilka { get; set; }
    }

    public class Students
    {
        [ForeignKey("UserProfile")]
        public int UserProfileId { get; set; }
        public virtual UserProfile UserProfile { get; set; }

        [Key]
        public int Id { get; set; }

        public string Sola { get; set; }
        public int LetoRojstva { get; set; }
    }

    public class UserRegistration
    {
        public UserDatas UserDatas { get; set; }
        public Instruktors Instruktors { get; set; }
        public Students Students { get; set; }
    }

    public class ExternalLogin
    {
        public string Provider { get; set; }
        public string ProviderDisplayName { get; set; }
        public string ProviderUserId { get; set; }
    }

    public class ServiceCreation
    {
        public int SubjectId { get; set; }
        public Services Services { get; set; }
        public decimal PricePerHour { get; set; }
    }

    [Table("Schedules")]
    public class Schedules
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey("Instruktors")]
        public int InstruktorId { get; set; }
        public virtual Instruktors Instruktors { get; set; }

        public DateTime ScheduleDate { get; set; }
        public bool Available { get; set; }
    }

    [Table("Subjects")]
    public class Subjects
    {
        [Key]
        public int Id { get; set; }
        public string SubjectName { get; set; }

    }

    [Table("Blobs")]
    public class Blobs
    {
        [Key]
        public int Id { get; set; }
        public string Blob { get; set; }
    }

    [Table("Services")]
    public class Services
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey("Instruktors")]
        public int InstructorId { get; set; }
        public virtual Instruktors Instruktors { get; set; }

        [ForeignKey("Subjects")]
        public int SubjectId { get; set; }
        public virtual Subjects Subjects { get; set; }

        public bool Active { get; set; }
        public DateTime CreationDate { get; set; }
    }

    [Table("SubjectRoles")]
    public class SubjectRoles
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey("Instruktors")]
        public int InstructorId { get; set; }
        public virtual Instruktors Instruktors { get; set; }

        [ForeignKey("Subjects")]
        public int SubjectId { get; set; }
        public virtual Subjects Subjects { get; set; }

        public decimal PricePerHour { get; set; }
    }

    [Table("Orders")]
    public class Orders
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey("Instruktors")]
        public int InstructorId { get; set; }
        public virtual Instruktors Instruktors { get; set; }

        [ForeignKey("Subjects")]
        public int SubjectId { get; set; }
        public virtual Subjects Subjects { get; set; }

        [ForeignKey("Students")]
        public int StudentId { get; set; }
        public virtual Students Students { get; set; }

        public DateTime OrderDate { get; set; }
        public bool Approved { get; set; }
    }

    [Table("Ratings")]
    public class Ratings
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey("Instruktors")]
        public int InstructorId { get; set; }
        public virtual Instruktors Instruktors { get; set; }

        [ForeignKey("Students")]
        public int StudentId { get; set; }
        public virtual Students Students { get; set; }

        [ForeignKey("Orders")]
        public int OrderId { get; set; }
        public virtual Orders Orders { get; set; }

        public int Rating { get; set; }
    }

    public class Profesor
    {
        public Int64 IDrec { get; set; }
        public int UserProfileID { get; set; }
        public string Podjetje { get; set; }
        public string DavcnaStevilka { get; set; }
        public string Name { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public int CountOfOrders { get; set; }
        public string AvgOcena { get; set; }
        public string Subjects { get; set; }
    }

    public class UnpaidOrders
    {
        public int Id { get; set; }
        public int StudentId { get; set; }
        public string Name { get; set; }
        public string LastName { get; set; }
        public string Podjetje { get; set; }
        public string DavcnaStevilka { get; set; }
        public string SubjectName { get; set; }
        public DateTime OrderDate { get; set; }
        public decimal PricePerHour { get; set; }
    }

    #region JsonModels

    public class ResultDay
    {
        public string hour { get; set; }
    }
    public class ResultMonth
    {
        public string month { get; set; }
    }

    public class ResultSubjectProvider
    {
        public string Instructor { get; set; }
        public int InstructorId { get; set; }
        public decimal PricePerHour { get; set; }
    }
    public class JsonEventDay
    {
        public int success { get; set; }
        public List<ResultDay> result { get; set; }
    }
    public class JsonEventMonth
    {
        public int success { get; set; }
        public List<ResultMonth> result { get; set; }
    }

    public class JsonEventSubjectProviders
    {
        public int success { get; set; }
        public List<ResultSubjectProvider> result { get; set; }
    }

    #endregion
}
