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

namespace RWCS_Desktop
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public string url = "https://localhost:44395/api";
        private int _userId;
        private string _userName;
        private string _email;
        private string _jiraBaseUrl;
        private string _jiraApiKey;

        private bool _hasProjects;
        private bool _hasTasks;
        private bool _hasSprints;

        private string _selectedProjectKey;
        private ProjectMember _selectedProjectMember;
        private JiraTask _selectedTask;

        private Project _selectedProject;

        private DispatcherTimer workSessionTimer;
        private TimeSpan _currentSessionTime;

        Dictionary<string, string> _projects = new Dictionary<string, string>();
        List<JiraTask> _allTasks = new List<JiraTask>();
       
        

        private bool _isConnected;

        public MainWindow(int userId, string userName, string email, string jiraApiKey, string jiraBaseUrl)
        {
            InitializeComponent();
            _userId = userId;
            _userName = userName;
            _email = email;
            _jiraBaseUrl = jiraBaseUrl;
            
            _jiraApiKey = jiraApiKey;
            Initialization();
            InitializeWorkSessionTimer();

        }

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

            _hasProjects = await GetJiraProjects();

            

            Mouse.OverrideCursor = System.Windows.Input.Cursors.Arrow;
        }

        private void SetStatusLabels(string status, System.Windows.Media.Brush color)
        {
            status_Label.Content = status;
            status_Label.Foreground = color;
            connectionStatus.Content = status;
            connectionStatus.Foreground = color;
        }
       

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
                    foreach(var sprint in sprints)
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

                if(_allTasks.Count > 0)
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

        private bool GetActiveSprintsList()
        {
            sprintListBox.Items.Clear();
            var sprintNames = _allTasks.Select(x => x.SprintName).Distinct();

            foreach(var name in sprintNames)
            {
                sprintListBox.Items.Add(name);
            }
            
            if(sprintListBox.Items.Count > 0)
            {
                return true;
            }
            else
            {
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

        private string Replace_UnderScore_WithTwo(string str)
        {
            char und = "_".ToCharArray()[0];
            string new_str = "";
            foreach(char c in str)
            {
                new_str += c;
                if (c == und)
                {
                    new_str+=und;
                }
            }

            return new_str;
        }

        private void screenshotButton_Click(object sender, RoutedEventArgs e)
        {
            double screenLeft = SystemParameters.VirtualScreenLeft;
            double screenTop = SystemParameters.VirtualScreenTop;
            double screenWidth = SystemParameters.VirtualScreenWidth;
            double screenHeight = SystemParameters.VirtualScreenHeight;

            using (Bitmap bmp = new Bitmap((int)screenWidth,
                (int)screenHeight))
            {
                using (Graphics g = Graphics.FromImage(bmp))
                {
                    String filename = "ScreenCapture-" + DateTime.Now.ToString("ddMMyyyy-hhmmss") + ".png";
                    Opacity = .0;
                    g.CopyFromScreen((int)screenLeft, (int)screenTop, 0, 0, bmp.Size);
                    bmp.Save("C:\\Users\\denpa\\Desktop\\Диплом\\screenshots\\" + filename);
                    Opacity = 1;
                }

            }
        }

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

        private void UpdateCredentials()
        {
            _email = email_TextBox.Text;
            _jiraApiKey = jiraApiKey_TextBox.Text;
            _jiraBaseUrl = $"{jiraBaseUrl_TextBox.Text}";
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

                _selectedProject =  await GetProjectInfoWith_Domain_And_ProjectKey(_jiraBaseUrl, _selectedProjectKey);
                Mouse.OverrideCursor = System.Windows.Input.Cursors.Arrow;
                if (_selectedProject == null)
                {
                    if (MessageBox.Show("Цього проекту ще нема в системі, завантажити?", "Question", MessageBoxButton.YesNo, MessageBoxImage.Information) == MessageBoxResult.Yes)
                    {
                        
                        firstUser = true;
                        await CreateNewProject();

                        _selectedProject = await GetProjectInfoWith_Domain_And_ProjectKey(_jiraBaseUrl, _selectedProjectKey);
                        Mouse.OverrideCursor = System.Windows.Input.Cursors.Wait;                       


                    }
                    else
                    {
                        await Refresh();
                    }       
                }


                if (_selectedProject != null)
                {
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

                    if(_selectedProjectMember!= null && _selectedProjectMember.Role == UserRole.AgileManager)
                    {
                        usersTabItem.Visibility = Visibility.Visible;
                        generateReport.Visibility = Visibility.Visible;
                    }
                    else
                    {
                        usersTabItem.Visibility = Visibility.Hidden;
                        generateReport.Visibility = Visibility.Hidden;
                    }

                    if (_selectedProjectMember != null && _selectedProject != null)
                    {
                        await GetJiraTasksAndSprints();
                    }
                }
                
                Mouse.OverrideCursor = System.Windows.Input.Cursors.Arrow;


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
            usersTabItem.Visibility = Visibility.Hidden;
            generateReport.Visibility = Visibility.Hidden;
            _selectedProject = null;
            _selectedProjectKey = null;
            _selectedProjectMember = null;
            _selectedTask = null;


            
            _hasProjects = await GetJiraProjects();
            Mouse.OverrideCursor = System.Windows.Input.Cursors.Arrow;
        }

       

        private void workSessionTick(object sender, EventArgs e)
        {
            _currentSessionTime+= new TimeSpan(0,0,1);

            timeLabel.Content = _currentSessionTime.ToString();

            
        }

        private void workSession_Button_Click(object sender, RoutedEventArgs e)
        {
            if (!workSessionTimer.IsEnabled)
            {
                workSession_Button.Background = System.Windows.Media.Brushes.Red;
                _currentSessionTime = new TimeSpan(0, 0, 0);
                workSession_Button.Content = "Зупинити";

                workSessionTimer.Start();
            }
            else
            {
                workSession_Button.Background = System.Windows.Media.Brushes.LightGray;
                workSession_Button.Content = "Почати";
                workSessionTimer.Stop();
            }
            
            
        }

        private void projectSettings_Click(object sender, RoutedEventArgs e)
        {
            //GET PROJECT ID
            SettingsWindow window = new SettingsWindow(1);
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
            selectedTaskTextBlock.Text = $"Обрано завдання{tasksList.SelectedItem} з спринту {sprintListBox.SelectedItem}";
        }
    }
}
