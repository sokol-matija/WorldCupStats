using Newtonsoft.Json;

namespace DataLayer.Models
{
	public class PlayerStats
	{
		[JsonProperty("name")]
		public string Name { get; set; } = string.Empty;

		[JsonProperty("count")]
		public int? Count { get; set; }
	}
}