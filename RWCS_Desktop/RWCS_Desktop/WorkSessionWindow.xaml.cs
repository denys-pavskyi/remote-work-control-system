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
    /// Interaction logic for WorkSessionWindow.xaml
    /// </summary>
    public partial class WorkSessionWindow : Window
    {

        private WorkSession _workSession;
        string _fullName = "";
        public string url = "https://localhost:44395/api";
        private int ind = 0;

        private List<string> URLs = new List<string>();

        public WorkSessionWindow(WorkSession workSession, string fullName)
        {
            InitializeComponent();

            _workSession = workSession;
            _fullName = fullName;
            Initialize();

            
            

        }

        public void Initialize()
        {
            nameLabel.Content = _fullName;
            workSessionIdLabel.Content = _workSession.Id;
            projectMemberIdLabel.Content = _workSession.ProjectMemberId;
            taskLabel.Content = _workSession.TaskKey;
            sprintLabel.Content = _workSession.SprintKey;
            dateLabel.Content = $"({_workSession.StartDate.ToString()})-({_workSession.EndDate.ToString()})";
            workTimeLabel.Content = _workSession.WorkTime;
        }


        


        private void closeButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

       

        private void nextScreenshotButton_Click(object sender, RoutedEventArgs e)
        {
            ind++;

            if (ind == URLs.Count)
            {
                ind = 0;
            }
            BitmapImage screenshot = new BitmapImage();
            screenshot.BeginInit();
            screenshot.UriSource = new Uri(URLs[ind]);
            screenshot.EndInit();
            screenshotImage.Source = screenshot;
        }

        private void prevScreenshotButton_Click(object sender, RoutedEventArgs e)
        {
            ind--;

            if (ind == -1)
            {
                ind = URLs.Count-1;
            }
            BitmapImage screenshot = new BitmapImage();
            screenshot.BeginInit();
            screenshot.UriSource = new Uri(URLs[ind]);
            screenshot.EndInit();
            screenshotImage.Source = screenshot;
        }

        private async void loadScreenshotsButton_Click(object sender, RoutedEventArgs e)
        {
            HttpClient client = new HttpClient();


            if (_workSession.ScreenActivityFolder != "-")
            {
                HttpResponseMessage response = await client.GetAsync($"{url}/screenshot/fromFolder/{_workSession.ScreenActivityFolder}");
                if (response.IsSuccessStatusCode)
                {
                    string resultContent = await response.Content.ReadAsStringAsync();
                    dynamic jsonObj = JsonConvert.DeserializeObject(resultContent);

                    URLs.Clear();
                    foreach (var item in jsonObj)
                    {
                        URLs.Add(item.ToString());
                    }

                    if (URLs.Count < 1)
                    {
                        screenshotStatus.Content = "Відсутні знімки для цієї сесії";
                    }
                    else
                    {
                        ind = 0;
                        BitmapImage screenshot = new BitmapImage();
                        screenshot.BeginInit();
                        screenshot.UriSource = new Uri(URLs[ind]);
                        screenshot.EndInit();
                        screenshotImage.Source = screenshot;
                        
                        screenshotStatus.Content = "";
                    }
                }
                else
                {
                    screenshotStatus.Content = "Не вдалось завантажити знімки";
                }
            }
            else
            {
                screenshotStatus.Content = "Для цього проекту вимкнений моніторинг активності екрану";
            }
            
        }
    }
}
