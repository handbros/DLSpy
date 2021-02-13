using DLSpy.Bases;
using DLSpy.Commands;
using DLSpy.Dialogs;
using DLSpy.Entities;
using DLSpy.Utilities;
using DLSpy.Views.Pages;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace DLSpy.ViewModels.Windows
{
    public class MainViewModel : ViewModelBase
    {
        #region ::Fields & Properties::

        private string _searchString = string.Empty;

        public string SearchString
        {
            get
            {
                return _searchString;
            }
            set
            {
                _searchString = value;
                RaisePropertyChanged();
            }
        }

        private bool _dialogIsOpen = false;

        public bool DialogIsOpen
        {
            get
            {
                return _dialogIsOpen;
            }
            set
            {
                _dialogIsOpen = value;
                RaisePropertyChanged();
            }
        }

        public object _dialogContent = null;

        public object DialogContent
        {
            get
            {
                return _dialogContent;
            }
            set
            {
                _dialogContent = value;
                RaisePropertyChanged();
            }
        }

        private Page _frameContent = new Page();

        public Page FrameContent
        {
            get
            {
                return _frameContent;
            }
            set
            {
                _frameContent = value;
                RaisePropertyChanged();
            }
        }

        // Pages & Controls
        private NotFoundErrorPage _notFoundErrorPage = new NotFoundErrorPage();

        private UnknownErrorPage _unknownErrorPage = new UnknownErrorPage();

        private WeakReference _workInformation;

        public WorkInformation WorkInformation
        {
            get
            {
                return _workInformation.Target as WorkInformation;
            }
            set
            {
                _workInformation = new WeakReference(value);
                RaisePropertyChanged();
            }
        }

        // Commands
        private DelegateCommand _searchCommand;

        public DelegateCommand SearchCommand
        {
            get
            {
                return (_searchCommand) ?? (_searchCommand = new DelegateCommand(Search));
            }
        }

        private DelegateCommand _closeDialogCommand;

        public DelegateCommand CloseDialogCommand
        {
            get
            {
                return (_closeDialogCommand) ?? (_closeDialogCommand = new DelegateCommand(CloseDialog));
            }
        }

        private DelegateCommand _showWorkPageCommand;

        public DelegateCommand ShowWorkPageCommand
        {
            get
            {
                return (_showWorkPageCommand) ?? (_showWorkPageCommand = new DelegateCommand(ShowWorkPage));
            }
        }

        private DelegateCommand _purchaseCommand;

        public DelegateCommand PurchaseCommand
        {
            get
            {
                return (_purchaseCommand) ?? (_purchaseCommand = new DelegateCommand(Purchase));
            }
        }

        #endregion

        #region ::Constructors::

        public MainViewModel()
        {
            WorkInformation = new WorkInformation();
        }

        #endregion

        #region ::Methods::

        public async void Search()
        {
            string code = string.Empty;

            if (CodeManager.GetCode(_searchString, out code)) // FLAG : Is the search string valid RJ/BJ/VJ code format?
            {
                var task = new Task<WorkInformation>(() =>
                {
                    WorkInformation work = new WorkInformation();

                    if (WorkManager.GetWork(code, out work))
                    {
                        return work;
                    }
                    else
                    {
                        return null;
                    }
                });
                task.Start();

                if (WorkInformation != null)
                {
                    WorkInformation.Dispose();
                }

                WorkInformation = await task; // Await async task.

                if (WorkInformation != null)
                {
                    ShowDialog();
                    SearchString = string.Empty;
                }
            }
            else // ELSE : Search the input string as keyword.
            {
                FrameContent = new SearchResultsPage(SearchString);
            }
        }

        public async void SearchByCode(string text)
        {
            var task = new Task<WorkInformation>(() =>
            {
                WorkInformation work = new WorkInformation();

                if (WorkManager.GetWork(text, out work))
                {
                    return work;
                }
                else
                {
                    return null;
                }
            });
            task.Start();

            if (WorkInformation != null)
            {
                WorkInformation.Dispose();
            }

            WorkInformation = await task; // Await async task.

            if (WorkInformation != null)
            {
                ShowDialog();
                SearchString = string.Empty;
            }
        }

        public void ShowDialog()
        {
            if (!DialogIsOpen)
            {
                DialogIsOpen = true;
            }
        }

        public void CloseDialog()
        {
            if (DialogIsOpen)
            {
                DialogIsOpen = false;
            }
        }

        public void ShowNotification(NotificationType type)
        {
            if (type == NotificationType.NotFoundError)
            {
                FrameContent = _notFoundErrorPage;
            }
            else if (type == NotificationType.UnknownError)
            {
                ReportManager.Instance.ExportReport();
                FrameContent = _unknownErrorPage;
            }
        }

        private void ShowWorkPage()
        {
            string url = string.Format(Settings.URL_WORK_ALL, WorkInformation.Code);
            HttpUtility.OpenUrl(url);
        }

        private void Purchase()
        {
            string url = string.Format(Settings.URL_WORK_PURCHASE_ALL, WorkInformation.Code);
            HttpUtility.OpenUrl(url);
        }

        #endregion
    }
}
