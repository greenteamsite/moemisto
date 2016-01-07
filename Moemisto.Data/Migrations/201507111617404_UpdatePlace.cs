namespace Moemisto.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdatePlace : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Places", "Metro", c => c.String(maxLength: 100));
            AlterColumn("dbo.Places", "MapsUrl", c => c.String(maxLength: 300));
            AlterColumn("dbo.Places", "Phone", c => c.String(maxLength: 150));
            DropColumn("dbo.Places", "Cost");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Places", "Cost", c => c.String());
            AlterColumn("dbo.Places", "Phone", c => c.String());
            AlterColumn("dbo.Places", "MapsUrl", c => c.String(maxLength: 150));
            AlterColumn("dbo.Places", "Metro", c => c.String());
        }
    }
}
