﻿using System.Windows;
using System.Windows.Forms;

namespace DLSpy.Styles
{
    /// <summary>
    /// MaterialWindow에 대한 상호작용 논리를 제공합니다.
    /// </summary>
    public partial class MaterialWindow : ResourceDictionary
    {
        #region ::Fields & Properties::

        Window _window;

        #endregion

        #region ::Constructors::

        public MaterialWindow()
        {

        }

        #endregion

        #region ::Methods::

        private void LimitWindowSize()
        {
            if (GetWindow())
            {
                Screen currentScreen = Screen.FromPoint(Cursor.Position);

                _window.MaxWidth = currentScreen.WorkingArea.Width + 16;
                _window.MaxHeight = currentScreen.WorkingArea.Height + 16;
            }
        }

        private bool GetWindow()
        {
            if (_window == null || _window != null)
            {
                _window = System.Windows.Application.Current.MainWindow;
                return true;
            }
            else
                return false;
        }

        #endregion

        #region ::Event Subscribers::

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            LimitWindowSize();
        }

        private void OnMinimize(object sender, RoutedEventArgs e)
        {
            if (GetWindow())
                _window.WindowState = WindowState.Minimized;
        }

        private void OnMaximize(object sender, RoutedEventArgs e)
        {
            if (GetWindow())
            {
                if (_window.WindowState == WindowState.Maximized)
                {
                    _window.WindowState = WindowState.Normal;
                }
                else
                {
                    LimitWindowSize();
                    _window.WindowState = WindowState.Maximized;
                }
            }
        }

        private void OnClose(object sender, RoutedEventArgs e)
        {
            if (GetWindow())
                _window.Close();
        }

        #endregion
    }
}
