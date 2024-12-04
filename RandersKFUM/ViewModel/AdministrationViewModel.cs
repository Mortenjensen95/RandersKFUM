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
        public RelayCommand NavigateToManageTeamViewCommand { get; }
        public RelayCommand NavigateToManageTeamLeaderCommand { get; }

        public AdministrationViewModel()
        {
            NavigateToManageTeamViewCommand = new RelayCommand(_ => NavigateToManageTeamView());
            NavigateToManageTeamLeaderCommand = new RelayCommand(_ => NavigateToTeamLeaderView());
        }

        private void NavigateToManageTeamView()
        {
            RandersKFUM.Utilities.NavigationService.NavigateTo(new ManageTeamView());   
        }

        private void NavigateToTeamLeaderView()
        {
            RandersKFUM.Utilities.NavigationService.NavigateTo(new ManageTeamLeaderView());
        }
    }
}
