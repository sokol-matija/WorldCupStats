using System.Drawing.Printing;

namespace WFA_WorldCupStats
{
	public class PrintManager
	{
		public void ShowPrintPreview(string report)
		{
			PrintDocument pd = new PrintDocument();
			pd.PrintPage += (sender, e) => PrintPage(sender, e, report);

			PrintPreviewDialog ppd = new PrintPreviewDialog
			{
				Document = pd
			};
			ppd.ShowDialog();
		}

		private void PrintPage(object sender, PrintPageEventArgs e, string report)
		{
			Font font = new Font("Arial", 12);
			float yPos = 0;
			int count = 0;
			float leftMargin = e.MarginBounds.Left;
			float topMargin = e.MarginBounds.Top;

			// Calculate the number of lines per page
			float linesPerPage = e.MarginBounds.Height / font.GetHeight();

			// Print each line of the report
			string[] lines = report.Split('\n');
			foreach (string line in lines)
			{
				yPos = topMargin + (count * font.GetHeight());
				e.Graphics.DrawString(line, font, Brushes.Black, leftMargin, yPos, new StringFormat());
				count++;

				if (count >= linesPerPage)
				{
					e.HasMorePages = true;
					return;
				}
			}

			e.HasMorePages = false;
		}

		public void Print(string report)
		{
			PrintDocument pd = new PrintDocument();
			pd.PrintPage += (sender, e) => PrintPage(sender, e, report);

			PrintDialog printDialog = new PrintDialog
			{
				Document = pd
			};

			if (printDialog.ShowDialog() == DialogResult.OK)
			{
				pd.Print();
			}
		}
	}
}