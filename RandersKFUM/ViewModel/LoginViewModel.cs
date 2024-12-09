using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using RandersKFUM.Utilities;
using RandersKFUM.View;
using RandersKFUM.Repository;

namespace RandersKFUM.ViewModel
{
    public class LoginViewModel : ViewModelBase
    {
        // Privat felt til repository, der håndterer brugere i databasen
        private readonly TeamLeaderRepository teamLeaderRepository;

        // Brugernavn og adgangskode properties, der binder til UI'et
        private string userName;
        public string UserName
        {
            get { return userName; }
            set
            {
                userName = value;
                OnPropertyChanged(nameof(UserName));
            }
        }

        private string password;
        public string Password
        {
            get { return password; }
            set
            {
                password = value;
                OnPropertyChanged(nameof(Password));
            }
        }

        // Kommandoer til login og logout
        public ICommand LoginCommand { get; set; }
        public ICommand LogoutCommand { get; set; }

        // Property til at holde styr på, om brugeren er logget ind eller ej
        private bool isLoggedIn;
        public bool IsLoggedIn
        {
            get { return isLoggedIn; }
            set
            {
                isLoggedIn = value;
                OnPropertyChanged(nameof(IsLoggedIn));
            }
        }

        // Constructor
        public LoginViewModel()
        {
            teamLeaderRepository = new TeamLeaderRepository(DatabaseConfig.GetConnectionString());

            LoginCommand = new RelayCommand(Login);
            LogoutCommand = new RelayCommand(Logout);

            IsLoggedIn = false; // Standard: brugeren er ikke logget ind
        }

        // Login-metode
        private void Login(object obj)
        {
            var user = teamLeaderRepository.GetByUsername(UserName); // Hent brugeren fra databasen baseret på brugernavn

            if (user != null && user.Password == Password) // Tjek om brugeren eksisterer, og om adgangskoden matcher
            {
                IsLoggedIn = true; // Brugeren er logget ind

                // Opret og vis AdministrationView vinduet
                RandersKFUM.Utilities.NavigationService.NavigateTo(new MainMenuView());
            }
            else
            {
                IsLoggedIn = false; // Fejl: Forkert brugernavn eller adgangskode

                // Eventuelt vis en fejlmeddelelse til brugeren
                MessageBox.Show("Forkert brugernavn eller adgangskode. Prøv igen.");
            }
        }


        // Logout-metode
        public void Logout(object obj)
        {
            IsLoggedIn = false; // Brugeren er logget ud
            UserName = string.Empty; // Ryd brugernavn
            Password = string.Empty; // Ryd adgangskode
                                     // Eventuel navigering til login-skærm eller opdatering af UI
        }
    }

}
