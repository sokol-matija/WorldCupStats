using DataLayer.Models;

namespace WFA_WorldCupStats
{
	public partial class PlayerControl : UserControl
	{
		private readonly string _starImagePath = Path.Combine(Application.StartupPath, "Resources", "star.png");
		private readonly string _defaultPlayerImagePath = Path.Combine(Application.StartupPath, "Resources", "profile.png");

		public Player Player { get; private set; }
		private bool _isFavorite;
		private bool _isSelected;
		private Color _defaultBackColor;

		public bool IsFavorite
		{
			get => _isFavorite;
			set
			{
				_isFavorite = value;
				UpdateFavoriteStatus();
			}
		}

		public bool IsSelected
		{
			get => _isSelected;
			set
			{
				_isSelected = value;
				UpdateSelectionStatus();
			}
		}

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
			UpdateSelectionStatus();
		}

		private void UpdateFavoriteStatus()
		{
			picFavorite.Visible = _isFavorite;
		}

		private void UpdateSelectionStatus()
		{
			this.BackColor = _isSelected ? Color.LightBlue : _defaultBackColor;
		}


		public void ApplyLocalization()
		{
			UpdateDisplay();
		}

		private void PlayerControl_MouseDown(object sender, MouseEventArgs e)
		{
			if (e.Button == MouseButtons.Left)
			{
				DoDragDrop(this, DragDropEffects.Move);
			}
		}

	}
}
