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
    // Goofy Code Need To Improve On V2
    public class FakeAC
    {
		public static Process _FNLauncherProcess;
		public static Process _FNAntiCheatProcess;

        public static Process _MACProcess;

        public static void Start(string Path69, string FileName, string args = "", string t = "r")
		{
            try
            {
				if (t == "t")
				{
                    ProcessStartInfo ProcessIG = new ProcessStartInfo()
                    {
                        FileName = Path.Combine(Path69, FileName),
                        Arguments = args,
                        CreateNoWindow = true,
                    };

                    _MACProcess = Process.Start(ProcessIG);
                    if (_MACProcess.Id == 0)
                    {
                        MessageBox.Show("FAILED STARTING!?!?!");
                    }
                    _MACProcess.Freeze();
                }
				if (File.Exists(Path.Combine(Path69, "FortniteGame\\Binaries\\Win64\\", FileName)))
				{
					ProcessStartInfo ProcessIG = new ProcessStartInfo()
					{
						FileName = Path.Combine(Path69, "FortniteGame\\Binaries\\Win64\\", FileName),
						Arguments = args,
						CreateNoWindow = true,
					};

					if(t == "r")
					{
						_FNAntiCheatProcess = Process.Start(ProcessIG);
						if (_FNAntiCheatProcess.Id == 0)
						{
							MessageBox.Show("FAILED STARTING!?!?!");
						}
						_FNAntiCheatProcess.Freeze();
					}else
					{
						_FNLauncherProcess = Process.Start(ProcessIG);
						if (_FNLauncherProcess.Id == 0)
						{
							MessageBox.Show("FAILED STARTING!?!?!");
						}
						_FNLauncherProcess.Freeze();
					}
					
				}
			}catch (Exception)
            {
				MessageBox.Show("THERE BEEN A ERROR");
			}
		}
	}
}
