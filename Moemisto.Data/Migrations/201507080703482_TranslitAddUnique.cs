namespace Moemisto.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class TranslitAddUnique : DbMigration
    {
        public override void Up()
        {
            DropIndex("dbo.Events", new[] { "TranslitUrl" });
        }
        
        public override void Down()
        {
            CreateIndex("dbo.Events", "TranslitUrl", unique: true);
        }
    }
}
