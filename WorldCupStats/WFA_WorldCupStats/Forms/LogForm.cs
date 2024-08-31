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
			InitializeMenuStrip();
			SetupContextMenu();
		}

		private void InitializeMenuStrip()
		{
			MenuStrip menuStrip = new MenuStrip();
			ToolStripMenuItem fileMenuItem = new ToolStripMenuItem("File");
			ToolStripMenuItem copyMenuItem = new ToolStripMenuItem("Copy All Logs");
			copyMenuItem.Click += CopyMenuItem_Click;

			fileMenuItem.DropDownItems.Add(copyMenuItem);
			menuStrip.Items.Add(fileMenuItem);

			this.MainMenuStrip = menuStrip;
			this.Controls.Add(menuStrip);
		}

		private void CopyMenuItem_Click(object sender, EventArgs e)
		{
			StringBuilder logContent = new StringBuilder();
			foreach (var item in listBoxLogs.Items)
			{
				logContent.AppendLine(item.ToString());
			}

			if (logContent.Length > 0)
			{
				Clipboard.SetText(logContent.ToString());
				MessageBox.Show("All log messages have been copied to the clipboard.", "Copy Successful", MessageBoxButtons.OK, MessageBoxIcon.Information);
			}
			else
			{
				MessageBox.Show("There are no log messages to copy.", "No Content", MessageBoxButtons.OK, MessageBoxIcon.Information);
			}
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
