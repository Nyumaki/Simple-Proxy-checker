using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.Security;
using System.IO;
using System.Threading.Tasks;

namespace SimpleProxyChecker
{
    class Program
    {
        public static IList<Proxy> proxyList = new List<Proxy>();
        static void Main()
        {
            Console.Title = "Proxy Checker Made By Ryuuu";
            ServicePointManager.ServerCertificateValidationCallback = new RemoteCertificateValidationCallback( delegate { return true; });
            string proxys = new WebClient().DownloadString("http://proxy.l337.tech/txt");
            using (StringReader reader = new StringReader(proxys))
                AddProxy(reader);
            Console.WriteLine("Made by Ryuuu for vrchat use");
            Console.ReadLine();
            TestProxyList();
            Console.ReadLine();
        }

        private static void AddProxy(StringReader reader)
        {
            string line;
            while ((line = reader.ReadLine()) != null)
            {
                Proxy p = Proxy.Parse(line);
                if (p != null)
                {
                    proxyList.Add(p);
                }
            }
        }

        public static int working = 0;
        public static int bad = 0;
        public static void TestProxyList()
        {
            int proxyNum = proxyList.Count;
            int proxyTested = 0;
            
            Task.Factory.StartNew(() =>
            {
                Parallel.ForEach(proxyList, new ParallelOptions() { MaxDegreeOfParallelism = 32 }, proxy =>
                {
                    proxy.PerformTest();
                    ++proxyTested;
                    Console.Title = "Proxy Checker Made By Ryuuu - Num of proxy Tested: " + proxyTested + " Working Proxy: " + working + " Bad Proxy: " + bad;
                });
            });
        }


    }
}
