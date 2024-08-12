using System;
using System.Linq;
using System.Threading.Tasks;
using DataLayer;
using DataLayer.Models;

class Program
{
	static async Task Main(string[] args)
	{
		try
		{
			Console.WriteLine("=== Testiranje API Data Providera ===");
			await TestApiProvider();

			Console.WriteLine("\n=== Testiranje JSON Data Providera ===");
			await TestJsonProvider();
		}
		catch (Exception ex)
		{
			Console.WriteLine($"Došlo je do pogreške: {ex.Message}");
		}
	}

	static async Task TestJsonProvider()
	{
		ConfigurationManager.SetConfiguration("DataSource", "json");
		IDataProvider jsonProvider = DataProviderFactory.CreateDataProvider();
		await RunTests(jsonProvider, "JSON");
	}


	static async Task TestApiProvider()
	{
		ConfigurationManager.SetConfiguration("DataSource", "api");
		IDataProvider apiProvider = DataProviderFactory.CreateDataProvider();
		await RunTests(apiProvider, "API");
	}

	static async Task RunTests(IDataProvider dataProvider, string providerName)
	{
		string[] genders = { "men", "women" };
		foreach (var gender in genders)
		{
			await TestTeams(dataProvider, providerName, gender);
			await TestMatches(dataProvider, providerName, gender);
			await TestPlayers(dataProvider, providerName, gender);
			await TestTopScorers(dataProvider, providerName, gender);
			await TestYellowCards(dataProvider, providerName, gender);
			await TestMatchesByAttendance(dataProvider, providerName, gender);
		}
		// Ove metode ne ovise o spolu, pa ih ne moramo mijenjati
		await TestSettings(dataProvider, providerName);
		//await TestFavoriteTeam(dataProvider, providerName);
		//await TestFavoritePlayers(dataProvider, providerName);
	}

	static async Task TestTeams(IDataProvider dataProvider, string providerName, string gender)
	{
		Console.WriteLine($"\n--- Test dohvaćanja timova ({providerName}, {gender}) ---");
		var teams = await dataProvider.GetTeamsAsync(gender);
		Console.WriteLine($"Broj timova: {teams.Count}");
		Console.WriteLine("Prvih 5 timova:");
		foreach (var team in teams.Take(5))
		{
			Console.WriteLine($"- {team.Country} ({team.FifaCode}): Grupa {team.GroupLetter}");
		}
	}

	static async Task TestMatches(IDataProvider dataProvider, string providerName, string gender)
	{
		Console.WriteLine($"\n--- Test dohvaćanja utakmica ({providerName}, {gender}) ---");
		var matches = await dataProvider.GetMatchesAsync(gender);
		Console.WriteLine($"Broj utakmica: {matches.Count}");
		Console.WriteLine("Posljednjih 5 utakmica:");
		foreach (var match in matches.TakeLast(5))
		{
			Console.WriteLine($"- {match.HomeTeamCountry} vs {match.AwayTeamCountry}: {match.HomeTeam.Goals}-{match.AwayTeam.Goals}");
		}
	}


	static async Task TestPlayers(IDataProvider dataProvider, string providerName, string gender)
	{
		Console.WriteLine($"\n--- Test dohvaćanja igrača za određeni tim ({providerName}, {gender}) ---");
		var teams = await dataProvider.GetTeamsAsync(gender);
		var firstTeamCode = teams.FirstOrDefault()?.FifaCode;
		if (firstTeamCode != null)
		{
			var players = await dataProvider.GetPlayersByTeamAsync(firstTeamCode, gender);
			Console.WriteLine($"Broj igrača za tim {firstTeamCode}: {players.Count}");
			Console.WriteLine("Prvih 5 igrača:");
			foreach (var player in players.Take(5))
			{
				Console.WriteLine($"- {player.Name} ({player.Position}), Broj: {player.ShirtNumber}");
			}
		}
		else
		{
			Console.WriteLine("Nije pronađen nijedan tim.");
		}
	}

	static async Task TestTopScorers(IDataProvider dataProvider, string providerName, string gender)
	{
		Console.WriteLine($"\n--- Test dohvaćanja najboljih strijelaca ({providerName}, {gender}) ---");
		var topScorers = await dataProvider.GetTopScorersAsync(gender, 5);
		Console.WriteLine("Top 5 strijelaca:");
		foreach (var scorer in topScorers)
		{
			Console.WriteLine($"- {scorer.Name}: {scorer.Count} golova");
		}
	}

	static async Task TestYellowCards(IDataProvider dataProvider, string providerName, string gender)
	{
		Console.WriteLine($"\n--- Test dohvaćanja igrača s najviše žutih kartona ({providerName}, {gender}) ---");
		var yellowCards = await dataProvider.GetYellowCardsAsync(gender, 5);
		Console.WriteLine("Top 5 igrača s najviše žutih kartona:");
		foreach (var player in yellowCards)
		{
			Console.WriteLine($"- {player.Name}: {player.Count} žutih kartona");
		}
	}

	static async Task TestMatchesByAttendance(IDataProvider dataProvider, string providerName, string gender)
	{
		Console.WriteLine($"\n--- Test dohvaćanja utakmica s najvećom posjećenošću ({providerName}, {gender}) ---");
		var attendedMatches = await dataProvider.GetMatchesByAttendanceAsync(gender, 5);
		Console.WriteLine("Top 5 utakmica po posjećenosti:");
		foreach (var match in attendedMatches)
		{
			Console.WriteLine($"- {match.HomeTeamCountry} vs {match.AwayTeamCountry}: {match.Attendance} gledatelja");
		}
	}

	static async Task TestSettings(IDataProvider dataProvider, string providerName)
	{
		Console.WriteLine($"\n--- Test spremanja i učitavanja postavki ({providerName}) ---");
		await dataProvider.SaveSettingsAsync("TestKey", "TestValue");
		var loadedSetting = await dataProvider.LoadSettingsAsync("TestKey");
		Console.WriteLine($"Učitana postavka: {loadedSetting}");
	}

	////TODO make this work again 
	//static async Task TestFavoriteTeam(IDataProvider dataProvider, string providerName)
	//{
	//	Console.WriteLine($"\n--- Test spremanja i učitavanja omiljenog tima ({providerName}) ---");
	//	await dataProvider.SaveFavoriteTeamAsync("CRO");
	//	var favoriteTeam = await dataProvider.LoadFavoriteTeamAsync();
	//	Console.WriteLine($"Omiljeni tim: {favoriteTeam}");
	//}

	////TOD
	//static async Task TestFavoritePlayers(IDataProvider dataProvider, string providerName)
	//{
	//	Console.WriteLine($"\n--- Test spremanja i učitavanja omiljenih igrača ({providerName}) ---");
	//	await dataProvider.SaveFavoritePlayersAsync(new List<string> { "Luka Modric", "Ivan Perisic", "Marcelo Brozovic" });
	//	var favoritePlayers = await dataProvider.LoadFavoritePlayersAsync();
	//	Console.WriteLine("Omiljeni igrači:");
	//	foreach (var player in favoritePlayers)
	//	{
	//		Console.WriteLine($"- {player}");
	//	}
	//}
}