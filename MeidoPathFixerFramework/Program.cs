using Microsoft.Win32;
using System;
using System.IO;

namespace MeidoPathFixer
{
	internal class Program
	{
		private static void Main()
		{
			Console.WriteLine("Do be advised, the path to be used is the path where the application is currently running. Place the app in your game directory before running it! The Application will also refuse to set a path where it cannot find the game launcher. Furthermore, it will detect if the current path holds CM or COM and set the path for it automatically.\n\nPress any key to continue, otherwise exit now.");

			Console.ReadKey();

			var path = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);

			if (path == null)
			{
				return;
			}

			if (File.Exists(path + @"\COM3D2.exe"))
			{
				if (IsTwoPointFive(path))
				{
					Registry.SetValue(@"HKEY_CURRENT_USER\Software\KISS\カスタムオーダーメイド3D2.5", "InstallPath", path);
				}
				else if (IsEnglish(path))
				{
					Registry.SetValue(@"HKEY_CURRENT_USER\Software\KISS\CUSTOM ORDER MAID3D 2", "InstallPath", path);
				}
				else
				{
					Registry.SetValue(@"HKEY_CURRENT_USER\Software\KISS\カスタムオーダーメイド3D2", "InstallPath", path);
				}
			}
			else if (File.Exists(path + @"\CM3D2.exe"))
			{
				Registry.SetValue(@"HKEY_CURRENT_USER\Software\KISS\カスタムメイド3D2", "InstallPath", path);
			}
		}

		private static bool IsTwoPointFive(string path)
		{
			if (File.Exists(path + "\\update.lst"))
			{
				foreach (var line in File.ReadAllLines(path + "\\update.lst"))
				{
					if (line.Contains("COM3D2x64.exe,"))
					{
						var version = line.Replace("COM3D2x64.exe,", "");

						if (int.Parse(version) >= 30000)
						{
							return true;
						}
						else
						{
							return false;
						}
					}
				}

				Console.WriteLine("We failed to find the appropriate line in the update.lst file! This is an issue you should look into...");

				Environment.Exit(5);
			}
			else
			{
				Console.WriteLine("You don't have an Update.lst file! We can't safely set your path like this and it indicates a huge issue with your game! Get it fixed!!");

				Environment.Exit(5);
			}

			return false;
		}

		private static bool IsEnglish(string path)
		{
			return File.Exists(path + "\\localize.dat");
		}
	}
}