using DataLayer.Models;
using System.Diagnostics;
using System.Windows;

namespace WPF_WorldCupStats
{
	public partial class TeamInfoWindow : Window
	{
		public TeamInfoWindow(Team team)
		{
			InitializeComponent();
			PopulateTeamInfo(team);
			DebugTeamInfo(team);
			
		}
		private void DebugTeamInfo(Team team)
		{
			System.Diagnostics.Debug.WriteLine($"FIFA Code: {team.FifaCode}");
			System.Diagnostics.Debug.WriteLine($"Games Played: {team.GamesPlayed}");
		}

		private void PopulateTeamInfo(Team team)
		{
				Debug.WriteLine($"Populating team info for: {team.Country}");
				Debug.WriteLine($"FIFA Code: {team.FifaCode}");
				Debug.WriteLine($"Games Played: {team.GamesPlayed}");
				Debug.WriteLine($"Wins: {team.Wins}");
				Debug.WriteLine($"Losses: {team.Losses}");
				Debug.WriteLine($"Draws: {team.Draws}");
				Debug.WriteLine($"Goals For: {team.GoalsFor}");
				Debug.WriteLine($"Goals Against: {team.GoalsAgainst}");
				Debug.WriteLine($"Goal Differential: {team.GoalDifferential}");

			if (team == null)
			{
				MessageBox.Show("No team data available.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
				Close();
				return;

			}

			txtTeamName.Text = team.Country;
			txtFifaCode.Text = $"FIFA Code: {team.FifaCode}";
			txtGamesPlayed.Text = $"Games Played: {team.GamesPlayed}";
			txtWins.Text = $"Wins: {team.Wins}";
			txtLosses.Text = $"Losses: {team.Losses}";
			txtDraws.Text = $"Draws: {team.Draws}";
			txtGoalsFor.Text = $"Goals For: {team.GoalsFor}";
			txtGoalsAgainst.Text = $"Goals Against: {team.GoalsAgainst}";
			txtGoalDifferential.Text = $"Goal Differential: {team.GoalDifferential}";
		}
	}
}