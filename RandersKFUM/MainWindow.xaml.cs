using RandersKFUM.View;
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

            // Initialiser ViewModel og giv Frame til navigation
            var viewModel = new ViewModels.MainMenuViewModel(MainFrame);

            // Naviger til hovedmenu
            MainFrame.Navigate(new MainMenuView(MainFrame));

            // Sæt DataContext til ViewModel
            DataContext = viewModel;
        }
    }
}