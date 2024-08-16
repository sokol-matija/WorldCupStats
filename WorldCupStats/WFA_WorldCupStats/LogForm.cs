using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WFA_WorldCupStats
{
	public partial class LogForm : Form
	{
		private ContextMenuStrip contextMenuStrip;
		public LogForm()
		{
			InitializeComponent();
			SetupContextMenu();
		}

		private void SetupContextMenu()
		{
			contextMenuStrip = new ContextMenuStrip();
			ToolStripMenuItem clearLogsItem = new ToolStripMenuItem("Clear Logs");
			clearLogsItem.Click += ClearLogsItem_Click;
			contextMenuStrip.Items.Add(clearLogsItem);

			listBoxLogs.ContextMenuStrip = contextMenuStrip;
		}

		private void ClearLogsItem_Click(object sender, EventArgs e)
		{
			listBoxLogs.Items.Clear();
		}


		public void Log(string message)
		{
			if (listBoxLogs.InvokeRequired)
			{
				listBoxLogs.Invoke(new Action<string>(Log), message);
			}
			else
			{
				listBoxLogs.Items.Add($"{DateTime.Now}: {message}");
				listBoxLogs.TopIndex = listBoxLogs.Items.Count - 1;
			}
		}
	}
}
