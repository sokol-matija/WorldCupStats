using System;
using System.Windows.Forms;
using System.Threading.Tasks;
using System.Globalization;
using WFA_WorldCupStats.Managers;

namespace WFA_WorldCupStats
{
    public partial class InitialSettingsForm : Form
	{
		private readonly SettingsManager _settingsManager;
		private readonly SettingsUIManager _settingsUIManager;

		public InitialSettingsForm(SettingsManager settingsManager)
		{
			InitializeComponent();
			_settingsManager = settingsManager;
			_settingsUIManager = new SettingsUIManager(this, _settingsManager);
			InitializeComboBoxes();
			ApplyLocalization();
		}

		private void InitializeComboBoxes()
		{
			_settingsUIManager.InitializeComboBoxes(cmbChampionship, cmbLanguage);
		}

		public void SetCurrentSettings()
		{
			_settingsUIManager.SetCurrentSettings(cmbChampionship, cmbLanguage);
		}

		private async void btnSave_Click(object sender, EventArgs e)
		{
			await _settingsUIManager.SaveSettings(cmbChampionship, cmbLanguage);
		}

		private void ApplyLocalization()
		{
			_settingsUIManager.ApplyLocalization(this, lblChampionship, lblLanguage, btnSave);
		}

		public void UpdateComboBoxDisplayText(ComboBox comboBox)
		{
			_settingsUIManager.UpdateComboBoxDisplayText(comboBox);
		}
	}

	public class SettingsUIManager
	{
		private readonly InitialSettingsForm _form;
		private readonly SettingsManager _settingsManager;

		public SettingsUIManager(InitialSettingsForm form, SettingsManager settingsManager)
		{
			_form = form;
			_settingsManager = settingsManager;
		}

		public void InitializeComboBoxes(ComboBox cmbChampionship, ComboBox cmbLanguage)
		{
			cmbChampionship.Items.Clear();
			cmbChampionship.Items.Add(new ComboBoxItem(SettingsConstants.MenChampionship, () => Strings.MensChampionship));
			cmbChampionship.Items.Add(new ComboBoxItem(SettingsConstants.WomenChampionship, () => Strings.WomensChampionship));

			cmbLanguage.Items.Clear();
			cmbLanguage.Items.Add(new ComboBoxItem(SettingsConstants.EnglishLanguage, () => Strings.EnglishLanguage));
			cmbLanguage.Items.Add(new ComboBoxItem(SettingsConstants.CroatianLanguage, () => Strings.CroatianLanguage));
		}

		public void SetCurrentSettings(ComboBox cmbChampionship, ComboBox cmbLanguage)
		{
			cmbChampionship.SelectedItem = cmbChampionship.Items.Cast<ComboBoxItem>()
				.FirstOrDefault(item => item.Value == _settingsManager.SelectedChampionship);
			cmbLanguage.SelectedItem = cmbLanguage.Items.Cast<ComboBoxItem>()
				.FirstOrDefault(item => item.Value == _settingsManager.SelectedLanguage);
		}

		public async Task SaveSettings(ComboBox cmbChampionship, ComboBox cmbLanguage)
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

				_form.DialogResult = DialogResult.OK;
				_form.Close();
			}
			catch (Exception ex)
			{
				MessageBox.Show($"Error saving settings: {ex.Message}", Strings.Error, MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}

		public void ApplyLocalization(Form form, Label lblChampionship, Label lblLanguage, Button btnSave)
		{
			form.Text = Strings.SettingsFormTitle;
			lblChampionship.Text = Strings.ChampionshipLabel;
			lblLanguage.Text = Strings.LanguageLabel;
			btnSave.Text = Strings.SaveButton;

			UpdateComboBoxDisplayText((ComboBox)form.Controls["cmbChampionship"]);
			UpdateComboBoxDisplayText((ComboBox)form.Controls["cmbLanguage"]);
		}

		public void UpdateComboBoxDisplayText(ComboBox comboBox)
		{
			foreach (ComboBoxItem item in comboBox.Items)
			{
				item.UpdateDisplayText();
			}
			comboBox.Refresh();
		}

		private void ChangeLanguage(string language)
		{
			CultureInfo culture = language == SettingsConstants.CroatianLanguage
				? new CultureInfo("hr-HR")
				: new CultureInfo("en-US");

			Thread.CurrentThread.CurrentUICulture = culture;
			ApplyLocalization(_form, _form.Controls["lblChampionship"] as Label, _form.Controls["lblLanguage"] as Label, _form.Controls["btnSave"] as Button);
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