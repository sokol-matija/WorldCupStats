using DataLayer.Models;

namespace WFA_WorldCupStats
{
	public partial class PlayerControl : UserControl
	{
		private readonly string _starImagePath = Path.Combine(Application.StartupPath, "Resources", "star.png");
		private readonly string _defaultPlayerImagePath = Path.Combine(Application.StartupPath, "Resources", "profile.png");
		private string _customPlayerImagePath;

		public Player Player { get; private set; }
		private bool _isFavorite;
		private bool _isSelected;
		private ContextMenuStrip _contextMenu;
		private Color _defaultBackColor;

		public event EventHandler<EventArgs> FavoriteToggled;
		public event EventHandler<EventArgs> SelectionToggled;

		public bool IsFavorite
		{
			get => _isFavorite;
			set
			{
				if (_isFavorite != value)
				{
					_isFavorite = value;
					UpdateFavoriteStatus();
					UpdateContextMenu();
					OnFavoriteToggled();
				}
			}
		}

		public bool IsSelected
		{
			get => _isSelected;
			set
			{
				if (_isSelected != value)
				{
					_isSelected = value;
					UpdateSelectionStatus();
					UpdateContextMenu();
					OnSelectionToggled();
				}
			}
		}

		public PlayerControl(Player player)
		{
			Player = player;
			InitializeComponent();
			_defaultBackColor = BackColor;
			LoadImages();
			UpdateDisplay();
			CreateContextMenu();
			this.MouseDown += PlayerControl_MouseDown; 
		}

		private void CreateContextMenu()
		{
			_contextMenu = new ContextMenuStrip();
			UpdateContextMenu();
			this.ContextMenuStrip = _contextMenu; 
		}

		private void UpdateContextMenu()
		{
			_contextMenu.Items.Clear();

			ToolStripMenuItem setImageItem = new ToolStripMenuItem(Strings.SetPlayerImage);
			setImageItem.Click += SetImageItem_Click;
			_contextMenu.Items.Add(setImageItem);

			var toggleFavoriteItem = new ToolStripMenuItem(IsFavorite ? Strings.RemoveFromFavorites : Strings.AddToFavorites);
			toggleFavoriteItem.Click += (sender, e) => IsFavorite = !IsFavorite;
			_contextMenu.Items.Add(toggleFavoriteItem);

			var toggleSelectItem = new ToolStripMenuItem(IsSelected ? Strings.Deselect : Strings.Select);
			toggleSelectItem.Click += (sender, e) => IsSelected = !IsSelected;
			_contextMenu.Items.Add(toggleSelectItem);
		}

		private void SetImageItem_Click(object? sender, EventArgs e)
		{
			throw new NotImplementedException();
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
			else if (e.Button == MouseButtons.Right)
			{
				UpdateContextMenu(); // Update before showing
				_contextMenu.Show(this, e.Location);
			}
		}



		protected virtual void OnFavoriteToggled()
		{
			FavoriteToggled?.Invoke(this, EventArgs.Empty);
		}

		protected virtual void OnSelectionToggled()
		{
			SelectionToggled?.Invoke(this, EventArgs.Empty);
		}

	}

}
