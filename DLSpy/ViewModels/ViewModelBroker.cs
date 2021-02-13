using DLSpy.Bases;
using DLSpy.ViewModels.Pages;
using DLSpy.ViewModels.Windows;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DLSpy.ViewModels
{
    public class ViewModelBroker : ViewModelBase
    {
        public static ViewModelBroker _instance = null;

        public static ViewModelBroker Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new ViewModelBroker();
                }

                return _instance;
            }
            set
            {
                _instance = value;
            }
        }

        public MainViewModel Main { get; set; }

        public ViewModelBroker()
        {
            Main = new MainViewModel();
        }
    }
}
