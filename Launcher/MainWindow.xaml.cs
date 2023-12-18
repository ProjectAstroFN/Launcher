using ModernWpf.Controls;
using ModernWpf;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Meowscles.Pages;
using ModernWpf.Media.Animation;
using System;
using System.Net;
using DiscordRPC;
using Windows.Media.Protection.PlayReady;
using System.ComponentModel;
using Meowscles.Services;
//using WpfApp6.Services;

namespace Meowscles
{


	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		// Sets Up The Pages
		Home home = new Home();
        Download download = new Download();
        Settings settings = new Settings();
        private DiscordRpcClient client;

        public MainWindow()
		{
			InitializeComponent();
			InitializeDiscordRPC();

			

		/*	WebClient omg = new WebClient();
			try
			{
				string fds = omg.DownloadString("http://api.eonfn.com/eon/version");


				if (fds == "0.1")
				{
					//
				}
				else
				{
					MessageBox.Show("Please Update The Launcher");
					Application.Current.Shutdown();
				}
			}catch (Exception ex)
			{
				MessageBox.Show("Failed Connecting To Eon Servers, Please Check The Discord");
				Application.Current.Shutdown();
			}*/
			
		}

        private void NavView_Loaded(object sender, RoutedEventArgs e)
        {
			ContentFrame.Navigate(home);
			NavView.SelectedItem = NavView.MenuItems[0];

			

			//base.OnStartup(e);
			//ContentFrame.Navigate(home);
		}

		private void NavView_SelectionChanged(ModernWpf.Controls.NavigationView sender, ModernWpf.Controls.NavigationViewSelectionChangedEventArgs args)
		{
			if (args.IsSettingsSelected)
			{
				ContentFrame.Navigate(settings);
            }
            else
			{
				NavigationViewItem item = args.SelectedItem as NavigationViewItem;

				if (item.Tag.ToString() == "Play")
				{
					ContentFrame.Navigate(home);
				}
                if (item.Tag.ToString() == "Download")
                {
                    ContentFrame.Navigate(download);
                }
            }
		}

		private void ContentFrame_NavigationFailed(object sender, NavigationFailedEventArgs e)
		{
			throw new Exception("Failed to load Page."); // Never TBH
		}

        private void Window_Closed(object sender, EventArgs e)
        {
            client.Dispose();
        }

        private void InitializeDiscordRPC()
        {
            client = new DiscordRpcClient("783719033639534602");

            client.OnReady += (sender, e) =>
            {
                Console.WriteLine("Discord RPC is ready.");
            };
            client.Initialize();

			DiscordRPC.Button joinButton = new DiscordRPC.Button
            {
                Label = "Join The Discord",
                Url = "https://discord.gg/C2TzjxYpqh"
            };

            var buttons = new DiscordRPC.Button[1];
			buttons[0] = joinButton;

            client.SetPresence(new RichPresence
            {
                Details = "Project Meowscles",
                State = "Playing Project Meowscles",
                Timestamps = Timestamps.Now,
                Assets = new Assets
                {
                    LargeImageKey = "meowscles",
                    LargeImageText = "Project Meowscles",
                },
				Buttons = buttons
            });
        }
    }
}
