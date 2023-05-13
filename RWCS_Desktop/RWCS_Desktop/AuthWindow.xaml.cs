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
    /// Interaction logic for AuthWindow.xaml
    /// </summary>
    public partial class AuthWindow : Window
    {
        public string url = "https://localhost:44395/api";
        public AuthWindow()
        {
            InitializeComponent();
        }

        private async void log_in_Button_Click(object sender, RoutedEventArgs e)
        {
            Mouse.OverrideCursor = System.Windows.Input.Cursors.Wait;
            HttpClient client = new HttpClient();

            var request = new LoginRequest { UserName = loginTextBox.Text, Password = passwordTextBox.Text };

            var jsonLogin = JsonConvert.SerializeObject(request);
            var content = new StringContent(jsonLogin, Encoding.UTF8, "application/json");
            HttpResponseMessage response = await client.PostAsync(url+"/login", content);

            if (response.IsSuccessStatusCode)
            {
                messageText.Foreground = Brushes.Green;
                messageText.Content = "Success";
                string resultContent = await response.Content.ReadAsStringAsync();
                var result = JsonConvert.DeserializeObject<LoginResponse>(resultContent);
                Mouse.OverrideCursor = System.Windows.Input.Cursors.Arrow;

                MainWindow window = new MainWindow(result.Id, result.Username);
                this.Close();
                window.Show();
            }
            else
            {
                messageText.Foreground =  Brushes.Red;
                messageText.Content = "Wrong username or password";
            }
            
           

        }

        
    }
}
