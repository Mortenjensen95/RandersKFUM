using System.ComponentModel;
using System.Windows.Controls;
using System.Windows.Input;

namespace RandersKFUM.ViewModels
{
    public class MainMenuViewModel : INotifyPropertyChanged
    {
        private bool _isLoggedIn;
        private bool _isAdmin;
        private string _loggedInUser;
        private readonly Frame _navigationFrame; // Frame til navigation

        public MainMenuViewModel(Frame navigationFrame)
        {
            // Initialtilstand
            IsLoggedIn = false;
            IsAdmin = false;
            LoggedInUser = string.Empty;

            // NavigationFrame initialiseres
            _navigationFrame = navigationFrame;

            // Kommandoer
            NavigateToBookingOverviewCommand = new RelayCommand(NavigateToBookingOverview);
            NavigateToBookingCommand = new RelayCommand(NavigateToBooking);
            NavigateToAdministrationCommand = new RelayCommand(NavigateToAdministration, CanAccessAdministration);
            LoginLogoutCommand = new RelayCommand(LoginLogout);
        }

        // Egenskab: Loginstatus
        public bool IsLoggedIn
        {
            get => _isLoggedIn;
            set
            {
                _isLoggedIn = value;
                OnPropertyChanged(nameof(IsLoggedIn));
                OnPropertyChanged(nameof(LoginButtonText));
                OnPropertyChanged(nameof(IsAdminVisible));
                OnPropertyChanged(nameof(LoggedInUserDisplay));
            }
        }

        // Egenskab: Administratorstatus
        public bool IsAdmin
        {
            get => _isAdmin;
            set
            {
                _isAdmin = value;
                OnPropertyChanged(nameof(IsAdmin));
                OnPropertyChanged(nameof(IsAdminVisible));
            }
        }

        // Egenskab: Den aktuelt loggede bruger
        public string LoggedInUser
        {
            get => _loggedInUser;
            set
            {
                _loggedInUser = value;
                OnPropertyChanged(nameof(LoggedInUser));
                OnPropertyChanged(nameof(LoggedInUserDisplay));
            }
        }

        // Dynamisk tekst for Login/Logout-knappen
        public string LoginButtonText => IsLoggedIn ? "Log ud" : "Log ind";

        // Tekst til at vise den aktuelle bruger
        public string LoggedInUserDisplay => IsLoggedIn ? $"Logget ind som: {LoggedInUser}" : "Ingen bruger logget ind";

        // Synlighed for Administration-knappen
        public bool IsAdminVisible => IsLoggedIn && IsAdmin;

        // Kommandoer
        public ICommand NavigateToBookingOverviewCommand { get; }
        public ICommand NavigateToBookingCommand { get; }
        public ICommand NavigateToAdministrationCommand { get; }
        public ICommand LoginLogoutCommand { get; }

        // Naviger til Bookingoversigt
        private void NavigateToBookingOverview()
        {
            _navigationFrame.Navigate(new Views.BookingOverviewView());
        }

        // Naviger til Opret Booking
        private void NavigateToBooking()
        {
            _navigationFrame.Navigate(new Views.BookingView());
        }

        // Naviger til Administration
        private void NavigateToAdministration()
        {
            _navigationFrame.Navigate(new Views.AdministrationView());
        }

        // Bestem om Administration kan tilgås
        private bool CanAccessAdministration()
        {
            return IsAdminVisible;
        }

        // Login/Logout-funktionalitet
        private void LoginLogout()
        {
            if (IsLoggedIn)
            {
                // Log ud logik
                IsLoggedIn = false;
                IsAdmin = false;
                LoggedInUser = string.Empty;
            }
            else
            {
                // Log ind logik (til demonstration)
                LoggedInUser = "Admin"; // Simulerer en bruger
                IsLoggedIn = true;

                // Bestem, om brugeren er en administrator
                IsAdmin = DetermineIfUserIsAdmin(LoggedInUser);
            }
        }

        // Simuleret metode til at afgøre, om en bruger er administrator
        private bool DetermineIfUserIsAdmin(string username)
        {
            // Simuler en liste af administratorer
            var administrators = new[] { "Admin", "SuperUser" };
            return administrators.Contains(username);
        }

        // Event for property changes
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
