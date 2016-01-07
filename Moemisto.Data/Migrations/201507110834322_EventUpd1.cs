namespace Moemisto.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class EventUpd1 : DbMigration
    {
        public override void Up()
        {
            DropIndex("dbo.Articles", new[] { "TranslitUrl" });
            DropIndex("dbo.Categories", new[] { "TranslitUrl" });
            DropIndex("dbo.PlaceTypes", new[] { "TranslitUrl" });
            DropIndex("dbo.PlaceTypeGroups", new[] { "TranslitUrl" });
            DropIndex("dbo.EventTypes", new[] { "TranslitUrl" });
            CreateIndex("dbo.Articles", "TranslitUrl", unique: true);
            CreateIndex("dbo.Categories", "TranslitUrl", unique: true);
            CreateIndex("dbo.PlaceTypes", "TranslitUrl", unique: true);
            CreateIndex("dbo.PlaceTypeGroups", "TranslitUrl", unique: true);
            CreateIndex("dbo.EventTypes", "TranslitUrl", unique: true);
        }
        
        public override void Down()
        {
            DropIndex("dbo.EventTypes", new[] { "TranslitUrl" });
            DropIndex("dbo.PlaceTypeGroups", new[] { "TranslitUrl" });
            DropIndex("dbo.PlaceTypes", new[] { "TranslitUrl" });
            DropIndex("dbo.Categories", new[] { "TranslitUrl" });
            DropIndex("dbo.Articles", new[] { "TranslitUrl" });
            CreateIndex("dbo.EventTypes", "TranslitUrl");
            CreateIndex("dbo.PlaceTypeGroups", "TranslitUrl");
            CreateIndex("dbo.PlaceTypes", "TranslitUrl");
            CreateIndex("dbo.Categories", "TranslitUrl");
            CreateIndex("dbo.Articles", "TranslitUrl");
        }
    }
}
