using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.ServiceModel.Syndication;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;
using System.Web.Hosting;
using System.Xml;

public class Feeds
{
    public string webpage { get; set; }
    public string feedurl { get; set; }
    public string category { get; set; }
}

public class Feeder
{
    private static string _masterFile = HostingEnvironment.MapPath("~/App_Data/master.xml");
    private static string _feedFile = HostingEnvironment.MapPath("~/App_Data/feed.xml");
    FeedData fdata;// = new FeedData();
    List<FeedData> feedData = new List<FeedData>();

    public async Task DownloadFeeds()
    {
        var rss = new SyndicationFeed("Programmer Feeds", "Personal Feeds", null);
        string jsonFeed = File.ReadAllText(System.Web.Hosting.HostingEnvironment.MapPath("~/App_Data/feeder.json"));
        var feeds = JsonConvert.DeserializeObject<List<Feeds>>(jsonFeed);


        foreach (var f in feeds)
        {
            SyndicationFeed feed = await DownloadFeed(f.feedurl);
            rss.Items = rss.Items.Union(feed.Items).GroupBy(i => i.Title.Text).Select(i => i.First()).OrderByDescending(i => i.PublishDate.Date);
        }

        using (XmlWriter writer = XmlWriter.Create(_masterFile))
            rss.SaveAsRss20(writer);

        using (XmlWriter writer = XmlWriter.Create(_feedFile))
        {
            rss.Items = rss.Items.Take(10);
            rss.SaveAsRss20(writer);
        }
    }

    private async Task<SyndicationFeed> DownloadFeed(string url)
    {
        try
        {
            using (WebClient client = new WebClient())
            {
                var stream = await client.OpenReadTaskAsync(url);
                return SyndicationFeed.Load(XmlReader.Create(stream));
            }
        }
        catch (Exception ex)
        {
            //Trace.Warn("Feed Collector", "Couldn't download: " + url, ex);
            return new SyndicationFeed();
        }
    }

    private IEnumerable<SyndicationItem> GetData()
    {
        using (XmlReader reader = XmlReader.Create(_masterFile))
        {
            //var count = int.Parse(config.AppSettings["postsPerPage"]);
            var items = SyndicationFeed.Load(reader).Items;//.Skip((_page - 1) * count).Take(count);
            return items.Select(item => { CleanItem(item); return item; });
            //return SyndicationFeed.Load(reader).Items;
        }
    }

    private static void CleanItem(SyndicationItem item)
    {
        string summary = item.Summary != null ? item.Summary.Text : ((TextSyndicationContent)item.Content).Text;
        summary = Regex.Replace(summary, "<[^>]*>", ""); // Strips out HTML
        item.Summary = new TextSyndicationContent(string.Join("", summary.Take(300)) + "...");
    }

    public List<FeedData> ReadData()
    {
        //Load teh document
        XmlDocument doc = new XmlDocument();
        XmlNamespaceManager namespaces = new XmlNamespaceManager(doc.NameTable);
        namespaces.AddNamespace("ns", "urn:hl7-org:v3");
        doc.Load(_masterFile);

        var nodeList = doc.DocumentElement.SelectNodes("channel/item");

        foreach (XmlNode node in nodeList)
        {
            string title = node["title"]?.InnerText;
            string description = node["description"]?.InnerText;

            string link = string.Empty;
            string pubdate = string.Empty;

            if(node["link"] != null)
            {
                link = node["link"]?.InnerText;
            }
            else if(node["a10:link"] != null)
            {
                link = node["a10:link"].Attributes[0].InnerText;
                //link = node["a10:link"].InnerText;
            }

            if (node["pubDate"] != null)
            {
                pubdate = node["pubDate"].InnerText;
            }
            else if (node["a10:updated"] != null)
            {
                pubdate = node["a10:updated"].InnerText;
            }

            fdata = new FeedData();
            fdata.publishdate = Convert.ToDateTime(pubdate).ToString("yyyy-MM-dd HH:mm"); ;
            fdata.link = link;
            fdata.title = title;
            if (Regex.Replace(description, "<[^>]*>", "").Length > 300)
            {
                fdata.description = Regex.Replace(description, "<[^>]*>", "").Substring(0, 299) + "...";
            }
            else
            {
                fdata.description = Regex.Replace(description, "<[^>]*>", "");
            }
                feedData.Add(fdata);
        }

        return feedData;

    }
}
