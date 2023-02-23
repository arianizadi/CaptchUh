using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Net;
using System.Text;
using System.Windows;
using System.Windows.Input;

namespace CaptchUh
{
    public partial class LoginWindow : Window
    {

        private bool isLoggedIn = false;
        private string externalip;
        public string errorWebhook = "https://discordapp.com/api/webhooks/553015394273853440/k7QqQwflXwKesVhNNCewBJNsbAGwLUGcrhy--5XbZcHq_eJU1BKvvNlcgSYOLJ2mgfCO";


        protected override void OnMouseLeftButtonDown(MouseButtonEventArgs e)
        {
            base.OnMouseLeftButtonDown(e);
            this.DragMove();
        }

        public LoginWindow()
        {
            externalip = new WebClient().DownloadString("https://ipv4.icanhazip.com/");
            InitializeComponent();
            WindowStartupLocation = System.Windows.WindowStartupLocation.CenterScreen;
            try
            {
                TextReader tr = new StreamReader("SavedLogin.txt");
                DiscordTextBox.Text = tr.ReadLine();
                KeyTextBox.Text = tr.ReadLine();
                tr.Close();
            }
            catch
            {
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {

        }

        private void VerifyButton_Click(object sender, RoutedEventArgs e)
        {
            VerifyButton.IsEnabled = false;
            try
            {
                string username = Environment.UserName;
                using (WebClient webClient = new WebClient())
                {
                    NameValueCollection data1 = new NameValueCollection()
                    {
                        ["username"] = DiscordTextBox.Text,
                        ["password"] = KeyTextBox.Text,
                        ["submit"] = "submit"
                    };
                    string str = Encoding.UTF8.GetString(webClient.UploadValues("https://vernal-mark.000webhostapp.com/asdkfjkdsjf.php", "POST", data1));
                    string userHWID2 = GetMachineGuid().ToString();

                    if (str.Contains("Key Is Real"))
                    {
                        using (new WebClient())
                        {
                            NameValueCollection data2 = new NameValueCollection()
                            {
                                ["username"] = DiscordTextBox.Text,
                                ["content"] =  "Authenticated!\nHWID: " + userHWID2 + "\nIP: " + externalip
                            };
                            webClient.UploadValues("https://discordapp.com/api/webhooks/536761618571198464/QGlJ93wU590cK7jlZqPWhxpcVHd6Zobet5XgTg53SgzdRDAG8uz59I1TagoLLyebWP2u", "POST", data2);
                        }
                        isLoggedIn = true;
                    } else
                    {
                        LoginTitle.Content = "smh how did u forget ur key?";
                        string userHWID = GetMachineGuid().ToString();

                        using (new WebClient())
                        {
                            NameValueCollection data3 = new NameValueCollection()
                            {
                                ["username"] = DiscordTextBox.Text,
                                ["content"] = "Failed Authentication!\nUser Info: " + username + "\nIP: " + externalip + "HWID: " + userHWID + "\nKey Used: " + KeyTextBox.Text
                            };
                            webClient.UploadValues("https://discordapp.com/api/webhooks/536761618571198464/QGlJ93wU590cK7jlZqPWhxpcVHd6Zobet5XgTg53SgzdRDAG8uz59I1TagoLLyebWP2u", "POST", data3);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                using (WebClient webClient = new WebClient())
                {
                    NameValueCollection data2 = new NameValueCollection()
                    {
                        ["username"] = DiscordTextBox.Text,
                        ["content"] = ex.ToString()
                    };
                    webClient.UploadValues(errorWebhook, "POST", data2);
                }
            }
            if (isLoggedIn)
            {
                try
                {
                    TextWriter tw = new StreamWriter("SavedLogin.txt");
                    tw.WriteLine(DiscordTextBox.Text.ToString());
                    tw.WriteLine(KeyTextBox.Text.ToString());
                    tw.Close();
                }
                catch (Exception ex)
                {
                    using (WebClient webClient = new WebClient())
                    {
                        NameValueCollection data2 = new NameValueCollection()
                        {
                            ["username"] = DiscordTextBox.Text,
                            ["content"] = ex.ToString()
                        };
                        webClient.UploadValues(errorWebhook, "POST", data2);
                    }
                }
                this.Hide();
                MainWindow mainWindow = new MainWindow();
                this.Close();
                mainWindow.Show();
            }
            VerifyButton.IsEnabled = true;
        }

        public string GetMachineGuid()
        {
            string location = @"SOFTWARE\Microsoft\Cryptography";
            string name = "MachineGuid";

            using (RegistryKey localMachineX64View =
                RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry64))
            {
                using (RegistryKey rk = localMachineX64View.OpenSubKey(location))
                {
                    if (rk == null)
                        throw new KeyNotFoundException(
                            string.Format("Key Not Found: {0}", location));

                    object machineGuid = rk.GetValue(name);
                    if (machineGuid == null)
                        throw new IndexOutOfRangeException(
                            string.Format("Index Not Found: {0}", name));

                    return machineGuid.ToString();
                }
            }
        }
    }
}
