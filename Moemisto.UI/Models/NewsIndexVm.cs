using System.Collections.Generic;

namespace Moemisto.UI.Models
{
    public class NewsIndexVm
    {
        public List<ArticleBaseVm> TopNews { get; set; }

        /// <summary>
        /// For pagination News
        /// </summary>
        public PaginationVm Pagination { get; set; }

        /// <summary>
        /// For pagination Articles
        /// </summary>
        public PaginationVm PaginationArticle { get; set; }
     }
}