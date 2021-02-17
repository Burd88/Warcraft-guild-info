using System.Collections.Generic;
using System.Windows.Media.Imaging;

namespace Warcraft
{
    // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse); 
    public class Self_guild
    {
        public string href { get; set; }
    }

    public class Links_guild
    {
        public Self_guild self { get; set; }
    }

    public class Key_guild
    {
        public string href { get; set; }
    }

    public class Realm_guild
    {
        public Key_guild key { get; set; }
        public string name { get; set; }
        public int id { get; set; }
        public string slug { get; set; }
    }

    public class Faction_guild
    {
        public string type { get; set; }
        public string name { get; set; }
    }

    public class Guild_guild
    {
        public Key_guild key { get; set; }
        public string name { get; set; }
        public int id { get; set; }
        public Realm_guild realm { get; set; }
        public Faction_guild faction { get; set; }
    }

    public class PlayableClass_guild
    {
        public Key_guild key { get; set; }
        public int id { get; set; }
    }

    public class PlayableRace_guild
    {
        public Key_guild key { get; set; }
        public int id { get; set; }
    }

    public class Character_guild
    {
        public Key_guild key { get; set; }
        public string name { get; set; }
        public int id { get; set; }
        public Realm_guild realm { get; set; }
        public int level { get; set; }
        public PlayableClass_guild playable_class { get; set; }
        public PlayableRace_guild playable_race { get; set; }
    }

    public class Member_guild
    {
        public Character_guild character { get; set; }
        public int rank { get; set; }
    }

    public class Root_guild
    {
        public Links_guild _links { get; set; }
        public Guild_guild guild { get; set; }
        public List<Member_guild> members { get; set; }
    }
    public class User
    {


        public string Name { get; set; }

        public int Level { get; set; }

        public BitmapImage Class { get; set; }
        public BitmapImage Spec { get; set; }
        public string Raid_progress { get; set; }
        public int Rank { get; set; }
        public double MythicScore { get; set; }
        public int Ilvl { get; set; }
        public BitmapImage Coven { get; set; }
        public int Coven_lvl { get; set; }
        public string Coven_soul { get; set; }
        public string link { get; set; }
    }
}
