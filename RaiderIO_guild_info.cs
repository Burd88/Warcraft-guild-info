using Newtonsoft.Json;
using System;

namespace Warcraft
{
    public class Guild_Normal
    {
        public int world { get; set; }
        public int region { get; set; }
        public int realm { get; set; }
    }

    public class Guild_Heroic
    {
        public int world { get; set; }
        public int region { get; set; }
        public int realm { get; set; }
    }

    public class Guild_Mythic
    {
        public int world { get; set; }
        public int region { get; set; }
        public int realm { get; set; }
    }

    public class Guild_CastleNathria
    {
        public Guild_Normal normal { get; set; }
        public Guild_Heroic heroic { get; set; }
        public Guild_Mythic mythic { get; set; }
        public string summary { get; set; }
        public int total_bosses { get; set; }
        public int normal_bosses_killed { get; set; }
        public int heroic_bosses_killed { get; set; }
        public int mythic_bosses_killed { get; set; }
    }

    public class Guild_RaidRankings
    {
        [JsonProperty("castle-nathria")]
        public Guild_CastleNathria CastleNathria { get; set; }
    }

    public class Guild_RaidProgression
    {
        [JsonProperty("castle-nathria")]
        public Guild_CastleNathria CastleNathria { get; set; }
    }

    public class RaiderIO_guild_info
    {
        public string name { get; set; }
        public string faction { get; set; }
        public string region { get; set; }
        public string realm { get; set; }
        public DateTime last_crawled_at { get; set; }
        public string profile_url { get; set; }
        public Guild_RaidRankings raid_rankings { get; set; }
        public Guild_RaidProgression raid_progression { get; set; }
    }


}
