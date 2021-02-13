using DLSpy.Bases;
using DLSpy.Entities;
using DLSpy.Utilities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DLSpy.ViewModels
{
    public class ReportManager : ViewModelBase
    {
        private static ReportManager _instance = null;

        public static ReportManager Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new ReportManager();
                }

                return _instance;
            }
            set
            {
                _instance = value;
            }
        }

        private StringBuilder _builder = new StringBuilder();

        public string Report
        {
            get
            {
                return _builder.ToString();
            }
        }

        private string _latestSavePath = string.Empty;

        public string LatestSavePath
        {
            get
            {
                return _latestSavePath;
            }
            set
            {
                _latestSavePath = value;
                RaisePropertyChanged();
            }
        }

        public void AddReport(string text, ReportType type = ReportType.Information)
        {
            _builder.AppendLine($"{DateTime.Now.ToString("HH:mm:ss.fff")} - [{type}] {text}");
        }

        public void ResetReport()
        {
            _builder.Clear();
        }

        public void ExportReport()
        {
            string savePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "report", $"Report {DateTime.Now.ToString("yyyy-MM-dd HH-mm-ss-fff")}.txt");
            string report = _builder.ToString();

            if (!Directory.Exists(Path.GetDirectoryName(savePath)))
            {
                Directory.CreateDirectory(Path.GetDirectoryName(savePath));
            }

            FileManager.WriteTextFile(savePath, report, Encoding.UTF8);

            LatestSavePath = savePath;
        }
    }
}
