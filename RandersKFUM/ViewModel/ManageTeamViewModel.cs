using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RandersKFUM.Utilities;
using RandersKFUM.Model;
using RandersKFUM.Repository;
using RandersKFUM.View;
using System.Windows;

namespace RandersKFUM.ViewModel
{
    internal class ManageTeamViewModel : ViewModelBase
    {
        public ObservableCollection<Team> Teams { get; set; }
        public ObservableCollection<TeamLeader> TeamLeaders { get; set; }
        private readonly TeamRepository teamRepository;
        private readonly TeamLeaderRepository teamLeaderRepository;

        public RelayCommand CreateTeamCommand => new RelayCommand(execute => CreateTeam());
        public RelayCommand DeleteTeamCommand => new RelayCommand(execute => DeleteTeam(), canExecute => SelectedItem != null);
        public RelayCommand SaveChangesCommand => new RelayCommand(execute => SaveChanges(), canExecute => SelectedItem != null);
        public RelayCommand NavigateBackToAdministrationViewCommand => new RelayCommand(execute => NavigateBackToAdministrationView());


        public ManageTeamViewModel()
        {
            string connectionString = DatabaseConfig.GetConnectionString();
            teamRepository = new TeamRepository(connectionString);
            teamLeaderRepository = new TeamLeaderRepository(connectionString);
            Teams = new ObservableCollection<Team>(teamRepository.GetAll());
            TeamLeaders = new ObservableCollection<TeamLeader>(teamLeaderRepository.GetAll());
        }



        private Team selectedItem;
        public Team SelectedItem
        {
            get { return selectedItem; }
            set
            {
                selectedItem = value;
                OnPropertyChanged();
            }
        }

        

        private void CreateTeam()
        {
            // Opret et net team med standardværdier (vi har har endnu ikke gemt til databasen
            var newTeam = new Team
            {
                TeamName = "New Team",
                TeamType = "Default Type",
                TeamLeaderId = TeamLeaders.FirstOrDefault()?.TeamLeaderId ?? 0 // Eksempeldata
            };

            Teams.Add(newTeam);

            // sæt som valgt, så brugeren kan redigere
            SelectedItem = newTeam;
        }



        private void DeleteTeam()
        {
            if (SelectedItem == null) return;

            teamRepository.Delete(SelectedItem.TeamId); // Slet fra databasen
            Teams.Remove(SelectedItem);                 // Fjern fra ObservableCollection
        }

        private void SaveChanges()
        {
            if (SelectedItem == null) return;

            try
            {
                // Tjek, om det er en ny TeamLeader (hvis ID ikke er sat endnu, eller det er en defaultværdi som 0)
                if (SelectedItem.TeamId == 0)
                {
                    // Opret ny TeamLeader i databasen
                    teamRepository.Add(SelectedItem);
                }
                else
                {
                    // Opdater eksisterende TeamLeader
                    teamRepository.Update(SelectedItem);
                }
                var updatedTeams = teamRepository.GetAll();

                Teams.Clear();
                foreach (var team in updatedTeams)
                {
                    // Match TeamLeaderId til en TeamLeader fra TeamLeaders
                    team.TeamLeader = TeamLeaders.FirstOrDefault(tl => tl.TeamLeaderId == team.TeamLeaderId);
                    Teams.Add(team);
                }

                MessageBox.Show("Ændringer gemt!", "Succes", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                // Tjek, om fejlen er relateret til unikke brugernavne
                if (ex.Message.Contains("UNIQUE constraint failed") || ex.Message.Contains("Duplicate entry"))
                {
                    MessageBox.Show("Holdnavnet er allerede i brug. Vælg venligst et andet.", "Fejl", MessageBoxButton.OK, MessageBoxImage.Error);
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
