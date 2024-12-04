using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RandersKFUM.Utilities;
using RandersKFUM.View;

namespace RandersKFUM.ViewModel
{
    internal class BookingOverviewViewModel
    {
        public RelayCommand NavigateBackToMainMenuCommand { get; }

        public BookingOverviewViewModel() 
        {
            NavigateBackToMainMenuCommand = new RelayCommand(_ => NavigateBackToMainMenuView());
        }

        private void NavigateBackToMainMenuView()
        {
            RandersKFUM.Utilities.NavigationService.NavigateTo(new MainMenuView());
        }

    }
}
