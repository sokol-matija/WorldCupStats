namespace WFA_WorldCupStats
{
	partial class Form1
	{
		/// <summary>
		///  Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		///  Clean up any resources being used.
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
		///  Required method for Designer support - do not modify
		///  the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
			cmbTeams = new ComboBox();
			lblChooseTeam = new Label();
			lstMatches = new ListBox();
			lstYellowCards = new ListBox();
			lstTopScorers = new ListBox();
			lblMatches = new Label();
			lblYellowCards = new Label();
			lblTopScorers = new Label();
			pnlAllPlayers = new Panel();
			pnlFavoritePlayers = new Panel();
			lblAllPlayers = new Label();
			lblFavoritePlayers = new Label();
			menuStrip = new MenuStrip();
			mnuSettings = new ToolStripMenuItem();
			mnuPrint = new ToolStripMenuItem();
			mnuChosePrintType = new ToolStripMenuItem();
			mnuPrintPreview = new ToolStripMenuItem();
			mnuPrinting = new ToolStripMenuItem();
			mnuPrintStatistics = new ToolStripMenuItem();
			printDialog = new PrintDialog();
			printDocument = new System.Drawing.Printing.PrintDocument();
			printPreviewDialog = new PrintPreviewDialog();
			statusStrip = new StatusStrip();
			btnMoveToFavorites = new Button();
			menuStrip.SuspendLayout();
			SuspendLayout();
			// 
			// cmbTeams
			// 
			cmbTeams.FormattingEnabled = true;
			cmbTeams.Location = new Point(38, 74);
			cmbTeams.Name = "cmbTeams";
			cmbTeams.Size = new Size(212, 38);
			cmbTeams.TabIndex = 0;
			cmbTeams.SelectedIndexChanged += cmbTeams_SelectedIndexChanged;
			// 
			// lblChooseTeam
			// 
			lblChooseTeam.AutoSize = true;
			lblChooseTeam.Location = new Point(38, 40);
			lblChooseTeam.Name = "lblChooseTeam";
			lblChooseTeam.Size = new Size(142, 30);
			lblChooseTeam.TabIndex = 1;
			lblChooseTeam.Text = "Choose Team:";
			// 
			// lstMatches
			// 
			lstMatches.FormattingEnabled = true;
			lstMatches.ItemHeight = 30;
			lstMatches.Location = new Point(1509, 78);
			lstMatches.Name = "lstMatches";
			lstMatches.Size = new Size(283, 424);
			lstMatches.TabIndex = 2;
			// 
			// lstYellowCards
			// 
			lstYellowCards.FormattingEnabled = true;
			lstYellowCards.ItemHeight = 30;
			lstYellowCards.Location = new Point(1156, 78);
			lstYellowCards.Name = "lstYellowCards";
			lstYellowCards.Size = new Size(278, 424);
			lstYellowCards.TabIndex = 3;
			// 
			// lstTopScorers
			// 
			lstTopScorers.FormattingEnabled = true;
			lstTopScorers.ItemHeight = 30;
			lstTopScorers.Location = new Point(796, 78);
			lstTopScorers.Name = "lstTopScorers";
			lstTopScorers.Size = new Size(278, 424);
			lstTopScorers.TabIndex = 4;
			// 
			// lblMatches
			// 
			lblMatches.AutoSize = true;
			lblMatches.Location = new Point(1509, 40);
			lblMatches.Name = "lblMatches";
			lblMatches.Size = new Size(97, 30);
			lblMatches.TabIndex = 5;
			lblMatches.Text = "Matches:";
			// 
			// lblYellowCards
			// 
			lblYellowCards.AutoSize = true;
			lblYellowCards.Location = new Point(1156, 40);
			lblYellowCards.Name = "lblYellowCards";
			lblYellowCards.Size = new Size(134, 30);
			lblYellowCards.TabIndex = 6;
			lblYellowCards.Text = "Yellow Cards:";
			// 
			// lblTopScorers
			// 
			lblTopScorers.AutoSize = true;
			lblTopScorers.Location = new Point(796, 40);
			lblTopScorers.Name = "lblTopScorers";
			lblTopScorers.Size = new Size(119, 30);
			lblTopScorers.TabIndex = 7;
			lblTopScorers.Text = "Top Scorers";
			// 
			// pnlAllPlayers
			// 
			pnlAllPlayers.AllowDrop = true;
			pnlAllPlayers.AutoScroll = true;
			pnlAllPlayers.BorderStyle = BorderStyle.FixedSingle;
			pnlAllPlayers.Location = new Point(38, 190);
			pnlAllPlayers.Margin = new Padding(4);
			pnlAllPlayers.Name = "pnlAllPlayers";
			pnlAllPlayers.Size = new Size(430, 360);
			pnlAllPlayers.TabIndex = 9;
			// 
			// pnlFavoritePlayers
			// 
			pnlFavoritePlayers.AllowDrop = true;
			pnlFavoritePlayers.AutoScroll = true;
			pnlFavoritePlayers.BorderStyle = BorderStyle.FixedSingle;
			pnlFavoritePlayers.Location = new Point(38, 608);
			pnlFavoritePlayers.Margin = new Padding(4);
			pnlFavoritePlayers.Name = "pnlFavoritePlayers";
			pnlFavoritePlayers.Size = new Size(430, 412);
			pnlFavoritePlayers.TabIndex = 10;
			// 
			// lblAllPlayers
			// 
			lblAllPlayers.AutoSize = true;
			lblAllPlayers.Location = new Point(38, 156);
			lblAllPlayers.Margin = new Padding(4, 0, 4, 0);
			lblAllPlayers.Name = "lblAllPlayers";
			lblAllPlayers.Size = new Size(108, 30);
			lblAllPlayers.TabIndex = 0;
			lblAllPlayers.Text = "All Players";
			// 
			// lblFavoritePlayers
			// 
			lblFavoritePlayers.AutoSize = true;
			lblFavoritePlayers.Location = new Point(38, 573);
			lblFavoritePlayers.Margin = new Padding(4, 0, 4, 0);
			lblFavoritePlayers.Name = "lblFavoritePlayers";
			lblFavoritePlayers.Size = new Size(156, 30);
			lblFavoritePlayers.TabIndex = 11;
			lblFavoritePlayers.Text = "Favorite Players";
			// 
			// menuStrip
			// 
			menuStrip.ImageScalingSize = new Size(28, 28);
			menuStrip.Items.AddRange(new ToolStripItem[] { mnuSettings, mnuPrint });
			menuStrip.Location = new Point(0, 0);
			menuStrip.Name = "menuStrip";
			menuStrip.Size = new Size(1818, 38);
			menuStrip.TabIndex = 12;
			menuStrip.Text = "menuStrip1";
			// 
			// mnuSettings
			// 
			mnuSettings.Name = "mnuSettings";
			mnuSettings.Size = new Size(105, 34);
			mnuSettings.Text = "Settings";
			mnuSettings.Click += mnuSettings_Click;
			// 
			// mnuPrint
			// 
			mnuPrint.DropDownItems.AddRange(new ToolStripItem[] { mnuChosePrintType, mnuPrintPreview, mnuPrinting, mnuPrintStatistics });
			mnuPrint.Name = "mnuPrint";
			mnuPrint.Size = new Size(74, 34);
			mnuPrint.Text = "Print";
			// 
			// mnuChosePrintType
			// 
			mnuChosePrintType.Name = "mnuChosePrintType";
			mnuChosePrintType.Size = new Size(315, 40);
			mnuChosePrintType.Text = "Chose Print Type";
			// 
			// mnuPrintPreview
			// 
			mnuPrintPreview.Name = "mnuPrintPreview";
			mnuPrintPreview.Size = new Size(315, 40);
			mnuPrintPreview.Text = "Print Preview";
			// 
			// mnuPrinting
			// 
			mnuPrinting.Name = "mnuPrinting";
			mnuPrinting.Size = new Size(315, 40);
			mnuPrinting.Text = "Print";
			// 
			// mnuPrintStatistics
			// 
			mnuPrintStatistics.Name = "mnuPrintStatistics";
			mnuPrintStatistics.Size = new Size(315, 40);
			mnuPrintStatistics.Text = "Print Statistics";
			mnuPrintStatistics.Click += printStatistics_Click;
			// 
			// printDialog
			// 
			printDialog.UseEXDialog = true;
			// 
			// printPreviewDialog
			// 
			printPreviewDialog.AutoScrollMargin = new Size(0, 0);
			printPreviewDialog.AutoScrollMinSize = new Size(0, 0);
			printPreviewDialog.ClientSize = new Size(400, 300);
			printPreviewDialog.Document = printDocument;
			printPreviewDialog.Enabled = true;
			printPreviewDialog.Icon = (Icon)resources.GetObject("printPreviewDialog.Icon");
			printPreviewDialog.Name = "printPreviewDialog";
			printPreviewDialog.Visible = false;
			// 
			// statusStrip
			// 
			statusStrip.ImageScalingSize = new Size(28, 28);
			statusStrip.Location = new Point(0, 1016);
			statusStrip.Name = "statusStrip";
			statusStrip.Size = new Size(1818, 22);
			statusStrip.TabIndex = 13;
			statusStrip.Text = "statusStrip1";
			// 
			// btnMoveToFavorites
			// 
			btnMoveToFavorites.Location = new Point(250, 143);
			btnMoveToFavorites.Name = "btnMoveToFavorites";
			btnMoveToFavorites.Size = new Size(218, 40);
			btnMoveToFavorites.TabIndex = 14;
			btnMoveToFavorites.Text = "Move To Favorites";
			btnMoveToFavorites.UseVisualStyleBackColor = true;
			btnMoveToFavorites.Visible = false;
			btnMoveToFavorites.Click += btnMoveToFavorites_Click;
			// 
			// Form1
			// 
			AutoScaleDimensions = new SizeF(12F, 30F);
			AutoScaleMode = AutoScaleMode.Font;
			ClientSize = new Size(1818, 1038);
			Controls.Add(btnMoveToFavorites);
			Controls.Add(statusStrip);
			Controls.Add(lblFavoritePlayers);
			Controls.Add(lblAllPlayers);
			Controls.Add(pnlFavoritePlayers);
			Controls.Add(pnlAllPlayers);
			Controls.Add(lblTopScorers);
			Controls.Add(lblYellowCards);
			Controls.Add(lblMatches);
			Controls.Add(lstTopScorers);
			Controls.Add(lstYellowCards);
			Controls.Add(lstMatches);
			Controls.Add(lblChooseTeam);
			Controls.Add(cmbTeams);
			Controls.Add(menuStrip);
			MainMenuStrip = menuStrip;
			Name = "Form1";
			StartPosition = FormStartPosition.CenterScreen;
			Text = "World Cup Statistics";
			menuStrip.ResumeLayout(false);
			menuStrip.PerformLayout();
			ResumeLayout(false);
			PerformLayout();
		}

		#endregion

		public ComboBox cmbTeams;
		public Label lblChooseTeam;
		public ListBox lstMatches;
		public ListBox lstYellowCards;
		public ListBox lstTopScorers;
		public Label lblMatches;
		public Label lblYellowCards;
		public Label lblTopScorers;
		public Panel pnlAllPlayers;
		public Panel pnlFavoritePlayers;
		public Label lblAllPlayers;
		public Label lblFavoritePlayers;
		public MenuStrip menuStrip;
		public PrintDialog printDialog;
		public System.Drawing.Printing.PrintDocument printDocument;
		public PrintPreviewDialog printPreviewDialog;
		public StatusStrip statusStrip;
		public ToolStripMenuItem mnuSettings;
		public ToolStripMenuItem mnuPrint;
		public ToolStripMenuItem mnuChosePrintType;
		public ToolStripMenuItem mnuPrintPreview;
		public ToolStripMenuItem mnuPrinting;
		public ToolStripMenuItem mnuPrintStatistics;
		public Button btnMoveToFavorites;
	}
}
