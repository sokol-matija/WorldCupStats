using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Models
{
	public class Team
	{
		public int Id { get; set; }
		public string Country { get; set; }
		[JsonProperty("alternate_name")]
		public string AlternateName { get; set; }
		[JsonProperty("fifa_code")]
		public string FifaCode { get; set; }
		[JsonProperty("group_id")]
		public int GroupId { get; set; }
		[JsonProperty("group_letter")]
		public string GroupLetter { get; set; }
		public int Wins { get; set; }
		public int Draws { get; set; }
		public int Losses { get; set; }
		[JsonProperty("games_played")]
		public int GamesPlayed { get; set; }
		public int Points { get; set; }
		[JsonProperty("goals_for")]
		public int GoalsFor { get; set; }
		[JsonProperty("goals_against")]
		public int GoalsAgainst { get; set; }
		[JsonProperty("goal_differential")]
		public int GoalDifferential { get; set; }
	}
}
