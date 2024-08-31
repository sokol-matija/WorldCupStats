using DataLayer.Models;
using System.Text;
using WFA_WorldCupStats.Controls;

namespace WFA_WorldCupStats.Managers
{
	public class StatisticsManager
	{
		public void UpdateStatisticsPanel(Panel panel, List<PlayerStats> stats)
		{
			panel.Invoke((MethodInvoker)delegate
			{
				panel.Controls.Clear();
				foreach (var stat in stats)
				{
					//TODO: Implement RankingPlayerControl
					//var control = new RankingPlayerControl(stat, _dataProvider);
					//panel.Controls.Add(control);
				}
			});
		}

		public void UpdateMatchesPanel(Panel panel, List<Match> matches)
		{
			panel.Invoke((MethodInvoker)delegate
			{
				panel.Controls.Clear();
				foreach (var match in matches)
				{
					var control = new MatchControl(match);
					panel.Controls.Add(control);
				}
			});
		}

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
				//report.AppendLine($"{control.}: {control.Count}");
			}
			report.AppendLine();
		}

		private void AppendMatchItems(StringBuilder report, string title, Panel panel)
		{
			report.AppendLine(title);
			foreach (MatchControl control in panel.Controls)
			{
				//report.AppendLine($"{control.MatchInfo}");
			}
			report.AppendLine();
		}
	}
}