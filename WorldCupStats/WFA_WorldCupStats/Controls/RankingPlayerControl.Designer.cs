namespace WFA_WorldCupStats.Controls
{
	partial class RankingPlayerControl
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
			lblName = new Label();
			lblCount = new Label();
			picPlayer = new PictureBox();
			((System.ComponentModel.ISupportInitialize)picPlayer).BeginInit();
			SuspendLayout();
			// 
			// lblName
			// 
			lblName.AutoSize = true;
			lblName.Location = new Point(18, 14);
			lblName.Name = "lblName";
			lblName.Size = new Size(68, 30);
			lblName.TabIndex = 0;
			lblName.Text = "label1";
			// 
			// lblCount
			// 
			lblCount.AutoSize = true;
			lblCount.Location = new Point(18, 61);
			lblCount.Name = "lblCount";
			lblCount.Size = new Size(68, 30);
			lblCount.TabIndex = 1;
			lblCount.Text = "label2";
			// 
			// picPlayer
			// 
			picPlayer.Location = new Point(172, 7);
			picPlayer.Name = "picPlayer";
			picPlayer.Size = new Size(98, 88);
			picPlayer.TabIndex = 2;
			picPlayer.TabStop = false;
			// 
			// RankingPlayerControl
			// 
			AutoScaleDimensions = new SizeF(12F, 30F);
			AutoScaleMode = AutoScaleMode.Font;
			BorderStyle = BorderStyle.FixedSingle;
			Controls.Add(picPlayer);
			Controls.Add(lblCount);
			Controls.Add(lblName);
			Name = "RankingPlayerControl";
			Size = new Size(282, 96);
			((System.ComponentModel.ISupportInitialize)picPlayer).EndInit();
			ResumeLayout(false);
			PerformLayout();
		}

		#endregion

		private Label lblName;
		private Label lblCount;
		private PictureBox picPlayer;
	}
}
