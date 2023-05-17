using Newtonsoft.Json;
using RWCS_Desktop.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
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
    /// Interaction logic for RegistrationWindow.xaml
    /// </summary>
    public partial class RegistrationWindow : Window
    {

        public string url = "https://localhost:44395/api";

        public RegistrationWindow()
        {
            InitializeComponent();
        }

        private async void registerButton_Click(object sender, RoutedEventArgs e)
        {
            Mouse.OverrideCursor = System.Windows.Input.Cursors.Wait;
            HttpClient client = new HttpClient();


            string firstName = firstNameBox.Text.Trim();
            string lastName = lastNameBox.Text.Trim();
            string userName = userNameBox.Text.Trim();
            string password = passwordBox.Text.Trim();
            string email = emailBox.Text.Trim();


            User newUser = new User() { 
                            FirstName = firstName, 
                            LastName = lastName, 
                            UserName = userName, 
                            Password = password, 
                            Email = email, 
                            JiraApiKey = "-", 
                            JiraBaseUrl = "-",
                            ProjectMemberIds = new List<int>()
            };
            var jsonLogin = JsonConvert.SerializeObject(newUser);
            var content = new StringContent(jsonLogin, Encoding.UTF8, "application/json");

            try
            {
                HttpResponseMessage response = await client.PostAsync(url + "/user", content);

                if (response.IsSuccessStatusCode)
                {
                    errorLabel.Foreground = Brushes.Green;
                    errorLabel.Content = "Реєстрація пройшла успішно!";
                    Mouse.OverrideCursor = System.Windows.Input.Cursors.Arrow;
                    Thread.Sleep(2000);
                    AuthWindow newWindow = new AuthWindow();
                    newWindow.Show();
                    this.Close();
                }
                else
                {
                    errorLabel.Foreground = Brushes.Red;
                    errorLabel.Content = "Сталася помилка";
                }
            }
            catch
            {
                errorLabel.Foreground = Brushes.Red;
                errorLabel.Content = "Помилка підключення до сервера";
            }
            Mouse.OverrideCursor = System.Windows.Input.Cursors.Arrow;

            
        }

        private void cancelButton_Click(object sender, RoutedEventArgs e)
        {
            AuthWindow newWindow = new AuthWindow();
            newWindow.Show();
            this.Close();
        }
    }
}
