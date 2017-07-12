namespace Cinema.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DataMigration1 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Baskets",
                c => new
                    {
                        idFilms = c.Int(nullable: false),
                        ID = c.Int(nullable: false, identity: true),
                        idUsers = c.Int(nullable: false),
                        CoutTicket = c.Int(nullable: false),
                        DateBuy = c.DateTime(nullable: false),
                        status = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Films", t => t.idFilms, cascadeDelete: true)
                .Index(t => t.idFilms);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Baskets", "idFilms", "dbo.Films");
            DropIndex("dbo.Baskets", new[] { "idFilms" });
            DropTable("dbo.Baskets");
        }
    }
}
