namespace WFA_WorldCupStats
{
	partial class MainForm
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
			cmbTeams = new ComboBox();
			lblChooseTeam = new Label();
			lblMatches = new Label();
			lblYellowCards = new Label();
			lblTopScorers = new Label();
			pnlAllPlayers = new Panel();
			pnlFavoritePlayers = new Panel();
			lblAllPlayers = new Label();
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
			pnlTopScorers = new Panel();
			pnlYellowCards = new Panel();
			pnlMatches = new Panel();
			lblFavoritePlayers = new Label();
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
			// lblMatches
			// 
			lblMatches.AutoSize = true;
			lblMatches.Font = new Font("Segoe UI", 14.1428576F, FontStyle.Regular, GraphicsUnit.Point, 238);
			lblMatches.Location = new Point(999, 142);
			lblMatches.Name = "lblMatches";
			lblMatches.Size = new Size(150, 45);
			lblMatches.TabIndex = 5;
			lblMatches.Text = "Matches:";
			// 
			// lblYellowCards
			// 
			lblYellowCards.AutoSize = true;
			lblYellowCards.Font = new Font("Segoe UI", 14.1428576F, FontStyle.Regular, GraphicsUnit.Point, 238);
			lblYellowCards.Location = new Point(509, 560);
			lblYellowCards.Name = "lblYellowCards";
			lblYellowCards.Size = new Size(208, 45);
			lblYellowCards.TabIndex = 6;
			lblYellowCards.Text = "Yellow Cards:";
			// 
			// lblTopScorers
			// 
			lblTopScorers.AutoSize = true;
			lblTopScorers.Font = new Font("Segoe UI", 14.1428576F, FontStyle.Regular, GraphicsUnit.Point, 238);
			lblTopScorers.Location = new Point(509, 136);
			lblTopScorers.Name = "lblTopScorers";
			lblTopScorers.Size = new Size(186, 45);
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
			lblAllPlayers.Font = new Font("Segoe UI", 14.1428576F, FontStyle.Regular, GraphicsUnit.Point, 238);
			lblAllPlayers.Location = new Point(38, 136);
			lblAllPlayers.Margin = new Padding(4, 0, 4, 0);
			lblAllPlayers.Name = "lblAllPlayers";
			lblAllPlayers.Size = new Size(167, 45);
			lblAllPlayers.TabIndex = 0;
			lblAllPlayers.Text = "All Players";
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
			mnuChosePrintType.Size = new Size(286, 40);
			mnuChosePrintType.Text = "Chose Print Type";
			// 
			// mnuPrintPreview
			// 
			mnuPrintPreview.Name = "mnuPrintPreview";
			mnuPrintPreview.Size = new Size(286, 40);
			mnuPrintPreview.Text = "Print Preview";
			// 
			// mnuPrinting
			// 
			mnuPrinting.Name = "mnuPrinting";
			mnuPrinting.Size = new Size(286, 40);
			mnuPrinting.Text = "Print";
			// 
			// mnuPrintStatistics
			// 
			mnuPrintStatistics.Name = "mnuPrintStatistics";
			mnuPrintStatistics.Size = new Size(286, 40);
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
			// pnlTopScorers
			// 
			pnlTopScorers.AutoScroll = true;
			pnlTopScorers.Location = new Point(509, 190);
			pnlTopScorers.Name = "pnlTopScorers";
			pnlTopScorers.Size = new Size(414, 360);
			pnlTopScorers.TabIndex = 15;
			// 
			// pnlYellowCards
			// 
			pnlYellowCards.AutoScroll = true;
			pnlYellowCards.Location = new Point(509, 608);
			pnlYellowCards.Name = "pnlYellowCards";
			pnlYellowCards.Size = new Size(414, 405);
			pnlYellowCards.TabIndex = 16;
			// 
			// pnlMatches
			// 
			pnlMatches.AutoScroll = true;
			pnlMatches.Location = new Point(999, 190);
			pnlMatches.Name = "pnlMatches";
			pnlMatches.Size = new Size(649, 823);
			pnlMatches.TabIndex = 17;
			// 
			// lblFavoritePlayers
			// 
			lblFavoritePlayers.AutoSize = true;
			lblFavoritePlayers.Font = new Font("Segoe UI", 14.1428576F, FontStyle.Regular, GraphicsUnit.Point, 238);
			lblFavoritePlayers.Location = new Point(38, 558);
			lblFavoritePlayers.Margin = new Padding(4, 0, 4, 0);
			lblFavoritePlayers.Name = "lblFavoritePlayers";
			lblFavoritePlayers.Size = new Size(244, 45);
			lblFavoritePlayers.TabIndex = 11;
			lblFavoritePlayers.Text = "Favorite Players";
			// 
			// MainForm
			// 
			AutoScaleDimensions = new SizeF(12F, 30F);
			AutoScaleMode = AutoScaleMode.Font;
			ClientSize = new Size(1818, 1038);
			Controls.Add(pnlMatches);
			Controls.Add(pnlYellowCards);
			Controls.Add(pnlTopScorers);
			Controls.Add(btnMoveToFavorites);
			Controls.Add(statusStrip);
			Controls.Add(lblFavoritePlayers);
			Controls.Add(lblAllPlayers);
			Controls.Add(pnlFavoritePlayers);
			Controls.Add(pnlAllPlayers);
			Controls.Add(lblTopScorers);
			Controls.Add(lblYellowCards);
			Controls.Add(lblMatches);
			Controls.Add(lblChooseTeam);
			Controls.Add(cmbTeams);
			Controls.Add(menuStrip);
			MainMenuStrip = menuStrip;
			Name = "MainForm";
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
		public Label lblMatches;
		public Label lblYellowCards;
		public Label lblTopScorers;
		public Panel pnlAllPlayers;
		public Panel pnlFavoritePlayers;
		public Label lblAllPlayers;
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
		public Panel pnlTopScorers;
		public Panel pnlYellowCards;
		public Panel pnlMatches;
		public Label lblFavoritePlayers;
	}
}
