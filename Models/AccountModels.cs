using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Globalization;
using System.Web.Security;

namespace TPO_Seminar.Models
{
    public class UsersContext : DbContext
    {
        public UsersContext()
            : base("DefaultConnection")
        {
        }

        public DbSet<UserProfile> UserProfiles { get; set; }
        public DbSet<UserDatas> UserDatas { get; set; }
        public DbSet<Instruktors> Instruktors { get; set; }
        public DbSet<Students> Students { get; set; }
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
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Novo geslo")]
        public string NewPassword { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm new password")]
        [Compare("NewPassword", ErrorMessage = "The new password and confirmation password do not match.")]
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
}
