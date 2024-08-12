using DataLayer.Models;
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
	public partial class PlayerControl : UserControl
	{
		//TODO: Make selection of multiple players possible with color change
		public Player Player { get; }
		public bool IsFavorite { get; set; }
		public bool IsSelected { get; set; }
		private Color _defaultBackColor;

		private readonly string _starImagePath = Path.Combine(Application.StartupPath, "Resources", "star.png");
		private readonly string _defaultPlayerImagePath = Path.Combine(Application.StartupPath, "Resources", "profile.png");


		public PlayerControl(Player player)
		{
			Player = player;
			InitializeComponent();
			_defaultBackColor = BackColor;
			LoadImages();
			UpdateDisplay();
		}

		private void LoadImages()
		{
			try
			{
				if (File.Exists(_starImagePath))
				{
					picFavorite.Image = Image.FromFile(_starImagePath);
					picFavorite.SizeMode = PictureBoxSizeMode.Zoom;
				}

				if (File.Exists(_defaultPlayerImagePath))
				{
					picPlayer.Image = Image.FromFile(_defaultPlayerImagePath);
					picPlayer.SizeMode = PictureBoxSizeMode.Zoom;
				}
			}
			catch (Exception ex)
			{
				MessageBox.Show($"Error loading images: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}

		private void UpdateDisplay()
		{
			lblName.Text = Player.Name;
			lblNumber.Text = $"{Strings.Number}: {Player.ShirtNumber}";
			lblPosition.Text = $"{Strings.PlayerPosition}: {Player.Position}";
			chkCaptain.Text = Strings.Capitain;
			chkCaptain.Checked = Player.Captain;
			UpdateFavoriteStatus();
		}

		public void UpdateFavoriteStatus()
		{
			picFavorite.Visible = IsFavorite;
		}

		private void PlayerControl_MouseDown(object sender, MouseEventArgs e)
		{
			if (e.Button == MouseButtons.Left)
			{
				DoDragDrop(this, DragDropEffects.Move);
			}
		}

		private void PlayerControl_MouseClick(object sender, MouseEventArgs e)
		{
			if (e.Button == MouseButtons.Left && ModifierKeys == Keys.Control)
			{
				IsSelected = !IsSelected;
				this.BackColor = IsSelected ? Color.LightBlue : _defaultBackColor;
			}
		}
		public void ApplyLocalization()
		{
			UpdateDisplay();
		}
	}
}
