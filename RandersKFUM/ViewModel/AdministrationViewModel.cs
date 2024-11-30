using RandersKFUM.Utilities;
using RandersKFUM.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace RandersKFUM.ViewModel
{
    public class AdministrationViewModel
    {
        public ICommand OpenManageTeamViewCommand { get; }
        public ICommand OpenManageTeamLeaderViewCommand { get; }

        public AdministrationViewModel()
        {
            // Brug en parameter, selvom den ikke er nødvendig (kan være null), der skal være en parameter ellers brokker relaycommand sig
            OpenManageTeamViewCommand = new RelayCommand(
                parameter => MainWindow.NavigationService.NavigateTo<ManageTeamViewModel>(),
                parameter => true // Kan udføres altid
            );

            OpenManageTeamLeaderViewCommand = new RelayCommand(
                parameter => MainWindow.NavigationService.NavigateTo<ManageTeamLeaderViewModel>(),
                parameter => true // Kan udføres altid
            );
        }

    }
}
