namespace Moemisto.UI.Areas.Admin.Models
{
    public class AdminPictureItemVm
    {
        public int PictureId { get; set; }
        public string Title { get; set; }
        public string Url { get; set; }
        /// <summary>
        /// Для відображення першою в новині
        /// </summary>
        public bool Top { get; set; }
    }
}