using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace SimpleProxyChecker
{
    public class Proxy
    {
        public IPEndPoint IPEndPoint { get; set; }

        public Proxy(IPEndPoint endPoint) => IPEndPoint = endPoint;

        public static Proxy Parse(string str)
        {
            str = str.Replace(';', ':');
            str = str.Replace(',', ':');
            string[] parts = str.Split(':');

            try
            {
                string ipStr = parts[0];
                string portStr = parts[1];
                IPAddress.TryParse(ipStr, out IPAddress ip);

                return new Proxy(new IPEndPoint(ip, int.Parse(portStr)));
            }
            catch (Exception)
            {
                return null;
            }
        }
        public void PerformTest()
        {
            TestProxy(this);
        }

        public static void TestProxy(Proxy proxy)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create("https://www.vrchat.net/");
            request.Proxy = new WebProxy(proxy.IPEndPoint.Address.ToString(), proxy.IPEndPoint.Port);
            request.UserAgent = "Mozilla/5.0 (Windows NT 6.1; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/39.0.2171.95 Safari/537.36";
            request.Timeout = 2000;

            try
            {
                WebResponse response = request.GetResponse();
                Console.WriteLine(proxy.IPEndPoint.Address.ToString() + ":" + proxy.IPEndPoint.Port);
                Program.working++;
            }
            catch (Exception)
            {
                Program.bad++;
            }
        }
    }
}
