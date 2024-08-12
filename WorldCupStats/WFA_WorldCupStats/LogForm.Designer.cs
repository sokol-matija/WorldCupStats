namespace WFA_WorldCupStats
{
	partial class LogForm
	{
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing && (components != null))
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Windows Form Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			listBoxLogs = new ListBox();
			SuspendLayout();
			// 
			// listBoxLogs
			// 
			listBoxLogs.Dock = DockStyle.Fill;
			listBoxLogs.FormattingEnabled = true;
			listBoxLogs.ItemHeight = 30;
			listBoxLogs.Location = new Point(0, 0);
			listBoxLogs.Name = "listBoxLogs";
			listBoxLogs.Size = new Size(540, 450);
			listBoxLogs.TabIndex = 0;
			// 
			// LogForm
			// 
			AutoScaleDimensions = new SizeF(12F, 30F);
			AutoScaleMode = AutoScaleMode.Font;
			ClientSize = new Size(540, 450);
			Controls.Add(listBoxLogs);
			Name = "LogForm";
			Text = "LogForm";
			ResumeLayout(false);
		}

		#endregion

		private ListBox listBoxLogs;
	}
}