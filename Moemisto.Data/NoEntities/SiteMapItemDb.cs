using System;
using Moemisto.Data.Entities;

namespace Moemisto.Data.NoEntities
{
    public class SiteMapItemDb
    {
        /// <summary>
        /// URL of the page.
        /// </summary>
        public string Url { get; set; }

        /// <summary>
        /// The date of last modification of the file.
        /// </summary>
        public DateTime? LastModified { get; set; }

        /// <summary>
        /// How frequently the page is likely to change.
        /// </summary>
        public SitemapChangeFrequency? ChangeFrequency { get; set; }

        /// <summary>
        /// The priority of this URL relative to other URLs on your site. Valid values range from 0.0 to 1.0.
        /// </summary>
        public double? Priority { get; set; }
    }
}
