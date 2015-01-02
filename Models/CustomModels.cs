using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace TPO_Seminar.Models
{
    public class CustomModels : DbContext
    {
        //
        // GET: /CustomModels/

        public CustomModels()
            : base("DefaultConnection")
        {
        }
        public DbSet<Subjects> Subjects { get; set; }
        public DbSet<Services> Services { get; set; }
        public DbSet<Blobs> Blobs { get; set; }

    }

    public class ServiceCreation
    {
        public int SubjectId { get;set; }
        public Services Services { get; set; }
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
}
