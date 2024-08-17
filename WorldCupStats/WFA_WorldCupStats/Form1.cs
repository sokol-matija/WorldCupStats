using DataLayer;
using DataLayer.Models;
using System.Drawing.Drawing2D;
using System.Drawing.Printing;
using System.Text;

namespace WFA_WorldCupStats
{
	public partial class Form1 : Form
	{
		private readonly IDataProvider _dataProvider;
		private string _selectedChampionship;
		private string _selectedLanguage;
		private string _favoriteTeam;
		private List<Player> _allPlayers;
		private List<PlayerControl> _favoritePlayers = new List<PlayerControl>();
		private List<PlayerControl> _selectedPlayers = new List<PlayerControl>();
		private LogForm logForm;


		public Form1()
		{
			logForm = new LogForm();
			logForm.Show();
			InitializeComponent();
			_dataProvider = DataProviderFactory.CreateDataProvider();
			LoadInitialSettings();
			ApplyLocalization();
		}

		private async void LoadInitialSettings()
		{
			string championship = await _dataProvider.LoadSettingsAsync("Championship");
			string language = await _dataProvider.LoadSettingsAsync("Language");
			_favoriteTeam = await _dataProvider.LoadFavoriteTeamAsync();

			logForm.Log($"Loaded settings - Championship: {championship}, Language: {language}, Favorite Team: {_favoriteTeam}");

			if (string.IsNullOrEmpty(championship) || string.IsNullOrEmpty(language))
			{
				using (var settingsForm = new InitialSettingsForm(_dataProvider))
				{
					if (settingsForm.ShowDialog() == DialogResult.OK)
					{
						_selectedChampionship = await _dataProvider.LoadSettingsAsync("Championship");
						_selectedLanguage = await _dataProvider.LoadSettingsAsync("Language");
						await ApplySettings();
					}
					else
					{
						Application.Exit();
					}
				}
			}
			else
			{
				_selectedChampionship = championship;
				_selectedLanguage = language;
				await ApplySettings();
			}
		}

		private async Task ApplySettings()
		{
			// Promjena jezika
			ChangeLanguage(_selectedLanguage);

			await LoadChampionshipData();
			// U�itavanje podataka za odabrano prvenstvo
			ClearSelection();

			// A�uriranje UI-a
			ApplyLocalization();

			await LoadPlayerData();
		}

		private async Task LoadPlayerData()
		{
			if (cmbTeams.SelectedItem is Team selectedTeam)
			{
				await LoadPlayerDetails(selectedTeam.FifaCode);
			}
		}

		private void ChangeLanguage(string language)
		{
			if (language == "hr" || language == "Croatian" || language == "Hrvatski")
			{
				Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo("hr-HR");
			}
			else
			{
				Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo("en-US");
			}
			ApplyLocalization();
		}

		private void ApplyLocalization()
		{
			// Postavljanje naslova glavne forme
			this.Text = Strings.MainFormTitle;

			// Lokalizacija izbornika
			mnuSettings.Text = Strings.SettingsMenuItem;
			mnuChosePrintType.Text = Strings.ChosePrintTypeMenuItem;
			mnuPrintPreview.Text = Strings.PrintPreviewMenuItem;
			mnuPrint.Text = Strings.PrintMenuItem;
			mnuPrintStatistics.Text = Strings.PrintStatisticsMenuItem;


			// Lokalizacija oznaka
			lblTopScorers.Text = Strings.TopScorersLabel;
			lblYellowCards.Text = Strings.YellowCardsLabel;
			lblAllPlayers.Text = Strings.AllPlayersLabel;
			lblFavoritePlayers.Text = Strings.FavoritePlayersLabel;
			lblMatches.Text = Strings.MatchesLabel;
			lblChooseTeam.Text = Strings.ChooseTeamLabel;

			foreach (Control control in pnlAllPlayers.Controls)
			{
				if (control is PlayerControl playerControl)
				{
					playerControl.ApplyLocalization();
				}
			}

			foreach (Control control in pnlFavoritePlayers.Controls)
			{
				if (control is PlayerControl playerControl)
				{
					playerControl.ApplyLocalization();
				}
			}
		}


		private async Task LoadChampionshipData()
		{
			try
			{
				var teams = await _dataProvider.GetTeamsAsync(_selectedChampionship.ToLower());
				logForm.Log($"Loaded {teams.Count} teams");

				cmbTeams.DataSource = teams;
				cmbTeams.DisplayMember = "Country";
				cmbTeams.ValueMember = "FifaCode";

				cmbTeams.Format += (s, e) =>
				{
					if (e.ListItem is Team team)
					{
						e.Value = $"{team.Country} ({team.FifaCode})";
					}
				};


				if (!string.IsNullOrEmpty(_favoriteTeam))
				{
					var team = teams.FirstOrDefault(t => t.FifaCode == _favoriteTeam);
					if (team != null)
					{
						logForm.Log($"Found matching team: {team.Country} ({team.FifaCode})");
						cmbTeams.SelectedValue = team.FifaCode;
					}
					else
					{
						logForm.Log($"No matching team found for FIFA code: {_favoriteTeam}");
					}
				}
				else
				{
					logForm.Log("No favorite team saved");
				}

				if (cmbTeams.SelectedItem is Team selectedTeam)
				{
					logForm.Log($"Currently selected team: {selectedTeam.Country} ({selectedTeam.FifaCode})");
				}
				else
				{
					logForm.Log("No team currently selected");
				}

				var matches = await _dataProvider.GetMatchesAsync(_selectedChampionship.ToLower());
				UpdateMatchesList(matches);

				await LoadAndDisplayStatistics();
			}
			catch (Exception ex)
			{
				MessageBox.Show($"Error loading championship data: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}

		private void UpdateMatchesList(List<Match> matches)
		{
			lstMatches.Items.Clear();
			foreach (var match in matches)
			{
				lstMatches.Items.Add($"{match.HomeTeamCountry} vs {match.AwayTeamCountry} - {match.HomeTeam.Goals}:{match.AwayTeam.Goals}");
			}
		}

		private async Task LoadAndDisplayStatistics()
		{
			var topScorers = await _dataProvider.GetTopScorersAsync(_selectedChampionship.ToLower(), 5);
			var yellowCards = await _dataProvider.GetYellowCardsAsync(_selectedChampionship.ToLower(), 5);

			UpdateStatisticsList(lstTopScorers, topScorers);
			UpdateStatisticsList(lstYellowCards, yellowCards);
		}

		private void UpdateStatisticsList(ListBox listBox, List<PlayerStats> stats)
		{
			listBox.Items.Clear();
			foreach (var stat in stats)
			{
				listBox.Items.Add($"{stat.Name}: {stat.Count}");
			}
		}

		//TODO: Delete
		private void UpdateUILanguage()
		{
			// A�uriranje svih UI elemenata s lokaliziranim stringovima
			this.Text = Strings.MainFormTitle;
			//grpTeams.Text = Strings.TeamsGroupBox;
			//lblSelectTeam.Text = Strings.SelectTeamLabel;
			//grpMatches.Text = Strings.MatchesGroupBox;
			//grpStatistics.Text = Strings.StatisticsGroupBox;
			//lblTopScorers.Text = Strings.TopScorersLabel;
			//lblYellowCards.Text = Strings.YellowCardsLabel;
			//btnLoadPlayerDetails.Text = Strings.LoadPlayerDetailsButton;
			//btnPrintStatistics.Text = Strings.PrintStatisticsButton;
			//mnuFile.Text = Strings.FileMenu;
			//mnuSettings.Text = Strings.SettingsMenuItem;
			//mnuExit.Text = Strings.ExitMenuItem;
		}

		private void btnLoadPlayerDetails_Click(object sender, EventArgs e)
		{
			if (cmbTeams.SelectedItem is Team selectedTeam)
			{
				LoadPlayerDetails(selectedTeam.FifaCode);
			}
			else
			{
				MessageBox.Show(Strings.SelectTeamFirst, Strings.Error, MessageBoxButtons.OK, MessageBoxIcon.Warning);
			}
		}
		private async Task LoadPlayerDetails(string fifaCode)
		{
			try
			{
				ShowLoadingIndicator(true);

				_allPlayers = await _dataProvider.GetPlayersByTeamAsync(fifaCode, _selectedChampionship.ToLower());
				logForm.Log($"Loaded {_allPlayers.Count} players for team {fifaCode}");

				_favoritePlayers = new List<PlayerControl>(); // Initialize _favoritePlayers

				await LoadFavoritePlayers(fifaCode);

				UpdatePlayerPanels();
			}
			catch (Exception ex)
			{
				logForm.Log($"Error loading player details: {ex.Message}");
				MessageBox.Show($"Error loading player details: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
			finally
			{
				ShowLoadingIndicator(false);
			}
		}

		private void ShowLoadingIndicator(bool isLoading)
		{
			if (InvokeRequired)
			{
				Invoke(new Action<bool>(ShowLoadingIndicator), isLoading);
				return;
			}

			// Onemogu�ite/omogu�ite kontrole tijekom u�itavanja
			cmbTeams.Enabled = !isLoading;
			pnlAllPlayers.Enabled = !isLoading;
			pnlFavoritePlayers.Enabled = !isLoading;
			// Dodajte ostale kontrole koje �elite onemogu�iti/omogu�iti tijekom u�itavanja

			// Promijenite kursor da indicira u�itavanje
			this.Cursor = isLoading ? Cursors.WaitCursor : Cursors.Default;
		}

		private void UpdatePlayerPanels()
		{
			if (_allPlayers == null)
			{
				logForm.Log("_allPlayers is null. Cannot update player panels.");
				return;
			}

			pnlAllPlayers.Controls.Clear();
			pnlFavoritePlayers.Controls.Clear();

			foreach (var player in _allPlayers)
			{
				var isFavorite = _favoritePlayers?.Any(fp => fp.Player.Name == player.Name) ?? false;
				var playerControl = new PlayerControl(player) { IsFavorite = isFavorite };
				playerControl.MouseDown += PlayerControl_MouseDown;

				pnlAllPlayers.Controls.Add(playerControl);

				if (isFavorite)
				{
					var favPlayerControl = new PlayerControl(player) { IsFavorite = true };
					favPlayerControl.MouseDown += PlayerControl_MouseDown;
					pnlFavoritePlayers.Controls.Add(favPlayerControl);
				}
			}

			ArrangePanelControls(pnlAllPlayers);
			ArrangePanelControls(pnlFavoritePlayers);

			logForm.Log($"Updated player panels. All players: {pnlAllPlayers.Controls.Count}, Favorite players: {pnlFavoritePlayers.Controls.Count}");
		}
		private void ArrangePanelControls(Panel panel)
		{
			int yPosition = 0;
			foreach (Control control in panel.Controls)
			{
				control.Location = new Point(0, yPosition);
				yPosition += control.Height + 5; // 5 piksela razmaka
			}
		}

		private void PlayerControl_MouseDown(object sender, MouseEventArgs e)
		{
			if (e.Button == MouseButtons.Right)
			{
				var playerControl = (PlayerControl)sender;
				ShowPlayerContextMenu(playerControl, e.Location);
			}
		}

		private void ShowPlayerContextMenu(PlayerControl playerControl, Point location)
		{
			var contextMenu = new ContextMenuStrip();

			var toggleFavoriteItem = new ToolStripMenuItem(playerControl.IsFavorite ? "Remove from favorites" : "Add to favorites");
			toggleFavoriteItem.Click += (sender, e) => ToggleFavoritePlayer(playerControl);
			contextMenu.Items.Add(toggleFavoriteItem);

			var toggleSelectItem = new ToolStripMenuItem(playerControl.IsSelected ? "Deselect" : "Select");
			toggleSelectItem.Click += (sender, e) => ToggleSelectPlayer(playerControl);
			contextMenu.Items.Add(toggleSelectItem);

			contextMenu.Show(playerControl, location);
		}

		private void ToggleSelectPlayer(PlayerControl playerControl)
		{
			playerControl.IsSelected = !playerControl.IsSelected;

			if (playerControl.IsSelected)
			{
				if (!_selectedPlayers.Contains(playerControl))
				{
					_selectedPlayers.Add(playerControl);
				}
			}
			else
			{
				_selectedPlayers.Remove(playerControl);
			}

			UpdatePlayerControl(pnlAllPlayers, playerControl);
			UpdatePlayerControl(pnlFavoritePlayers, playerControl);

			UpdateMoveToFavoritesButtonVisibility();

			logForm.Log($"Player {playerControl.Player.Name} {(playerControl.IsSelected ? "selected" : "deselected")}. Total selected: {_selectedPlayers.Count}");
		}

		private void UpdateMoveToFavoritesButtonVisibility()
		{
			btnMoveToFavorites.Visible = _selectedPlayers != null && _selectedPlayers.Count > 0;
		}

		private void UpdatePlayerControl(Panel panel, PlayerControl updatedControl)
		{
			foreach (Control control in panel.Controls)
			{
				if (control is PlayerControl playerControl && playerControl.Player.Name == updatedControl.Player.Name)
				{
					playerControl.IsSelected = updatedControl.IsSelected;
					break;
				}
			}
		}

		private async Task ToggleFavoritePlayer(PlayerControl playerControl)
		{
			if (playerControl.IsFavorite)
			{
				_favoritePlayers.RemoveAll(pc => pc.Player.Name == playerControl.Player.Name);
				playerControl.IsFavorite = false;
				logForm.Log($"Removed player from favorites: {playerControl.Player.Name}");

				// We no longer need to manually remove the control from pnlFavoritePlayers
				// as UpdatePlayerPanels will handle this for us
			}
			else
			{
				if (_favoritePlayers.Count < 3)
				{
					_favoritePlayers.Add(playerControl);
					playerControl.IsFavorite = true;
					logForm.Log($"Added player to favorites: {playerControl.Player.Name}");

					// We no longer need to manually add the control to pnlFavoritePlayers
					// as UpdatePlayerPanels will handle this for us
				}
				else
				{
					MessageBox.Show("You can only have up to 3 favorite players.", "Limit Reached", MessageBoxButtons.OK, MessageBoxIcon.Information);
					logForm.Log("Attempt to add more than 3 favorite players");
					return;
				}
			}

			// Update both panels to reflect the changes
			UpdatePlayerPanels();

			await SaveFavoritePlayers();
		}

		private async Task SaveFavoritePlayers()
		{
			var currentTeam = (Team)cmbTeams.SelectedItem;
			if (currentTeam != null)
			{
				var favoritePlayerNames = _favoritePlayers.Select(pc => pc.Player.Name).ToList();
				await _dataProvider.SaveFavoritePlayersAsync(currentTeam.FifaCode, favoritePlayerNames);
				logForm.Log($"Saved favorite players for team {currentTeam.FifaCode}: {string.Join(", ", favoritePlayerNames)}");
			}
			else
			{
				logForm.Log("No team selected, favorites not saved");
			}
		}

		private async Task LoadFavoritePlayers(string fifaCode)
		{
			var favoritePlayerNames = await _dataProvider.LoadFavoritePlayersAsync(fifaCode);
			logForm.Log($"Loaded {favoritePlayerNames.Count} favorite player names for team {fifaCode}");

			_favoritePlayers.Clear();

			foreach (var playerName in favoritePlayerNames)
			{
				var player = _allPlayers.FirstOrDefault(p => p.Name == playerName);
				if (player != null)
				{
					_favoritePlayers.Add(new PlayerControl(player) { IsFavorite = true });
				}
			}

			logForm.Log($"Created {_favoritePlayers.Count} PlayerControls for favorite players");
		}


		private void pnlAllPlayers_DragEnter(object sender, DragEventArgs e)
		{
			e.Effect = e.AllowedEffect;
		}

		private void pnlFavoritePlayers_DragEnter(object sender, DragEventArgs e)
		{
			e.Effect = e.AllowedEffect;
		}

		private async void pnlAllPlayers_DragDrop(object sender, DragEventArgs e)
		{
			PlayerControl playerControl = (PlayerControl)e.Data.GetData(typeof(PlayerControl));
			await ToggleFavoritePlayer(playerControl);
			UpdatePlayerPanels();
			logForm.Log($"Drag and drop event triggered for player: {playerControl.Player.Name}");
		}

		private async void pnlFavoritePlayers_DragDrop(object sender, DragEventArgs e)
		{
			PlayerControl playerControl = (PlayerControl)e.Data.GetData(typeof(PlayerControl));
			await ToggleFavoritePlayer(playerControl);
			UpdatePlayerPanels();
			logForm.Log($"Drag and drop event triggered for player: {playerControl.Player.Name}");
		}


		private void btnPrintStatistics_Click(object sender, EventArgs e)
		{
			// Implementacija ispisa statistike
			// Ovo bi moglo uklju�ivati stvaranje PDF dokumenta ili kori�tenje PrintDocument klase
		}

		private void mnuSettings_Click(object sender, EventArgs e)
		{
			using (var settingsForm = new InitialSettingsForm(_dataProvider))
			{
				if (settingsForm.ShowDialog() == DialogResult.OK)
				{
					LoadInitialSettings();
				}
			}
		}

		private void mnuExit_Click(object sender, EventArgs e)
		{
			Application.Exit();
		}

		private async void cmbTeams_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (cmbTeams.SelectedItem is Team selectedTeam)
			{
				logForm.Log($"Team changed to: {selectedTeam.Country} ({selectedTeam.FifaCode})");
				await _dataProvider.SaveFavoriteTeamAsync(selectedTeam.FifaCode);
				ClearSelection(); // Add this line
				await LoadPlayerDetails(selectedTeam.FifaCode);
			}
		}

		private void ClearSelection()
		{
			_selectedPlayers?.Clear();
			UpdatePlayerPanels();
			UpdateMoveToFavoritesButtonVisibility();
			logForm?.Log("All player selections cleared.");
		}

		private void printDocument_PrintPage(object sender, System.Drawing.Printing.PrintPageEventArgs e)
		{
			e.Graphics.SmoothingMode = SmoothingMode.HighQuality;
			//TODO 

		}


		private void chosePrintType_Click(object sender, EventArgs e)
		{
			printDialog.ShowDialog();
		}

		private void printPreview_Click(object sender, EventArgs e)
		{
			printPreviewDialog.ShowDialog();
		}

		private void print_Click(object sender, EventArgs e)
		{
			printDocument.Print();
		}

		private void settingsToolStripMenuItem_Click(object sender, EventArgs e)
		{
			OpenSettings();
		}

		private async void OpenSettings()
		{
			using (var settingsForm = new InitialSettingsForm(_dataProvider))
			{
				settingsForm.SetCurrentSettings(_selectedChampionship, _selectedLanguage);
				if (settingsForm.ShowDialog() == DialogResult.OK)
				{
					_selectedChampionship = await _dataProvider.LoadSettingsAsync("Championship");
					_selectedLanguage = await _dataProvider.LoadSettingsAsync("Language");
					ChangeLanguage(_selectedLanguage);  // Add this line
					await ApplySettings();
				}
			}
		}

		private void printStatistics_Click(object sender, EventArgs e)
		{
			string report = GenerateStatisticsReport();

			PrintDocument pd = new PrintDocument();
			pd.PrintPage += (s, ev) =>
			{
				ev.Graphics.DrawString(report, new Font("Arial", 12), Brushes.Black, ev.MarginBounds);
			};

			PrintPreviewDialog ppd = new PrintPreviewDialog();
			ppd.Document = pd;
			ppd.ShowDialog();
		}

		private string GenerateStatisticsReport()
		{
			StringBuilder report = new StringBuilder();
			report.AppendLine("World Cup Statistics Report");
			report.AppendLine("===========================");
			report.AppendLine();

			// Add top scorers
			report.AppendLine("Top Scorers:");
			foreach (var item in lstTopScorers.Items)
			{
				report.AppendLine(item.ToString());
			}
			report.AppendLine();

			// Add yellow cards
			report.AppendLine("Most Yellow Cards:");
			foreach (var item in lstYellowCards.Items)
			{
				report.AppendLine(item.ToString());
			}
			report.AppendLine();

			// Add matches
			report.AppendLine("Matches:");
			foreach (var item in lstMatches.Items)
			{
				report.AppendLine(item.ToString());
			}

			return report.ToString();


		}

		private void btnMoveToFavorites_Click(object sender, EventArgs e)
		{
			MoveSelectedPlayersToFavorites();
		}

		private void MoveSelectedPlayersToFavorites()
		{
			if (_selectedPlayers == null || _selectedPlayers.Count == 0)
			{
				return;
			}

			foreach (var playerControl in _selectedPlayers.ToList())
			{
				if (_favoritePlayers.Count < 3)
				{
					if (!_favoritePlayers.Any(fp => fp.Player.Name == playerControl.Player.Name))
					{
						playerControl.IsFavorite = true;
						_favoritePlayers.Add(playerControl);
						logForm.Log($"Added player to favorites: {playerControl.Player.Name}");
					}
				}
				else
				{
					MessageBox.Show("You can only have up to 3 favorite players.", "Limit Reached", MessageBoxButtons.OK, MessageBoxIcon.Information);
					break;
				}
			}

			_selectedPlayers.Clear();
			UpdatePlayerPanels();
			UpdateMoveToFavoritesButtonVisibility();
			SaveFavoritePlayers();
		}
	}
}
