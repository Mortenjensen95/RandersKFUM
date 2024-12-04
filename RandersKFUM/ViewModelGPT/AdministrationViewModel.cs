using System;
using System.Windows.Input;
using RandersKFUM.View;
using RandersKFUM.Utilities;


namespace RandersKFUM.ViewModels
{
    public class AdministrationViewModel
    {
        private readonly Action _navigateBackToMenu;

        public AdministrationViewModel(Action navigateBackToMenu)
        {
            _navigateBackToMenu = navigateBackToMenu;

            // Initialiser kommandoer
            OpenManageTeamViewCommand = new RelayCommand(OpenManageTeamView);
            OpenManageTeamLeaderViewCommand = new RelayCommand(OpenManageTeamLeaderView);
            NavigateBackToMenuCommand = new RelayCommand(NavigateBackToMenu);
        }

        // Kommandoer
        public ICommand OpenManageTeamViewCommand { get; }
        public ICommand OpenManageTeamLeaderViewCommand { get; }
        public ICommand NavigateBackToMenuCommand { get; }

        // Metoder
        private void OpenManageTeamView()
        {
            var manageTeamView = new ManageTeamView(); // Instantiér vinduet
            manageTeamView.Show(); // Åbn vinduet
        }

        private void OpenManageTeamLeaderView()
        {
            var manageTeamLeaderView = new ManageTeamLeaderView(); // Instantiér vinduet
            manageTeamLeaderView.Show(); // Åbn vinduet
        }

        private void NavigateBackToMenu()
        {
            _navigateBackToMenu?.Invoke();
        }
    }
}
