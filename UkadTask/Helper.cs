using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace UkadTask
{
    public static class Helper
    {
        public static bool UrlExists(string url)
        {
            using (var client = new WebClient())
            {
                try
                {
                    using (client.OpenRead(url))
                    {
                        return true;
                    }
                }
                catch
                {
                    return false;
                }
            }
        }
        public static string ConvertUrlToSitemap(string url)
        {
            return $"{url}/sitemap.xml";
        }
        public static int GetResponseTime(string url)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            Stopwatch timer = new Stopwatch();

            timer.Start();

            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            response.Close();

            timer.Stop();

            return timer.Elapsed.Milliseconds;
        }
        public static void PrintOrderedByTime(List<Url> list)
        {
            foreach (var item in list.OrderBy(x => x.ElapsedTime))
            {
                Console.WriteLine($"{item.UrlName}, Response time(ms): {item.ElapsedTime}");
            }
            Console.WriteLine("\n");
        }
        public static void Print(List<string> list)
        {
            foreach (var item in list)
            {
                Console.WriteLine($"{item}");
            }
            Console.WriteLine("\n");
        }
    }
}
