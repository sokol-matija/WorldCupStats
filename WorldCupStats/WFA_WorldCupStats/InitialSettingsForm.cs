using DataLayer;
using System;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Globalization;
using System.Drawing.Interop;

namespace WFA_WorldCupStats
{
	public partial class InitialSettingsForm : Form
	{
		private readonly IDataProvider _dataProvider;

		public InitialSettingsForm(IDataProvider dataProvider)
		{
			InitializeComponent();
			_dataProvider = dataProvider;
			ApplyLocalization();
			LoadSettingsAsync();
		}


		private void ApplyLocalization()
		{
			this.Text = Strings.SettingsFormTitle;
			lblChampionship.Text = Strings.ChampionshipLabel;
			lblLanguage.Text = Strings.LanguageLabel;
			btnSave.Text = Strings.SaveButton;

			cmbChampionship.Items[0] = Strings.MensChampionship;
			cmbChampionship.Items[1] = Strings.WomensChampionship;
			cmbLanguage.Items[0] = Strings.EnglishLanguage;
			cmbLanguage.Items[1] = Strings.CroatianLanguage;
		}

		public void SetCurrentSettings(string championship, string language)
		{
			cmbChampionship.SelectedItem = championship == "men" ? Strings.MensChampionship : Strings.WomensChampionship;
			cmbLanguage.SelectedItem = language == "English" ? Strings.EnglishLanguage : Strings.CroatianLanguage;
		}

		private async Task LoadSettingsAsync()
		{
			string championship = await _dataProvider.LoadSettingsAsync("Championship");
			string language = await _dataProvider.LoadSettingsAsync("Language");

			SetCurrentSettings(championship, language);

			if (!string.IsNullOrEmpty(language))
			{
				ChangeLanguage(language);
			}
		}

		private void ChangeLanguage(string language)
		{
			Thread.CurrentThread.CurrentUICulture = language == "Croatian"
				? new CultureInfo("hr-HR")
				: new CultureInfo("en-US");

			ApplyLocalization();
		}

		private async void btnSave_Click(object sender, EventArgs e)
		{
			if (cmbChampionship.SelectedItem == null || cmbLanguage.SelectedItem == null)
			{
				MessageBox.Show(Strings.SelectBothError, Strings.Error, MessageBoxButtons.OK, MessageBoxIcon.Error);
				return;
			}

			string championship = cmbChampionship.SelectedItem.ToString() == Strings.MensChampionship ? "men" : "women";
			string language = cmbLanguage.SelectedItem.ToString() == Strings.EnglishLanguage ? "English" : "Croatian";

			await _dataProvider.SaveSettingsAsync("Championship", championship);
			await _dataProvider.SaveSettingsAsync("Language", language);
			ChangeLanguage(language);

			DialogResult = DialogResult.OK;
			Close();
		}
	}
}