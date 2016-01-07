using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Xml;
using Terradue.ServiceModel.Syndication;

namespace Moemisto.UI.Services.Feed
{
    /// <summary>
    /// Represents a class that is used to render an Atom 1.0 feed by using an <see cref="SyndicationFeed"/> instance
    /// representing the feed.
    /// </summary>
    public sealed class RssActionResult : ActionResult
    {
        private readonly SyndicationFeed _syndicationFeed;
        /// <summary>
        /// Initializes a new instance of the <see cref="RssActionResult"/> class.
        /// </summary>
        /// <param name="syndicationFeed">The Atom 1.0 <see cref="SyndicationFeed" />.</param>
        public RssActionResult(SyndicationFeed syndicationFeed)
        {
            _syndicationFeed = syndicationFeed;
        }
        /// <summary>
        /// Executes the call to the ActionResult method and returns the created feed to the output response.
        /// </summary>
        /// <param name="context">The context in which the result is executed. The context information includes the
        /// controller, HTTP content, request context, and route data.</param>
        public override void ExecuteResult(ControllerContext context)
        {
            context.HttpContext.Response.ContentType = "application/xml";
            Rss20FeedFormatter feedFormatter = new Rss20FeedFormatter(_syndicationFeed);
            XmlWriterSettings xmlWriterSettings = new XmlWriterSettings {Encoding = Encoding.UTF8};
            if (HttpContext.Current.IsDebuggingEnabled)
            {
                // Indent the XML for easier viewing but only in Debug mode. In Release mode, everything is output on
                // one line for best performance.
                xmlWriterSettings.Indent = true;
            }
            using (XmlWriter xmlWriter = XmlWriter.Create(context.HttpContext.Response.Output, xmlWriterSettings))
            {
                feedFormatter.WriteTo(xmlWriter);
            }
        }
    }
}