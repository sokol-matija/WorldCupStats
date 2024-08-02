using DataLayer.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace DataLayer.Models
{
	public class Match
	{
		[JsonProperty("venue")]
		public string Venue { get; set; } = string.Empty;

		[JsonProperty("location")]
		public string Location { get; set; } = string.Empty;

		[JsonProperty("status")]
		public string Status { get; set; } = string.Empty;

		[JsonProperty("time")]
		public string Time { get; set; } = string.Empty;

		[JsonProperty("fifa_id")]
		public string FifaId { get; set; } = string.Empty;

		[JsonProperty("weather")]
		public Weather Weather { get; set; } = new Weather();

		[JsonProperty("attendance")]
		public string Attendance { get; set; } = string.Empty;

		[JsonProperty("officials")]
		public List<string> Officials { get; set; } = new List<string>();

		[JsonProperty("stage_name")]
		public string StageName { get; set; } = string.Empty;

		[JsonProperty("home_team_country")]
		public string HomeTeamCountry { get; set; } = string.Empty;

		[JsonProperty("away_team_country")]
		public string AwayTeamCountry { get; set; } = string.Empty;

		[JsonProperty("datetime")]
		public DateTime Datetime { get; set; }

		[JsonProperty("winner")]
		public string? Winner { get; set; }

		[JsonProperty("winner_code")]
		public string? WinnerCode { get; set; }

		[JsonProperty("home_team")]
		public TeamResult HomeTeam { get; set; } = new TeamResult();

		[JsonProperty("away_team")]
		public TeamResult AwayTeam { get; set; } = new TeamResult();

		[JsonProperty("home_team_events")]
		public List<TeamEvent> HomeTeamEvents { get; set; } = new List<TeamEvent>();

		[JsonProperty("away_team_events")]
		public List<TeamEvent> AwayTeamEvents { get; set; } = new List<TeamEvent>();

		[JsonProperty("home_team_statistics")]
		public TeamStatistics HomeTeamStatistics { get; set; } = new TeamStatistics();

		[JsonProperty("away_team_statistics")]
		public TeamStatistics AwayTeamStatistics { get; set; } = new TeamStatistics();

		[JsonProperty("last_event_update_at")]
		public DateTime LastEventUpdateAt { get; set; }

		[JsonProperty("last_score_update_at")]
		public DateTime? LastScoreUpdateAt { get; set; }
	}
}