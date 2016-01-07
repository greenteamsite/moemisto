using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Linq;
using Moemisto.Data.Contexts;
using Terradue.ServiceModel.Syndication;

namespace Moemisto.UI.Services.Feed
{
    /// <summary>
    /// Builds <see cref="SyndicationFeed"/>'s containing meta data about the feed and the feed entries.
    /// Note: We are targeting Atom 1.0 over RSS 2.0 because Atom 1.0 is a newer and more well defined format. Atom 1.0
    /// is a standard and RSS is not. See http://rehansaeed.com/building-rssatom-feeds-for-asp-net-mvc/.
    /// </summary>
    public sealed class FeedService : IFeedService
    {
        /// <summary>
        /// The feed universally unique identifier. Do not use the URL of your feed as this can change.
        /// A much better ID is to use a GUID which you can generate from Tools->Create GUID in Visual Studio.
        /// </summary>
        private const string SiteUrl = "http://moemisto.com.ua";
        private const string FeedUrl = "http://moemisto.com.ua/feed";
        private const string FeedId = "eb66cff9-1701-422b-a44a-f8deff2a47c8";
        private const string PubSubHubbubHubUrl = "https://pubsubhubbub.appspot.com/";

        private readonly HttpClient _httpClient;
        private readonly FeedContext _feedContext;
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="FeedService"/> class.
        /// </summary>
        public FeedService(FeedContext feedContext)
        {
            _feedContext = feedContext;
            _httpClient = new HttpClient();
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Gets the feed containing meta data about the feed and the feed entries.
        /// </summary>
        /// <param name="cancellationToken">A <see cref="CancellationToken"/> signifying if the request is cancelled.</param>
        /// <returns>A <see cref="SyndicationFeed"/>.</returns>
        public async Task<SyndicationFeed> GetFeed(CancellationToken cancellationToken)
        {
            SyndicationFeed feed = new SyndicationFeed()
            {
                // id (Required) - The feed universally unique identifier.
                Id = FeedId,
                // title (Required) - Contains a human readable title for the feed. Often the same as the title of the 
                //                    associated website. This value should not be blank.
                Title = SyndicationContent.CreatePlaintextContent("Моє місто - про Київ цікаво"),
                // items (Required) - The items to add to the feed.
                Items = await GetItems(cancellationToken),
                // subtitle (Recommended) - Contains a human-readable description or subtitle for the feed.
                Description = SyndicationContent.CreatePlaintextContent("Київські новини та аналітика, повна афіша Києва, сервіси для киян"),
                // updated (Optional) - Indicates the last time the feed was modified in a significant way.
                LastUpdatedTime = DateTimeOffset.Now,
                Language = "uk-UA",
            };

            // self link (Required) - The URL for the syndication feed.
            feed.Links.Add(SyndicationLink.CreateSelfLink(new Uri(FeedUrl), "Rss"));

            // alternate link (Recommended) - The URL for the web page showing the same data as the syndication feed.
            //feed.Links.Add(SyndicationLink.CreateAlternateLink(new Uri(SiteUrl), "Html"));

            // hub link (Recommended) - The URL for the PubSubHubbub hub. Used to push new entries to subscribers 
            //                          instead of making them poll the feed. See feed updated method below.
            feed.Links.Add(new SyndicationLink(new Uri(PubSubHubbubHubUrl), "hub", null, null, 0));


            // author (Recommended) - Names one author of the feed. A feed may have multiple author elements. A feed 
            //                        must contain at least one author element unless all of the entry elements contain 
            //                        at least one author element.
            feed.Authors.Add(GetPerson());

            // category (Optional) - Specifies a category that the feed belongs to. A feed may have multiple category 
            //                       elements.
            feed.Categories.Add(new SyndicationCategory("Новини"));

            // contributor (Optional) - Names one contributor to the feed. An feed may have multiple contributor 
            //                          elements.
            feed.Contributors.Add(GetPerson());

            return feed;
        }

        /// <summary>
        /// Publishes the fact that the feed has updated to subscribers using the PubSubHubbub v0.4 protocol.
        /// </summary>
        /// <remarks>
        /// The PubSubHubbub is an open standard created by Google which allows subscription of feeds and allows 
        /// updates to be pushed to them rather than them having to poll the feed. This means subscribers get live
        /// updates as they happen and also we may save some bandwidth because we have less polling of our feed.
        /// See https://pubsubhubbub.googlecode.com/git/pubsubhubbub-core-0.4.html for PubSubHubbub v0.4 specification.
        /// See https://github.com/pubsubhubbub for PubSubHubbub GitHub projects.
        /// See http://pubsubhubbub.appspot.com/ for Google's implementation of the PubSubHubbub hub we are using.
        /// </remarks>
        public Task PublishUpdate()
        {
            return _httpClient.PostAsync(
                PubSubHubbubHubUrl,
                new FormUrlEncodedContent(
                    new[]
                    {
                        new KeyValuePair<string, string>("hub.mode", "publish"),
                        new KeyValuePair<string, string>("hub.url", FeedUrl)
                    }));
        }

        #endregion

        #region Private Methods

        private SyndicationPerson GetPerson()
        {
            return new SyndicationPerson()
            {
                // name (Required) - conveys a human-readable name for the person.
                Name = "moemisto.com.ua",
                // uri (Optional) - contains a home page for the person.
                Uri = SiteUrl,
                // email (Optional) - contains an email address for the person.
                Email = "moemisto.com.ua@gmail.com (Моє Місто)"
            };
        }

        /// <summary>
        /// Gets the collection of <see cref="SyndicationItem"/>'s that represent the atom entries.
        /// </summary>
        /// <param name="cancellationToken">A <see cref="CancellationToken"/> signifying if the request is cancelled.</param>
        /// <returns>A collection of <see cref="SyndicationItem"/>'s.</returns>
        private async Task<List<SyndicationItem>> GetItems(CancellationToken cancellationToken)
        {
            List<SyndicationItem> items = new List<SyndicationItem>();

            foreach (var article in _feedContext.GetArticlesToday())
            {
                SyndicationItem item = new SyndicationItem()
                {
                    // id (Required) - Identifies the entry using a universally unique and permanent URI. Two entries 
                    //                 in a feed can have the same value for id if they represent the same entry at 
                    //                 different points in time.
                    Id = string.Format("{0}/{1}", SiteUrl, article.Url),
                    // title (Required) - Contains a human readable title for the entry. This value should not be blank.
                    Title = SyndicationContent.CreatePlaintextContent(article.Title),
                    // description (Recommended) - A summary of the entry.
                    //Summary = SyndicationContent.CreatePlaintextContent(article.Summary),
                    // updated (Optional) - Indicates the last time the entry was modified in a significant way. This 
                    //                      value need not change after a typo is fixed, only after a substantial 
                    //                      modification. Generally, different entries in a feed will have different 
                    //                      updated timestamps.
                    LastUpdatedTime = article.DatePublish,
                    // published (Optional) - Contains the time of the initial creation or first availability of the entry.
                    PublishDate = article.DatePublish,
                    // rights (Optional) - Conveys information about rights, e.g. copyrights, held in and over the entry.
                    Copyright = new TextSyndicationContent(
                        string.Format("© {0} - {1}", DateTime.Now.Year, "moemisto.com.ua")),
                    // link (Recommended) - Identifies a related Web page. An entry must contain an alternate link if there 
                    //                      is no content element.
                    Links = new Collection<SyndicationLink>
                    {
                        SyndicationLink.CreateAlternateLink(new Uri(string.Format("{0}/{1}", SiteUrl, article.Url)))
                    },  
                     
                    // AND/OR
                    // Text content  (Optional) - Contains or links to the complete content of the entry. Content must be 
                    //                            provided if there is no alternate link.
                    // item.Content = SyndicationContent.CreatePlaintextContent("The actual plain text content of the entry");
                    // HTML content (Optional) - Content can be plain text or HTML. Here is a HTML example.
                    Content = SyndicationContent.CreatePlaintextContent(article.Summary) , //SyndicationContent.CreateHtmlContent(article.Content)
                    
                    // author (Optional) - Names one author of the entry. An entry may have multiple authors. An entry must 
                    //                     contain at least one author element unless there is an author element in the 
                    //                     enclosing feed, or there is an author element in the enclosed source element.
                    //item.Authors.Add(GetPerson());

                    // contributor (Optional) - Names one contributor to the entry. An entry may have multiple contributor elements.
                    //item.Contributors.Add(GetPerson());

                    // category (Optional) - Specifies a category that the entry belongs to. A entry may have multiple 
                    //                       category elements.

                };

                item.Categories.Add(new SyndicationCategory(article.CategoryName));
                item.ElementExtensions.Add(
                    new XElement("enclosure",
                        new XAttribute("type", "image/jpeg"),
                        new XAttribute("url", string.Format("{0}/{1}", SiteUrl, article.PictureUrl))
                        ).CreateReader()
                );

                items.Add(item);
            }

            return items;
        }

        #endregion
    }
}