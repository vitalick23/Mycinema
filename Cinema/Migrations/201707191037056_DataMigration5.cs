namespace Cinema.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DataMigration5 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AspNetUsers", "IpHistory", c => c.String());
            AlterColumn("dbo.Baskets", "IdUsers", c => c.String());
            DropColumn("dbo.Baskets", "Status");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Baskets", "Status", c => c.Int(nullable: false));
            AlterColumn("dbo.Baskets", "IdUsers", c => c.Int(nullable: false));
            DropColumn("dbo.AspNetUsers", "IpHistory");
        }
    }
}
