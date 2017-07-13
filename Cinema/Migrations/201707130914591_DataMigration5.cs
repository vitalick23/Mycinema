namespace Cinema.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DataMigration5 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Films", "Name", c => c.String(nullable: false));
            AlterColumn("dbo.Films", "Image", c => c.Binary(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Films", "Image", c => c.Binary());
            AlterColumn("dbo.Films", "Name", c => c.String());
        }
    }
}
