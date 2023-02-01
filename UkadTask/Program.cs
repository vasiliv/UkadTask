using System.Diagnostics;
using System.Xml;
using System;
using UkadTask;
using HtmlAgilityPack;
using static System.Net.Mime.MediaTypeNames;
using System.Net.Http;
using Microsoft.AspNetCore.Http;
using System.Reflection;
using System.Linq;
using System.Xml.Linq;

Stopwatch stopwatch = new Stopwatch();
List<Url> urlList = new List<Url>();
List<Url> xmlList = new List<Url>();

Console.Write("Please enter url address: ");
string urlName = Console.ReadLine().TrimEnd('/');

//string urlName = "https://jwt.io";
//string urlName = "https://lenta.ru/";
//string urlName = "https://www.google.com";
//string urlName = "https://ss.ge/";

HtmlWeb web = new HtmlWeb();
if (Helper.UrlExists(urlName))
{
    HtmlDocument htmlDoc = web.Load(urlName);    

    //Use the SelectNodes method to find all the "a" elements in the document:
    HtmlNodeCollection htmlNodes = htmlDoc.DocumentNode.SelectNodes("//a[@href]");

    foreach (HtmlNode node in htmlNodes)
    {
        stopwatch.Start();
        //Select only outer links
        if (node.Attributes["href"].Value.Contains("http"))
        {
            stopwatch.Stop();
            urlList.Add(new Url { UrlName = node.Attributes["href"].Value.TrimEnd('/'), ElapsedTime = stopwatch.ElapsedMilliseconds });
        }
    }
}
else
{
    Console.WriteLine("Url does not exist");
    return;
}

XmlDocument xmlDoc = new XmlDocument();

string sitemapUrl = Helper.ConvertUrlToSitemap(urlName);
if (Helper.UrlExists(sitemapUrl))
{
    xmlDoc.Load(sitemapUrl);
    foreach (XmlNode node in xmlDoc.DocumentElement)
    {
        stopwatch.Start();
        if (node.InnerText.Contains("http"))
        {
            stopwatch.Stop();
            //add urls located in <loc> node to xmlList
            xmlList.Add(new Url { UrlName = node.FirstChild.InnerXml, ElapsedTime = stopwatch.ElapsedMilliseconds });
        }
    }
}
else
{
    Console.WriteLine("Sitemap.xml does not exist");
}

//Extract only unique members
urlList = urlList.GroupBy(u => u.UrlName).Select(u => u.FirstOrDefault()).ToList();
Console.WriteLine($"Urls(html documents) found after crawling a website: {urlList.Count}");
Console.WriteLine($"Urls found in sitemap.xml: {xmlList.Count}");

var xmlListOnlyName = xmlList.Select(x => x.UrlName);
var urlListOnlyName = urlList.Select(x => x.UrlName);

Console.WriteLine("\n1. Merge ordered by timing");
var totalList = xmlList.Concat(urlList).ToList();
Helper.PrintOrderedByTime(totalList);

Console.WriteLine();
Console.WriteLine("\n2. Urls founded in sitemap.xml but not founded after crawling a web site:");
var exceptXml = xmlListOnlyName.Except(urlListOnlyName).ToList();
Helper.Print(exceptXml);

Console.WriteLine();
Console.WriteLine("\n3. Urls founded by the crawling the website but not in sitemap.xml");
var exceptUrl = urlListOnlyName.Except(xmlListOnlyName).ToList();
Helper.Print(exceptUrl);

