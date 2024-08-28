using DataLayer.Models;
using System.Drawing.Printing;
using System.Resources;
using System.Text;

namespace WFA_WorldCupStats
{
	public class UIManager
	{
		private readonly Form1 _form;
		private readonly LogForm _logForm;

		public UIManager(Form1 form, LogForm logForm)
		{
			_form = form;
			_logForm = logForm;
		}

		public void ChangeLanguage(string language)
		{
			if (language == "hr" || language == "Croatian" || language == "Hrvatski")
			{
				Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo("hr-HR");
			}
			else
			{
				Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo("en-US");
			}
		}

		public void ApplyLocalization()
		{
			_form.Text = Strings.MainFormTitle;

			LocalizeMenuItems();

			LocalizeLabels();

			LocalizeButtons();

			LocalizePlayerControls(_form.pnlAllPlayers);
			LocalizePlayerControls(_form.pnlFavoritePlayers);

			_form._logForm.Log("UI localized successfully.");
		}

		private void LocalizeMenuItems()
		{
			_form.mnuSettings.Text = Strings.SettingsMenuItem;
			_form.mnuChosePrintType.Text = Strings.ChosePrintTypeMenuItem;
			_form.mnuPrintPreview.Text = Strings.PrintPreviewMenuItem;
			_form.mnuPrint.Text = Strings.PrintMenuItem;
			_form.mnuPrintStatistics.Text = Strings.PrintStatisticsMenuItem;
		}

		private void LocalizeLabels()
		{
			_form.lblTopScorers.Text = Strings.TopScorersLabel;
			_form.lblYellowCards.Text = Strings.YellowCardsLabel;
			_form.lblAllPlayers.Text = Strings.AllPlayersLabel;
			_form.lblFavoritePlayers.Text = Strings.FavoritePlayersLabel;
			_form.lblMatches.Text = Strings.MatchesLabel;
			_form.lblChooseTeam.Text = Strings.ChooseTeamLabel;
		}

		private void LocalizeButtons()
		{
			_form.btnMoveToFavorites.Text = Strings.MoveToFavorites;
			// Add other buttons here as needed
		}

		private void LocalizePlayerControls(Panel panel)
		{
			foreach (Control control in panel.Controls)
			{
				if (control is PlayerControl playerControl)
				{
					playerControl.ApplyLocalization();
				}
			}
		}

		public void ShowLoadingIndicator(bool isLoading)
		{
			_form.Invoke((MethodInvoker)delegate
			{
				_form.Cursor = isLoading ? Cursors.WaitCursor : Cursors.Default;
				// Disable/enable controls as needed
			});
		}

		public void PopulateTeamsComboBox(List<Team> teams, string favoriteTeam)
		{
			_form.Invoke((MethodInvoker)delegate
			{
				_form.cmbTeams.DataSource = teams;
				_form.cmbTeams.DisplayMember = "DisplayName";
				_form.cmbTeams.ValueMember = "FifaCode";

				if (!string.IsNullOrEmpty(favoriteTeam))
				{
					var team = teams.FirstOrDefault(t => t.FifaCode == favoriteTeam);
					if (team != null)
					{
						_form.cmbTeams.SelectedValue = team.FifaCode;
					}
				}
			});
		}

		public void UpdateMatchesList(List<Match> matches)
		{
			_form.Invoke((MethodInvoker)delegate
			{
				_form.lstMatches.Items.Clear();
				foreach (var match in matches)
				{
					_form.lstMatches.Items.Add($"{match.HomeTeamCountry} vs {match.AwayTeamCountry} - {match.HomeTeam.Goals}:{match.AwayTeam.Goals}");
				}
			});
		}

		public void UpdateStatisticsList(ListBox listBox, List<PlayerStats> stats)
		{
			_form.Invoke((MethodInvoker)delegate
			{
				listBox.Items.Clear();
				foreach (var stat in stats)
				{
					listBox.Items.Add($"{stat.Name}: {stat.Count}");
				}
			});
		}

		public void UpdatePlayerPanels(List<Player> allPlayers, HashSet<PlayerControl> favoritePlayers)
		{
			_form.Invoke((MethodInvoker)delegate
			{
				_form.pnlAllPlayers.Controls.Clear();
				_form.pnlFavoritePlayers.Controls.Clear();

				foreach (var player in allPlayers)
				{
					var existingControl = favoritePlayers.FirstOrDefault(fp => fp.Player.Name == player.Name);
					PlayerControl playerControl;

					if (existingControl != null)
					{
						playerControl = existingControl;
					}
					else
					{
						playerControl = new PlayerControl(player, _logForm);
						playerControl.FavoriteToggled += _form.PlayerControl_FavoriteToggled;
						playerControl.SelectionToggled += _form.PlayerControl_SelectionToggled;
					}

					if (playerControl.IsFavorite)
					{
						_form.pnlFavoritePlayers.Controls.Add(playerControl);
					}
					else
					{
						_form.pnlAllPlayers.Controls.Add(playerControl);
					}
				}

				ArrangePanelControls(_form.pnlAllPlayers);
				ArrangePanelControls(_form.pnlFavoritePlayers);
			});
		}

		private void ArrangePanelControls(Panel panel)
		{
			int yPosition = 0;
			foreach (Control control in panel.Controls)
			{
				control.Location = new Point(0, yPosition);
				yPosition += control.Height + 5; // 5 pixels spacing
			}
			panel.Refresh();
		}

		public void UpdateMoveToFavoritesButtonVisibility(int selectedPlayersCount)
		{
			_form.Invoke((MethodInvoker)delegate
			{
				_form.btnMoveToFavorites.Visible = selectedPlayersCount > 0;
			});
		}

		public string GenerateStatisticsReport(ListBox topScorers, ListBox yellowCards, ListBox matches)
		{
			StringBuilder report = new StringBuilder();
			report.AppendLine("World Cup Statistics Report");
			report.AppendLine("===========================");
			report.AppendLine();

			AppendListBoxItems(report, "Top Scorers:", topScorers);
			AppendListBoxItems(report, "Most Yellow Cards:", yellowCards);
			AppendListBoxItems(report, "Matches:", matches);

			return report.ToString();
		}

		private void AppendListBoxItems(StringBuilder report, string title, ListBox listBox)
		{
			report.AppendLine(title);
			foreach (var item in listBox.Items)
			{
				report.AppendLine(item.ToString());
			}
			report.AppendLine();
		}

		public void ShowPrintPreview(string report)
		{
			PrintDocument pd = new PrintDocument();
			pd.PrintPage += (s, ev) =>
			{
				ev.Graphics.DrawString(report, new Font("Arial", 12), Brushes.Black, ev.MarginBounds);
			};

			PrintPreviewDialog ppd = new PrintPreviewDialog();
			ppd.Document = pd;
			ppd.ShowDialog();
		}
	}
}