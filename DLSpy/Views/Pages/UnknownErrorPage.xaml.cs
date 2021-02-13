using DLSpy.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace DLSpy.Views.Pages
{
    /// <summary>
    /// ErrorPage.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class UnknownErrorPage : Page
    {
        public UnknownErrorPage()
        {
            InitializeComponent();
            this.DataContext = ReportManager.Instance;
        }
    }
}
