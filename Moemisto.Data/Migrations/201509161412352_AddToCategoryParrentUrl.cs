namespace Moemisto.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddToCategoryParrentUrl : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Categories", "ParrentUrl", c => c.String(maxLength: 150));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Categories", "ParrentUrl");
        }
    }
}
