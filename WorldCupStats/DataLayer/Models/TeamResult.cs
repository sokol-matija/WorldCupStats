using Newtonsoft.Json;

public class TeamResult
{
	[JsonProperty("country")]
	public string Country { get; set; } = string.Empty;

	[JsonProperty("code")]
	public string Code { get; set; } = string.Empty;

	[JsonProperty("goals")]
	public int? Goals { get; set; }

	[JsonProperty("penalties")]
	public int? Penalties { get; set; }
}