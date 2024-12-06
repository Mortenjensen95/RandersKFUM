using RandersKFUM.Model;
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
    /// Interaction logic for EditBookingView.xaml
    /// </summary>
    public partial class EditBookingView : Page
    {
        public EditBookingView(Booking booking)
        {
            try
            {
                InitializeComponent();

                // Hent connection string fra DatabaseConfig
                var connectionString = DatabaseConfig.GetConnectionString();

                // Sæt DataContext med en EditBookingViewModel, der modtager repositories
                DataContext = new EditBookingViewModel(
                    booking,
                    new FieldRepository(connectionString),
                    new LockerRoomRepository(connectionString),
                    new BookingRepository(connectionString),
                    new TeamRepository(connectionString)
                );
            }
            catch (Exception ex)
            {
                // Håndter eventuelle fejl under initialisering
                MessageBox.Show($"En fejl opstod under initialisering: {ex.Message}", "Fejl", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
