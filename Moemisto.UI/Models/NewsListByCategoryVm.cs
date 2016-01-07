namespace Moemisto.UI.Models
{
    public class NewsListByCategoryVm: NewsIndexVm
    {
        public string CategoryUrl { get; set; }

        /// <summary>
        /// Category Title
        /// </summary>
        public string CategoryTitle { get; set; }
    }
}