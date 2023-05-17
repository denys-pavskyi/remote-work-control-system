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

namespace RWCS_Desktop
{
    /// <summary>
    /// Interaction logic for SettingsWindow.xaml
    /// </summary>
    public partial class SettingsWindow : Window
    {
        private Project _project;
        public string url = "https://localhost:44395/api";


        public SettingsWindow(Project project)
        {
            InitializeComponent();
            _project = project;
            Initialize();
        }

        private async void Initialize()
        {
            makeScreenshotsCheckBox.IsChecked = _project.IsScreenActivityControlEnabled;
            intervalSlider.Value = _project.ScreenshotInterval;
            intervalSlider.ValueChanged += intervalSlider_ValueChanged;
            
            currentIntervalLabel.Content = intervalSlider.Value;
        }

        private void closeButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private async void saveButton_Click(object sender, RoutedEventArgs e)
        {
            Mouse.OverrideCursor = System.Windows.Input.Cursors.Wait;
            HttpClient client = new HttpClient();

            Project newProject = new Project()
            {
                Id = _project.Id,
                IsScreenActivityControlEnabled = (bool)makeScreenshotsCheckBox.IsChecked,
                ScreenshotInterval = (int)intervalSlider.Value,
                JiraDomain = _project.JiraDomain,
                ProjectKey = _project.ProjectKey,
                ProjectMemberIds = _project.ProjectMemberIds,
                ProjectTitle = _project.ProjectTitle
            };

            var json = JsonConvert.SerializeObject(newProject);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            try
            {
                HttpResponseMessage response = await client.PutAsync(url + $"/project/{_project.Id}", content);

                if (response.IsSuccessStatusCode)
                {

                    string resultContent = await response.Content.ReadAsStringAsync();
                    var result = JsonConvert.DeserializeObject<LoginResponse>(resultContent);

                    Mouse.OverrideCursor = System.Windows.Input.Cursors.Arrow;

                    
                }
                
            }
            catch
            {
            }
            Mouse.OverrideCursor = System.Windows.Input.Cursors.Arrow;
        }

        private void intervalSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            currentIntervalLabel.Content = ((int)intervalSlider.Value).ToString();
        }
    }
}
