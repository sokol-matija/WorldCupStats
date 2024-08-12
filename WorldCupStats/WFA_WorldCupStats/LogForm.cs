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
		public LogForm()
		{
			InitializeComponent();
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
