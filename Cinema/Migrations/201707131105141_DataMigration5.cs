namespace Cinema.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DataMigration5 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Films", "Name", c => c.String());
            AlterColumn("dbo.Films", "Image", c => c.Binary());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Films", "Image", c => c.Binary(nullable: false));
            AlterColumn("dbo.Films", "Name", c => c.String(nullable: false));
        }
    }
}
