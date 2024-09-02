using System.Text;
using DataLayer.Models;
using WFA_WorldCupStats.Controls;

namespace WFA_WorldCupStats.Managers
{
	public class StatisticsManager
	{
		public string GenerateStatisticsReport(Panel topScorers, Panel yellowCards, Panel matches)
		{
			StringBuilder report = new StringBuilder();
			report.AppendLine("World Cup Statistics Report");
			report.AppendLine("===========================");
			report.AppendLine();

			AppendPanelItems(report, "Top Scorers:", topScorers);
			AppendPanelItems(report, "Most Yellow Cards:", yellowCards);
			AppendMatchItems(report, "Matches:", matches);

			return report.ToString();
		}

		private void AppendPanelItems(StringBuilder report, string title, Panel panel)
		{
			report.AppendLine(title);
			foreach (RankingPlayerControl control in panel.Controls)
			{
				report.AppendLine($"{control.PlayerName}: {control.Count}");
			}
			report.AppendLine();
		}

		private void AppendMatchItems(StringBuilder report, string title, Panel panel)
		{
			report.AppendLine(title);
			foreach (MatchControl control in panel.Controls)
			{
				report.AppendLine($"{control.HomeTeam} vs {control.AwayTeam}: Attendance {control.Attendance}");
			}
			report.AppendLine();
		}
	}
}