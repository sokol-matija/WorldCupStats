using Newtonsoft.Json;
using System.Collections.Generic;

namespace DataLayer.Models
{
	public class GroupResult
	{
		[JsonProperty("id")]
		public int? Id { get; set; }

		[JsonProperty("letter")]
		public string Letter { get; set; } = string.Empty;

		[JsonProperty("ordered_teams")]
		public List<Team> OrderedTeams { get; set; } = new List<Team>();
	}
}