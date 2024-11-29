using System;
using System.Collections.Generic;
using System.Linq;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;
using RandersKFUM.Utilities;
using RandersKFUM.Model;
using RandersKFUM.Repository;

namespace RandersKFUM.ViewModel
{
    internal class ManageTeamLeaderViewModel : ViewModelBase
    {
        public ObservableCollection<TeamLeader> TeamLeaders { get; set; }
        private readonly TeamLeaderRepository teamLeaderRepository;

        public RelayCommand CreateTeamLeaderCommand => new RelayCommand(execute => CreateTeamLeader());
        public RelayCommand DeleteTeamLeaderCommand => new RelayCommand(execute => DeleteTeamLeader(), canExecute => SelectedItem != null);
        public RelayCommand SaveChangesCommand => new RelayCommand(execute => SaveChanges(), canExecute => SelectedItem != null);

        public ManageTeamLeaderViewModel()
        {
            string connectionString = DatabaseConfig.GetConnectionString();
            teamLeaderRepository = new TeamLeaderRepository(connectionString);
            TeamLeaders = new ObservableCollection<TeamLeader>();
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
            var newTeamLeader = new TeamLeader
            {
                Name = "New Team Leader", // Eksempeldata, justér efter behov
                UserName = "DefaultUserName",
                Password = "DefaultPassword",
                Phone = 0,
                Email = "default@example.com"
            };

            teamLeaderRepository.Add(newTeamLeader); // Gem i databasen
            TeamLeaders.Add(newTeamLeader);           // Opdater ObservableCollection
        }

        private void DeleteTeamLeader()
        {
            teamLeaderRepository.Delete(SelectedItem.TeamLeaderId); // Slet fra databasen
            TeamLeaders.Remove(SelectedItem);                       // Fjern fra ObservableCollection
        }

        private void SaveChanges()
        {
            teamLeaderRepository.Update(SelectedItem); // Gem ændringer i databasen
        }
    }
}
