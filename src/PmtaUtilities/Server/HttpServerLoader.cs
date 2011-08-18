using System;
using System.IO;
using System.Net;
using System.Text.RegularExpressions;
using System.Xml.Linq;
using PmtaUtilities.Configuration.Parser;
using PmtaUtilities.Extensions;
using PmtaUtilities.XML.Filler;
using Sprache;

namespace PmtaUtilities.Server
{
    public class HttpServerLoader
    {
        protected StatusFiller _statusFiller;
        protected JobFiller _jobFiller;
  

        
        public HttpServerLoader(StatusFiller statusFiller, JobFiller jobFiller)
        {
            _statusFiller = statusFiller;
            _jobFiller = jobFiller;
        }

        public Server FromURL(Uri serverWebUrl)
        {
            return new Server
                                {
                                    baseUrl = serverWebUrl,
                                    Jobs = _jobFiller.Extract(XDocument.Parse(GetWebPage(serverWebUrl.Combine("jobs?format=xml").ToString()))),
                                    Status = _statusFiller.Extract(XDocument.Parse(GetWebPage(serverWebUrl.Combine("status?format=xml").ToString()))),
                                    Configuration = PmtaParser.Configuration().Parse(GetConfigFromWeb(serverWebUrl.Combine("editconfig").ToString()))
                                };    
        }

        public static string GetConfigFromWeb(string configAddress)
        {
            string pmtaConfigUrl = configAddress;
            Regex textarea = new Regex("<textarea cols=\"\\d+\" rows=\"\\d+\" name=\"file\" .+\\>\\n(?<link>(?:[^\\<]*\\n)+)");

            string result = GetWebPage(pmtaConfigUrl);

            Match fark = textarea.Match(result);
            string theGoods = System.Web.HttpUtility.HtmlDecode(fark.Groups["link"].Value);
            return theGoods.Substring(0, theGoods.Length - 1);
        }
        private static string GetWebPage(string url)
        {
            HttpWebRequest myRequest = (HttpWebRequest)WebRequest.Create(url);
            myRequest.Method = "GET";
            WebResponse myResponse = myRequest.GetResponse();
            StreamReader sr = new StreamReader(myResponse.GetResponseStream(), System.Text.Encoding.UTF8);
            string result = sr.ReadToEnd();
            sr.Close();
            myResponse.Close();

            return result;
        }

    }
}
