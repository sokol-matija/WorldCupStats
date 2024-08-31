namespace WFA_WorldCupStats
{
	partial class PlayerControl
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PlayerControl));
			lblName = new Label();
			lblNumber = new Label();
			lblPosition = new Label();
			chkCaptain = new CheckBox();
			picFavorite = new PictureBox();
			picPlayer = new PictureBox();
			((System.ComponentModel.ISupportInitialize)picFavorite).BeginInit();
			((System.ComponentModel.ISupportInitialize)picPlayer).BeginInit();
			SuspendLayout();
			// 
			// lblName
			// 
			lblName.AutoSize = true;
			lblName.Location = new Point(26, 15);
			lblName.Margin = new Padding(4, 0, 4, 0);
			lblName.Name = "lblName";
			lblName.Size = new Size(69, 30);
			lblName.TabIndex = 0;
			lblName.Text = "Name";
			// 
			// lblNumber
			// 
			lblNumber.AutoSize = true;
			lblNumber.Location = new Point(26, 45);
			lblNumber.Margin = new Padding(4, 0, 4, 0);
			lblNumber.Name = "lblNumber";
			lblNumber.Size = new Size(89, 30);
			lblNumber.TabIndex = 1;
			lblNumber.Text = "Number";
			// 
			// lblPosition
			// 
			lblPosition.AutoSize = true;
			lblPosition.Location = new Point(28, 75);
			lblPosition.Margin = new Padding(4, 0, 4, 0);
			lblPosition.Name = "lblPosition";
			lblPosition.Size = new Size(86, 30);
			lblPosition.TabIndex = 2;
			lblPosition.Text = "Position";
			// 
			// chkCaptain
			// 
			chkCaptain.AutoCheck = false;
			chkCaptain.AutoSize = true;
			chkCaptain.Location = new Point(28, 110);
			chkCaptain.Margin = new Padding(4);
			chkCaptain.Name = "chkCaptain";
			chkCaptain.Size = new Size(110, 34);
			chkCaptain.TabIndex = 3;
			chkCaptain.Text = "Captain";
			chkCaptain.UseVisualStyleBackColor = true;
			// 
			// picFavorite
			// 
			picFavorite.Image = (Image)resources.GetObject("picFavorite.Image");
			picFavorite.Location = new Point(182, 104);
			picFavorite.Margin = new Padding(4);
			picFavorite.Name = "picFavorite";
			picFavorite.Size = new Size(50, 40);
			picFavorite.TabIndex = 4;
			picFavorite.TabStop = false;
			// 
			// picPlayer
			// 
			picPlayer.Image = (Image)resources.GetObject("picPlayer.Image");
			picPlayer.Location = new Point(240, 4);
			picPlayer.Margin = new Padding(4);
			picPlayer.Name = "picPlayer";
			picPlayer.Size = new Size(137, 145);
			picPlayer.TabIndex = 5;
			picPlayer.TabStop = false;
			// 
			// PlayerControl
			// 
			AutoScaleDimensions = new SizeF(12F, 30F);
			AutoScaleMode = AutoScaleMode.Font;
			BorderStyle = BorderStyle.FixedSingle;
			Controls.Add(picPlayer);
			Controls.Add(picFavorite);
			Controls.Add(chkCaptain);
			Controls.Add(lblPosition);
			Controls.Add(lblNumber);
			Controls.Add(lblName);
			Margin = new Padding(4);
			Name = "PlayerControl";
			Size = new Size(379, 151);
			MouseDown += PlayerControl_MouseDown;
			((System.ComponentModel.ISupportInitialize)picFavorite).EndInit();
			((System.ComponentModel.ISupportInitialize)picPlayer).EndInit();
			ResumeLayout(false);
			PerformLayout();
		}

		#endregion

		private Label lblName;
		private Label lblNumber;
		private Label lblPosition;
		private CheckBox chkCaptain;
		private PictureBox picFavorite;
		private PictureBox picPlayer;
	}
}
