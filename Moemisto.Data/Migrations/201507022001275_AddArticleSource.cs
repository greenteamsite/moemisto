namespace Moemisto.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddArticleSource : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ArticleSources",
                c => new
                    {
                        ArticleSourceId = c.Int(nullable: false, identity: true),
                        Name = c.String(maxLength: 150),
                        Url = c.String(maxLength: 150),
                    })
                .PrimaryKey(t => t.ArticleSourceId);
            
            AddColumn("dbo.Articles", "ArticleSourceId", c => c.Int());
            CreateIndex("dbo.Articles", "ArticleSourceId");
            AddForeignKey("dbo.Articles", "ArticleSourceId", "dbo.ArticleSources", "ArticleSourceId");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Articles", "ArticleSourceId", "dbo.ArticleSources");
            DropIndex("dbo.Articles", new[] { "ArticleSourceId" });
            DropColumn("dbo.Articles", "ArticleSourceId");
            DropTable("dbo.ArticleSources");
        }
    }
}
