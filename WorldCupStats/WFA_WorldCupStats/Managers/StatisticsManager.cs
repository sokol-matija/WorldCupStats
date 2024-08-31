using DataLayer.Models;
using System.Text;

namespace WFA_WorldCupStats.Managers
{
    public class StatisticsManager
    {
        public void UpdateStatisticsList(MainForm form, ListBox listBox, List<PlayerStats> stats)
        {
            form.Invoke((MethodInvoker)delegate
            {
                listBox.Items.Clear();
                foreach (var stat in stats)
                {
                    listBox.Items.Add($"{stat.Name}: {stat.Count}");
                }
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
    }
}