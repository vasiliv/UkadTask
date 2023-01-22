using System.Diagnostics;
using System.Xml;
using System;
using UkadTask;
using HtmlAgilityPack;

Console.Write("Please enter url address: ");
string urlName = Console.ReadLine();

Stopwatch stopwatch = new Stopwatch();
List<Url> urlList = new List<Url>();

HtmlWeb web = new HtmlWeb();
HtmlDocument htmlDoc = web.Load(urlName);

//Use the SelectNodes method to find all the "a" elements in the document:
HtmlNodeCollection htmlNodes = htmlDoc.DocumentNode.SelectNodes("//a");

//Iterate through the nodes and check the "href" attribute of each node to see if it starts with "http"
foreach (HtmlNode node in htmlNodes)
{
    stopwatch.Start();
    if (node.Attributes["href"].Value.StartsWith("http"))
    {
        stopwatch.Stop();
        urlList.Add(new Url { UrlName = node.Attributes["href"].Value, ElapsedTime = stopwatch.ElapsedTicks });
    }
}
Console.WriteLine($"Urls(html documents) found after crawling a website: {urlList.Count}");

//Sitemap.xml file is placed at \bin\Debug\net5.0 folder
XmlDocument xmlDoc = new XmlDocument();
List<Url> xmlList = new List<Url>();
xmlDoc.Load("Sitemap.xml");
XmlNodeList xmlNodes = xmlDoc.SelectNodes("/root/url");

foreach (XmlNode node in xmlNodes)
{
    xmlList.Add(new Url { UrlName = node.InnerText.Replace("\n", "").Trim() });
}
Console.WriteLine($"Urls found in sitemap: {xmlList.Count}");

//Concatinate url and xml Lists
List<Url> totalList = urlList.Concat(xmlList).ToList();

foreach (var item in totalList)
{
    Console.WriteLine($"{item.UrlName} {item.ElapsedTime}");
}