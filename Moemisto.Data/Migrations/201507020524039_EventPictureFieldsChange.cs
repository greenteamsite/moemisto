namespace Moemisto.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class EventPictureFieldsChange : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Events", "PicturePath", c => c.String(maxLength: 100));
            AddColumn("dbo.Events", "PictureFileName", c => c.String(maxLength: 150));
            AddColumn("dbo.Events", "PictureFileNameSmall", c => c.String(maxLength: 150));
            DropColumn("dbo.Events", "PosterUrl");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Events", "PosterUrl", c => c.String(maxLength: 150));
            DropColumn("dbo.Events", "PictureFileNameSmall");
            DropColumn("dbo.Events", "PictureFileName");
            DropColumn("dbo.Events", "PicturePath");
        }
    }
}
