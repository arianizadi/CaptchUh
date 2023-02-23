using System;
using System.IO;
using System.Windows;
using System.Windows.Input;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium;
using System.Threading;
using OpenQA.Selenium.Support.UI;
using System.Net;
using System.Collections.Specialized;

namespace CaptchUh
{
    public partial class MainWindow : Window
    {
        public string userEMAIL = "";
        public string userDISCORD = "";
        public string userPASSWORD = "";
        public bool isDone = true;
        public bool isReallyDone = true;
        public string errorWebhook = "https://discordapp.com/api/webhooks/553015394273853440/k7QqQwflXwKesVhNNCewBJNsbAGwLUGcrhy--5XbZcHq_eJU1BKvvNlcgSYOLJ2mgfCO";
        public bool shouldQuit = false;

        protected override void OnMouseLeftButtonDown(MouseButtonEventArgs e)
        {
            base.OnMouseLeftButtonDown(e);
            this.DragMove();
        }

        public MainWindow()
        {
            InitializeComponent();
            WindowStartupLocation = System.Windows.WindowStartupLocation.CenterScreen;
            FirstTaskStop.IsEnabled = false;
            FirstTaskStart.IsEnabled = true;
            try
            {
                TextReader tr1 = new StreamReader("SavedInfo.txt");
                userEMAIL = tr1.ReadLine();
                userPASSWORD = tr1.ReadLine();
                tr1.Close();
                TextReader tr2 = new StreamReader("SavedLogin.txt");
                userDISCORD = tr2.ReadLine();
                tr2.Close();
            } catch
            {

            }

        }

        private void GenMenuBtn_Click(object sender, RoutedEventArgs e)
        {
            SettingsMenuBtn.IsEnabled = true;
            GenMenuBtn.IsEnabled = false;
            ProxiesMenuBtn.IsEnabled = true;

            TasksTab.IsSelected = true;
        }

        private void SettingsMenuBtn_Click(object sender, RoutedEventArgs e)
        {
            SettingsMenuBtn.IsEnabled = false;
            GenMenuBtn.IsEnabled = true;
            ProxiesMenuBtn.IsEnabled = true;
            SettingsTab.IsSelected = true;
        }

        private void ProxiesMenuBtn_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Not Available Yet ;)");
        }
        
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (isReallyDone == false)
            {
                MessageBox.Show("Stop Your Task / Wait For Cleanup To End!");
            } else
            {
                System.Windows.Application.Current.Shutdown();
            }
        }

        private void SetUserAccountBtn_Click(object sender, RoutedEventArgs e)
        {
            if (EmailTextBox.Text.ToLower() == "email" || EmailTextBox.Text == "" || PasswordTextBox.Text.ToLower() == "password" || PasswordTextBox.Text == "")
            {
                MessageBox.Show("How sped are you? Put ur email and password in the textboxes smh. I should take ur key just for this but its alright. I forgive u.");
            } else
            {
                MessageBox.Show("Disable Phone Auth on Google Account Before Running :)");
                userPASSWORD = PasswordTextBox.Text;
                userEMAIL = EmailTextBox.Text;
                try
                {
                    TextWriter tw = new StreamWriter("SavedInfo.txt");
                    tw.WriteLine(userEMAIL);
                    tw.WriteLine(userPASSWORD);
                    tw.Close();
                } catch
                {
                    
                }
                FirstTaskLbl.Content = $"{userEMAIL}";
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            if (userEMAIL != "")
            {
                FirstTaskLbl.Content = $"{userEMAIL}";
            }
        }

        private void FirstTaskStop_Click(object sender, RoutedEventArgs e)
        {
            isDone = true;
            FirstTaskStop.IsEnabled = false;
        }

        private void FirstTaskStart_Click(object sender, RoutedEventArgs e)
        {
            isDone = false;
            Thread thread = new Thread(() =>
            {
                BrowseThread();
            });
            thread.Start();
            FirstTaskStart.IsEnabled = false;
        }

        public static void ExplicitWait(ChromeDriver driver, String elementID)
        {
            if (elementID == "password")
            {
                new WebDriverWait(driver, TimeSpan.FromSeconds(60)).Until(ExpectedConditions.ElementToBeClickable(By.Name(elementID)));
            } else
            {
                new WebDriverWait(driver, TimeSpan.FromSeconds(60)).Until(ExpectedConditions.ElementToBeClickable(By.Id(elementID)));
            }
        }

        public void BrowseThread()
        {
            isReallyDone = false;
            isDone = false;
            Dispatcher.BeginInvoke(new Action(() =>
            {
                this.FirstTaskStop.IsEnabled = true;
                this.FirstTaskStart.IsEnabled = false;
            }));

            ChromeOptions options = new ChromeOptions();
            //options.AddArgument("--headless");
            options.AddArgument("--user-agent=Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/72.0.3626.109 Safari/537.36");
            ChromeDriverService service = ChromeDriverService.CreateDefaultService();
            service.HideCommandPromptWindow = true;
            ChromeDriver driver = new ChromeDriver(service, options);
            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(60);
            try
            {
                Dispatcher.BeginInvoke(new Action(() =>
                {
                    this.FirstTaskLbl.Content = "Drivers Initialized";
                    this.logTextBox.AppendText("Task 1: Drivers Initialized\n");
                }));

                Dispatcher.BeginInvoke(new Action(() =>
                {
                    this.FirstTaskLbl.Content = "Logging Into " + userEMAIL.ToString();
                    this.logTextBox.AppendText("Task 1: Logging Into " + userEMAIL.ToString() + "\n");
                }));
                if (isDone == false)
                {
                    driver.Navigate().GoToUrl("https://www.google.com/");
                    ExplicitWait(driver, "gb_70");
                    driver.FindElementById("gb_70").Click();
                    System.Threading.Thread.Sleep(4000);
                    ExplicitWait(driver, "identifierId");
                    driver.FindElementById("identifierId").SendKeys(userEMAIL);
                    System.Threading.Thread.Sleep(4000);
                    ExplicitWait(driver, "identifierNext");
                    driver.FindElementById("identifierNext").Click();
                    System.Threading.Thread.Sleep(4000);
                    ExplicitWait(driver, "password");
                    driver.FindElementByName("password").SendKeys(userPASSWORD);
                    System.Threading.Thread.Sleep(4000);
                    ExplicitWait(driver, "passwordNext");
                    driver.FindElementById("passwordNext").Click();
                    System.Threading.Thread.Sleep(4000);
                }
                
                if (driver.Url == "https://www.google.com/" && isDone == false)
                {
                    Dispatcher.BeginInvoke(new Action(() =>
                    {
                        this.FirstTaskLbl.Content = "Logged Into " + userEMAIL.ToString();
                        this.logTextBox.AppendText("Task 1: Logged Into " + userEMAIL.ToString() + "\n");
                    }));
                    System.Threading.Thread.Sleep(1000);
                    Dispatcher.BeginInvoke(new Action(() =>
                    {
                        this.FirstTaskLbl.Content = "Starting Activity";
                        this.logTextBox.AppendText("Task 1: Starting Activity\n");
                    }));
                    driver.Navigate().GoToUrl("https://www.youtube.com/watch?v=DdjZL9R1cPI&list=PLv3TTBr1W_9tppikBxAE_G6qjWdBljBHJ");
                } else
                {
                    Dispatcher.BeginInvoke(new Action(() =>
                    {
                        this.FirstTaskLbl.Content = "Failed To Log In To " + userEMAIL.ToString();
                        this.logTextBox.AppendText("Task 1: Failed To Log Into " + userEMAIL.ToString() + "\n");
                    }));
                    isDone = true;
                }
                Dispatcher.BeginInvoke(new Action(() =>
                {
                    this.FirstTaskLbl.Content = "Stealing One Clicks From Google's Back Pocket";
                }));
                while (isDone == false)
                {
                    for (int x = 0; x <= 120; x++)
                    {
                        if (isDone == false)
                        {
                            System.Threading.Thread.Sleep(500);
                        }
                        else
                        {
                            x = 1000;
                        }
                    }
                }

                Dispatcher.BeginInvoke(new Action(() =>
                {
                    this.FirstTaskLbl.Content = "Cleaning Up";
                    this.logTextBox.AppendText("Task 1: Cleaning Up\n");
                }));

                System.Threading.Thread.Sleep(500);

                Dispatcher.BeginInvoke(new Action(() =>
                {
                    this.FirstTaskLbl.Content = userEMAIL.ToString();
                }));

                driver.Close();
                driver.Dispose();

            } catch (Exception ex)
            {
                driver.Close();
                driver.Dispose();
                Dispatcher.BeginInvoke(new Action(() =>
                {
                    this.FirstTaskLbl.Content = userEMAIL.ToString();
                    this.logTextBox.AppendText("Task 1: Error Occurred\n");
                }));
                using (WebClient webClient = new WebClient())
                {
                    NameValueCollection data3 = new NameValueCollection()
                    {
                        ["username"] = userEMAIL.ToString(),
                        ["content"] = ex.ToString()
                    };
                    webClient.UploadValues(errorWebhook, "POST", data3);
                }
                isDone = true;
            }
            Dispatcher.BeginInvoke(new Action(() =>
            {
                this.FirstTaskStop.IsEnabled = false;
                this.FirstTaskStart.IsEnabled = true;
                this.logTextBox.AppendText("\n");
            }));
            isReallyDone = true;

        }

        private void MiniButton_Click(object sender, RoutedEventArgs e)
        {
            base.WindowState = WindowState.Minimized;
        }
    }
}
