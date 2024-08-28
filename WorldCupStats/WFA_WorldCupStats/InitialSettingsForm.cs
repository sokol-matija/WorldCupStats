using System;
using System.Windows.Forms;
using System.Threading.Tasks;
using System.Globalization;

namespace WFA_WorldCupStats
{
	public partial class InitialSettingsForm : Form
	{
		private readonly LogForm _logForm;
		private readonly SettingsManager _settingsManager;
		public InitialSettingsForm(SettingsManager settingsManager)
		{
			InitializeComponent();
			_settingsManager = settingsManager;
			InitializeComboBoxes();
			ApplyLocalization();
		}


		private void InitializeComboBoxes()
		{
			cmbChampionship.Items.Clear();
			cmbChampionship.Items.Add(new ComboBoxItem(SettingsManager.MenChampionship, () => Strings.MensChampionship));
			cmbChampionship.Items.Add(new ComboBoxItem(SettingsManager.WomenChampionship, () => Strings.WomensChampionship));

			cmbLanguage.Items.Clear();
			cmbLanguage.Items.Add(new ComboBoxItem(SettingsManager.EnglishLanguage, () => Strings.EnglishLanguage));
			cmbLanguage.Items.Add(new ComboBoxItem(SettingsManager.CroatianLanguage, () => Strings.CroatianLanguage));
		}

		public void SetCurrentSettings()
		{
			cmbChampionship.SelectedItem = cmbChampionship.Items.Cast<ComboBoxItem>()
				.FirstOrDefault(item => item.Value == _settingsManager.SelectedChampionship);
			cmbLanguage.SelectedItem = cmbLanguage.Items.Cast<ComboBoxItem>()
				.FirstOrDefault(item => item.Value == _settingsManager.SelectedLanguage);
		}

		private async void btnSave_Click(object sender, EventArgs e)
		{
			if (cmbChampionship.SelectedItem == null || cmbLanguage.SelectedItem == null)
			{
				MessageBox.Show(Strings.SelectBothError, Strings.Error, MessageBoxButtons.OK, MessageBoxIcon.Error);
				return;
			}

			try
			{
				string championship = ((ComboBoxItem)cmbChampionship.SelectedItem).Value;
				string language = ((ComboBoxItem)cmbLanguage.SelectedItem).Value;

				await _settingsManager.SaveSettingsAsync(championship, language);
				ChangeLanguage(language);

				DialogResult = DialogResult.OK;
				Close();
			}
			catch (Exception ex)
			{
				MessageBox.Show($"Error saving settings: {ex.Message}", Strings.Error, MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}

		private void ApplyLocalization()
		{
			this.Text = Strings.SettingsFormTitle;
			lblChampionship.Text = Strings.ChampionshipLabel;
			lblLanguage.Text = Strings.LanguageLabel;
			btnSave.Text = Strings.SaveButton;

			UpdateComboBoxDisplayText(cmbChampionship);
			UpdateComboBoxDisplayText(cmbLanguage);
		}

		private void UpdateComboBoxDisplayText(ComboBox comboBox)
		{
			foreach (ComboBoxItem item in comboBox.Items)
			{
				item.UpdateDisplayText();
			}
			comboBox.Refresh();
		}

		private void ChangeLanguage(string language)
		{
			CultureInfo culture = language == SettingsManager.CroatianLanguage
				? new CultureInfo("hr-HR")
				: new CultureInfo("en-US");

			Thread.CurrentThread.CurrentUICulture = culture;
			ApplyLocalization();
		}
	}

	public class ComboBoxItem
	{
		public string Value { get; }
		private readonly Func<string> _displayTextProvider;

		public ComboBoxItem(string value, Func<string> displayTextProvider)
		{
			Value = value;
			_displayTextProvider = displayTextProvider;
		}

		public void UpdateDisplayText()
		{
			_displayText = _displayTextProvider();
		}

		private string _displayText;
		public override string ToString() => _displayText;
	}
}