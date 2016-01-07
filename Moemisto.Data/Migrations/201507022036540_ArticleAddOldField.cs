namespace Moemisto.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ArticleAddOldField : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Articles", "Old", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Articles", "Old");
        }
    }
}
