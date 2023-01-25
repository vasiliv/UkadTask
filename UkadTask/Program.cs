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

Console.Write("Please enter url address: ");
string urlName = Console.ReadLine();

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

foreach (HtmlNode node in htmlNodes)
{
    stopwatch.Start();    
    if (node.OuterHtml.Contains("/"))
    {
        stopwatch.Stop();
        urlList.Add(new Url { UrlName = node.Attributes["href"].Value.TrimEnd('/'), ElapsedTime = stopwatch.ElapsedTicks });
    }
}
Console.WriteLine($"Urls(html documents) found after crawling a website: {urlList.Count}");

var xmlListOnlyName = xmlList.Select(x => x.UrlName);
var urlListOnlyName = urlList.Select(x => x.UrlName);

Console.WriteLine();
Console.WriteLine("1. Merge ordered by timing");
var totalList = xmlList.Concat(urlList).OrderBy(i => i.ElapsedTime);

foreach (var item in totalList)
{
    Console.WriteLine($"{item.UrlName} {item.ElapsedTime}");
}

Console.WriteLine();
Console.WriteLine("2. Except URL");
var exceptXml = xmlListOnlyName.Except(urlListOnlyName);
foreach (var item in exceptXml)
{
    Console.WriteLine(item);
}

Console.WriteLine();
Console.WriteLine("3. Except Sitemap.xml");
var exceptUrl = urlListOnlyName.Except(xmlListOnlyName);
foreach (var item in exceptUrl)
{
    Console.WriteLine(item);
}

