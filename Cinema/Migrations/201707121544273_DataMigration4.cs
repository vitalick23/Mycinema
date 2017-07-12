namespace Cinema.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DataMigration4 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Sessions",
                c => new
                    {
                        IdFilms = c.Int(nullable: false),
                        IdSession = c.Int(nullable: false, identity: true),
                        ReleaseDate = c.DateTime(nullable: false),
                        CountTicket = c.Int(nullable: false),
                        Price = c.Double(nullable: false),
                    })
                .PrimaryKey(t => t.IdSession)
                .ForeignKey("dbo.Films", t => t.IdFilms, cascadeDelete: true)
                .Index(t => t.IdFilms);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Sessions", "IdFilms", "dbo.Films");
            DropIndex("dbo.Sessions", new[] { "IdFilms" });
            DropTable("dbo.Sessions");
        }
    }
}
