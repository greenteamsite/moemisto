namespace Moemisto.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddPlace_PlaceOldTelId : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Places", "PlaceOldTelId", c => c.Int(nullable: false));
            CreateIndex("dbo.Places", "PlaceOldTelId");
        }
        
        public override void Down()
        {
            DropIndex("dbo.Places", new[] { "PlaceOldTelId" });
            DropColumn("dbo.Places", "PlaceOldTelId");
        }
    }
}
