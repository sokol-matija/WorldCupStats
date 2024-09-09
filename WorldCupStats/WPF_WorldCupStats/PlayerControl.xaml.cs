using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WPF_WorldCupStats
{
	/// <summary>
	/// Interaction logic for PlayerControl.xaml
	/// </summary>
	public partial class PlayerControl : UserControl
	{
		public PlayerControl()
		{
			InitializeComponent();
			this.MouseLeftButtonDown += PlayerControl_MouseLeftButtonDown;
		}

		private void PlayerControl_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
		{
			// Pozovite odgovarajući event handler u glavnom prozoru
			//TODO: Fix this error in the code below
			//((MainWindow)Window.GetWindow(this)).PlayerControl_MouseLeftButtonDown(this, e);
		}

		public void SetPlayerInfo(string name, string number, string imagePath)
		{
			txtPlayerInfo.Text = $"{name}\n{number}";
			if (File.Exists(imagePath))
			{
				imgPlayer.Source = new BitmapImage(new Uri(imagePath));
			}
			else
			{
				// TODO: Postavite zadanu sliku igrača
			}
		}
	}
}
