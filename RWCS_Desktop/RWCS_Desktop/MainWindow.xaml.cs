using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Drawing;
using System.IO;
using System.Drawing.Imaging;

using System.Net.Http;
using Newtonsoft.Json;
using RWCS_Desktop.Entities;
using System.Windows.Threading;
using System.Drawing.Drawing2D;
using Microsoft.AspNetCore.Http.Internal;
using Microsoft.AspNetCore.Http;
using Azure.Storage.Blobs;

namespace RWCS_Desktop
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public string url = "https://localhost:44395/api";
        private string _screenshotPath = "DefaultEndpointsProtocol=https;AccountName=rwcsblopcontainer;AccountKey=HJ+6AUcDqM3cAIU4q5mdGefzzNMlYicVHkq4mQqTX4sOvDa9eQdjjd7PyeyDbpuhSb7cetAmAedt+ASty3fwpA==;EndpointSuffix=core.windows.net";
        private int _userId;
        private string _userName;
        private string _email;
        private string _jiraBaseUrl;
        private string _jiraApiKey;

        private string _selectedProjectKey;
        private ProjectMember _selectedProjectMember;
        private JiraTask _selectedTask;
        private JiraTask _workSessionTask;

        private string _foldername = "";
        private DateTime _startDate = new DateTime();

        private DateTime _currentDate = new DateTime();
        

        private Project _selectedProject;

        private DispatcherTimer workSessionTimer;
        private TimeSpan _currentSessionTime;

        private List<ProjectMember> _selectedProjectMembers = new List<ProjectMember>();
        private ProjectMember _memberToChange = new ProjectMember();

        Dictionary<string, string> _projects = new Dictionary<string, string>();
        List<JiraTask> _allTasks = new List<JiraTask>();

        private bool _IsScreenActivityControlEnabled = false;
        private float _interval = 15f;


        List<WorkSession> myWorkSessions = new List<WorkSession>();
        List<WorkSession> allWorkSessions = new List<WorkSession>();


        private bool _isConnected;

        public MainWindow(int userId, string userName, string email, string jiraApiKey, string jiraBaseUrl, TimeSpan timer)
        {
            InitializeComponent();
            _userId = userId;
            _userName = userName;
            _email = email;
            _jiraBaseUrl = jiraBaseUrl;
            timeLabel.Content = timer.ToString();
            _jiraApiKey = jiraApiKey;
            Initialization();
            InitializeWorkSessionTimer();
            usersTabItem.Visibility = Visibility.Hidden;

        }

        #region Initializations

        private void InitializeWorkSessionTimer()
        {
            workSessionTimer = new DispatcherTimer();
            workSessionTimer.Interval = TimeSpan.FromSeconds(1d);
            workSessionTimer.Tick += workSessionTick;
        }

        private async void Initialization()
        {
            usernameInfo.Content = Replace_UnderScore_WithTwo(_userName);
            jiraApiKey_TextBox.Text = _jiraApiKey;
            jiraBaseUrl_TextBox.Text = _jiraBaseUrl;
            workSession_Button.Background = System.Windows.Media.Brushes.LightGray;
            email_TextBox.Text = _email;
            Mouse.OverrideCursor = System.Windows.Input.Cursors.Wait;
            _isConnected = await TestConnection();
            if (_isConnected)
            {
                SetStatusLabels("Success", System.Windows.Media.Brushes.Green);
            }
            else
            {
                SetStatusLabels("Authorization failed", System.Windows.Media.Brushes.Red);
            }
            rolePicker.Items.Add("Developer");
            rolePicker.Items.Add("AgileManager");
            rolePicker.SelectedItem = "AgileManager";
            await GetJiraProjects();
            projectSessionTabItem.Visibility = Visibility.Hidden;
            statsTabItem.Visibility = Visibility.Hidden;



            Mouse.OverrideCursor = System.Windows.Input.Cursors.Arrow;
        }

        #endregion


        #region Help functions

        private void SetStatusLabels(string status, System.Windows.Media.Brush color)
        {
            status_Label.Content = status;
            status_Label.Foreground = color;
            connectionStatus.Content = status;
            connectionStatus.Foreground = color;
        }

        private bool GetActiveSprintsList()
        {
            sprintListBox.Items.Clear();
            var sprintNames = _allTasks.Select(x => x.SprintName).Distinct();

            foreach (var name in sprintNames)
            {
                sprintListBox.Items.Add(name);
            }

            if (sprintListBox.Items.Count > 0)
            {
                return true;
            }
            else
            {
                return false;
            }

        }


        private string Replace_UnderScore_WithTwo(string str)
        {
            char und = "_".ToCharArray()[0];
            string new_str = "";
            foreach (char c in str)
            {
                new_str += c;
                if (c == und)
                {
                    new_str += und;
                }
            }

            return new_str;
        }


        private void UpdateCredentials()
        {
            _email = email_TextBox.Text;
            _jiraApiKey = jiraApiKey_TextBox.Text;
            _jiraBaseUrl = $"{jiraBaseUrl_TextBox.Text}";
        }


        public static byte[] ImageToByte(System.Drawing.Image img)
        {
            using (var stream = new MemoryStream())
            {
                img.Save(stream, System.Drawing.Imaging.ImageFormat.Png);
                return stream.ToArray();
            }
        }



        #endregion


        #region Requests to API
        private async Task<bool> GetJiraProjects()
        {
            if (!_isConnected)
            {
                return false;
            }

            try
            {

                HttpClient client = new HttpClient();
                var plainTextBytes = System.Text.Encoding.UTF8.GetBytes($"{_email}:{_jiraApiKey}");
                string val = System.Convert.ToBase64String(plainTextBytes);
                client.DefaultRequestHeaders.Add("Authorization", "Basic " + val);
                HttpResponseMessage response = await client.GetAsync($"https://{_jiraBaseUrl}.atlassian.net/" + "rest/api/3/project/");
                string resultContent = await response.Content.ReadAsStringAsync();


                dynamic jsonObj = JsonConvert.DeserializeObject(resultContent);

                if (jsonObj == null)
                {
                    return false;
                }

                _projects.Clear();
                projectsList.Items.Clear();
                foreach (var item in jsonObj)
                {
                    _projects.Add((string)item["key"], (string)item["name"]);
                }

                if (_projects.Count > 0)
                {
                    foreach (var item in _projects)
                    {
                        projectsList.Items.Add($"{item.Value} ({item.Key})");
                    }
                }



                return true;
            }
            catch
            {
                MessageBox.Show("Не вдалось отримати список проектів");
                return false;

            }

        }

        private async Task<bool> GetJiraTasksAndSprints()
        {

            if (!_isConnected)
            {
                return false;
            }

            try
            {

                HttpClient client = new HttpClient();
                var plainTextBytes = System.Text.Encoding.UTF8.GetBytes($"{_email}:{_jiraApiKey}");
                string val = System.Convert.ToBase64String(plainTextBytes);
                client.DefaultRequestHeaders.Add("Authorization", "Basic " + val);
                HttpResponseMessage response = await client.GetAsync($"https://{_jiraBaseUrl}.atlassian.net/" + $"rest/api/2/search?jql=project%20%3D%20{_selectedProjectKey}%20AND%20Sprint%20in%20openSprints()");
                string resultContent = await response.Content.ReadAsStringAsync();


                dynamic jsonObj = JsonConvert.DeserializeObject(resultContent);

                if (jsonObj == null)
                {
                    return false;
                }

                _allTasks.Clear();
                tasksList.Items.Clear();
                selectedTaskTextBlock.Text = "";
                var issues = jsonObj["issues"];
                foreach (var issue in issues)
                {
                    string key = (string)issue["key"];
                    string name = (string)issue["fields"]["summary"];
                    string status = (string)issue["fields"]["status"]["name"];

                    var sprints = issue["fields"]["customfield_10020"];

                    string sprint_name = "";
                    foreach (var sprint in sprints)
                    {
                        if (sprint["state"] == "active")
                        {
                            sprint_name = sprint["name"];
                            break;
                        }
                    }

                    JiraTask new_task = new JiraTask() { TaskName = name, TaskKey = key, Status = status, SprintName = sprint_name };

                    _allTasks.Add(new_task);
                }

                if (_allTasks.Count > 0)
                {
                    return GetActiveSprintsList();
                }
                else
                {
                    return false;
                }

            }
            catch
            {
                MessageBox.Show("Не вдалось отримати список завдань");
                return false;

            }

        }

        private async Task<bool> TestConnection()
        {

            if (_jiraApiKey == "-" || _jiraBaseUrl == "-")
            {
                return false;
            }

            HttpClient client = new HttpClient();
            var plainTextBytes = System.Text.Encoding.UTF8.GetBytes($"{_email}:{_jiraApiKey}");
            string val = System.Convert.ToBase64String(plainTextBytes);
            client.DefaultRequestHeaders.Add("Authorization", "Basic " + val);

            try
            {
                HttpResponseMessage response = await client.GetAsync($"https://{jiraBaseUrl_TextBox.Text}.atlassian.net" + "/rest/auth/1/session");

                if (response.IsSuccessStatusCode)
                {
                    await UpdateJiraCredentials();
                    return true;
                }
            }
            catch
            {
                return false;
            }




            return false;
        }


        private async Task UpdateJiraCredentials()
        {
            HttpClient client = new HttpClient();
            try
            {
                HttpResponseMessage response = await client.GetAsync(url + $"/user/{_userId}");

                string resultContent = await response.Content.ReadAsStringAsync();
                var result = JsonConvert.DeserializeObject<User>(resultContent);
                if ((result.JiraApiKey != _jiraApiKey || result.JiraBaseUrl != _jiraBaseUrl) || result.Email != _email)
                {
                    result.JiraApiKey = _jiraApiKey;
                    result.JiraBaseUrl = _jiraBaseUrl;
                    result.Email = _email;

                    var newUser = JsonConvert.SerializeObject(result);
                    var content = new StringContent(newUser, Encoding.UTF8, "application/json");
                    HttpResponseMessage response1 = await client.PutAsync(url + $"/user/{_userId}", content);
                }
            }
            catch
            {
                SetStatusLabels("Помилка доступу до бази даних", System.Windows.Media.Brushes.Red);
            }


        }


        public async Task GetProjecMembersWithRoles()
        {
            if (await GetProjectMembers())
            {

                HttpClient httpClient = new HttpClient();
                List<string> text_users = new List<string>();
                foreach (var projectMember in _selectedProjectMembers)
                {
                    HttpResponseMessage response = await httpClient.GetAsync(url + $"/User/{projectMember.UserId}");

                    string resultContent = await response.Content.ReadAsStringAsync();


                    User user = JsonConvert.DeserializeObject<User>(resultContent);


                    text_users.Add($"{projectMember.Id}| {user.FirstName} {user.LastName}   Роль: {projectMember.Role}");

                }
                if (usersListBox.Items.Count > 0)
                {
                    usersListBox.Items.Clear();
                }

                foreach (var user in text_users)
                {
                    usersListBox.Items.Add(user);
                }

            }
        }


        private async Task<bool> GetProjectMembers()
        {
            HttpClient httpClient = new HttpClient();


            try
            {
                HttpResponseMessage response = await httpClient.GetAsync(url + $"/projectMembers/project/{_selectedProject.Id}");

                if (response.IsSuccessStatusCode)
                {
                    string resultContent = await response.Content.ReadAsStringAsync();


                    dynamic jsonObj = JsonConvert.DeserializeObject(resultContent);
                    _selectedProjectMembers.Clear();
                    foreach (var item in jsonObj)
                    {
                        int id = item["id"];
                        int userId = item["userId"];
                        int projectId = item["projectId"];
                        int role_id = item["role"];
                        UserRole Role = UserRole.AgileManager;
                        if (role_id == 0)
                        {
                            Role = UserRole.Developer;
                        }

                        ProjectMember member = new ProjectMember()
                        {
                            Id = id,
                            Role = Role,
                            EmployeeScreenActivityIds = new List<int>(),
                            UserId = userId,
                            ProjectId = projectId,
                            WorkSessionIds = new List<int>()
                        };

                        _selectedProjectMembers.Add(member);
                    }
                    infoLabel.Content = "";
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch
            {
                return false;
            }

        }

        private async Task<bool> CreateNewProject()
        {
            HttpClient httpClient = new HttpClient();

            Project new_project = new Project
            {
                ProjectKey = _selectedProjectKey,
                IsScreenActivityControlEnabled = false,
                ProjectMemberIds = new List<int>(),
                JiraDomain = _jiraBaseUrl,
                ProjectTitle = _projects[_selectedProjectKey],
                ScreenshotInterval = 60
            };

            var newProj = JsonConvert.SerializeObject(new_project);
            var httpContent = new StringContent(newProj, Encoding.UTF8, "application/json");
            try
            {
                HttpResponseMessage response = await httpClient.PostAsync(url + $"/project", httpContent);

                if (response.IsSuccessStatusCode)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch
            {
                return false;
            }

        }

        private async Task<ProjectMember> GetProjectMemberWith_UserId_And_ProjectId(int userId, int projectId)
        {
            HttpClient client = new HttpClient();

            try
            {
                HttpResponseMessage response = await client.GetAsync(url + $"/projectMember/{userId}/{projectId}");

                if (response.IsSuccessStatusCode)
                {
                    string resultContent = await response.Content.ReadAsStringAsync();
                    ProjectMember projectMember = JsonConvert.DeserializeObject<ProjectMember>(resultContent);
                    infoLabel.Content = "";
                    return projectMember;
                }

            }
            catch
            {
                infoLabel.Content = "Сталася помилка";
                return null;
            }
            return null;
        }

        private async Task<Project> GetProjectInfoWith_Domain_And_ProjectKey(string domain, string project_key)
        {
            HttpClient client = new HttpClient();

            try
            {
                HttpResponseMessage response = await client.GetAsync(url + $"/project/{domain}/{project_key}");

                if (response.IsSuccessStatusCode)
                {
                    string resultContent = await response.Content.ReadAsStringAsync();
                    Project project = JsonConvert.DeserializeObject<Project>(resultContent);
                    infoLabel.Content = "";
                    return project;
                }

            }
            catch
            {
                infoLabel.Content = "Сталася помилка";
                return null;
            }
            return null;
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
                WorkTime = (float)_currentSessionTime.TotalMinutes,
                ScreenActivityFolder = _foldername
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

        private async Task<bool> ChangeProjectMemberRole(UserRole newRole)
        {
            HttpClient httpClient = new HttpClient();

            ProjectMember newdata = _memberToChange;
            newdata.Role = newRole;

            var newProj = JsonConvert.SerializeObject(newdata);
            var httpContent = new StringContent(newProj, Encoding.UTF8, "application/json");
            try
            {
                HttpResponseMessage response = await httpClient.PutAsync(url + $"/projectMember/{newdata.Id}", httpContent);

                if (response.IsSuccessStatusCode)
                {

                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch
            {
                return false;
            }

        }


        private async Task<string> GetProjectMemberName(int id)
        {
            string name = "";

            HttpClient client = new HttpClient();

            HttpResponseMessage response = await client.GetAsync(url + $"/projectMember/{id}/fullName");
            name = await response.Content.ReadAsStringAsync();
            return name;
        }

        #endregion




        private void logoutButton_Click(object sender, RoutedEventArgs e)
        {
            AuthWindow authWindow = new AuthWindow();
            authWindow.Show();
            this.Close();

        }

        private async void checkConnection_Button_Click(object sender, RoutedEventArgs e)
        {
            UpdateCredentials();
            Mouse.OverrideCursor = System.Windows.Input.Cursors.Wait;
            _isConnected = await TestConnection();
            if (_isConnected)
            {
                SetStatusLabels("Success", System.Windows.Media.Brushes.Green);
                await Refresh();
            }
            else
            {
                SetStatusLabels("Authorization failed", System.Windows.Media.Brushes.Red);
            }
            Mouse.OverrideCursor = System.Windows.Input.Cursors.Arrow;
        }

        

        private async void projectsList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Mouse.OverrideCursor = System.Windows.Input.Cursors.Wait;
            bool firstUser = false;
            if (projectsList.SelectedItem != null)
            {
                HttpClient client = new HttpClient();
                var select = projectsList.SelectedItem.ToString();
                select = select.Split(" ")[1];
                select = select.Substring(1, select.Length - 2);
                _selectedProjectKey = select;
                _currentDate = new DateTime();

                _selectedProject =  await GetProjectInfoWith_Domain_And_ProjectKey(_jiraBaseUrl, _selectedProjectKey);
                Mouse.OverrideCursor = System.Windows.Input.Cursors.Arrow;
                if (_selectedProject == null)
                {
                    if (MessageBox.Show("Цього проекту ще нема в системі, завантажити?", "Question", MessageBoxButton.YesNo, MessageBoxImage.Information) == MessageBoxResult.Yes)
                    {
                        
                        firstUser = true;
                        await CreateNewProject();

                        _selectedProject = await GetProjectInfoWith_Domain_And_ProjectKey(_jiraBaseUrl, _selectedProjectKey);
                        infoLabel.Content = "";
                        Mouse.OverrideCursor = System.Windows.Input.Cursors.Wait;                       


                    }
                    else
                    {
                        await Refresh();
                    }       
                }


                if (_selectedProject != null)
                {
                    _currentDate = DateTime.Now;
                    statsTabItem.Visibility = Visibility.Visible;
                    _selectedProjectMember = await GetProjectMemberWith_UserId_And_ProjectId(_userId, _selectedProject.Id);



                    if (_selectedProjectMember == null)
                    {
                        ProjectMember new_project_member = new ProjectMember
                        {
                            EmployeeScreenActivityIds = new List<int>(),
                            ProjectId = _selectedProject.Id,
                            UserId = _userId,
                            WorkSessionIds = new List<int>()
                        };

                        if (firstUser)
                        {
                            new_project_member.Role = UserRole.AgileManager;
                        }
                        else
                        {
                            new_project_member.Role = UserRole.Developer;
                        }

                        var newProjMember = JsonConvert.SerializeObject(new_project_member);
                        var httpContent = new StringContent(newProjMember, Encoding.UTF8, "application/json");
                        try
                        {
                            HttpResponseMessage response = await client.PostAsync(url + $"/ProjectMember", httpContent);

                            if (response.IsSuccessStatusCode)
                            {
                                _selectedProjectMember = await GetProjectMemberWith_UserId_And_ProjectId(_userId, _selectedProject.Id);
                                infoLabel.Content = "";
                            }

                        }
                        catch
                        {
                            infoLabel.Content = "Сталася помилка";
                        }
                    }
                    statsTabItem.Visibility = Visibility.Visible;

                    if (_selectedProjectMember!= null && _selectedProjectMember.Role == UserRole.AgileManager)
                    {
                        usersTabItem.Visibility = Visibility.Visible;
                        projectSettings.Visibility = Visibility.Visible;
                        projectSessionTabItem.Visibility = Visibility.Visible;
                    }
                    else
                    {
                        usersTabItem.Visibility = Visibility.Hidden;
                        projectSettings.Visibility = Visibility.Hidden;
                        projectSessionTabItem.Visibility = Visibility.Hidden;
                    }

                    if (_selectedProjectMember != null && _selectedProject != null)
                    {
                        await GetJiraTasksAndSprints();

                        if (_selectedProjectMember.Role == UserRole.AgileManager)
                        {
                            usersTabItem.Visibility = Visibility.Visible;
                            await GetProjecMembersWithRoles();

                        }
                    }
                }
                else
                {
                    statsTabItem.Visibility = Visibility.Hidden;
                }
                
                Mouse.OverrideCursor = System.Windows.Input.Cursors.Arrow;


            }

        }

        #region Statistics

        public async Task LoadStatisticsForProjectMember(int projectMemberId)
        {
            

        
            DateTime w1 = StartOfWeek(_currentDate, DayOfWeek.Monday);
            DateTime w2 = w1.AddDays(7).AddMinutes(-1);

            currentWeekLabel.Content = $"{w1.ToShortDateString()}-{w2.ToShortDateString()}";

            StatisticsData week = await GetStatisticsInDateRangeForUser(w1, w2, _selectedProjectMember.Id);
            StatisticsData overall = await GetStatisticsByProjectMemberId(_selectedProjectMember.Id);

            weekHours.Content = week.HoursWorking.ToString();
            weekTasks.Content = week.NumberOfTasks.ToString();
            weekSessions.Content = week.NumberOfSessions.ToString();
            overallHours.Content = overall.HoursWorking.ToString();
            overallTasks.Content = overall.NumberOfTasks.ToString();
            overallSessions.Content = overall.NumberOfSessions.ToString();

            GetWorkSessions(true);


        }

        public async void GetWorkSessions(bool forCurrentUser)
        {
            HttpClient httpClient = new HttpClient();

            HttpResponseMessage response;
            if (forCurrentUser)
            {
                response = await httpClient.GetAsync(url + $"/workSessions/byProjectMemberId/{_selectedProjectMember.Id}");
                myWorkSessions.Clear();
                workSessionsListBox.Items.Clear();
            }
            else
            {
                response = await httpClient.GetAsync(url + $"/workSessions/byProjectId/{_selectedProject.Id}");
                allWorkSessions.Clear();
                allWorkSessionsListBox.Items.Clear();
            }

            string resultContent = await response.Content.ReadAsStringAsync();
            dynamic jsonObj = JsonConvert.DeserializeObject(resultContent);

            foreach(var item in jsonObj)
            {
                int id = item["id"];
                int projectMemberId = item["projectMemberId"];
                string sprintKey = item["sprintKey"];
                string taskKey = item["taskKey"];
                DateTime startDate = item["startDate"];
                DateTime endDate = item["endDate"];
                float workTime = item["workTime"];
                string screenActivityFolder = item["screenActivityFolder"];

                WorkSession session = new WorkSession()
                {
                    Id = id,
                    ProjectMemberId = projectMemberId,
                    SprintKey = sprintKey,
                    TaskKey = taskKey,
                    StartDate = startDate,
                    EndDate = endDate,
                    WorkTime = workTime,
                    ScreenActivityFolder = screenActivityFolder
                };

                String session_to_string = $"{id}|  Від:{projectMemberId}|{sprintKey}:{taskKey}|   ({startDate.ToShortDateString()})-({endDate.ToShortDateString()})   Час:{MathF.Round(workTime,2)}";

                if (forCurrentUser)
                {
                   myWorkSessions.Add(session);
                   workSessionsListBox.Items.Add(session_to_string);
                }
                else
                {
                    allWorkSessions.Add(session);
                    allWorkSessionsListBox.Items.Add(session_to_string);
                }
            }

            
        }

        public async Task<StatisticsData> GetStatisticsInDateRangeForUser(DateTime t1, DateTime t2, int id)
        {
            HttpClient httpClient = new HttpClient();
            HttpResponseMessage response = await httpClient.GetAsync(url + $"/Statistics?start={t1.Year}-{t1.Month}-{t1.Day}&end={t2.Year}-{t2.Month}-{t2.Day}&projectMemberId={id}");
            string resultContent = await response.Content.ReadAsStringAsync();


            StatisticsData stats = JsonConvert.DeserializeObject<StatisticsData>(resultContent);
            return stats;
        }

        public async Task<StatisticsData> GetStatisticsByProjectMemberId(int id)
        {
            HttpClient httpClient = new HttpClient();
            HttpResponseMessage response = await httpClient.GetAsync(url + $"/Statistics/projectMemberId/{id}");
            string resultContent = await response.Content.ReadAsStringAsync();


            StatisticsData stats = JsonConvert.DeserializeObject<StatisticsData>(resultContent);
            return stats;
        }

        public DateTime StartOfWeek(DateTime dt, DayOfWeek startOfWeek)
        {
            int diff = (7 + (dt.DayOfWeek - startOfWeek)) % 7;
            return dt.AddDays(-1 * diff).Date;
        }

        #endregion

        

        private async void refreshButton_Click(object sender, RoutedEventArgs e)
        {
            await Refresh();
        }

        public async Task Refresh()
        {
            Mouse.OverrideCursor = System.Windows.Input.Cursors.Wait;
            UpdateCredentials();

            projectsList.Items.Clear();
            tasksList.Items.Clear();
            sprintListBox.Items.Clear();
            selectedTaskTextBlock.Text = "";
            infoLabel.Content = "";
            usersTabItem.Visibility = Visibility.Hidden;
            projectSettings.Visibility = Visibility.Hidden;
            projectSettings.Visibility = Visibility.Hidden;
            statsTabItem.Visibility = Visibility.Hidden;
            projectSessionTabItem.Visibility = Visibility.Hidden;
            _selectedProject = null;
            _selectedProjectKey = null;
            _selectedProjectMember = null;
            _selectedTask = null;
            _currentDate = new DateTime();


            await GetJiraProjects();
            Mouse.OverrideCursor = System.Windows.Input.Cursors.Arrow;
        }

       

        private async void workSessionTick(object sender, EventArgs e)
        {
            _currentSessionTime+= new TimeSpan(0,0,1);

            timeLabel.Content = _currentSessionTime.ToString();

            if(_IsScreenActivityControlEnabled && _currentSessionTime.TotalSeconds % _interval == 0)
            {
                bool t = workSessionTimer.IsEnabled;
                HttpClient client = new HttpClient();
                double screenLeft = SystemParameters.VirtualScreenLeft;
                double screenTop = SystemParameters.VirtualScreenTop;
                double screenWidth = SystemParameters.VirtualScreenWidth;
                double screenHeight = SystemParameters.VirtualScreenHeight;


                using (Bitmap bmp = new Bitmap((int)screenWidth, (int)screenHeight))
                {

                    bmp.SetResolution(96.0F, 96.0F);

                    using (Graphics g = Graphics.FromImage(bmp))
                    {
                        String filename = "ScreenCapture-" + DateTime.Now.ToString("ddMMyyyy-hhmmss") + ".png";
                        
                        g.CopyFromScreen((int)screenLeft, (int)screenTop, 0, 0, bmp.Size);


                        byte[] picture = ImageToByte(bmp);

                        BlobContainerClient blobContainerClient = new BlobContainerClient(_screenshotPath, "bloprwcs");


                        MemoryStream stream = new MemoryStream(picture);
                        stream.Position = 0;

                        MultipartFormDataContent form = new MultipartFormDataContent();
                        form.Add(new ByteArrayContent(picture, 0, picture.Count()), "file", $"{_foldername}/{filename}");


                        HttpResponseMessage response = await client.PostAsync($"{url}/screenshot", form);
                        

                    }

                }
            }
            
        }


        private async void workSession_Button_Click(object sender, RoutedEventArgs e)
        {
            HttpClient client = new HttpClient();

            if (!workSessionTimer.IsEnabled)
            {
                if (_selectedTask != null)
                {
                    infoLabel.Content = "";
                    _currentSessionTime = new TimeSpan(0, 0, 0);
                    _IsScreenActivityControlEnabled = _selectedProject.IsScreenActivityControlEnabled;
                    _workSessionTask = _selectedTask;
                    _startDate = DateTime.Now;

                    _interval = _selectedProject.ScreenshotInterval;


                    if (_IsScreenActivityControlEnabled)
                    {
                        _foldername = _startDate.ToString("ddMMyyyy-hhmm") + $"_user_{_userId}";

                    }
                    else
                    {
                        _foldername = "-";
                    }


                    workSession_Button.Background = System.Windows.Media.Brushes.Red;
                    workSession_Button.Foreground = System.Windows.Media.Brushes.White;
                    workSession_Button.Content = "Зупинити";

                    workSessionTimer.Start();
                }
                else
                {
                    infoLabel.Content = "Потрібено обрати завдання";
                }
            }
            else
            {
                workSessionTimer.Stop(); 
                workSession_Button.Background = (SolidColorBrush)new BrushConverter().ConvertFrom("#4C4C6D");
                workSession_Button.Foreground = System.Windows.Media.Brushes.DarkOrange;
                workSession_Button.Content = "Почати";
                await CreateNewWorkSession();
            }

            

            
            
        }

        
        

        private void projectSettings_Click(object sender, RoutedEventArgs e)
        {
            SettingsWindow window = new SettingsWindow(_selectedProject);
            window.Show();
        }

        private void sprintListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string selectedSprint = sprintListBox.SelectedItem as string;

            if (selectedSprint != null)
            {
                tasksList.Items.Clear();

                List<JiraTask> tasksInSprint = _allTasks.Where(x => x.SprintName == selectedSprint).OrderBy(x => x.Status).ToList();
                foreach(var task in tasksInSprint)
                {
                    tasksList.Items.Add($"({task.TaskKey} : {task.Status}) {task.TaskName} ");
                }
            }
            

            
            
        }

        private void tasksList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (tasksList.SelectedItem != null)
            {
                selectedTaskTextBlock.Text = $"Обрано завдання{tasksList.SelectedItem} з спринту {sprintListBox.SelectedItem}";

                string taskKey = tasksList.SelectedItem.ToString().Split(":")[0];
                taskKey = taskKey.Trim().Substring(1, taskKey.Length - 2);
                _selectedTask = _allTasks.FirstOrDefault(x => x.TaskKey == taskKey);
            }
           
        }

        private void usersListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (usersListBox.Items.Count > 0)
            {
                userLabel.Content = usersListBox.SelectedItem;

                _memberToChange = _selectedProjectMembers[usersListBox.SelectedIndex];
            }
            
        }

        private async void changeRoleButton_Click(object sender, RoutedEventArgs e)
        {
            if (_memberToChange != null)
            {
                UserRole role = UserRole.AgileManager;
                if(rolePicker.SelectedItem == "Developer")
                {
                    role = UserRole.Developer;
                }
                await ChangeProjectMemberRole(role);
                
                await GetProjecMembersWithRoles();
            }
        }

        

        private async void toStatsLabel_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            await LoadStatisticsForProjectMember(_selectedProjectMember.Id);
        }

        private async void leftButton_Click(object sender, RoutedEventArgs e)
        {
            _currentDate = _currentDate.AddDays(-7);
            await LoadStatisticsForProjectMember(_selectedProjectMember.Id);
        }

        private async void rightButton_Click(object sender, RoutedEventArgs e)
        {
            _currentDate = _currentDate.AddDays(7);
            await LoadStatisticsForProjectMember(_selectedProjectMember.Id);
        }

        private async void nowButton_Click(object sender, RoutedEventArgs e)
        {
            _currentDate = DateTime.Now;
            await LoadStatisticsForProjectMember(_selectedProjectMember.Id);
        }

        private async void workSessionsListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (workSessionsListBox.SelectedItem != null)
            {
                var select = workSessionsListBox.SelectedItem.ToString();
                select = select.Split("|")[0];

                int id = int.Parse(select);

                string fullName = await GetProjectMemberName(myWorkSessions.FirstOrDefault(x => x.Id == id).ProjectMemberId);
                WorkSessionWindow window = new WorkSessionWindow(myWorkSessions.FirstOrDefault(x => x.Id == id), fullName);
                window.Show();
            }
            
        }



        private async void allWorkSessionsListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (allWorkSessionsListBox.SelectedItem != null)
            {
                var select = allWorkSessionsListBox.SelectedItem.ToString();
                select = select.Split("|")[0];

                int id = int.Parse(select);


                string fullName = await GetProjectMemberName(allWorkSessions.FirstOrDefault(x => x.Id == id).ProjectMemberId);
                WorkSessionWindow window = new WorkSessionWindow(allWorkSessions.FirstOrDefault(x => x.Id == id), fullName);
                window.Show();
            }

           
        }

        private void toProjectSessionLabel_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            GetWorkSessions(false);
        }
    }
}
