using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace RandersKFUM.Utilities
{
    public static class NavigationService
    {
        public static Frame MainFrame { get; set; }

        public static void NavigateTo(Page page)
        {
            MainFrame?.Navigate(page);
        }
    }
}
