namespace Moemisto.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Articles",
                c => new
                    {
                        ArticleId = c.Int(nullable: false, identity: true),
                        Title = c.String(maxLength: 150),
                        ShortInfo = c.String(maxLength: 350),
                        Info = c.String(),
                        Tags = c.String(maxLength: 150),
                        DateCreate = c.DateTime(nullable: false),
                        DatePublish = c.DateTime(nullable: false),
                        Top = c.Boolean(nullable: false),
                        Rate = c.Int(nullable: false),
                        CategoryId = c.Int(nullable: false),
                        AuthorId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ArticleId)
                .ForeignKey("dbo.Categories", t => t.CategoryId, cascadeDelete: true)
                .Index(t => t.CategoryId);
            
            CreateTable(
                "dbo.Categories",
                c => new
                    {
                        CategoryId = c.Int(nullable: false, identity: true),
                        Type = c.Int(nullable: false),
                        Name = c.String(nullable: false, maxLength: 50),
                    })
                .PrimaryKey(t => t.CategoryId);
            
            CreateTable(
                "dbo.Comments",
                c => new
                    {
                        CommentId = c.Int(nullable: false, identity: true),
                        Info = c.String(maxLength: 250),
                        UserId = c.Guid(nullable: false),
                        Rate = c.Int(nullable: false),
                        Article_ArticleId = c.Int(),
                        Event_EventId = c.Int(),
                        Place_PlaceId = c.Int(),
                    })
                .PrimaryKey(t => t.CommentId)
                .ForeignKey("dbo.Articles", t => t.Article_ArticleId)
                .ForeignKey("dbo.Events", t => t.Event_EventId)
                .ForeignKey("dbo.Places", t => t.Place_PlaceId)
                .Index(t => t.Article_ArticleId)
                .Index(t => t.Event_EventId)
                .Index(t => t.Place_PlaceId);
            
            CreateTable(
                "dbo.Pictures",
                c => new
                    {
                        PictureId = c.Int(nullable: false, identity: true),
                        Title = c.String(nullable: false, maxLength: 150),
                        Path = c.String(maxLength: 100),
                        FileName = c.String(maxLength: 150),
                        FileNameHomeTop = c.String(maxLength: 150),
                        FileNameSmall = c.String(maxLength: 150),
                        Top = c.Boolean(nullable: false),
                        ArticleId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.PictureId)
                .ForeignKey("dbo.Articles", t => t.ArticleId, cascadeDelete: true)
                .Index(t => t.ArticleId);
            
            CreateTable(
                "dbo.Events",
                c => new
                    {
                        EventId = c.Int(nullable: false, identity: true),
                        Title = c.String(maxLength: 150),
                        TypeId = c.Int(nullable: false),
                        PosterUrl = c.String(maxLength: 150),
                        ShortInfo = c.String(maxLength: 250),
                        Info = c.String(),
                        Published = c.Boolean(nullable: false),
                        Rate = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.EventId)
                .ForeignKey("dbo.EventTypes", t => t.TypeId, cascadeDelete: true)
                .Index(t => t.TypeId);
            
            CreateTable(
                "dbo.EventSchedules",
                c => new
                    {
                        EventScheduleId = c.Int(nullable: false, identity: true),
                        PlaceId = c.Int(nullable: false),
                        PriceFrom = c.Int(nullable: false),
                        PriceTo = c.Int(nullable: false),
                        EventId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.EventScheduleId)
                .ForeignKey("dbo.Events", t => t.EventId, cascadeDelete: true)
                .ForeignKey("dbo.Places", t => t.PlaceId, cascadeDelete: true)
                .Index(t => t.PlaceId)
                .Index(t => t.EventId);
            
            CreateTable(
                "dbo.Places",
                c => new
                    {
                        PlaceId = c.Int(nullable: false, identity: true),
                        PlaceTypeId = c.Int(nullable: false),
                        Title = c.String(maxLength: 150),
                        Address = c.String(maxLength: 150),
                        Metro = c.String(),
                        Cost = c.String(),
                        MapsUrl = c.String(maxLength: 150),
                        SiteUrl = c.String(maxLength: 100),
                        LogoUrl = c.String(maxLength: 150),
                        Phone = c.String(),
                        Info = c.String(),
                        Rate = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.PlaceId)
                .ForeignKey("dbo.PlaceTypes", t => t.PlaceTypeId, cascadeDelete: true)
                .Index(t => t.PlaceTypeId);
            
            CreateTable(
                "dbo.PlaceTypes",
                c => new
                    {
                        PlaceTypeId = c.Int(nullable: false, identity: true),
                        Title = c.String(maxLength: 100),
                        ImageUrl = c.String(maxLength: 150),
                        PlaceTypeGroupId = c.Int(nullable: false),
                        TitleOne = c.String(),
                    })
                .PrimaryKey(t => t.PlaceTypeId)
                .ForeignKey("dbo.PlaceTypeGroups", t => t.PlaceTypeGroupId, cascadeDelete: true)
                .Index(t => t.PlaceTypeGroupId);
            
            CreateTable(
                "dbo.PlaceTypeGroups",
                c => new
                    {
                        PlaceTypeGroupId = c.Int(nullable: false, identity: true),
                        Title = c.String(maxLength: 100),
                        ImageUrl = c.String(maxLength: 150),
                    })
                .PrimaryKey(t => t.PlaceTypeGroupId);
            
            CreateTable(
                "dbo.EventScheduleDateTimes",
                c => new
                    {
                        EventScheduleDateTimeId = c.Int(nullable: false, identity: true),
                        EventScheduleId = c.Int(nullable: false),
                        StartEvent = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.EventScheduleDateTimeId)
                .ForeignKey("dbo.EventSchedules", t => t.EventScheduleId, cascadeDelete: true)
                .Index(t => t.EventScheduleId);
            
            CreateTable(
                "dbo.EventTypes",
                c => new
                    {
                        EventTypeId = c.Int(nullable: false, identity: true),
                        Title = c.String(maxLength: 150),
                    })
                .PrimaryKey(t => t.EventTypeId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Events", "TypeId", "dbo.EventTypes");
            DropForeignKey("dbo.EventScheduleDateTimes", "EventScheduleId", "dbo.EventSchedules");
            DropForeignKey("dbo.EventSchedules", "PlaceId", "dbo.Places");
            DropForeignKey("dbo.Places", "PlaceTypeId", "dbo.PlaceTypes");
            DropForeignKey("dbo.PlaceTypes", "PlaceTypeGroupId", "dbo.PlaceTypeGroups");
            DropForeignKey("dbo.Comments", "Place_PlaceId", "dbo.Places");
            DropForeignKey("dbo.EventSchedules", "EventId", "dbo.Events");
            DropForeignKey("dbo.Comments", "Event_EventId", "dbo.Events");
            DropForeignKey("dbo.Pictures", "ArticleId", "dbo.Articles");
            DropForeignKey("dbo.Comments", "Article_ArticleId", "dbo.Articles");
            DropForeignKey("dbo.Articles", "CategoryId", "dbo.Categories");
            DropIndex("dbo.EventScheduleDateTimes", new[] { "EventScheduleId" });
            DropIndex("dbo.PlaceTypes", new[] { "PlaceTypeGroupId" });
            DropIndex("dbo.Places", new[] { "PlaceTypeId" });
            DropIndex("dbo.EventSchedules", new[] { "EventId" });
            DropIndex("dbo.EventSchedules", new[] { "PlaceId" });
            DropIndex("dbo.Events", new[] { "TypeId" });
            DropIndex("dbo.Pictures", new[] { "ArticleId" });
            DropIndex("dbo.Comments", new[] { "Place_PlaceId" });
            DropIndex("dbo.Comments", new[] { "Event_EventId" });
            DropIndex("dbo.Comments", new[] { "Article_ArticleId" });
            DropIndex("dbo.Articles", new[] { "CategoryId" });
            DropTable("dbo.EventTypes");
            DropTable("dbo.EventScheduleDateTimes");
            DropTable("dbo.PlaceTypeGroups");
            DropTable("dbo.PlaceTypes");
            DropTable("dbo.Places");
            DropTable("dbo.EventSchedules");
            DropTable("dbo.Events");
            DropTable("dbo.Pictures");
            DropTable("dbo.Comments");
            DropTable("dbo.Categories");
            DropTable("dbo.Articles");
        }
    }
}
