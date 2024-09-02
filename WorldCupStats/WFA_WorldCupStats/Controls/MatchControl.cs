using System;
using System.Windows.Forms;
using DataLayer.Models;

namespace WFA_WorldCupStats.Controls
{
	public partial class MatchControl : UserControl
	{
		public string HomeTeam { get; private set; }
		public string AwayTeam { get; private set; }
		public string Attendance { get; private set; }

		public MatchControl(Match match)
		{
			InitializeComponent();
			HomeTeam = match.HomeTeamCountry;
			AwayTeam = match.AwayTeamCountry;
			Attendance = match.Attendance;

			lblTeams.Text = $"{HomeTeam} vs {AwayTeam}";
			lblScore.Text = $"{match.HomeTeam.Goals} - {match.AwayTeam.Goals}";
			lblAttendance.Text = $"Attendance: {Attendance}";
			lblLocation.Text = $"Location: {match.Location}";
		}
	}
}