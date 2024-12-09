using RandersKFUM.Utilities;
using RandersKFUM.View;
using RandersKFUM.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RandersKFUM.ViewModel
{
    internal class MainMenuViewModel
    {

        private readonly LoginViewModel _loginViewModel;

        public RelayCommand NavigateToBookingOverviewViewCommand { get; }
        public RelayCommand NavigateToBookingViewCommand { get; }
        public RelayCommand NavigateToAdministrationViewCommand { get; }
        public RelayCommand LogOutCommand { get; }

        public MainMenuViewModel(LoginViewModel loginViewModel)
        {
            _loginViewModel = loginViewModel;

            NavigateToBookingOverviewViewCommand = new RelayCommand(_ => NavigateToBookingOverviewView());
            NavigateToBookingViewCommand = new RelayCommand(_ => NavigateToBookingView());
            NavigateToAdministrationViewCommand = new RelayCommand(_ => NavigateToAdministrationView());
            LogOutCommand = new RelayCommand(_ => _loginViewModel.LogOut(null));
        }

        private void NavigateToBookingOverviewView()
        {
            RandersKFUM.Utilities.NavigationService.NavigateTo(new BookingOverviewView());
        }

        private void NavigateToBookingView()
        {
            RandersKFUM.Utilities.NavigationService.NavigateTo(new BookingView());
        }

        private void NavigateToAdministrationView()
        {
            RandersKFUM.Utilities.NavigationService.NavigateTo(new AdministrationView());
        }

    }
}