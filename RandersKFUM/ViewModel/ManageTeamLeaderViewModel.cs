using System;
using System.Collections.Generic;
using System.Linq;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;
using RandersKFUM.Utilities;
using RandersKFUM.Model;
using RandersKFUM.Repository;
using System.Windows;
using RandersKFUM.View;

namespace RandersKFUM.ViewModel
{
    internal class ManageTeamLeaderViewModel : ViewModelBase
    {
        public ObservableCollection<TeamLeader> TeamLeaders { get; set; }
        private readonly TeamLeaderRepository teamLeaderRepository;

        public RelayCommand CreateTeamLeaderCommand => new RelayCommand(execute => CreateTeamLeader());
        public RelayCommand DeleteTeamLeaderCommand => new RelayCommand(execute => DeleteTeamLeader(), canExecute => SelectedItem != null);
        public RelayCommand SaveChangesCommand => new RelayCommand(execute => SaveChanges(), canExecute => SelectedItem != null);
        public RelayCommand NavigateBackToAdministrationViewCommand => new RelayCommand(execute => NavigateBackToAdministrationView());

        public ManageTeamLeaderViewModel()
        {
            string connectionString = DatabaseConfig.GetConnectionString();
            teamLeaderRepository = new TeamLeaderRepository(connectionString);
            TeamLeaders = new ObservableCollection<TeamLeader>(teamLeaderRepository.GetAll());

        }


        private TeamLeader selectedItem;

        public TeamLeader SelectedItem
        {
            get { return selectedItem; }
            set
            {
                selectedItem = value;
                OnPropertyChanged();
            }
        }

        private void CreateTeamLeader()
        {
            // Opret en ny TeamLeader med standardværdier (ikke gemt i databasen endnu)
            var newTeamLeader = new TeamLeader
            {
                Name = "",
                UserName = "",
                Password = "",
                Phone = "",
                Email = ""
            };

            // Tilføj til ObservableCollection
            TeamLeaders.Add(newTeamLeader);

            // Sæt som valgt, så brugeren kan redigere
            SelectedItem = newTeamLeader;
        }



        private void DeleteTeamLeader()
        {
            if (SelectedItem == null) return;

            try
            {
                teamLeaderRepository.Delete(SelectedItem.TeamLeaderId);
                TeamLeaders.Remove(SelectedItem);                       
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Kunne ikke slette holdleder: {ex.Message}", "Fejl", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }


        private void SaveChanges()
        {
            if (SelectedItem == null) return;

            try
            {
                // Tjek, om det er en ny TeamLeader (hvis ID ikke er sat endnu, eller det er en defaultværdi som 0)
                if (SelectedItem.TeamLeaderId == 0)
                {
                    // Opret ny TeamLeader i databasen
                    teamLeaderRepository.Add(SelectedItem);
                }
                else
                {
                    // Opdater eksisterende TeamLeader
                    teamLeaderRepository.Update(SelectedItem);
                }
                TeamLeaders = new ObservableCollection<TeamLeader>(teamLeaderRepository.GetAll());
                OnPropertyChanged(nameof(TeamLeaders));

                MessageBox.Show("Ændringer gemt!", "Succes", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                // Tjek, om fejlen er relateret til unikke brugernavne
                if (ex.Message.Contains("UNIQUE constraint failed") || ex.Message.Contains("Duplicate entry"))
                {
                    MessageBox.Show("Brugernavnet er allerede i brug. Vælg venligst et andet.", "Fejl", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                else
                {
                    // Generisk fejlbesked for andre fejl
                    MessageBox.Show($"Der opstod en fejl: {ex.Message}", "Fejl", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void NavigateBackToAdministrationView()
        {
            SelectedItem = null;
            RandersKFUM.Utilities.NavigationService.NavigateTo(new AdministrationView());
        }

    }
}
