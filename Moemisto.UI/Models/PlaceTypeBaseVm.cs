namespace Moemisto.UI.Models
{
    public class PlaceTypeBaseVm
    {
        public int Id { get; set; }
        public string GroupUrl { get; set; }
        public string TranslitUrl { get; set; }
        public string Title { get; set; }
        public string ImageUrl { get; set; }

        public string ActionName { get; set; }
        public string TypeTitle { get; set; }
    }
}