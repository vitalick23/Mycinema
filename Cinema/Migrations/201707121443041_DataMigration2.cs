namespace Cinema.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DataMigration2 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Films", "Image", c => c.Binary());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Films", "Image", c => c.Byte(nullable: false));
        }
    }
}
