namespace Cinema.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class MigrateDB : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Baskets", "IdFilms", "dbo.Films");
            DropIndex("dbo.Baskets", new[] { "IdFilms" });
            AddColumn("dbo.Baskets", "IdSession", c => c.Int(nullable: false));
            CreateIndex("dbo.Baskets", "IdSession");
            AddForeignKey("dbo.Baskets", "IdSession", "dbo.Sessions", "IdSession", cascadeDelete: true);
            DropColumn("dbo.Baskets", "IdFilms");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Baskets", "IdFilms", c => c.Int(nullable: false));
            DropForeignKey("dbo.Baskets", "IdSession", "dbo.Sessions");
            DropIndex("dbo.Baskets", new[] { "IdSession" });
            DropColumn("dbo.Baskets", "IdSession");
            CreateIndex("dbo.Baskets", "IdFilms");
            AddForeignKey("dbo.Baskets", "IdFilms", "dbo.Films", "idFilms", cascadeDelete: true);
        }
    }
}
