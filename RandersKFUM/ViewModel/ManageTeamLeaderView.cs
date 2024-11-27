using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Input;
using RandersKFUM.Model;
using RandersKFUM.Repository;
using RandersKFUM.Utilities;
using RandersKFUM.View;



namespace RandersKFUM.ViewModels
{
    public class ManageTeamLeaderViewModel : INotifyPropertyChanged
    {
        private readonly TeamLeaderRepository _repository;

        private TeamLeader _selectedTeamLeader;
        private bool _isTeamLeaderSelected;

        public ManageTeamLeaderViewModel(TeamLeaderRepository repository)
        {
            _repository = repository;

            // Hent eksisterende holdledere fra databasen
            TeamLeaders = new ObservableCollection<TeamLeader>(_repository.GetAll());

            // Kommandoer
            CreateTeamLeaderCommand = new RelayCommand(CreateTeamLeader);
            UpdateTeamLeaderCommand = new RelayCommand(UpdateTeamLeader, () => IsTeamLeaderSelected);
            DeleteTeamLeaderCommand = new RelayCommand(DeleteTeamLeader, () => IsTeamLeaderSelected);
            SaveChangesCommand = new RelayCommand(SaveChanges);
        }

        // Properties
        public ObservableCollection<TeamLeader> TeamLeaders { get; }

        public TeamLeader SelectedTeamLeader
        {
            get => _selectedTeamLeader;
            set
            {
                _selectedTeamLeader = value;
                IsTeamLeaderSelected = _selectedTeamLeader != null;
                OnPropertyChanged(nameof(SelectedTeamLeader));
            }
        }

        public bool IsTeamLeaderSelected
        {
            get => _isTeamLeaderSelected;
            set
            {
                _isTeamLeaderSelected = value;
                OnPropertyChanged(nameof(IsTeamLeaderSelected));
                RefreshCommands();
            }
        }

        // Commands
        public ICommand CreateTeamLeaderCommand { get; }
        public ICommand UpdateTeamLeaderCommand { get; }
        public ICommand DeleteTeamLeaderCommand { get; }
        public ICommand SaveChangesCommand { get; }

        // Methods

        // Opret en ny holdleder
        private void CreateTeamLeader()
        {
            var newTeamLeader = new TeamLeader
            {
                Name = "Ny Holdleder",
                UserName = "Brugernavn",
                Password = "",
                Phone = 0,
                Email = "example@example.com"
            };

            TeamLeaders.Add(newTeamLeader);
            SelectedTeamLeader = newTeamLeader;
        }

        // Opdater den valgte holdleder
        private void UpdateTeamLeader()
        {
            if (SelectedTeamLeader != null)
            {
                // Ændringer gemmes automatisk, når vi kalder SaveChanges
            }
        }

        // Slet den valgte holdleder
        private void DeleteTeamLeader()
        {
            if (SelectedTeamLeader == null) return;

            // Fjern fra ObservableCollection
            TeamLeaders.Remove(SelectedTeamLeader);

            // Slet fra databasen
            _repository.Delete(SelectedTeamLeader.TeamLeaderId);
        }

        // Gem ændringer i databasen
        private void SaveChanges()
        {
            foreach (var teamLeader in TeamLeaders)
            {
                if (teamLeader.TeamLeaderId == 0)
                {
                    // Opret ny holdleder
                    _repository.Add(teamLeader);
                }
                else
                {
                    // Opdater eksisterende holdleder
                    _repository.Update(teamLeader);
                }
            }
        }

        private void RefreshCommands()
        {
            (UpdateTeamLeaderCommand as RelayCommand)?.RaiseCanExecuteChanged();
            (DeleteTeamLeaderCommand as RelayCommand)?.RaiseCanExecuteChanged();
        }

        // INotifyPropertyChanged implementation
        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
