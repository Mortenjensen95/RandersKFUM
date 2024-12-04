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
        public static RandersKFUM.Utilities.NavigationService NavigationService { get; private set; }

        public MainWindow()
        {
            InitializeComponent();

            // Initialiser NavigationService
            NavigationService = new RandersKFUM.Utilities.NavigationService();
            NavigationService.Configure(MainFrame);

            // Registrér Views og ViewModels
            // NavigationService.Register<MainMenuViewModel, MainMenuView>();
            // NavigationService.Register<BookingOverviewViewModel, BookingOverviewView>();
            NavigationService.Register<BookingViewModel, BookingView>();
            NavigationService.Register<AdministrationViewModel, AdministrationView>();
            NavigationService.Register<ManageTeamViewModel, ManageTeamView>();
            NavigationService.Register<ManageTeamLeaderViewModel, ManageTeamLeaderView>();

            // Naviger til start View
            // NavigationService.NavigateTo<MainMenuViewModel>();
        }
    }
}