using System;
using System.IO;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace DataLayer
{
	public static class SharedSettings
	{
		public static string SettingsFilePath { get; private set; }

		static SharedSettings()
		{
			string solutionDir = FindSolutionDirectory();
			string dataLayerPath = Path.Combine(solutionDir, "DataLayer");
			string assetsPath = Path.Combine(dataLayerPath, "Assets");
			SettingsFilePath = Path.Combine(assetsPath, "settings.json");

			if (!Directory.Exists(assetsPath))
			{
				Directory.CreateDirectory(assetsPath);
			}
		}

		private static string FindSolutionDirectory()
		{
			string currentDir = AppDomain.CurrentDomain.BaseDirectory;
			while (currentDir != null)
			{
				if (File.Exists(Path.Combine(currentDir, "WorldCupStats.sln")))
				{
					return currentDir;
				}
				currentDir = Directory.GetParent(currentDir)?.FullName;
			}
			throw new DirectoryNotFoundException("Solution directory not found.");
		}

		public static async Task<Dictionary<string, string>> LoadSettingsAsync()
		{
			if (File.Exists(SettingsFilePath))
			{
				var json = await File.ReadAllTextAsync(SettingsFilePath);
				return JsonConvert.DeserializeObject<Dictionary<string, string>>(json);
			}
			return new Dictionary<string, string>();
		}

		public static async Task SaveSettingsAsync(Dictionary<string, string> settings)
		{
			var json = JsonConvert.SerializeObject(settings);
			await File.WriteAllTextAsync(SettingsFilePath, json);
		}
	}
}