using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DataLayer;
using DataLayer.Models;

namespace WFA_WorldCupStats.Controls
{
	public partial class MatchControl : UserControl
	{
		public MatchControl(Match match)
		{
			 InitializeComponent();
        lblTeams.Text = $"{match.HomeTeamCountry} vs {match.AwayTeamCountry}";
        lblScore.Text = $"{match.HomeTeam.Goals} - {match.AwayTeam.Goals}";
        lblAttendance.Text = $"Attendance: {match.Attendance}";
        lblLocation.Text = $"Location: {match.Location}";

		}
	}
}
