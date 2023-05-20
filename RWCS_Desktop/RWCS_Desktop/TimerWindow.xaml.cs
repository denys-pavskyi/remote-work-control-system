using Newtonsoft.Json;
using RWCS_Desktop.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace RWCS_Desktop
{
    /// <summary>
    /// Interaction logic for TimerWindow.xaml
    /// </summary>
    public partial class TimerWindow : Window
    {
        private int _userId;
        private string _userName;
        private string _email;
        private string _jiraBaseUrl;
        private string _jiraApiKey;
        private DispatcherTimer workSessionTimer;
        private TimeSpan _currentSessionTime;
        public string url = "https://localhost:44395/api";

        private JiraTask _workSessionTask;
        private ProjectMember _selectedProjectMember;
        private DateTime _startDate;


        public TimerWindow(int userId, string userName, string email, string jiraApiKey, string jiraBaseUrl, JiraTask task, ProjectMember project_member, DateTime startDate)
        {
            InitializeComponent();
            _userId = userId;
            _userName = userName;
            _email = email;
            _jiraBaseUrl = jiraBaseUrl;
            _workSessionTask = task;
            _selectedProjectMember = project_member;
            _startDate = startDate;

            _jiraApiKey = jiraApiKey;
            InitializeWorkSessionTimer();
        }

        private async void stopTimerButton_Click(object sender, RoutedEventArgs e)
        {
            workSessionTimer.Stop();
            MainWindow window = new MainWindow(_userId, _userName, _email, _jiraApiKey, _jiraBaseUrl, _currentSessionTime);
            await CreateNewWorkSession();
            window.Show();
            this.Close();
        }

        private void InitializeWorkSessionTimer()
        {
            workSessionTimer = new DispatcherTimer();
            workSessionTimer.Interval = TimeSpan.FromSeconds(1d);
            workSessionTimer.Tick += workSessionTick;
            workSessionTimer.Start();
        }

        private async void workSessionTick(object sender, EventArgs e)
        {
            _currentSessionTime += new TimeSpan(0, 0, 1);

            timeLabel.Content = _currentSessionTime.ToString();
        }


        private async Task CreateNewWorkSession()
        {
            HttpClient client = new HttpClient();

            DateTime endDate = DateTime.Now;
            WorkSession workSession = new WorkSession()
            {
                ProjectMemberId = _selectedProjectMember.Id,
                StartDate = _startDate,
                EndDate = endDate,
                SprintKey = _workSessionTask.SprintName,
                TaskKey = _workSessionTask.TaskKey,
                WorkTime =(float)_currentSessionTime.TotalMinutes
            };
            var json = JsonConvert.SerializeObject(workSession);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            try
            {
                HttpResponseMessage response = await client.PostAsync(url + "/WorkSession", content);

            }
            catch
            {
                
            }
        }


    }
}
