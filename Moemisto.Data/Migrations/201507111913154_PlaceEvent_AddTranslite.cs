namespace Moemisto.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class PlaceEvent_AddTranslite : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Events", "TranslitUrl", c => c.String(maxLength: 150));
            AddColumn("dbo.Places", "TranslitUrl", c => c.String(maxLength: 150));
            CreateIndex("dbo.Events", "TranslitUrl");
            CreateIndex("dbo.Places", "TranslitUrl");
        }
        
        public override void Down()
        {
            DropIndex("dbo.Places", new[] { "TranslitUrl" });
            DropIndex("dbo.Events", new[] { "TranslitUrl" });
            DropColumn("dbo.Places", "TranslitUrl");
            DropColumn("dbo.Events", "TranslitUrl");
        }
    }
}
