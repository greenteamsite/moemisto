using System.Collections.Generic;

namespace Moemisto.UI.Models
{
    public class NewsDetailsVm: ArticleBaseVm
    {
        // Сюди додаємо специфічні для сторінки поля, яких немає в Base - класі
        public List<ArticleBaseVm> SimilarNews { get; set; }
    }
}