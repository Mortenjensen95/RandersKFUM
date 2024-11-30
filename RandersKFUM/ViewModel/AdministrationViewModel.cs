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
            OpenManageTeamLeaderViewCommand = new RelayCommand(() =>
                MainWindow.NavigationService.NavigateTo<ManageTeamViewModel>());

            OpenManageTeamLeaderViewCommand = new RelayCommand(() =>
                MainWindow.NavigationService.NavigateTo<ManageTeamLeaderViewModel>());
        }
    }
}
