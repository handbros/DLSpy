using DLSpy.ViewModels;
using System;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Runtime.InteropServices;

namespace DLSpy.Utilities
{
    public static class HttpUtility
    {
        public static string GetResponse(string url)
        {
            ReportManager.Instance.AddReport($"HttpUtility::Run > GetResponse({url})");

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

            return responseText;
        }

        public static void OpenUrl(string url)
        {
            try
            {
                ReportManager.Instance.AddReport($"HttpUtility::Run > OpenUrl({url})");
                Process.Start(url);
            }
            catch (Exception ex)
            {
                ReportManager.Instance.AddReport($"HttpUtility::Exception occured > {ex.Message}\r\n{ex.StackTrace}");

                // hack because of this: https://github.com/dotnet/corefx/issues/10361
                if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                {
                    url = url.Replace("&", "^&");
                    Process.Start(new ProcessStartInfo("cmd", $"/c start {url}") { CreateNoWindow = true });
                }
                else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
                {
                    Process.Start("xdg-open", url);
                }
                else if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
                {
                    Process.Start("open", url);
                }
                else
                {
                    throw;
                }
            }
        }
    }
}
