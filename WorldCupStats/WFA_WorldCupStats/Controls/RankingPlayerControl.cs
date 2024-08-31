using System;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Forms;
using DataLayer;
using DataLayer.Models;

namespace WFA_WorldCupStats.Controls
{
	public partial class RankingPlayerControl : UserControl
	{
		private readonly string _defaultPlayerImagePath = Path.Combine(Application.StartupPath, "Resources", "profile.png");
		private readonly IDataProvider _dataProvider;

		public string PlayerName => lblName.Text;
		public int Count => int.Parse(lblCount.Text);

		public RankingPlayerControl(PlayerStats playerStats, IDataProvider dataProvider)
		{
			InitializeComponent();
			_dataProvider = dataProvider;

			lblName.Text = playerStats.Name;
			lblCount.Text = playerStats.Count.ToString();

			LoadPlayerImageAsync(playerStats.Name);
		}

		private async Task LoadPlayerImageAsync(string playerName)
		{
			try
			{
				string customImagePath = Path.Combine(Application.StartupPath, "PlayerImages", $"{playerName}.png");
				if (File.Exists(customImagePath))
				{
					picPlayer.Image = Image.FromFile(customImagePath);
				}
				else
				{
					// Pokušaj dohvatiti sliku igrača iz podatkovnog sloja
					var playerImageData = await _dataProvider.GetPlayerImageAsync(playerName);
					if (playerImageData != null && playerImageData.Length > 0)
					{
						using (var ms = new MemoryStream(playerImageData))
						{
							picPlayer.Image = Image.FromStream(ms);
						}
					}
					else
					{
						// Ako nema prilagođene slike ni slike iz podatkovnog sloja, koristi zadanu sliku
						picPlayer.Image = File.Exists(_defaultPlayerImagePath)
							? Image.FromFile(_defaultPlayerImagePath)
							: null;
					}
				}

				picPlayer.SizeMode = PictureBoxSizeMode.Zoom;
			}
			catch (Exception ex)
			{
				// Logirajte grešku ili je prikažite korisniku ako je potrebno
				Console.WriteLine($"Error loading player image: {ex.Message}");

				// U slučaju greške, postavite zadanu sliku
				if (File.Exists(_defaultPlayerImagePath))
				{
					picPlayer.Image = Image.FromFile(_defaultPlayerImagePath);
				}
			}
		}
	}
}