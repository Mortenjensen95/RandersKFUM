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
        public RelayCommand NavigateToBookingOverviewViewCommand { get; }
        public RelayCommand NavigateToBookingViewCommand { get; }
        public RelayCommand NavigateToAdministrationViewCommand { get; }
        public RelayCommand NavigateToLoginViewCommand { get; }

        public MainMenuViewModel()
        {
            NavigateToBookingOverviewViewCommand = new RelayCommand(_ => NavigateToBookingOverviewView());
            NavigateToBookingViewCommand = new RelayCommand(_ => NavigateToBookingView());
            NavigateToAdministrationViewCommand = new RelayCommand(_ => NavigateToAdministrationView());
            // NavigateToLoginViewCommand = new RelayCommand(_ => NavigateToLoginView());
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

        /*private void NavigateToLoginView()
        {
            RandersKFUM.Utilities.NavigationService.NavigateTo(new LoginView());
        }
        */
    }
}