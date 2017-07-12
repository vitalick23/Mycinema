namespace Cinema.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DataMigration3 : DbMigration
    {
        public override void Up()
        {
            DropIndex("dbo.Baskets", new[] { "idFilms" });
            CreateIndex("dbo.Baskets", "IdFilms");
            DropColumn("dbo.Films", "releaseData");
            DropColumn("dbo.Films", "coutTicket");
            DropColumn("dbo.Films", "price");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Films", "price", c => c.Double(nullable: false));
            AddColumn("dbo.Films", "coutTicket", c => c.Int(nullable: false));
            AddColumn("dbo.Films", "releaseData", c => c.DateTime(nullable: false));
            DropIndex("dbo.Baskets", new[] { "IdFilms" });
            CreateIndex("dbo.Baskets", "idFilms");
        }
    }
}
