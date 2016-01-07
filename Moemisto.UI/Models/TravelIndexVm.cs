using System.Collections.Generic;

namespace Moemisto.UI.Models
{
    public class TravelIndexVm
    {
        // TODO: Додати поля, які потрібні для Індекс сторінки
        public List<ArticleBaseVm> TopTravels { get; set; }
        public List<ArticleBaseVm> LastTravels { get; set; }
    }
}