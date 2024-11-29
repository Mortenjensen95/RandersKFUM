using System;
using System.Windows.Controls;

namespace RandersKFUM.Utilities
{
    public class NavigationService
    {
        private readonly Frame _mainFrame;

        public NavigationService(Frame mainFrame)
        {
            _mainFrame = mainFrame ?? throw new ArgumentNullException(nameof(mainFrame));
        }

        /// <summary>
        /// Navigates to the specified page.
        /// </summary>
        /// <param name="page">The page to navigate to.</param>
        public void NavigateTo(Page page)
        {
            if (page == null) throw new ArgumentNullException(nameof(page));
            _mainFrame.Navigate(page);
        }

        /// <summary>
        /// Navigates back to the previous page in the navigation history.
        /// </summary>
        public void GoBack()
        {
            if (_mainFrame.CanGoBack)
            {
                _mainFrame.GoBack();
            }
        }

        /// <summary>
        /// Checks if navigation can go back.
        /// </summary>
        public bool CanGoBack => _mainFrame.CanGoBack;
    }
}
