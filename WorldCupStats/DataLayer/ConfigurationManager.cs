using System;
using System.IO;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace DataLayer
{
	public class ConfigurationManager
	{
		private const string ConfigFileName = "config.json";
		private static Dictionary<string, string> _configurations;

		static ConfigurationManager()
		{
			LoadConfigurations();
		}

		private static void LoadConfigurations()
		{
			if (!File.Exists(ConfigFileName))
			{
				_configurations = new Dictionary<string, string>();
				SaveConfigurations();
			}
			else
			{
				string json = File.ReadAllText(ConfigFileName);
				_configurations = JsonConvert.DeserializeObject<Dictionary<string, string>>(json);
			}
		}

		public static string GetConfiguration(string key)
		{
			if (_configurations.TryGetValue(key, out string value))
			{
				return value;
			}
			throw new KeyNotFoundException($"Ključ '{key}' nije pronađen u konfiguraciji.");
		}

		public static void SetConfiguration(string key, string value)
		{
			_configurations[key] = value;
			SaveConfigurations();
		}

		private static void SaveConfigurations()
		{
			string json = JsonConvert.SerializeObject(_configurations, Formatting.Indented);
			File.WriteAllText(ConfigFileName, json);
		}
	}
}