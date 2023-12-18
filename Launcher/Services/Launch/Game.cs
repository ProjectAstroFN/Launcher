using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Meowscles.Services.Launch
{
	public static class Game
	{
		public static Process _FortniteProcess;
		public static void Start(string PATH, string args, string Email, string Password)
		{
			if(Email == null || Password == null)
			{
				MessageBox.Show("No login set! Please set your credentials in Settings.");
				return;
			}
			if (File.Exists(Path.Combine(PATH, /*"FortniteGame\\Binaries\\Win64\\", "FortniteClient-Win64-Shipping.exe"*/ "MeowsclesEAC.exe")))
			{
				Game._FortniteProcess = new Process()
				{
					StartInfo = new ProcessStartInfo()
					{
						Arguments = $"-AUTH_LOGIN={Email} -AUTH_PASSWORD={Password} -AUTH_TYPE=epic " + args,
						FileName = Path.Combine(PATH, /*"FortniteGame\\Binaries\\Win64\\", "FortniteClient-Win64-Shipping.exe"*/ "MeowsclesEAC.exe")
					},
					EnableRaisingEvents = true
				};
				Game._FortniteProcess.Exited += new EventHandler(Game.OnFortniteExit);
				Game._FortniteProcess.Start();


			}

		}

		public static void OnFortniteExit(object sender, EventArgs e)
		{
			Process fortniteProcess = Game._FortniteProcess;
			if (fortniteProcess != null && fortniteProcess.HasExited)
			{
				Game._FortniteProcess = (Process)null;
			}
			if (FakeAC._FNLauncherProcess != null && !FakeAC._FNLauncherProcess.HasExited) FakeAC._FNLauncherProcess?.Kill();
			if (FakeAC._FNAntiCheatProcess != null && !FakeAC._FNLauncherProcess.HasExited) FakeAC._FNAntiCheatProcess?.Kill();
		}
	}
}
