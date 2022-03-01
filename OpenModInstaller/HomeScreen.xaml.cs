//Based on OpenInstaller
using ChaseLabs.CLUpdate;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;
using NHotkey;
using NHotkey.Wpf;

namespace OpenModInstaller
{
    /// <summary>
    /// Interaction logic for HomeScreen.xaml
    /// </summary>
    public partial class HomeScreen : Page
    {
        [DllImport("wininet.dll")]
        private extern static bool InternetGetConnectedState(out int conn, int val);
        Dispatcher dis = Dispatcher.CurrentDispatcher;
        public HomeScreen()
        {
            InitializeComponent();
            HotkeyManager.Current.AddOrReplace("Increment", Key.D, ModifierKeys.Control, OnIncrement);
            Thread thread = new Thread(Animate);
            thread.Start();
            Thread thread2 = new Thread(UpdateUI);
            thread2.Start();
        }
        private void BackAbout_Click(object sender, RoutedEventArgs e)
        {
            Thread thread = new Thread(Install);
            thread.Start();
        }
        //Threading starts here -- 8/2/2022@03:15, YAG-dev, 22.2+
        private void Animate()
        {
            string IsAnimated = System.IO.Path.Combine(Environment.CurrentDirectory, @"..\..\", "UpdateInfo", "Settings", "NotAnimated.txt");
            if (File.Exists(IsAnimated))
            {

            }
            else
            {
                this.Dispatcher.Invoke(() =>
                {
                    GridMain.Opacity = 0;
                    Grid r = (Grid)GridMain;
                    DoubleAnimation animation = new DoubleAnimation(1, TimeSpan.FromMilliseconds(250));
                    r.BeginAnimation(Grid.OpacityProperty, animation);
                });
            }
        }
        private void OnIncrement(object sender, HotkeyEventArgs e)
        {
            PhoneInfo.Visibility = Visibility.Visible;
            Title_Copy.Visibility = Visibility.Visible;
            DbgRectangle.Visibility = Visibility.Visible;
            BugReport_Copy.Visibility = Visibility.Visible;
            BugReport.Visibility = Visibility.Visible;
        }
        private void UpdateUI()
        {
            this.Dispatcher.Invoke(() =>
            {
                PhoneInfo.Visibility = Visibility.Hidden;
                Title_Copy.Visibility = Visibility.Hidden;
                DbgRectangle.Visibility = Visibility.Hidden;
                BugReport_Copy.Visibility = Visibility.Hidden;
                BugReport.Visibility = Visibility.Hidden;
                if (SourceChord.FluentWPF.SystemTheme.AppTheme == SourceChord.FluentWPF.ApplicationTheme.Dark)
                {
                    DeviceInfoImg_Copy.Source = (ImageSource)new ImageSourceConverter().ConvertFrom(new Uri(@"pack://application:,,,/OpenModInstaller;Component/tt-down-dark.png"));
                }
                else
                {
                    DeviceInfoImg_Copy.Source = (ImageSource)new ImageSourceConverter().ConvertFrom(new Uri(@"pack://application:,,,/OpenModInstaller;Component/tt-down-light.png"));
                }
            });
        }
        private void Install()
        {
            int Out;
            if (InternetGetConnectedState(out Out, 0) == true)
            {
                string url = "https://www.dropbox.com/s/s2g6cqgztwzf50f/release.zip?dl=1";
                string remote_version_url = "https://www.dropbox.com/s/iavj5ah7cy7v9ei/version.txt?dl=1";
                string version_key = "Server: ";
                string update_path = System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), ".minecraft", "Update", "Download");
                string application_path = System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), ".minecraft", "UpdateFiles");
                string local_version_path = System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), ".minecraft", "UpdateInfo", "CurrentVersion", "VersionString.txt");
                string launch_exe = "TrebleToolkitLauncher.exe";
                this.Dispatcher.Invoke(() =>
                {
                    if (PhoneInfo.IsChecked == true)
                    {
                        url = BugReport.Text;
                        remote_version_url = BugReport_Copy.Text;
                    }
                });
                System.Diagnostics.Process process = new System.Diagnostics.Process();
                System.Diagnostics.ProcessStartInfo startInfo = new System.Diagnostics.ProcessStartInfo();
                Task.Run(() =>
                {
                    dis.Invoke(() =>
                    {
                        DeviceSpecificFeatures_Copy.IsEnabled = false;
                        DeviceSpecificFeatures_Copy.Content = "Preparing Folders...";
                        status_pgr.Visibility = Visibility.Visible;
                        status_pgr.Value = 0;
                        status_pgr.Value += 0;
                    }, DispatcherPriority.Normal);
                    var update = Updater.Init(url, update_path, application_path, launch_exe);

                    if (UpdateManager.CheckForUpdate(version_key, local_version_path, remote_version_url))
                    {
                        dis.Invoke(() =>
                        {
                            DeviceSpecificFeatures_Copy.Content = "Downloading...";
                            status_pgr.Value += 20;
                        }, DispatcherPriority.Normal);

                        update.Download();

                        dis.Invoke(() =>
                        {
                            DeviceSpecificFeatures_Copy.Content = "Decompressing...";
                            status_pgr.Value += 20;
                        }, DispatcherPriority.Normal);

                        update.Unzip();

                        dis.Invoke(() =>
                        {
                            DeviceSpecificFeatures_Copy.Content = "Cleaning Up...";
                            status_pgr.Value += 20;
                        }, DispatcherPriority.Normal);

                        update.CleanUp();


                        dis.Invoke(() =>
                        {
                            DeviceSpecificFeatures_Copy.Content = "Running Modpack specific scripts...";
                            status_pgr.Value += 20;
                        }, DispatcherPriority.Normal);

                        using (var client = new System.Net.WebClient())
                        {

                        }

                    }
                    if(File.Exists(System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), ".minecraft", "UpdateFiles", "script.txt")))
                    {
                        string script = File.ReadAllText(System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), ".minecraft", "UpdateFiles", "script.txt"));
                        if(Title_Copy.Visibility == Visibility.Visible)
                        {
                            startInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Normal;
                        }
                        else
                        {
                            startInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
                        }
                        startInfo.WorkingDirectory = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
                        startInfo.FileName = "cmd.exe";
                        startInfo.Arguments = script;
                        process.StartInfo = startInfo;
                        process.Start();
                        process.WaitForExit();
                    }
                    else
                    {
                        if (Title_Copy.Visibility == Visibility.Visible)
                        {
                            startInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Normal;
                        }
                        else
                        {
                            startInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
                        }
                        startInfo.WorkingDirectory = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
                        startInfo.FileName = "cmd.exe";
                        startInfo.Arguments = "/C cd .minecraft & RD /s /q mods";
                        process.StartInfo = startInfo;
                        process.Start();
                        process.WaitForExit();
                    }
                    System.Diagnostics.Process process1 = new System.Diagnostics.Process();
                    System.Diagnostics.ProcessStartInfo startInfo1 = new System.Diagnostics.ProcessStartInfo();
                    if (Title_Copy.Visibility == Visibility.Visible)
                    {
                        startInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Normal;
                    }
                    else
                    {
                        startInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
                    }
                    startInfo.FileName = "cmd.exe";
                    startInfo.Arguments = "/C cd %AppData% & cd .minecraft & cd UpdateFiles & cd .. & robocopy /E UpdateFiles . & RD /s /q UpdateFiles";
                    process.StartInfo = startInfo;
                    process.Start();
                    process.WaitForExit();
                    dis.Invoke(() =>
                    {
                        DeviceSpecificFeatures_Copy.Content = "Install Completed";
                        DeviceSpecificFeatures_Copy.IsEnabled = true;
                        status_pgr.Value += 20;
                    }, DispatcherPriority.Normal);
                });
            }
            else
            {

            }
        }
    }
}
