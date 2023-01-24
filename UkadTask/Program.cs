using System.Diagnostics;
using System.Xml;
using System;
using UkadTask;
using HtmlAgilityPack;
using static System.Net.Mime.MediaTypeNames;
using System.Net.Http;
using Microsoft.AspNetCore.Http;
using System.Reflection;

//Console.Write("Please enter url address: ");
//string urlName = Console.ReadLine();
//string urlName = "https://www.ambebi.ge/";
string urlName = "https://www.github.com/";


Stopwatch stopwatch = new Stopwatch();
List<Url> urlList = new List<Url>();

XmlDocument xmlDoc = new XmlDocument();
List<Url> xmlList = new List<Url>();

//Load relative path
var fileName = Path.Combine(Path.GetDirectoryName(Assembly.GetEntryAssembly().Location), @"Sitemap.xml");
xmlDoc.Load(fileName);

XmlNodeList xmlNodes = xmlDoc.SelectNodes("/root/url");

foreach (XmlNode node in xmlNodes)
{
    stopwatch.Start();
    if (node.InnerText.Contains("/"))
    {
        stopwatch.Start();
        xmlList.Add(new Url { UrlName = node.InnerText.Replace("\n", "").Trim(), ElapsedTime = stopwatch.ElapsedTicks });
    }    
}
Console.WriteLine($"Urls found in Sitemap.mxl: {xmlList.Count}");

HtmlWeb web = new HtmlWeb();
HtmlDocument htmlDoc = web.Load(urlName);

//Use the SelectNodes method to find all the "a" elements in the document:
HtmlNodeCollection htmlNodes = htmlDoc.DocumentNode.SelectNodes("//a[@href]");

//Iterate through the nodes and check the "href" attribute of each node to see if it starts with "http"
foreach (HtmlNode node in htmlNodes)
{
    stopwatch.Start();
    //if (node.Attributes["href"].Value.Contains("http"))
    if (node.OuterHtml.Contains("/"))
    {
        stopwatch.Stop();
        urlList.Add(new Url { UrlName = node.Attributes["href"].Value.TrimEnd('/'), ElapsedTime = stopwatch.ElapsedTicks });
    }
}
Console.WriteLine($"Urls(html documents) found after crawling a website: {urlList.Count}");

//Concatinate url and xml Lists
//List<Url> totalList = xmlList.Concat(urlList).Distinct().ToList();

var xmlListOnlyName = xmlList.Select(x => x.UrlName);
var urlListOnlyName = urlList.Select(x => x.UrlName);
var all = xmlListOnlyName.Concat(urlListOnlyName).Distinct();
foreach (var url in all)
{
    
}
//var list1 = new List<Url>;
//foreach (Url urlxml in urlList)


//xmllist
//url list
//totallist
foreach (var item in totalList)
{
    Console.WriteLine($"{item.UrlName} {item.ElapsedTime}");
}