namespace TPO_Seminar.Migrations
{
    using System;
    using System.Data.Entity.Migrations;

    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.UserProfile",
                c => new
                    {
                        UserId = c.Int(nullable: false, identity: true),
                        UserName = c.String(),
                    })
                .PrimaryKey(t => t.UserId);

            CreateTable(
                "dbo.UserDatas",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserName = c.String(nullable: false),
                        Password = c.String(nullable: false, maxLength: 100),
                        Email = c.String(),
                        Name = c.String(),
                        LastName = c.String(),
                    })
                .PrimaryKey(t => t.Id);

            CreateTable(
                "dbo.Instruktors",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Podjetje = c.String(),
                        DavcnaStevilka = c.String(),
                    })
                .PrimaryKey(t => t.Id);

            CreateTable(
    "dbo.Students",
    c => new
    {
        Id = c.Int(nullable: false, identity: true),
        Sola = c.String(),
        LetoRojstva = c.Int(nullable: false)
    })
    .PrimaryKey(t => t.Id);


            CreateTable(
    "dbo.Subjects",
    c => new
    {
        Id = c.Int(nullable: false, identity: true),
        SubjectName = c.String()
    })
    .PrimaryKey(t => t.Id);

            CreateTable(
                "dbo.Services",
                c => new
                {
                    Id = c.Int(nullable: false, identity: true),
                    Active = c.Boolean(),
                    CreationDate = c.DateTime(),
                    InstructorId = c.Int(nullable: false),
                    SubjectId = c.Int(nullable: false)
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("Subjects", t => t.SubjectId)
                .ForeignKey("Instruktors", t => t.InstructorId);




        }

        public override void Down()
        {
            /*DropTable("dbo.Instruktors");
            DropTable("dbo.UserDatas");
            DropTable("dbo.UserProfile");*/
        }
    }
}
