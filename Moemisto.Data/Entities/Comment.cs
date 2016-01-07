using System;
using System.ComponentModel.DataAnnotations;

namespace Moemisto.Data.Entities
{
    public class Comment
    {
        public int CommentId { get; set; }
        [MaxLength(250)]
        public string Info { get; set; }
        /// <summary>
        /// Id того хто прокоментував
        /// </summary>
        public Guid UserId { get; set; }
        /// <summary>
        /// Рейтинг коментаря
        /// </summary>
        public int Rate { get; set; }
    }
}
