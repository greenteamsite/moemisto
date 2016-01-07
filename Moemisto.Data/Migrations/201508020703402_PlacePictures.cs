namespace Moemisto.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class PlacePictures : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Places", "PicturePath", c => c.String(maxLength: 100));
            AddColumn("dbo.Places", "PictureFileName", c => c.String(maxLength: 150));
            AddColumn("dbo.Places", "PictureFileNameSmall", c => c.String(maxLength: 150));
            DropColumn("dbo.Places", "LogoUrl");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Places", "LogoUrl", c => c.String(maxLength: 150));
            DropColumn("dbo.Places", "PictureFileNameSmall");
            DropColumn("dbo.Places", "PictureFileName");
            DropColumn("dbo.Places", "PicturePath");
        }
    }
}
