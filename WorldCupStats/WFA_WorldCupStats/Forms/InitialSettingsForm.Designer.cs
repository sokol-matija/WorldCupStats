namespace WFA_WorldCupStats
{
	partial class InitialSettingsForm
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
			lblChampionship = new Label();
			cmbChampionship = new ComboBox();
			lblLanguage = new Label();
			cmbLanguage = new ComboBox();
			btnSave = new Button();
			SuspendLayout();
			// 
			// lblChampionship
			// 
			lblChampionship.AutoSize = true;
			lblChampionship.Location = new Point(66, 82);
			lblChampionship.Name = "lblChampionship";
			lblChampionship.Size = new Size(146, 30);
			lblChampionship.TabIndex = 0;
			lblChampionship.Text = "Championship";
			// 
			// cmbChampionship
			// 
			cmbChampionship.FormattingEnabled = true;
			cmbChampionship.Items.AddRange(new object[] { "men", "women" });
			cmbChampionship.Location = new Point(66, 124);
			cmbChampionship.Name = "cmbChampionship";
			cmbChampionship.Size = new Size(212, 38);
			cmbChampionship.TabIndex = 1;
			// 
			// lblLanguage
			// 
			lblLanguage.AutoSize = true;
			lblLanguage.Location = new Point(66, 203);
			lblLanguage.Name = "lblLanguage";
			lblLanguage.Size = new Size(104, 30);
			lblLanguage.TabIndex = 2;
			lblLanguage.Text = "Language";
			// 
			// cmbLanguage
			// 
			cmbLanguage.FormattingEnabled = true;
			cmbLanguage.Items.AddRange(new object[] { "en", "hr" });
			cmbLanguage.Location = new Point(66, 236);
			cmbLanguage.Name = "cmbLanguage";
			cmbLanguage.Size = new Size(212, 38);
			cmbLanguage.TabIndex = 3;
			// 
			// btnSave
			// 
			btnSave.Location = new Point(66, 317);
			btnSave.Name = "btnSave";
			btnSave.Size = new Size(212, 40);
			btnSave.TabIndex = 4;
			btnSave.Text = "Save";
			btnSave.UseVisualStyleBackColor = true;
			btnSave.Click += btnSave_Click;
			// 
			// InitialSettingsForm
			// 
			AutoScaleDimensions = new SizeF(12F, 30F);
			AutoScaleMode = AutoScaleMode.Font;
			ClientSize = new Size(344, 431);
			Controls.Add(btnSave);
			Controls.Add(cmbLanguage);
			Controls.Add(lblLanguage);
			Controls.Add(cmbChampionship);
			Controls.Add(lblChampionship);
			MaximizeBox = false;
			MinimizeBox = false;
			Name = "InitialSettingsForm";
			ShowIcon = false;
			StartPosition = FormStartPosition.CenterScreen;
			Text = "Settings";
			ResumeLayout(false);
			PerformLayout();
		}

		#endregion

		private Label lblChampionship;
		private ComboBox cmbChampionship;
		private Label lblLanguage;
		private ComboBox cmbLanguage;
		private Button btnSave;
	}
}