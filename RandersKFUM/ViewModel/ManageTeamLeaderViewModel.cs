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

        public RelayCommand CreateTeamLeaderCommand { get; }
        public RelayCommand DeleteTeamLeaderCommand { get; }
        public RelayCommand SaveChangesCommand { get; }
        public RelayCommand NavigateBackToAdministrationViewCommand { get; }

        public ManageTeamLeaderViewModel()
        {
            try
            {
                teamLeaderRepository = new TeamLeaderRepository(DatabaseConfig.GetConnectionString());
                TeamLeaders = new ObservableCollection<TeamLeader>(teamLeaderRepository.GetAll());
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Kunne ikke hente holdledere: {ex.Message}", "Fejl", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            CreateTeamLeaderCommand = new RelayCommand(_ => CreateTeamLeader());
            DeleteTeamLeaderCommand = new RelayCommand(_ => DeleteTeamLeader(), _ => SelectedItem != null);
            SaveChangesCommand = new RelayCommand(_ => SaveChanges(), _ => SelectedItem != null);
            NavigateBackToAdministrationViewCommand = new RelayCommand(_ => NavigateBackToAdministrationView());
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
                    Name = "",
                    UserName = "",
                    Password = "",
                    Phone = "",
                    Email = ""
                };
                TeamLeaders.Add(newTeamLeader);

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
                if (ex.Message.Contains("DELETE statement conflicted with the REFERENCE constraint"))
                {
                    MessageBox.Show("Du kan ikke slette denne holdleder, da holdlederen er tilknyttet et eller flere hold. Fjern holdlederen fra holdet først.", "Fejl", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                else
                {
                    MessageBox.Show($"Kunne ikke slette holdleder: {ex.Message}", "Fejl", MessageBoxButton.OK, MessageBoxImage.Error);
                }
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
