using System.Data.Entity;
using Moemisto.Data.Entities;

namespace Moemisto.Data
{
    public class DbMmContext: DbContext
    {
        public DbMmContext() : base("name=DbContext")
        {}

        public DbSet<Article> Articles { get; set; }
        public DbSet<ArticleSource> ArticleSources { get; set; }
        public DbSet<Picture> Pictures { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Event> Events { get; set; }
        public DbSet<EventSchedule> EventSchedules { get; set; }
        public DbSet<EventScheduleDateTime> EventScheduleDateTimes { get; set; }
        public DbSet<Place> Places { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<EventType> EventTypes { get; set; }
        public DbSet<PlaceType> PlaceTypes { get; set; }
        public DbSet<PlaceTypeGroup> PlaceTypeGroups { get; set; }
    }
}
