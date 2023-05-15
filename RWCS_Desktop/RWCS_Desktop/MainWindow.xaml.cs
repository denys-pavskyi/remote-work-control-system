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
using Atlassian.Jira;
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
        private int _selectedProjectId;

        private DispatcherTimer workSessionTimer;
        private TimeSpan _currentSessionTime;

        Dictionary<string, string> _projects = new Dictionary<string, string>();
        Dictionary<string, (string title, string status)> _tasks = new Dictionary<string, (string title, string status)>();

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
                HttpResponseMessage response = await client.GetAsync(_jiraBaseUrl + "rest/api/3/project/");
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


        private async Task<bool> GetJiraTasks()
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
                HttpResponseMessage response = await client.GetAsync(_jiraBaseUrl + $"rest/api/3/search?project%3D{_selectedProjectKey}%20AND%20(status%3DDONE)");
                string resultContent = await response.Content.ReadAsStringAsync();


                dynamic jsonObj = JsonConvert.DeserializeObject(resultContent);

                if (jsonObj == null)
                {
                    return false;
                }

                _tasks.Clear();
                tasksList.Items.Clear();
                var issues = jsonObj["issues"];
                foreach (var issue in issues)
                {
                    string key = (string)issue["key"];
                    string status = (string)issue["fields"]["status"]["name"];
                    string title = (string)issue["fields"]["summary"];
                    _tasks.Add(key, (title, status));  
                }

                if (_tasks.Count > 0)
                {
                    foreach (var item in _tasks)
                    {
                        tasksList.Items.Add($"{item.Value} ({item.Key})");
                    }
                }
                


                return true;
            }
            catch
            {
                MessageBox.Show("Не вдалось отримати список завдань");
                return false;

            }
            
        }

        private async Task<bool> GetJiraActiveSprints()
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
                HttpResponseMessage response = await client.GetAsync(_jiraBaseUrl + $"rest/api/3/search?project%3D{_selectedProjectKey}%20AND%20(status%3DDONE)");
                string resultContent = await response.Content.ReadAsStringAsync();


                dynamic jsonObj = JsonConvert.DeserializeObject(resultContent);

                if (jsonObj == null)
                {
                    return false;
                }

                _tasks.Clear();
                tasksList.Items.Clear();
                var issues = jsonObj["issues"];
                foreach (var issue in issues)
                {
                    string key = (string)issue["key"];
                    string status = (string)issue["fields"]["status"]["name"];
                    string title = (string)issue["fields"]["summary"];
                    _tasks.Add(key, (title, status));
                }

                if (_tasks.Count > 0)
                {
                    foreach (var item in _tasks)
                    {
                        tasksList.Items.Add($"{item.Value} ({item.Key})");
                    }
                }



                return true;
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
                HttpResponseMessage response = await client.GetAsync(_jiraBaseUrl + "/rest/auth/1/session");

                if (response.IsSuccessStatusCode)
                {
                    await UpdateJiraCredentials();
                    return true;
                }
            }
            catch(Exception e)
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
            _jiraBaseUrl = jiraBaseUrl_TextBox.Text;
        }

        private async void projectsList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var select = projectsList.SelectedItem.ToString();
            select = select.Split(" ")[1];
            select = select.Substring(1, select.Length - 2);
            _selectedProjectKey = select;
            //await GetJiraTasks();
        }

        private async void refreshButton_Click(object sender, RoutedEventArgs e)
        {
            Refresh();
        }

        public async void Refresh()
        {
            Mouse.OverrideCursor = System.Windows.Input.Cursors.Wait;
            UpdateCredentials();

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
    }
}
