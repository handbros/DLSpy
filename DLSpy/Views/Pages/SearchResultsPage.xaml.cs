using DLSpy.ViewModels;
using DLSpy.ViewModels.Pages;
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
    /// SearchResultsPage.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class SearchResultsPage : Page
    {
        SearchResultsViewModel viewModel;

        public SearchResultsPage(string text)
        {
            InitializeComponent();
            viewModel = new SearchResultsViewModel(text);
            this.DataContext = viewModel;
            this.Unloaded += OnUnloaded;

            //Dispatcher.ShutdownStarted += OnDispatcherShutdownStarted;
        }

        private void OnUnloaded(object sender, RoutedEventArgs e)
        {
            this.DataContext = null;
            viewModel.Dispose();
        }

        private void OnDispatcherShutdownStarted(object sender, EventArgs e)
        {
            var disposable = DataContext as IDisposable;

            if (!ReferenceEquals(null, disposable))
            {
                disposable.Dispose();
            }
        }

    }
}
