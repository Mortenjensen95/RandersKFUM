using System;
using System.Windows.Input;
using RandersKFUM.View;

namespace RandersKFUM.ViewModels
{
    public class AdministrationViewModel
    {
        private readonly Action _navigateBackToMenu;

        public AdministrationViewModel(Action navigateBackToMenu)
        {
            _navigateBackToMenu = navigateBackToMenu;

            // Initialiser kommandoer
            OpenManageTeamWindowCommand = new RelayCommand(OpenManageTeamWindow);
            OpenManageTeamLeaderWindowCommand = new RelayCommand(OpenManageTeamLeaderWindow);
            NavigateBackToMenuCommand = new RelayCommand(NavigateBackToMenu);
        }

        // Kommandoer
        public ICommand OpenManageTeamWindowCommand { get; }
        public ICommand OpenManageTeamLeaderWindowCommand { get; }
        public ICommand NavigateBackToMenuCommand { get; }

        // Metoder
        private void OpenManageTeamWindow()
        {
            var manageTeamWindow = new ManageTeamWindow(); // Instantiér vinduet
            manageTeamWindow.Show(); // Åbn vinduet
        }

        private void OpenManageTeamLeaderWindow()
        {
            var manageTeamLeaderWindow = new ManageTeamLeaderWindow(); // Instantiér vinduet
            manageTeamLeaderWindow.Show(); // Åbn vinduet
        }

        private void NavigateBackToMenu()
        {
            _navigateBackToMenu?.Invoke();
        }
    }
}
