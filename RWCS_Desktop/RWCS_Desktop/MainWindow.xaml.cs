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
            
        }

        private async void Initialization()
        {
            usernameInfo.Content = Replace_UnderScore_WithTwo(_userName);
            jiraApiKey_TextBox.Text = _jiraApiKey;
            jiraBaseUrl_TextBox.Text = _jiraBaseUrl;
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

            List<string> result = new List<string>();

            HttpClient client = new HttpClient();
            var plainTextBytes = System.Text.Encoding.UTF8.GetBytes($"{_email}:{_jiraApiKey}");
            string val = System.Convert.ToBase64String(plainTextBytes);
            client.DefaultRequestHeaders.Add("Authorization", "Basic " + val);
            HttpResponseMessage response = await client.GetAsync(_jiraBaseUrl + "rest/api/3/project/");
            string resultContent = await response.Content.ReadAsStringAsync();
            

            dynamic jsonObj = JsonConvert.DeserializeObject(resultContent);

            if(jsonObj == null)
            {
                return false;
            }

            foreach(var item in jsonObj)
            {
                var tmp4 = (string)item["key"];
                var tmp1 = item.GetType();
                var tmp2 = item.GetProperty("key");
                var tmp3 = tmp2.GetValue(item, null);
                result.Add(item.GetType().GetProperty("key").GetValue(item, null));
            }

            

            return true;
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
            HttpResponseMessage response = await client.GetAsync(url + $"/user/{_userId}");

            string resultContent = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<User>(resultContent);
            if((result.JiraApiKey!= _jiraApiKey || result.JiraBaseUrl!= _jiraBaseUrl) || result.Email != _email)
            {
                result.JiraApiKey = _jiraApiKey;
                result.JiraBaseUrl = _jiraBaseUrl;
                result.Email = _email;

                var newUser = JsonConvert.SerializeObject(result);
                var content = new StringContent(newUser, Encoding.UTF8, "application/json");
                HttpResponseMessage response1 = await client.PutAsync(url + $"/user/{_userId}", content);
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

        private void projectsList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var tmp = ((Label)projectsList.SelectedItem).Content;
        }

        private async void refreshButton_Click(object sender, RoutedEventArgs e)
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
    }
}
