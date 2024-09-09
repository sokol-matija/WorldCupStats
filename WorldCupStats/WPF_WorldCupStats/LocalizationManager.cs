using System.Globalization;
using System.Threading;
using System.Windows;

namespace WPF_WorldCupStats
{
	public static class LocalizationManager
	{
		public static void SetLanguage(string languageCode)
		{
			Thread.CurrentThread.CurrentUICulture = new CultureInfo(languageCode);
			Thread.CurrentThread.CurrentCulture = new CultureInfo(languageCode);

			foreach (ResourceDictionary dict in Application.Current.Resources.MergedDictionaries)
			{
				if (dict.Source != null && dict.Source.OriginalString.StartsWith("Resources/Lang."))
				{
					string resourcePath = $"Resources/Lang.{languageCode}.xaml";
					ResourceDictionary resourceDict = new ResourceDictionary() { Source = new Uri(resourcePath, UriKind.Relative) };
					Application.Current.Resources.MergedDictionaries.Remove(dict);
					Application.Current.Resources.MergedDictionaries.Add(resourceDict);
					break;
				}
			}
		}
	}
}