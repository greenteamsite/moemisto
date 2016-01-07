using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using Moemisto.Data.Entities;

namespace Moemisto.Data.Contexts
{
    public class PlaceContext: BaseContext
    {
        private readonly DbMmContext _context;

        public PlaceContext(DbMmContext context)
            : base(context)
        {
            _context = context;
        }
        public List<PlaceTypeGroup> GetGroups()
        {
            return _context.PlaceTypes.Select(s => s.PlaceTypeGroup).Distinct().OrderBy(o => o.Order).ToList();
        }
        public List<PlaceType> GetTypes(int groupId)
        {
            return _context.Places.Where(w => w.PlaceType.PlaceTypeGroupId == groupId).Select(s => s.PlaceType).Distinct().OrderBy(o => o.Order).ToList();
        }
        public List<PlaceType> GetTypes(string groupUrl)
        {
            return _context.Places.Where(w => w.PlaceType.PlaceTypeGroup.TranslitUrl == groupUrl).Select(s => s.PlaceType).Distinct().OrderBy(o => o.Order).ToList();
        }
        public List<Place> GetPlaces(int typeId)
        {
            return _context.Places.Where(w => w.PlaceTypeId == typeId).ToList();
        }
        public string GetGroupTitle(int groupId)
        {
            return _context.PlaceTypes.Where(w => w.PlaceTypeGroupId == groupId).Select(s => s.PlaceTypeGroup.Title).Distinct().Single();
        }
        public string GetGroupTitle(string groupUrl)
        {
            return _context.PlaceTypes.Where(w => w.PlaceTypeGroup.TranslitUrl == groupUrl).Select(s => s.PlaceTypeGroup.Title).Distinct().Single();
        }
        public string GetGroupTitleByTypeId(int typeId)
        {
            return _context.PlaceTypes.Where(w => w.PlaceTypeId == typeId).Select(s => s.PlaceTypeGroup.Title).Distinct().Single();
        }
        public string GetTypeTitle(int typeId)
        {
            return _context.PlaceTypes.Where(w => w.PlaceTypeId == typeId).Select(s => s.Title).Single();
        }
        public PlaceType GetType(int typeId)
        {
            return _context.PlaceTypes.Single(w => w.PlaceTypeId == typeId);
        }
        public PlaceType GetType(string typeUrl)
        {
            return _context.PlaceTypes.Single(w => w.TranslitUrl == typeUrl);
        }
        public List<Place> GetOtherPlaces(int placeId, int typeId)
        {
            return _context.Places.Where(w => w.PlaceId != placeId && w.PlaceTypeId == typeId).Take(4).ToList();
        }

        public Place GetPlaceDetails(int placeId)
        {
            return _context.Places.Include(i => i.PlaceType).SingleOrDefault(s => s.PlaceId == placeId);
        }
        public Place GetPlaceDetails(string url)
        {
            return _context.Places.Include(i => i.PlaceType).SingleOrDefault(s => s.TranslitUrl == url);
        }

        public string GetGroupUrlByTypeId(int placeTypeId)
        {
            return _context.PlaceTypes.Where(w => w.PlaceTypeId == placeTypeId).Select(s => s.PlaceTypeGroup.TranslitUrl).Distinct().Single();
        }

        public string GetTypeUrlByTypeId(int placeTypeId)
        {
            return _context.PlaceTypes.Where(w => w.PlaceTypeId == placeTypeId).Select(s => s.TranslitUrl).Distinct().Single();
        }

        public string GetPlaceUrlById(int id)
        {
            return _context.Places.Where(w => w.PlaceId == id).Select(s => s.TranslitUrl).SingleOrDefault();
        }
    }
}
