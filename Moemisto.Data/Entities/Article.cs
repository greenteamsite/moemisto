using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Moemisto.Data.Entities
{
    public class Article
    {
        public int ArticleId { get; set; }
        [Index(IsUnique = true)]
        [MaxLength(150)]
        public string TranslitUrl { get; set; }
        [Index]
        [MaxLength(150)]
        public string Title { get; set; }
        [MaxLength(75)]
        public string TitleShort { get; set; }
        [MaxLength(350)]
        public string ShortInfo { get; set; }
        public string Info { get; set; }

        public ICollection<Picture> Pictures { get; set; }

        [MaxLength(150)]
        public string Tags { get; set; }

        public DateTime DateCreate { get; set; }
        public DateTime DatePublish { get; set; }
        /// <summary>
        /// Для відображення на першій сторінці у верхньому великому блоці
        /// </summary>
        public bool Top { get; set; }
        /// <summary>
        /// Рейтинг
        /// </summary>
        public int Rate { get; set; }

        public int CategoryId { get; set; }
        [ForeignKey("CategoryId")] 
        public virtual Category Category { get; set; }
        /// <summary>
        /// Id автора публікації
        /// </summary>
        public int AuthorId { get; set; }
        public int? ArticleSourceId { get; set; }
        [ForeignKey("ArticleSourceId")]
        public virtual ArticleSource Source { get; set; }
        /// <summary>
        /// Чи матеріал зі старого сайту
        /// </summary>
        public bool Old { get; set; }
        /// <summary>
        /// Коментары до статті
        /// </summary>
        public ICollection<Comment> Comments { get; set; }

    }
}
