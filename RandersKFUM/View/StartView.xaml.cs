using RandersKFUM.Repository;
using RandersKFUM.ViewModel;
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
    /// Interaction logic for StartView.xaml
    /// </summary>
    public partial class StartView : Page
    {
        public StartView()
        {
            InitializeComponent();

            // Hent connection string
            var connectionString = DatabaseConfig.GetConnectionString();

            // Opret repositories med connection string
            var fieldRepo = new FieldRepository(connectionString);
            var lockerRoomRepo = new LockerRoomRepository(connectionString);
            var bookingRepo = new BookingRepository(connectionString);
            var teamRepo = new TeamRepository(connectionString);

            // Initialiser ViewModel med repositories
            DataContext = new StartViewModel(fieldRepo, lockerRoomRepo, bookingRepo, teamRepo);
        }
    }
}
