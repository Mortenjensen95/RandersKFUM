using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RandersKFUM.Utilities;
using RandersKFUM.Model;
using RandersKFUM.Repository;

namespace RandersKFUM.ViewModel
{
    internal class ManageTeamViewModel : ViewModelBase
    {
        public ObservableCollection<Team> Teams { get; set; }
        private readonly TeamRepository teamRepository;
        private readonly TeamLeaderRepository teamLeaderRepository;

        public RelayCommand CreateTeamCommand => new RelayCommand(execute => CreateTeam());
        public RelayCommand DeleteTeamCommand => new RelayCommand(execute => DeleteTeam(), canExecute => SelectedItem != null);
        public RelayCommand SaveChangesCommand => new RelayCommand(execute => SaveChanges(), canExecute => SelectedItem != null);

        public ManageTeamViewModel()
        {
            string connectionString = DatabaseConfig.GetConnectionString();
            teamRepository = new TeamRepository(connectionString);
            teamLeaderRepository = new TeamLeaderRepository(connectionString);
            Teams = new ObservableCollection<Team>();
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
            var newTeam = new Team
            {
                TeamName = "New Team", // Eksempeldata, justér efter behov
                TeamType = "Default Type",
                TeamLeaderId = 0
            };

            teamRepository.Add(newTeam); // Gem i databasen
            Teams.Add(newTeam);           // Opdater ObservableCollection
        }

        private void DeleteTeam()
        {
            teamRepository.Delete(SelectedItem.TeamId); // Slet fra databasen
            Teams.Remove(SelectedItem);                 // Fjern fra ObservableCollection
        }

        private void SaveChanges()
        {
            teamRepository.Update(SelectedItem); // Gem ændringer i databasen
        }


    }
}
