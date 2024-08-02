namespace DataLayer
{
	public class DataProviderFactory
	{
		public static IDataProvider CreateDataProvider()
		{
			string dataSource = ConfigurationManager.GetConfiguration("DataSource");

			switch (dataSource?.ToLower())
			{
				case "api":
					return new ApiDataProvider();
				case "json":
					return new JsonDataProvider();
				default:
					throw new InvalidOperationException("Neispravan ili nedefiniran izvor podataka u konfiguraciji. Koristite 'api' ili 'json'.");
			}
		}
	}
}