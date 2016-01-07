namespace Moemisto.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddTranslitUrl : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Articles", "TranslitUrl", c => c.String(maxLength: 150));
            AddColumn("dbo.Categories", "TranslitUrl", c => c.String(maxLength: 150));
            AddColumn("dbo.PlaceTypes", "TranslitUrl", c => c.String(maxLength: 150));
            AddColumn("dbo.PlaceTypeGroups", "TranslitUrl", c => c.String(maxLength: 150));
            AddColumn("dbo.EventTypes", "TranslitUrl", c => c.String(maxLength: 150));
            CreateIndex("dbo.Articles", "TranslitUrl");
            CreateIndex("dbo.Articles", "Title");
            CreateIndex("dbo.Categories", "TranslitUrl");
            CreateIndex("dbo.Events", "Title");
            CreateIndex("dbo.PlaceTypes", "TranslitUrl");
            CreateIndex("dbo.PlaceTypeGroups", "TranslitUrl");
            CreateIndex("dbo.EventTypes", "TranslitUrl");
        }
        
        public override void Down()
        {
            DropIndex("dbo.EventTypes", new[] { "TranslitUrl" });
            DropIndex("dbo.PlaceTypeGroups", new[] { "TranslitUrl" });
            DropIndex("dbo.PlaceTypes", new[] { "TranslitUrl" });
            DropIndex("dbo.Events", new[] { "Title" });
            DropIndex("dbo.Categories", new[] { "TranslitUrl" });
            DropIndex("dbo.Articles", new[] { "Title" });
            DropIndex("dbo.Articles", new[] { "TranslitUrl" });
            DropColumn("dbo.EventTypes", "TranslitUrl");
            DropColumn("dbo.PlaceTypeGroups", "TranslitUrl");
            DropColumn("dbo.PlaceTypes", "TranslitUrl");
            DropColumn("dbo.Categories", "TranslitUrl");
            DropColumn("dbo.Articles", "TranslitUrl");
        }
    }
}
