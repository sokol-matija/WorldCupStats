using DataLayer.Models;
using System;
using System.IO;
using System.Windows;
using System.Windows.Media.Imaging;

namespace WPF_WorldCupStats
{
	public partial class PlayerInfoWindow : Window
	{
		public PlayerInfoWindow(Player player)
		{
			InitializeComponent();
			SetPlayerInfo(player);
		}

		private void SetPlayerInfo(Player player)
		{
			txtName.Text = player.Name;
			txtNumber.Text = player.ShirtNumber.ToString(); // Pretpostavljamo da je ShirtNumber int
			txtPosition.Text = player.Position;
			txtCaptain.Text = player.Captain ? "Yes" : "No";

			// Provjera postoji li svojstvo Goals, ako ne, postavite na "N/A"
			txtGoals.Text = player.GetType().GetProperty("Goals") != null
				? player.GetType().GetProperty("Goals").GetValue(player, null).ToString()
				: "N/A";

			// Provjera postoji li svojstvo YellowCards, ako ne, sakrijte ili postavite na "N/A"
			if (player.GetType().GetProperty("YellowCards") != null)
			{
				txtYellowCards.Text = player.GetType().GetProperty("YellowCards").GetValue(player, null).ToString();
			}
			else
			{
				txtYellowCards.Text = "N/A";
				// Ili možete sakriti label i TextBlock za žute kartone ako ta informacija nije dostupna
				// lblYellowCards.Visibility = Visibility.Collapsed;
				// txtYellowCards.Visibility = Visibility.Collapsed;
			}

			// Provjera postoji li svojstvo ImagePath, ako ne, koristite zadanu sliku
			if (player.GetType().GetProperty("ImagePath") != null)
			{
				string imagePath = player.GetType().GetProperty("ImagePath").GetValue(player, null) as string;
				if (!string.IsNullOrEmpty(imagePath) && File.Exists(imagePath))
				{
					imgPlayer.Source = new BitmapImage(new Uri(imagePath));
				}
				else
				{
					SetDefaultPlayerImage();
				}
			}
			else
			{
				SetDefaultPlayerImage();
			}
		}

		private void SetDefaultPlayerImage()
		{
			// Postavite zadanu sliku igrača
			string defaultImagePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Assets", "default_player.png");
			if (File.Exists(defaultImagePath))
			{
				imgPlayer.Source = new BitmapImage(new Uri(defaultImagePath));
			}
			else
			{
				// Ako ni zadana slika ne postoji, možete postaviti neki tekst ili ostaviti prazno
				imgPlayer.Source = null;
			}
		}
	}
}