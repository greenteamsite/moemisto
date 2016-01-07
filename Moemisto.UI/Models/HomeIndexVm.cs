using System.Collections.Generic;

namespace Moemisto.UI.Models
{
    public class HomeIndexVm
    {
        public List<ArticleBaseVm> TopNews { get; set; }
        public List<ArticleBaseVm> LastNews { get; set; }
        public List<EventBaseVm> InterestedEvents { get; set; }
        public List<ArticleBaseVm> LastTravels { get; set; }
    }
}