using DLSpy.Entities;
using DLSpy.ViewModels;
using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace DLSpy.Utilities
{
    /// <summary>
    /// DLSite 검색과 관련된 도구를 제공합니다.
    /// </summary>
    public static class SearchManager
    {
        public static int GetItemsCount(string text)
        {
            try
            {
                ReportManager.Instance.AddReport($"SearchManager::Run > GetItemsCount({text})");

                // Initialize url and get response.
                text = WebUtility.UrlEncode(text);
                string url = string.Format(Settings.URL_SEARCH, text, 1);
                string response = HttpUtility.GetResponse(url);

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
            catch (Exception ex)
            {
                ReportManager.Instance.AddReport($"SearchManager::Exception occured > {ex.Message}\r\n{ex.StackTrace}");
                return 0;
            }
        }

        public static List<SearchResult> GetSearchResult(string text, uint page)
        {
            try
            {
                ReportManager.Instance.AddReport($"SearchManager::Run > GetItemsCount({text}, {page})");

                List<SearchResult> resultsList = new List<SearchResult>();

                // Initialize url and get response.
                text = WebUtility.UrlEncode(text);
                string url = string.Format(Settings.URL_SEARCH, text, page);
                string response = HttpUtility.GetResponse(url);

                HtmlDocument document = new HtmlDocument();
                document.LoadHtml(response);

                HtmlNode searchResultList = document.DocumentNode.SelectSingleNode(@"//ul[@class='n_worklist']");

                for (int i = 0; i < searchResultList.ChildNodes.Count; i++)
                {
                    HtmlNode tempNode = searchResultList.ChildNodes[i];

                    if (tempNode.OriginalName == "li") // Check HTML Tag type.
                    {
                        SearchResult result = new SearchResult();

                        HtmlNode parentNode = tempNode.SelectSingleNode("dl");

                        // Code
                        string tempCode = string.Empty;

                        HtmlNode code = parentNode.SelectSingleNode(@"dt/a");

                        if (CodeManager.GetCode(code.Attributes["href"].Value, out tempCode))
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
                            StringBuilder builder = new StringBuilder();
                            builder.Append("https:");
                            builder.Append(thumbParent.Attributes["src"].Value);

                            result.ThumbnailUrl = builder.ToString();
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
            catch (Exception ex)
            {
                ReportManager.Instance.AddReport($"SearchManager::Exception occured > {ex.Message}\r\n{ex.StackTrace}");
                return new List<SearchResult>();
            }
        }
    }
}
