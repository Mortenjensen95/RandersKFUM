using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows.Input;
using RandersKFUM.Model;
using RandersKFUM.Repository;

namespace RandersKFUM.ViewModels
{
    public class ManageTeamViewModel : INotifyPropertyChanged
    {
        private readonly TeamRepository _teamRepository;
        private readonly TeamLeaderRepository _teamLeaderRepository;

        private Team _selectedTeam;
        private bool _isTeamSelected;

        public ManageTeamViewModel(TeamRepository teamRepository, TeamLeaderRepository teamLeaderRepository)
        {
            _teamRepository = teamRepository;
            _teamLeaderRepository = teamLeaderRepository;

            // Hent eksisterende teams og holdledere fra databasen
            Teams = new ObservableCollection<Team>(_teamRepository.GetAll());
            TeamLeaders = new ObservableCollection<TeamLeader>(_teamLeaderRepository.GetAll());

            // Kommandoer
            CreateTeamCommand = new RelayCommand(CreateTeam);
            UpdateTeamCommand = new RelayCommand(UpdateTeam, () => IsTeamSelected);
            DeleteTeamCommand = new RelayCommand(DeleteTeam, () => IsTeamSelected);
            SaveChangesCommand = new RelayCommand(SaveChanges);
        }

        // Properties
        public ObservableCollection<Team> Teams { get; }
        public ObservableCollection<TeamLeader> TeamLeaders { get; }

        public Team SelectedTeam
        {
            get => _selectedTeam;
            set
            {
                _selectedTeam = value;
                IsTeamSelected = _selectedTeam != null;
                OnPropertyChanged(nameof(SelectedTeam));
            }
        }

        public bool IsTeamSelected
        {
            get => _isTeamSelected;
            set
            {
                _isTeamSelected = value;
                OnPropertyChanged(nameof(IsTeamSelected));
                RefreshCommands();
            }
        }

        // Commands
        public ICommand CreateTeamCommand { get; }
        public ICommand UpdateTeamCommand { get; }
        public ICommand DeleteTeamCommand { get; }
        public ICommand SaveChangesCommand { get; }

        // Methods

        // Opret et nyt hold
        private void CreateTeam()
        {
            var newTeam = new Team
            {
                TeamName = "Nyt Hold",
                TeamType = "Type",
                TeamLeaderId = TeamLeaders.FirstOrDefault()?.TeamLeaderId ?? 0
            };

            Teams.Add(newTeam);
            SelectedTeam = newTeam;
        }

        // Opdater det valgte hold
        private void UpdateTeam()
        {
            if (SelectedTeam != null)
            {
                // Ændringer gemmes automatisk med databinding
            }
        }

        // Slet det valgte hold
        private void DeleteTeam()
        {
            if (SelectedTeam == null) return;

            // Fjern fra ObservableCollection
            Teams.Remove(SelectedTeam);

            // Fjern fra databasen
            _teamRepository.Delete(SelectedTeam.TeamId);
        }

        // Gem ændringer i databasen
        private void SaveChanges()
        {
            foreach (var team in Teams)
            {
                if (team.TeamId == 0)
                {
                    // Opret nyt hold
                    _teamRepository.Add(team);
                }
                else
                {
                    // Opdater eksisterende hold
                    _teamRepository.Update(team);
                }
            }
        }

        private void RefreshCommands()
        {
            (UpdateTeamCommand as RelayCommand)?.RaiseCanExecuteChanged();
            (DeleteTeamCommand as RelayCommand)?.RaiseCanExecuteChanged();
        }

        // INotifyPropertyChanged implementation
        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
