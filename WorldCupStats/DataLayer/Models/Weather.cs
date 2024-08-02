using Newtonsoft.Json;

namespace DataLayer.Models
{
	public class Weather
	{
		[JsonProperty("humidity")]
		public string Humidity { get; set; } = string.Empty;

		[JsonProperty("temp_celsius")]
		public string TempCelsius { get; set; } = string.Empty;

		[JsonProperty("temp_farenheit")]
		public string TempFarenheit { get; set; } = string.Empty;

		[JsonProperty("wind_speed")]
		public string WindSpeed { get; set; } = string.Empty;

		[JsonProperty("description")]
		public string Description { get; set; } = string.Empty;
	}
}