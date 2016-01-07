using System;

namespace Moemisto.UI.Models
{
    public class PaginationVm
    {
        private int _countMaxVisiblePages;
        public Guid Id { get; set; }
        /// <summary>
        /// Id reloaded div 
        /// </summary>
        public string IdBox { get; set; }
        public int CountPages { get; set; }
        /// <summary>
        /// Скільки номерів сторінок можна виводити в блоці максимально (обов"язково непарне число)
        /// </summary>
        public int CountMaxVisiblePages {
            set
            {
                _countMaxVisiblePages = value % 2 !=  0 ? value : value -1;
            }
        }
        /// <summary>
        /// Скільки номерів сторінок виводити в блоці (для View)
        /// </summary>
        public int CountVisiblePages {
            get { return CountPages <= _countMaxVisiblePages ? CountPages : _countMaxVisiblePages; }
        }
        /// <summary>
        /// For getting page
        /// </summary>
        public string Url { get; set; }
    }
}