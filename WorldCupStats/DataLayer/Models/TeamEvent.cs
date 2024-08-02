using Newtonsoft.Json;
namespace DataLayer.Models
{
	public class TeamEvent
	{
		[JsonProperty("id")]
		public int? Id { get; set; }

		[JsonProperty("type_of_event")]
		public string TypeOfEvent { get; set; } = string.Empty;

		[JsonProperty("player")]
		public string Player { get; set; } = string.Empty;

		[JsonProperty("time")]
		public string Time { get; set; } = string.Empty;
	}
}