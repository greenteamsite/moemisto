namespace Moemisto.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class changeLengthTypes : DbMigration
    {
        public override void Up()
        {
            DropIndex("dbo.Categories", new[] { "TranslitUrl" });
            DropIndex("dbo.EventTypes", new[] { "TranslitUrl" });
            AlterColumn("dbo.Categories", "TranslitUrl", c => c.String(maxLength: 50));
            AlterColumn("dbo.Categories", "ParrentUrl", c => c.String(maxLength: 50));
            AlterColumn("dbo.EventTypes", "TranslitUrl", c => c.String(maxLength: 50));
            AlterColumn("dbo.EventTypes", "Title", c => c.String(maxLength: 50));
            AlterColumn("dbo.EventTypes", "TitleOne", c => c.String(maxLength: 50));
            CreateIndex("dbo.Categories", "TranslitUrl", unique: true);
            CreateIndex("dbo.Categories", "ParrentUrl");
            CreateIndex("dbo.EventTypes", "TranslitUrl", unique: true);
        }
        
        public override void Down()
        {
            DropIndex("dbo.EventTypes", new[] { "TranslitUrl" });
            DropIndex("dbo.Categories", new[] { "ParrentUrl" });
            DropIndex("dbo.Categories", new[] { "TranslitUrl" });
            AlterColumn("dbo.EventTypes", "TitleOne", c => c.String(maxLength: 150));
            AlterColumn("dbo.EventTypes", "Title", c => c.String(maxLength: 150));
            AlterColumn("dbo.EventTypes", "TranslitUrl", c => c.String(maxLength: 150));
            AlterColumn("dbo.Categories", "ParrentUrl", c => c.String(maxLength: 150));
            AlterColumn("dbo.Categories", "TranslitUrl", c => c.String(maxLength: 150));
            CreateIndex("dbo.EventTypes", "TranslitUrl", unique: true);
            CreateIndex("dbo.Categories", "TranslitUrl", unique: true);
        }
    }
}
