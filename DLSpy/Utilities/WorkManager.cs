using DLSpy.Entities;
using DLSpy.ViewModels;
using HtmlAgilityPack;
using Newtonsoft.Json.Linq;
using System;
using System.Text;

namespace DLSpy.Utilities
{
    /// <summary>
    /// DLSite 작품 정보와 관련된 도구를 제공합니다. 
    /// </summary>
    public static class WorkManager
    {
        public static string GetInnerTextsFromNode(HtmlNode node)
        {
            ReportManager.Instance.AddReport($"WorkManager::Run > GetInnerTextsFromNode(HtmlAgilityPack.HtmlNode)");

            if (node.ChildNodes.Count <= 0)
            {
                return node.InnerText;
            }
            else
            {
                StringBuilder builder = new StringBuilder();

                for (int i = 0; i < node.ChildNodes.Count; i++)
                {
                    HtmlNode child = node.ChildNodes[i];

                    if (child.InnerText == "\r" || child.InnerText == "\n")
                    {
                        continue;
                    }

                    builder.Append($"{GetInnerTextsFromNode(child)}");
                }

                return builder.ToString();
            }
        }

        public static bool GetWork(string code, out WorkInformation result)
        {
            try
            {
                ReportManager.Instance.AddReport($"WorkManager::Run > GetWork({code})");

                WorkInformation work = new WorkInformation();

                string workUrl = string.Empty;
                string workJsonUrl = string.Empty;

                // Check work type and initialize urls.
                string upperCaseCode = code.ToUpper();

                if (upperCaseCode.StartsWith("RJ"))
                {
                    workUrl = Settings.URL_WORK_RJ;
                    workJsonUrl = Settings.URL_WORK_JSON_RJ;
                }
                else if (upperCaseCode.StartsWith("BJ"))
                {
                    workUrl = Settings.URL_WORK_BJ;
                    workJsonUrl = Settings.URL_WORK_JSON_BJ;
                }
                else if (upperCaseCode.StartsWith("VJ"))
                {
                    workUrl = Settings.URL_WORK_VJ;
                    workJsonUrl = Settings.URL_WORK_JSON_VJ;
                }
                else
                {
                    result = null;
                    ViewModelBroker.Instance.Main.ShowNotification(NotificationType.UnknownError);
                    return false;
                }

                workUrl = string.Format(workUrl, code);
                workJsonUrl = string.Format(workJsonUrl, code);

                // Get responses.
                string workPageText = HttpUtility.GetResponse(workUrl);
                string jsonText = HttpUtility.GetResponse(workJsonUrl);

                JToken token = JToken.Parse(jsonText);

                if (token.Type == JTokenType.Object)
                {
                    JObject jsonObject = JObject.FromObject(token);

                    if (jsonObject.ContainsKey(code))
                    {
                        // Code
                        work.Code = code;

                        JObject workObject = JObject.FromObject(jsonObject.GetValue(code));

                        // Score
                        float tempScore;

                        if (float.TryParse(workObject["rate_average_2dp"].ToString(), out tempScore))
                        {
                            work.Score = tempScore;
                        }

                        // Stars
                        work.Stars = (int)Math.Round(work.Score);

                        // Download Count
                        if (!string.IsNullOrEmpty(workObject["dl_count"].ToString()))
                        {
                            work.DownloadCount = int.Parse(workObject["dl_count"].ToString());
                        }

                        // Price
                        if (workObject["price"] != null)
                        {
                            work.Price = int.Parse(workObject["price"].ToString());
                        }

                        // PriceString
                        if (workObject["price_str"] != null)
                        {
                            work.PriceString = workObject["price_str"].ToString();
                        }
                    }
                    else
                    {
                        result = null;
                        ViewModelBroker.Instance.Main.ShowNotification(NotificationType.NotFoundError);
                        return false;
                    }
                }
                else
                {
                    result = null;
                    ViewModelBroker.Instance.Main.ShowNotification(NotificationType.NotFoundError);
                    return false;
                }

                HtmlDocument document = new HtmlDocument();
                document.LoadHtml(workPageText);

                // Thumbnail Url
                HtmlNode thumb = document.DocumentNode.SelectSingleNode(@"//meta[@property='og:image']");
                work.ThumbnailUrl = thumb?.GetAttributeValue("content", @"https://www.dlsite.com/images/web/common/logo/pc/logo-dlsite-r18.png");

                // Title
                HtmlNode title = document.DocumentNode.SelectSingleNode(@"//*[@id=""work_name""]/a");
                work.Title = title?.InnerText;

                // Circle
                HtmlNode circle = document.DocumentNode.SelectSingleNode(@"//span[@class='maker_name']/a");
                work.Circle = circle?.InnerText;

                // Description
                HtmlNode description = document.DocumentNode.SelectSingleNode(@"//h3[@class='work_parts_heading']");
                work.Description = description?.InnerText;

                // Tags
                HtmlNodeCollection genres = document.DocumentNode.SelectNodes(@"//div[@class='main_genre']/a");

                if (genres != null && genres.Count > 0)
                {
                    for (int i = 0; i < genres.Count; i++)
                    {
                        HtmlNode node = genres[i];

                        if (!string.IsNullOrEmpty(node.InnerText))
                        {
                            work.Tags.Add(node.InnerText);
                        }
                    }
                }

                // Full Information
                StringBuilder builder = new StringBuilder();

                HtmlNode maker = document.DocumentNode.SelectSingleNode(@"//table[@id=""work_maker""]/tr");
                string makerContent = GetInnerTextsFromNode(maker.SelectSingleNode(@"td"));
                makerContent = makerContent.Replace("\r", string.Empty);
                makerContent = makerContent.Replace("\n", string.Empty);
                makerContent = makerContent.Trim();
                builder.Append($"{maker.SelectSingleNode(@"th").InnerText} : {makerContent}\r\n");

                HtmlNodeCollection infos = document.DocumentNode.SelectSingleNode(@"//table[@id=""work_outline""]").ChildNodes;

                for (int i = 0; i < infos.Count; i++)
                {
                    HtmlNode info = infos[i];

                    if (info.OriginalName == "tr")
                    {
                        string name = info.SelectSingleNode(@"th").InnerText;

                        HtmlNode contents = info.SelectSingleNode(@"td");
                        string contentString = GetInnerTextsFromNode(contents);

                        contentString = contentString.Trim();

                        builder.Append($"{name} : {contentString}\r\n");
                    }
                }

                work.FullInformation = builder.ToString();

                result = work;
                return true;
            }
            catch (Exception ex)
            {
                ReportManager.Instance.AddReport($"WorkManager::Exception occured > {ex.Message}\r\n{ex.StackTrace}");

                result = null;
                ViewModelBroker.Instance.Main.ShowNotification(NotificationType.UnknownError);
                return false;
            }
        }
    }
}
