using DataLayer;

namespace WFA_WorldCupStats
{
	public partial class Form1 : Form
	{
		private readonly IDataProvider _dataProvider;
		private string _selectedGender;
		private string _selectedLanguage;

		public Form1()
		{
			InitializeComponent();
			_dataProvider = DataProviderFactory.Create();
			LoadInitialSettings();
		}
	}
}
