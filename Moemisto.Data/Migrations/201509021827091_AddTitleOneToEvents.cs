namespace Moemisto.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddTitleOneToEvents : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.EventTypes", "TitleOne", c => c.String(maxLength: 150));
        }
        
        public override void Down()
        {
            DropColumn("dbo.EventTypes", "TitleOne");
        }
    }
}
