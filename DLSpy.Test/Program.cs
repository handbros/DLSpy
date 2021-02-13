using HtmlAgilityPack;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;

namespace DLSpy.Test
{
    class Program
    {
        // HTTP web client
        private static readonly HttpClient _webClient = new HttpClient();

        /// <summary>
        /// A regex for checking RJ code.
        /// </summary>
        public const string REGEX_MATCH_RJ = @"RJ[0-9]{1,6}";

        /// <summary>
        /// A regex for checking BJ code.
        /// </summary>
        public const string REGEX_MATCH_BJ = @"BJ[0-9]{1,6}";

        /// <summary>
        /// A regex for checking VJ code.
        /// </summary>
        public const string REGEX_MATCH_VJ = @"VJ[0-9]{1,6}";

        public const string URL_SEARCH = "https://www.dlsite.com/maniax/fsr/=/language/jp/sex_category%5B0%5D/male/keyword/{0}/order%5B0%5D/trend/per_page/30/page/{1}/?locale=ko_KR";

        public const string URL_WORK = "https://www.dlsite.com/maniax/work/=/product_id/{0}.html/?locale=ko_KR";

        public const string URL_WORK_INFO_JSON = "https://www.dlsite.com/maniax/product/info/ajax?product_id={0}&cdn_cache_min=1";

        static void Main(string[] args)
        {

            List<SearchResult> results =  GetSearchResult("sex", 1);

            Console.WriteLine($"------------------------------");

            foreach (SearchResult result in results)
            {
                Console.WriteLine($"CODE : {result.Code}");
                Console.WriteLine($"TYPE : {result.Type}");
                Console.WriteLine($"THUMB_URL : {result.ThumbnailUrl}");
                Console.WriteLine($"TITLE : {result.Title}");
                Console.WriteLine($"CIRCLE : {result.Circle}");
                Console.WriteLine($"------------------------------");
            }

            Console.WriteLine($"##### COUNT : {results.Count}");

            Console.WriteLine($"TOT_ITEMS : {GetItemsCount("sex")}");
        }

        /// <summary>
        /// Writes a text file in the specified path.
        /// </summary>
        /// <param name="filePath">Path where the text file will be saved.</param>
        /// <param name="text">Text to be writed.</param>
        /// <param name="encoding">The text encoding to use.</param>
        public static void WriteTextFile(string filePath, string text, Encoding encoding)
        {
            using (Stream stream = new FileStream(filePath, FileMode.Create, FileAccess.Write))
            {
                StreamWriter objSaveFile = new StreamWriter(stream, encoding);
                objSaveFile.Write(text);
                objSaveFile.Close();
                objSaveFile.Dispose();
            }
        }

        private static string GetResponse(string url)
        {
            string responseText = string.Empty;

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.Method = "GET";
            request.Timeout = 10000; // 10초

            using (HttpWebResponse resp = (HttpWebResponse)request.GetResponse())
            {
                HttpStatusCode status = resp.StatusCode;

                using (Stream respStream = resp.GetResponseStream())
                {
                    using (StreamReader sr = new StreamReader(respStream))
                    {
                        responseText = sr.ReadToEnd();
                    }
                }
            }

            WriteTextFile(@"E:\test.html", responseText, Encoding.UTF8);

            return responseText;
        }

        private static string StripHtml(string html)
        {
            return Regex.Replace(html, @"<(.|\n)*?>", string.Empty);
        }

        private static string GetInnerTextsFromNode(HtmlNode node)
        {
            if (node.ChildNodes.Count <= 0)
            {
                return node.InnerText;
            }
            else
            {
                string result = string.Empty;

                foreach (HtmlNode child in node.ChildNodes)
                {
                    if (child.InnerText == "\r" || child.InnerText == "\n")
                    {
                        continue;
                    }
                    
                    result += $"{GetInnerTextsFromNode(child)} ";
                }

                return result;
            }
        }

        public static bool GetCode(string text, out string result)
        {
            text = text.ToUpper();

            if (Regex.IsMatch(text, REGEX_MATCH_RJ))
            {
                Match match = Regex.Match(text, REGEX_MATCH_RJ);
                result = match.Value;
                return true;
            }
            else if (Regex.IsMatch(text, REGEX_MATCH_BJ))
            {
                Match match = Regex.Match(text, REGEX_MATCH_BJ);
                result = match.Value;
                return true;
            }
            else if (Regex.IsMatch(text, REGEX_MATCH_VJ))
            {
                Match match = Regex.Match(text, REGEX_MATCH_VJ);
                result = match.Value;
                return true;
            }
            else
            {
                result = null;
                return false;
            }
        }

        public static int GetItemsCount(string text)
        {
            // Initialize url and get response.
            text = WebUtility.UrlEncode(text);
            string url = string.Format(URL_SEARCH, text, 1);
            string response = GetResponse(url);

            HtmlDocument document = new HtmlDocument();
            document.LoadHtml(response);

            HtmlNode pageTotal = document.DocumentNode.SelectSingleNode(@"//div[@class='page_total']/strong[1]");

            if (pageTotal != null)
            {
                return int.Parse(pageTotal.InnerText);
            }
            else
            {
                return 0;
            }
        }

        private static List<SearchResult> GetSearchResult(string text, uint page)
        {
            List<SearchResult> resultsList = new List<SearchResult>();

            // Initialize url and get response.
            text = WebUtility.UrlEncode(text);
            string url = string.Format(URL_SEARCH, text, page);
            string response = GetResponse(url);

            HtmlDocument document = new HtmlDocument();
            document.LoadHtml(response);

            HtmlNode searchResultList = document.DocumentNode.SelectSingleNode(@"//ul[@class='n_worklist']");

            foreach (HtmlNode tempNode in searchResultList.ChildNodes)
            {
                if (tempNode.OriginalName == "li") // Check HTML Tag type.
                {
                    SearchResult result = new SearchResult();

                    HtmlNode parentNode = tempNode.SelectSingleNode("dl");

                    // Code
                    string tempCode = string.Empty;

                    HtmlNode code = parentNode.SelectSingleNode(@"dt/a");

                    if (GetCode(code.Attributes["href"].Value, out tempCode))
                    {
                        result.Code = tempCode;
                    }

                    // Type
                    HtmlNode type = parentNode.SelectSingleNode(@"dt/div/a");

                    if (type != null)
                    {
                        result.Type = type.InnerText;
                    }

                    // Thumbnail Url
                    HtmlNode thumbParent = parentNode.SelectSingleNode(@"dt/a/img");

                    if (thumbParent != null)
                    {
                        result.ThumbnailUrl = thumbParent.Attributes["src"].Value;
                    }

                    // Title
                    HtmlNode title = parentNode.SelectSingleNode(@"dd[@class='work_name']/div[@class='multiline_truncate']/a");

                    if (title != null)
                    {
                        result.Title = title.InnerText;
                    }

                    // Circle
                    HtmlNode circle = parentNode.SelectSingleNode(@"dd[@class='maker_name']/a");

                    if (circle != null)
                    {
                        result.Circle = circle.InnerText;
                    }

                    resultsList.Add(result);
                }
            }

            return resultsList;
        }
    }

    public class SearchResult
    {
        public string Code { get; set; }

        public string Type { get; set; }

        public string ThumbnailUrl { get; set; }

        public string Title { get; set; }

        public string Circle { get; set; }
    }
}
