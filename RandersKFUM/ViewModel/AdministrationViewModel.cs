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
        private readonly NavigationService _navigationService;

        public ICommand OpenManageTeamViewCommand { get; }
        public ICommand OpenManageTeamLeaderViewCommand { get; }
        public ICommand NavigateBackCommand { get; }

        /// <summary>
        /// Constructor for AdministrationViewModel.
        /// </summary>
        /// <param name="navigationService">An instance of NavigationService to handle navigation.</param>
        public AdministrationViewModel(NavigationService navigationService)
        {
            _navigationService = navigationService;

            // Commands initialization
            OpenManageTeamViewCommand = new RelayCommand(OpenManageTeamView);
            OpenManageTeamLeaderViewCommand = new RelayCommand(OpenManageTeamLeaderView);
            NavigateBackCommand = new RelayCommand(GoBack);
        }

        /// <summary>
        /// Navigate to the ManageTeamView page.
        /// </summary>
        private void OpenManageTeamView()
        {
            _navigationService.NavigateTo(new ManageTeamView());
        }

        /// <summary>
        /// Navigate to the ManageTeamLeaderView page.
        /// </summary>
        private void OpenManageTeamLeaderView()
        {
            _navigationService.NavigateTo(new ManageTeamLeaderView());
        }

        /// <summary>
        /// Navigate back to the previous page.
        /// </summary>
        private void GoBack()
        {
            _navigationService.GoBack();
        }
    }
}
