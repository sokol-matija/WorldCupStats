namespace WFA_WorldCupStats
{
	internal static class Program
	{
		/// <summary>
		///  The main entry point for the application.
		/// </summary>
		[STAThread]
		static void Main()
		{
			// To customize application configuration such as set high DPI settings or default font,
			// see https://aka.ms/applicationconfiguration.
			ApplicationConfiguration.Initialize();
			Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo("en-US");
			Application.Run(new MainForm());
		}
	}
}