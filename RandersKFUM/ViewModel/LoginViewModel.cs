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
        private readonly TeamLeaderRepository teamLeaderRepository;

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

        public ICommand LoginCommand { get; set; }
        public ICommand LogoutCommand { get; set; }

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

        public LoginViewModel()
        {
            teamLeaderRepository = new TeamLeaderRepository(DatabaseConfig.GetConnectionString());

            LoginCommand = new RelayCommand(Login);
            LogoutCommand = new RelayCommand(LogOut);

            IsLoggedIn = false;
        }

        public void Login(object obj)
        {
            try
            {
                var user = teamLeaderRepository.GetByUsername(UserName);

                if (user != null && user.Password == Password)
                {
                    IsLoggedIn = true;

                    RandersKFUM.Utilities.NavigationService.NavigateTo(new MainMenuView());
                }
                else
                {
                    IsLoggedIn = false;

                    MessageBox.Show("Forkert brugernavn eller adgangskode. Prøv igen.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Der opstod en uventet fejl. Prøv igen eller kontakt support.");

            }
        }

        public void LogOut(object obj)
        {
            IsLoggedIn = false;
            UserName = string.Empty;
            Password = string.Empty; 
                                     
            RandersKFUM.Utilities.NavigationService.NavigateTo(new StartView());
        }
    }

}
