using Newtonsoft.Json;
using System;

namespace Warcraft
{
    public class MythicPlusScores
    {
        public string all { get; set; }
        public string dps { get; set; }
        public string healer { get; set; }
        public string tank { get; set; }
        public string spec_0 { get; set; }
        public string spec_1 { get; set; }
        public string spec_2 { get; set; }
        public string spec_3 { get; set; }
    }
    public class CastleNathria
    {
        public string summary { get; set; }
        public string total_bosses { get; set; }
        public string normal_bosses_killed { get; set; }
        public string heroic_bosses_killed { get; set; }
        public string mythic_bosses_killed { get; set; }
    }

    public class RaidProgression
    {
        [JsonProperty("castle-nathria")]
        public CastleNathria CastleNathria { get; set; }
    }

    public class RaiderIO_info
    {
        public string name { get; set; }
        public string race { get; set; }
        public string @class { get; set; }
        public string active_spec_name { get; set; }
        public string active_spec_role { get; set; }
        public string gender { get; set; }
        public string faction { get; set; }
        public string achievement_points { get; set; }
        public string honorable_kills { get; set; }
        public string thumbnail_url { get; set; }
        public string region { get; set; }
        public string realm { get; set; }
        public DateTime last_crawled_at { get; set; }
        public string profile_url { get; set; }
        public string profile_banner { get; set; }
        public MythicPlusScores mythic_plus_scores { get; set; }
        public RaidProgression raid_progression { get; set; }
    }
}
