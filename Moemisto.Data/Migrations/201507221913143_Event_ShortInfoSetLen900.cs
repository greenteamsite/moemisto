namespace Moemisto.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Event_ShortInfoSetLen900 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Events", "ShortInfo", c => c.String(maxLength: 900));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Events", "ShortInfo", c => c.String(maxLength: 250));
        }
    }
}
