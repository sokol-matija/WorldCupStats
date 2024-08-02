using Newtonsoft.Json;
namespace DataLayer.Models
{
	public class Player
	{
		[JsonProperty("name")]
		public string Name { get; set; } = string.Empty;

		[JsonProperty("captain")]
		public bool Captain { get; set; }

		[JsonProperty("shirt_number")]
		public int? ShirtNumber { get; set; }

		[JsonProperty("position")]
		public string Position { get; set; } = string.Empty;

		[JsonProperty("goals")]
		public int? Goals { get; set; }
	}
}