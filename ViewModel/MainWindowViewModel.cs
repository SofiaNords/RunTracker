using Microsoft.Extensions.Configuration;
using RunTracker.Model;
using RunTracker.Repository;
using RunTracker.Services;
using System.Collections.ObjectModel;

namespace RunTracker.ViewModel
{
    class MainWindowViewModel : ViewModelBase
    {
        private IRunningSessionRepository _runningSessionRepository;

        private readonly IConfiguration? _configuration;

        private DatabaseService _databaseService;

        public ObservableCollection<RunningSession> RunningSessions { get; set; } = new ObservableCollection<RunningSession>();

        public MainWindowViewModel(IConfiguration? configuration)
        {
            _configuration = configuration;
            ConnectToDatabase();
            LoadRunningSessions();
        }

        private void ConnectToDatabase()
        {
            _databaseService = new DatabaseService(_configuration);
            _runningSessionRepository = new RunningSessionRepository(_databaseService.Database);
        }

        private async Task LoadRunningSessions()
        {
            var sessions = await _runningSessionRepository.GetAllAsync();
            RunningSessions.Clear();
            foreach (var session in sessions)
            {
                RunningSessions.Add(session);
            }

        }
    }
}
