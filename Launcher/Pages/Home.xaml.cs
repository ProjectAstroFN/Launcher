using System;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using Meowscles.Services;
using Meowscles.Services.Launch;
using System.Runtime.InteropServices;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ListView;
using DnsClient;
using System.Linq;
using System.Windows.Threading;
using System.IO.Compression;
using System.Windows.Forms;
using MessageBox = System.Windows.Forms.MessageBox;
using UserControl = System.Windows.Controls.UserControl;

public class Vars
{
    public static string Email = "NONE";
    public static string Password = "NONE";
    public static string Path = "NONE";
}

/*public class Launcher
{

    [DllImport("user32.dll")]
    static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);

    public static void Launch()
    {
        try
        {
            if (Vars.Path == "NONE")
            {
                Vars.Path = UpdateINI.ReadValue("Auth", "Path");
            }
            string GamePath = Vars.Path;
            if (GamePath != "NONE") // NONE THE BEST RESPONSE!
            {
                //MessageBox.Show(Path69);
                if (File.Exists(System.IO.Path.Combine(GamePath, "FortniteGame\\Binaries\\Win64\\FortniteClient-Win64-Shipping.exe")))
                {
                    if (Vars.Email == "NONE")
                    {
                        Vars.Email = UpdateINI.ReadValue("Auth", "Email");
                    }
                    if (Vars.Password == "NONE")
                    {
                        Vars.Password = UpdateINI.ReadValue("Auth", "Password");
                    }
                    if (Vars.Email == "NONE" || Vars.Password == "NONE")
                    {
                        MessageBox.Show("Please add your Meowscles info in settings!");
                        //this.Close();
                        //(new Form2()).Show();
                        return;
                    }
                    foreach (var proc in Process.GetProcessesByName("MeowsclesEAC"))
                    {
                        if (proc.MainModule.FileName.StartsWith(GamePath))
                        {
                            proc.Kill();
                            proc.WaitForExit();
                        }
                    }
                    foreach (var proc in Process.GetProcessesByName("FortniteClient-Win64-Shipping"))
                    {
                        try
                        {
                            if (proc.MainModule.FileName.StartsWith(GamePath))
                            {
                                proc.Kill();
                                proc.WaitForExit();
                            }
                        } catch {}
                    }
                    WebClient Client = new WebClient();
                    var lookup = new LookupClient();
                    var result = lookup.Query("meowscles.ploosh.dev", QueryType.TXT);
                    var record = result.Answers.TxtRecords().FirstOrDefault();
                    var ip = record?.Text.FirstOrDefault();
                    //Client.DownloadFile("https://cdn.discordapp.com/attachments/883144741838020629/1174502844636856380/Cobalt.dll", Path.Combine(Path69, "Engine\\Binaries\\ThirdParty\\NVIDIA\\NVaftermath\\Win64", "GFSDK_Aftermath_Lib.x64.dll"));
                    if (!File.Exists(System.IO.Path.Combine(GamePath, "MeowsclesEAC.exe")))
                    {
                        Client.DownloadFile($"{ip}/assets/MeowsclesEAC.zip", Path.Combine(GamePath, "MeowsclesEAC.zip"));
                        System.IO.Compression.ZipFile.ExtractToDirectory(Path.Combine(GamePath, "MeowsclesEAC.zip"), GamePath);
                        File.Delete(Path.Combine(GamePath, "MeowsclesEAC.zip"));
                        if (File.Exists(System.IO.Path.Combine(GamePath, "MeowsclesEAC.exe")))
                        {
                            var proc = new Process()
                            {
                                StartInfo = new ProcessStartInfo()
                                {
                                    Arguments = $"install bff26fa986424f4b8fa9dd3b6e853df8",
                                    FileName = Path.Combine(GamePath, "EasyAntiCheat", "EasyAntiCheat_EOS_Setup.exe")
                                },
                                EnableRaisingEvents = true
                            };
                            proc.Start();
                            proc.WaitForExit();
                        } else
                        {
                            MessageBox.Show("Failed to download/extract Meowscles EAC!");
                        }
                    }
                    if (!File.Exists(Path.Combine(GamePath, "Engine\\Binaries\\ThirdParty\\NVIDIA\\NVaftermath\\Win64", "GFSDK_Aftermath_Lib2.x64.dll")))
                    {
                        File.Move(Path.Combine(GamePath, "Engine\\Binaries\\ThirdParty\\NVIDIA\\NVaftermath\\Win64", "GFSDK_Aftermath_Lib.x64.dll"), Path.Combine(GamePath, "Engine\\Binaries\\ThirdParty\\NVIDIA\\NVaftermath\\Win64", "GFSDK_Aftermath_Lib2.x64.dll"));
                    }
                    Client.DownloadFile($"{ip}/assets/sigs.bin", Path.Combine(GamePath, "EasyAntiCheat\\Certificates", "base.bin"));
                    Client.DownloadFile($"{ip}/assets/GFSDK_Aftermath_Lib.x64.dll", Path.Combine(GamePath, "Engine\\Binaries\\ThirdParty\\NVIDIA\\NVaftermath\\Win64", "GFSDK_Aftermath_Lib.x64.dll"));

                    IntPtr h = Process.GetCurrentProcess().MainWindowHandle;
                    ShowWindow(h, 6);
                    //AntiCheat.Start(Path69);
                    Game.Start(GamePath, "-plooshfn -epicapp=Fortnite -epicenv=Prod -epiclocale=en-us -epicportal -nobe -fromfl=eac -fltoken=h1cdhchd10150221h130eB56 -skippatchcheck", Vars.Email, Vars.Password);
                    FakeAC.Start(GamePath, "FortniteClient-Win64-Shipping_EAC.exe", $"-epicapp=Fortnite -epicenv=Prod -epiclocale=en-us -epicportal -nobe -fromfl=eac -fltoken=h1cdhchd10150221h130eB56 -skippatchcheck", "r");
                    FakeAC.Start(GamePath, "FortniteLauncher.exe", $"-epicapp=Fortnite -epicenv=Prod -epiclocale=en-us -epicportal -nobe -fromfl=eac -fltoken=h1cdhchd10150221h130eB56 -skippatchcheck", "dsf");
                    try
                    {
                        Game._FortniteProcess.WaitForExit();
                    } catch (Exception) {}
                    try
                    {
                        FakeAC._FNLauncherProcess.Close();
                        FakeAC._FNAntiCheatProcess.Close();
                    }
                    catch (Exception)
                    {
                        MessageBox.Show("There has been an error closing Fake AC.");
                    }



                    //Injector.Start(PSBasics._FortniteProcess.Id, Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "EonCurl.dll"));
                }
                else
                {
                    MessageBox.Show("Please add your Meowscles info in settings!");
                }
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.ToString());
        }
    }

    public static void Kill()
    {
        try
        {
            Process.GetProcessById(Game._FortniteProcess.Id).Kill();
            FakeAC._FNLauncherProcess.Kill();
            FakeAC._FNAntiCheatProcess.Kill();
        } catch (Exception)
        {

        }
    }
}*/

public static class ZipArchiveExtensions
{
    public static void ExtractToDirectory(this ZipArchive archive, string destinationDirectoryName, bool overwrite)
    {
        if (!overwrite)
        {
            archive.ExtractToDirectory(destinationDirectoryName);
            return;
        }

        DirectoryInfo di = Directory.CreateDirectory(destinationDirectoryName);
        string destinationDirectoryFullPath = di.FullName;

        foreach (ZipArchiveEntry file in archive.Entries)
        {
            string completeFileName = Path.GetFullPath(Path.Combine(destinationDirectoryFullPath, file.FullName));

            if (!completeFileName.StartsWith(destinationDirectoryFullPath, StringComparison.OrdinalIgnoreCase))
            {
                throw new IOException("Trying to extract file outside of destination directory. See this link for more info: https://snyk.io/research/zip-slip-vulnerability");
            }

            if (file.Name == "")
            {// Assuming Empty for Directory
                Directory.CreateDirectory(Path.GetDirectoryName(completeFileName));
                continue;
            }
            File.Delete(completeFileName);
            file.ExtractToFile(completeFileName, true);
        }
    }
}

namespace Meowscles.Pages
{
    /// <summary>
    /// Interaction logic for Home.xaml
    /// </summary>
    public partial class Home : UserControl
	{
        Thread launcherThread;
        bool running = false;
        [DllImport("user32.dll")]
        static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);

        public static void DeleteDirectory(string target_dir)
        {
            string[] files = Directory.GetFiles(target_dir);
            string[] dirs = Directory.GetDirectories(target_dir);

            foreach (string file in files)
            {
                File.SetAttributes(file, FileAttributes.Normal);
                File.Delete(file);
            }

            foreach (string dir in dirs)
            {
                DeleteDirectory(dir);
            }

            Directory.Delete(target_dir, false);
        }

        public void Launch()
        {
            try
            {
                if (running) return;
                running = true;
                if (Vars.Path == "NONE")
                {
                    Vars.Path = UpdateINI.ReadValue("Auth", "Path");
                }
                string GamePath = Vars.Path;
                if (GamePath != "NONE") // NONE THE BEST RESPONSE!
                {
                    //MessageBox.Show(Path69);
                    if (File.Exists(System.IO.Path.Combine(GamePath, "FortniteGame\\Binaries\\Win64\\FortniteClient-Win64-Shipping.exe")))
                    {
                        if (Vars.Email == "NONE")
                        {
                            Vars.Email = UpdateINI.ReadValue("Auth", "Email");
                        }
                        if (Vars.Password == "NONE")
                        {
                            Vars.Password = UpdateINI.ReadValue("Auth", "Password");
                        }
                        if (Vars.Email == "NONE" || Vars.Password == "NONE")
                        {
                            MessageBox.Show("Please add your Meowscles login in settings!");
                            this.Dispatcher.Invoke(() =>
                            {
                                this.Button.Content = "Start Meowscles";
                                this.Button.Click += Button_Click;
                                this.Button.IsEnabled = true;
                            });
                            //this.Close();
                            //(new Form2()).Show();
                            return;
                        }
                        foreach (var proc in Process.GetProcessesByName("MeowsclesEAC"))
                        {
                            try
                            {
                                if (proc.MainModule.FileName.StartsWith(GamePath))
                                {
                                    proc.Kill();
                                    proc.WaitForExit();
                                }
                            } catch
                            {
                                MessageBox.Show("Meowscles is already running from your game path!");
                                this.Dispatcher.Invoke(() =>
                                {
                                    this.Button.Content = "Start Meowscles";
                                    this.Button.Click += Button_Click;
                                    this.Button.IsEnabled = true;
                                });
                                return;
                            }
                        }
                        foreach (var proc in Process.GetProcessesByName("FortniteClient-Win64-Shipping"))
                        {
                            try
                            {
                                if (proc.MainModule.FileName.StartsWith(GamePath))
                                {
                                    proc.Kill();
                                    proc.WaitForExit();
                                }
                            }
                            catch {
                                MessageBox.Show("Fortnite is already running from your game path!");
                                this.Dispatcher.Invoke(() =>
                                {
                                    this.Button.Content = "Start Meowscles";
                                    this.Button.Click += Button_Click;
                                    this.Button.IsEnabled = true;
                                });
                                return;
                            }
                        }
                        WebClient Client = new WebClient();
                        var lookup = new LookupClient();
                        var result = lookup.Query("meowscles.ploosh.dev", QueryType.TXT);
                        var record = result.Answers.TxtRecords().FirstOrDefault();
                        var ip = record?.Text.FirstOrDefault();
                        //Client.DownloadFile("https://cdn.discordapp.com/attachments/883144741838020629/1174502844636856380/Cobalt.dll", Path.Combine(Path69, "Engine\\Binaries\\ThirdParty\\NVIDIA\\NVaftermath\\Win64", "GFSDK_Aftermath_Lib.x64.dll"));
                        if (!File.Exists(System.IO.Path.Combine(GamePath, "MeowsclesEAC.exe")))
                        {
                            Client.DownloadFile($"{ip}/assets/MeowsclesEAC.zip", Path.Combine(GamePath, "MeowsclesEAC.zip"));
                            System.IO.Compression.ZipFile.ExtractToDirectory(Path.Combine(GamePath, "MeowsclesEAC.zip"), GamePath);
                            File.Delete(Path.Combine(GamePath, "MeowsclesEAC.zip"));
                            if (File.Exists(System.IO.Path.Combine(GamePath, "MeowsclesEAC.exe")) && Directory.Exists(System.IO.Path.Combine(GamePath, "EasyAntiCheat")))
                            {
                                var proc = new Process()
                                {
                                    StartInfo = new ProcessStartInfo()
                                    {
                                        Arguments = $"install bff26fa986424f4b8fa9dd3b6e853df8",
                                        FileName = Path.Combine(GamePath, "EasyAntiCheat", "EasyAntiCheat_EOS_Setup.exe")
                                    },
                                    EnableRaisingEvents = true
                                };
                                proc.Start();
                                proc.WaitForExit();
                                if (proc.ExitCode == 1223) {
                                    MessageBox.Show("UAC request denied!");
                                    File.Delete(Path.Combine(GamePath, "MeowsclesEAC.exe"));
                                    DeleteDirectory(Path.Combine(GamePath, "EasyAntiCheat"));
                                    this.Dispatcher.Invoke(() =>
                                    {
                                        this.Button.Content = "Start Meowscles";
                                        this.Button.Click += Button_Click;
                                        this.Button.IsEnabled = true;
                                    });
                                    return;
                                } else if (proc.ExitCode != 0)
                                {
                                    MessageBox.Show("Failed to install EAC!");
                                    File.Delete(Path.Combine(GamePath, "MeowsclesEAC.exe"));
                                    DeleteDirectory(Path.Combine(GamePath, "EasyAntiCheat"));
                                    this.Dispatcher.Invoke(() =>
                                    {
                                        this.Button.Content = "Start Meowscles";
                                        this.Button.Click += Button_Click;
                                        this.Button.IsEnabled = true;
                                    });
                                    return;
                                }
                            }
                            else
                            {
                                MessageBox.Show("Failed to download/extract EAC!");
                                this.Dispatcher.Invoke(() =>
                                {
                                    this.Button.Content = "Start Meowscles";
                                    this.Button.Click += Button_Click;
                                    this.Button.IsEnabled = true;
                                });
                                return;
                            }
                        } else
                        {
                            Client.DownloadFile($"{ip}/assets/MeowsclesEAC.zip", Path.Combine(GamePath, "MeowsclesEAC.zip"));
                            var fs = new FileStream(Path.Combine(GamePath, "MeowsclesEAC.zip"), FileMode.Open);
                            ZipArchiveExtensions.ExtractToDirectory(new ZipArchive(fs), GamePath, true);
                            fs.Close();
                            File.Delete(Path.Combine(GamePath, "MeowsclesEAC.zip"));
                        }
                        if (!File.Exists(Path.Combine(GamePath, "Engine\\Binaries\\ThirdParty\\NVIDIA\\NVaftermath\\Win64", "GFSDK_Aftermath_Lib2.x64.dll")))
                        {
                            File.Move(Path.Combine(GamePath, "Engine\\Binaries\\ThirdParty\\NVIDIA\\NVaftermath\\Win64", "GFSDK_Aftermath_Lib.x64.dll"), Path.Combine(GamePath, "Engine\\Binaries\\ThirdParty\\NVIDIA\\NVaftermath\\Win64", "GFSDK_Aftermath_Lib2.x64.dll"));
                        }
                        Client.DownloadFile($"{ip}/assets/sigs.bin", Path.Combine(GamePath, "EasyAntiCheat\\Certificates", "base.bin"));
                        Client.DownloadFile($"{ip}/assets/GFSDK_Aftermath_Lib.x64.dll", Path.Combine(GamePath, "Engine\\Binaries\\ThirdParty\\NVIDIA\\NVaftermath\\Win64", "GFSDK_Aftermath_Lib.x64.dll"));
                        //AntiCheat.Start(Path69);this.Dispatcher.Invoke(() =>
                        this.Dispatcher.Invoke(() =>
                        {
                            this.Button.Content = "Starting...";
                        });
                        Game.Start(GamePath, "-plooshfn -epicapp=Fortnite -epicenv=Prod -epiclocale=en-us -epicportal -nobe -fromfl=eac -fltoken=h1cdhchd10150221h130eB56 -skippatchcheck", Vars.Email, Vars.Password);
                        //FakeAC.Start(GamePath, "MeowsclesEAC.exe", "-plooshfn -epicapp=Fortnite -epicenv=Prod -epiclocale=en-us -epicportal -nobe -fromfl=eac -fltoken=h1cdhchd10150221h130eB56 -skippatchcheck", "t");
                        FakeAC.Start(GamePath, "FortniteClient-Win64-Shipping_EAC.exe", $"-epicapp=Fortnite -epicenv=Prod -epiclocale=en-us -epicportal -nobe -fromfl=eac -fltoken=h1cdhchd10150221h130eB56 -skippatchcheck", "r");
                        FakeAC.Start(GamePath, "FortniteLauncher.exe", $"-epicapp=Fortnite -epicenv=Prod -epiclocale=en-us -epicportal -nobe -fromfl=eac -fltoken=h1cdhchd10150221h130eB56 -skippatchcheck", "dsf");

                        //this.Button.Content = "Stop Meowscles";
                        //this.Button.Click += Button_Click_Stop;
                        //this.Button.IsEnabled = true;
                        IntPtr h = Process.GetCurrentProcess().MainWindowHandle;
                        ShowWindow(h, 6);
                        this.Dispatcher.Invoke(() =>
                        {
                            this.Button.Content = "Stop Meowscles";
                            this.Button.Click += Button_Click_Stop;
                            this.Button.IsEnabled = true;
                        });
                        try
                        {
                            Game._FortniteProcess.WaitForExit();
                        }
                        catch (Exception) { }
                        try
                        {
                            try
                            {
                                Kill();
                            } catch
                            {

                            }
                            this.Dispatcher.Invoke(() =>
                            {
                                this.Button.Content = "Start Meowscles";
                                this.Button.Click += Button_Click;
                                this.Button.IsEnabled = true;
                                //if (launcherThread.IsAlive) launcherThread.Abort();
                            });
                        }
                        catch (Exception)
                        {
                            MessageBox.Show("An error occurred while closing Fake AC!");
                            this.Dispatcher.Invoke(() =>
                            {
                                this.Button.Content = "Start Meowscles";
                                this.Button.Click += Button_Click;
                                this.Button.IsEnabled = true;
                            });
                        }



                        //Injector.Start(PSBasics._FortniteProcess.Id, Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "EonCurl.dll"));
                        running = false;
                    }
                    else
                    {
                        MessageBox.Show("Selected path is not a valid fortnite installation!");
                        this.Dispatcher.Invoke(() =>
                        {
                            this.Button.Content = "Start Meowscles";
                            this.Button.Click += Button_Click;
                            this.Button.IsEnabled = true;
                        });
                    }
                }
                else
                {
                    MessageBox.Show("Please add your game path in settings!");
                    this.Dispatcher.Invoke(() =>
                    {
                        this.Button.Content = "Start Meowscles";
                        this.Button.Click += Button_Click;
                        this.Button.IsEnabled = true;
                    });
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
                this.Dispatcher.Invoke(() =>
                {
                    this.Button.Content = "Start Meowscles";
                    this.Button.Click += Button_Click;
                    this.Button.IsEnabled = true;
                });
            }
        }

        public static void Kill()
        {
            try
            {
                if (Game._FortniteProcess != null && !Game._FortniteProcess.HasExited) Process.GetProcessById(Game._FortniteProcess.Id).Kill();
                if (FakeAC._FNLauncherProcess != null && !FakeAC._FNLauncherProcess.HasExited) FakeAC._FNLauncherProcess.Kill();
                if (FakeAC._FNAntiCheatProcess != null && !FakeAC._FNAntiCheatProcess.HasExited) FakeAC._FNAntiCheatProcess.Kill();
            }
            catch (Exception)
            {

            }
        }

        public Home()
		{
			InitializeComponent();
		}

        private void Button_Click_Stop(object sender, RoutedEventArgs e)
        {
            Kill();
            this.Button.Content = "Start Meowscles";
            this.Button.Click -= Button_Click_Stop;
            this.Button.Click += Button_Click;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
		{
            launcherThread = new Thread(Launch);
            launcherThread.Start();
            this.Button.Content = "Initializing...";
            this.Button.Click -= Button_Click;
            this.Button.IsEnabled = false;
            //this.PathBox.Text = commonOpenFileDialog.FileName;
        }
	}
}
