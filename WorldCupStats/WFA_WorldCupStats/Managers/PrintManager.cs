using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Printing;
using System.Text;
using System.Windows.Forms;
using DataLayer.Models;

namespace WFA_WorldCupStats.Managers
{
	public class PrintManager
	{
		private string _report;
		private int _currentPage;
		private readonly Font _titleFont = new Font("Arial", 14, FontStyle.Bold);
		private readonly Font _normalFont = new Font("Arial", 12);
		private int _linesPerPage;
		private List<string> _reportLines;

		public void PrepareReport(List<PlayerStats> topScorers, List<PlayerStats> yellowCards, List<Match> matches)
		{
			StringBuilder sb = new StringBuilder();
			sb.AppendLine("World Cup Statistics Report");
			sb.AppendLine("===========================");
			sb.AppendLine();

			AppendSection(sb, "Top Scorers:", topScorers, p => $"{p.Name}: {p.Count} goals");
			AppendSection(sb, "Yellow Cards:", yellowCards, p => $"{p.Name}: {p.Count} cards");
			AppendSection(sb, "Matches by Attendance:", matches, m => $"{m.HomeTeamCountry} vs {m.AwayTeamCountry}: {m.Attendance} attendees");

			_report = sb.ToString();
			_reportLines = _report.Split('\n').ToList();
		}

		private void AppendSection<T>(StringBuilder sb, string title, List<T> items, Func<T, string> formatter)
		{
			sb.AppendLine(title);
			foreach (var item in items)
			{
				sb.AppendLine(formatter(item));
			}
			sb.AppendLine();
		}

		public void ShowPrintPreview(string report = null)
		{
			if (report != null)
			{
				_report = report;
				_reportLines = _report.Split('\n').ToList();
			}

			PrintDocument pd = new PrintDocument();
			pd.PrintPage += PrintPage;

			PrintPreviewDialog ppd = new PrintPreviewDialog
			{
				Document = pd
			};
			ppd.ShowDialog();
		}

		public void Print(string report = null)
		{
			if (report != null)
			{
				_report = report;
				_reportLines = _report.Split('\n').ToList();
			}

			PrintDocument pd = new PrintDocument();
			pd.PrintPage += PrintPage;

			PrintDialog printDialog = new PrintDialog
			{
				Document = pd
			};

			if (printDialog.ShowDialog() == DialogResult.OK)
			{
				pd.Print();
			}
		}

		private void PrintPage(object sender, PrintPageEventArgs e)
		{
			float yPos = 0;
			int count = 0;
			float leftMargin = e.MarginBounds.Left;
			float topMargin = e.MarginBounds.Top;

			_linesPerPage = (int)((e.MarginBounds.Height - _titleFont.GetHeight(e.Graphics)) / _normalFont.GetHeight(e.Graphics));

			if (_currentPage == 0)
			{
				e.Graphics.DrawString("World Cup Statistics Report", _titleFont, Brushes.Black, leftMargin, topMargin);
				yPos = topMargin + _titleFont.GetHeight(e.Graphics) * 2;
				count++;
			}
			else
			{
				yPos = topMargin;
			}

			while (count < _linesPerPage && _currentPage * _linesPerPage + count < _reportLines.Count)
			{
				string line = _reportLines[_currentPage * _linesPerPage + count];
				e.Graphics.DrawString(line, _normalFont, Brushes.Black, leftMargin, yPos);
				count++;
				yPos += _normalFont.GetHeight(e.Graphics);
			}

			_currentPage++;

			if (_currentPage * _linesPerPage < _reportLines.Count)
			{
				e.HasMorePages = true;
			}
			else
			{
				e.HasMorePages = false;
				_currentPage = 0;
			}
		}

		public void PrintToPdf(string filePath)
		{
			// Ova metoda je placeholder. Za stvarnu implementaciju PDF ispisa,
			// trebat ćete koristiti biblioteku trećih strana poput iTextSharp ili PdfSharp.
			throw new NotImplementedException("PDF printing is not implemented yet.");
		}
	}
}