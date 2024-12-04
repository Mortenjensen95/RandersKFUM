using RandersKFUM.Repository;
using RandersKFUM.Utilities;
using RandersKFUM.ViewModel;
using RandersKFUM.Model;
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
        public BookingView()
        {
            InitializeComponent();
            DataContext = new BookingViewModel(new FieldRepository(DatabaseConfig.GetConnectionString()), new LockerRoomRepository(DatabaseConfig.GetConnectionString()), new BookingRepository(DatabaseConfig.GetConnectionString()), new TeamRepository(DatabaseConfig.GetConnectionString()));
        }
    }
}
