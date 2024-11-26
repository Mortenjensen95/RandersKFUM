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

namespace RandersKFUM.View
{
    /// <summary>
    /// Interaction logic for BookingOverviewView.xaml
    /// </summary>
    public partial class BookingOverviewView : Page
    {
        public BookingOverviewView(Frame navigationFrame)
        {
            InitializeComponent();
            DataContext = new ViewModels.BookingOverviewViewModel(navigationFrame);
        }
    }
}
