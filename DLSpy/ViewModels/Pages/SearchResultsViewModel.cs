using DLSpy.Bases;
using DLSpy.Entities;
using DLSpy.Utilities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DLSpy.ViewModels.Pages
{
    public class SearchResultsViewModel : ViewModelBase, IDisposable
    {
        private string _text = string.Empty;

        public string Text
        {
            get
            {
                return _text;
            }
            set
            {
                _text = value;
                RaisePropertyChanged();
            }
        }

        private string _caption = string.Empty;

        public string Caption
        {
            get
            {
                return _caption;
            }
            set
            {
                _caption = value;
                RaisePropertyChanged();
            }
        }

        private int _itemsCount = 0;

        public int ItemsCount
        {
            get
            {
                return _itemsCount;
            }
            set
            {
                _itemsCount = value;
                RaisePropertyChanged();
            }
        }

        private int _pagesCount = 0;

        public int PagesCount
        {
            get
            {
                return _pagesCount;
            }
            set
            {
                _pagesCount = value;
                RaisePropertyChanged();
            }
        }

        private int _currentPage = 1;

        public int CurrentPage
        {
            get
            {
                return _currentPage;
            }
            set
            {
                _currentPage = value;
                RaisePropertyChanged();

                if (_currentPage > 0 && _currentPage <= _pagesCount)
                {
                    Search(_currentPage);
                }
            }
        }

        private bool _scrollToTop = false;

        public bool ScrollToTop
        {
            get
            {
                return _scrollToTop;
            }
            set
            {
                _scrollToTop = value;
                RaisePropertyChanged();
            }
        }

        private List<SearchResult> _results = new List<SearchResult>();

        public List<SearchResult> Results
        {
            get
            {
                return _results;
            }
            set
            {
                _results = value;
                RaisePropertyChanged();
            }
        }

        public SearchResultsViewModel(string text)
        {
            _text = text;
            _caption = $"'{text}'에 대한 검색 결과";
            InitializePage();
            Search(1);
        }

        private async void InitializePage()
        {
            ReportManager.Instance.AddReport($"SearchResultsViewModel::Run > InitializePage()");

            var task = new Task<int>(() =>
            {
                return SearchManager.GetItemsCount(_text);
            });
            task.Start();

            ItemsCount = await task;
            PagesCount = (ItemsCount / 30) + 1;
        }

        private async void Search(int page)
        {
            ReportManager.Instance.AddReport($"SearchResultsViewModel::Run > Search({page})");

            var task = new Task<List<SearchResult>>(() =>
            {
                return SearchManager.GetSearchResult(_text, (uint)page);
            });
            task.Start();

            ScrollToTop = true;

            Results = await task;
        }

        private bool disposedValue;

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: 관리형 상태(관리형 개체)를 삭제합니다.
                    Text = null;
                    Caption = null;
                    _itemsCount = 0;
                    _pagesCount = 0;
                    _currentPage = 0;
                    _results.ForEach((obj) => obj.Dispose());
                    _results = null;
                }

                // TODO: 비관리형 리소스(비관리형 개체)를 해제하고 종료자를 재정의합니다.
                // TODO: 큰 필드를 null로 설정합니다.
                disposedValue = true;
            }
        }

        // // TODO: 비관리형 리소스를 해제하는 코드가 'Dispose(bool disposing)'에 포함된 경우에만 종료자를 재정의합니다.
        // ~SearchResultsViewModel()
        // {
        //     // 이 코드를 변경하지 마세요. 'Dispose(bool disposing)' 메서드에 정리 코드를 입력합니다.
        //     Dispose(disposing: false);
        // }

        public void Dispose()
        {
            // 이 코드를 변경하지 마세요. 'Dispose(bool disposing)' 메서드에 정리 코드를 입력합니다.
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }
}
