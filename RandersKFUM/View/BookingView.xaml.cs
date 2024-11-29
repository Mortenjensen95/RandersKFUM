using RandersKFUM.Repository;
using RandersKFUM.Utilities;
using RandersKFUM.ViewModels;
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
    /// Interaction logic for BookingView.xaml
    /// </summary>
    public partial class BookingView : Page
    {
        public BookingView(Frame navigationFrame)
        {
            InitializeComponent();

            // Brug DatabaseConfig til at hente connection string
            string connectionString = DatabaseConfig.GetConnectionString();

            // Initialiser ViewModel med repositories
            DataContext = new BookingViewModel(
                new BookingRepository(connectionString),
                new FieldRepository(connectionString),
                new LockerRoomRepository(connectionString),
                () => navigationFrame.Navigate(new MainMenuView(navigationFrame))
            );
        }
    }
}
