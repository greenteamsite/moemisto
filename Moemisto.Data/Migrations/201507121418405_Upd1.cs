namespace Moemisto.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Upd1 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.PlaceTypes", "Title", c => c.String(maxLength: 150));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.PlaceTypes", "Title", c => c.String(maxLength: 100));
        }
    }
}
