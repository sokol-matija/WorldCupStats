namespace WFA_WorldCupStats.Controls
{
	partial class MatchControl
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

		#region Component Designer generated code

		/// <summary> 
		/// Required method for Designer support - do not modify 
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			lblTeams = new Label();
			lblLocation = new Label();
			lblAttendance = new Label();
			lblScore = new Label();
			SuspendLayout();
			// 
			// lblTeams
			// 
			lblTeams.AutoSize = true;
			lblTeams.Location = new Point(28, 13);
			lblTeams.Name = "lblTeams";
			lblTeams.Size = new Size(68, 30);
			lblTeams.TabIndex = 0;
			lblTeams.Text = "label1";
			// 
			// lblLocation
			// 
			lblLocation.AutoSize = true;
			lblLocation.Location = new Point(28, 43);
			lblLocation.Name = "lblLocation";
			lblLocation.Size = new Size(68, 30);
			lblLocation.TabIndex = 1;
			lblLocation.Text = "label2";
			// 
			// lblAttendance
			// 
			lblAttendance.AutoSize = true;
			lblAttendance.Location = new Point(271, 43);
			lblAttendance.Name = "lblAttendance";
			lblAttendance.Size = new Size(68, 30);
			lblAttendance.TabIndex = 3;
			lblAttendance.Text = "label3";
			// 
			// lblScore
			// 
			lblScore.AutoSize = true;
			lblScore.Location = new Point(271, 13);
			lblScore.Name = "lblScore";
			lblScore.Size = new Size(68, 30);
			lblScore.TabIndex = 4;
			lblScore.Text = "label1";
			// 
			// MatchControl
			// 
			AutoScaleDimensions = new SizeF(12F, 30F);
			AutoScaleMode = AutoScaleMode.Font;
			BorderStyle = BorderStyle.FixedSingle;
			Controls.Add(lblScore);
			Controls.Add(lblAttendance);
			Controls.Add(lblLocation);
			Controls.Add(lblTeams);
			Name = "MatchControl";
			Size = new Size(508, 86);
			ResumeLayout(false);
			PerformLayout();
		}

		#endregion

		private Label lblTeams;
		private Label lblLocation;
		private Label lblAttendance;
		private Label lblScore;
	}
}
