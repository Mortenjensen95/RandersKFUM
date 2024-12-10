using RandersKFUM.View;
using RandersKFUM.ViewModel;
using RandersKFUM.Utilities;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace RandersKFUM
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            // Initialiser NavigationService.MainFrame
            RandersKFUM.Utilities.NavigationService.MainFrame = MainFrame;

            // Naviger til MainMenuView
            MainFrame.Navigate(new MainMenuView());
        }

    }
}