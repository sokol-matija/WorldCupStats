using DataLayer;
using DataLayer.Models;
using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace WFA_WorldCupStats
{
	public partial class PlayerControl : UserControl
	{
		private readonly string _starImagePath = Path.Combine(Application.StartupPath, "Resources", "star.png");
		private readonly string _defaultPlayerImagePath = Path.Combine(Application.StartupPath, "Resources", "profile.png");
		private readonly LogForm _logForm;
		private readonly IDataProvider _dataProvider;

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
					UpdateStatus();
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
					UpdateStatus();
					OnSelectionToggled();
				}
			}
		}

		public PlayerControl(Player player, LogForm logForm, IDataProvider dataProvider)
		{
			_logForm = logForm;
			Player = player;
			_dataProvider = dataProvider;
			InitializeComponent();
			_defaultBackColor = BackColor;
			CreateContextMenu();
			LoadImages();
			this.MouseDown += PlayerControl_MouseDown;
			this.DoubleClick += PlayerControl_DoubleClick;
			ApplyLocalization();
		}

		private void CreateContextMenu()
		{
			_contextMenu = new ContextMenuStrip();
			this.ContextMenuStrip = _contextMenu;
			UpdateContextMenu();
		}

		private void UpdateContextMenu()
		{
			_contextMenu.Items.Clear();

			_contextMenu.Items.Add(CreateMenuItem(Strings.SetPlayerImage, SetImageItem_Click));
			_contextMenu.Items.Add(CreateMenuItem(IsFavorite ? Strings.RemoveFromFavorites : Strings.AddToFavorites, (s, e) => IsFavorite = !IsFavorite));
			_contextMenu.Items.Add(CreateMenuItem(IsSelected ? Strings.Deselect : Strings.Select, (s, e) => IsSelected = !IsSelected));
		}

		private ToolStripMenuItem CreateMenuItem(string text, EventHandler clickHandler)
		{
			var item = new ToolStripMenuItem(text);
			item.Click += clickHandler;
			return item;
		}

		private void SetImageItem_Click(object sender, EventArgs e)
		{
			SetPlayerImage();
		}

		private void PlayerControl_DoubleClick(object sender, EventArgs e)
		{
			SetPlayerImage();
		}

		private async void SetPlayerImage()
		{
			using (OpenFileDialog openFileDialog = new OpenFileDialog())
			{
				openFileDialog.Filter = "Image Files (*.png;*.jpg;*.jpeg)|*.png;*.jpg;*.jpeg";
				openFileDialog.Title = Strings.SetPlayerImage;

				if (openFileDialog.ShowDialog() == DialogResult.OK)
				{
					try
					{
						byte[] imageData = File.ReadAllBytes(openFileDialog.FileName);
						await _dataProvider.SavePlayerImageAsync(Player.Name, imageData);
						LoadPlayerImage();
						_logForm.Log($"Image set for player: {Player.Name}");
					}
					catch (Exception ex)
					{
						MessageBox.Show($"{Strings.Error}: {ex.Message}", Strings.Error, MessageBoxButtons.OK, MessageBoxIcon.Error);
						_logForm.Log($"Error setting image for player {Player.Name}: {ex.Message}");
					}
				}
			}
		}

		private void LoadImages()
		{
			try
			{
				LoadImageToControl(picFavorite, _starImagePath);
				LoadPlayerImage();
			}
			catch (Exception ex)
			{
				MessageBox.Show($"{Strings.Error}: {ex.Message}", Strings.Error, MessageBoxButtons.OK, MessageBoxIcon.Error);
				_logForm.Log($"Error loading images: {ex.Message}");
			}
		}

		private void LoadImageToControl(PictureBox pictureBox, string imagePath)
		{
			if (File.Exists(imagePath))
			{
				using (var stream = new FileStream(imagePath, FileMode.Open, FileAccess.Read))
				{
					pictureBox.Image = Image.FromStream(stream);
				}
				pictureBox.SizeMode = PictureBoxSizeMode.Zoom;
			}
		}

		private async void LoadPlayerImage()
		{
			try
			{
				byte[] imageData = await _dataProvider.GetPlayerImageAsync(Player.Name);
				if (imageData != null && imageData.Length > 0)
				{
					using (var ms = new MemoryStream(imageData))
					{
						picPlayer.Image = Image.FromStream(ms);
					}
				}
				else
				{
					LoadImageToControl(picPlayer, _defaultPlayerImagePath);
				}
				picPlayer.SizeMode = PictureBoxSizeMode.Zoom;
			}
			catch (Exception ex)
			{
				_logForm.Log($"Error loading player image for {Player.Name}: {ex.Message}");
				LoadImageToControl(picPlayer, _defaultPlayerImagePath);
			}
		}

		public void ApplyLocalization()
		{
			lblName.Text = Player.Name;
			lblNumber.Text = $"{Strings.Number}: {Player.ShirtNumber}";
			lblPosition.Text = $"{Strings.PlayerPosition}: {Player.Position}";
			chkCaptain.Text = Strings.Capitain;
			chkCaptain.Checked = Player.Captain;
			UpdateStatus();
			UpdateContextMenu();
		}

		private void UpdateStatus()
		{
			picFavorite.Visible = _isFavorite;
			this.BackColor = _isSelected ? Color.LightBlue : _defaultBackColor;
			UpdateContextMenu();
		}

		private void PlayerControl_MouseDown(object sender, MouseEventArgs e)
		{
			if (e.Button == MouseButtons.Left)
			{
				DoDragDrop(this, DragDropEffects.Move);
			}
			else if (e.Button == MouseButtons.Right)
			{
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