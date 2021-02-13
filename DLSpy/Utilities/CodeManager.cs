using DLSpy.ViewModels;
using System.Text.RegularExpressions;

namespace DLSpy.Utilities
{
    /// <summary>
    /// DLSite 제품 코드와 관련된 도구를 제공합니다.
    /// </summary>
    public static class CodeManager
    {
        public static bool GetCode(string text, out string result)
        {
            ReportManager.Instance.AddReport($"CodeManager::Run > GetCode({text})");

            text = text.ToUpper();

            if (Regex.IsMatch(text, Settings.REGEX_MATCH_RJ))
            {
                Match match = Regex.Match(text, Settings.REGEX_MATCH_RJ);
                result = match.Value;
                return true;
            }
            else if (Regex.IsMatch(text, Settings.REGEX_MATCH_BJ))
            {
                Match match = Regex.Match(text, Settings.REGEX_MATCH_BJ);
                result = match.Value;
                return true;
            }
            else if (Regex.IsMatch(text, Settings.REGEX_MATCH_VJ))
            {
                Match match = Regex.Match(text, Settings.REGEX_MATCH_VJ);
                result = match.Value;
                return true;
            }
            else
            {
                ReportManager.Instance.AddReport($"CodeManager::Exception occured > Invalid code format.");
                result = null;
                return false;
            }
        }
    }
}
