namespace Warcraft
{
    public class Self_charackter_full_info
    {
        public string href { get; set; }
    }

    public class Links_charackter_full_info
    {
        public Self_charackter_full_info self { get; set; }
    }

    public class Gender_charackter_full_info
    {
        public string type { get; set; }
        public string name { get; set; }
    }

    public class Faction_charackter_full_info
    {
        public string type { get; set; }
        public string name { get; set; }
    }

    public class Key_charackter_full_info
    {
        public string href { get; set; }
    }

    public class Race_charackter_full_info
    {
        public Key_charackter_full_info key { get; set; }
        public string name { get; set; }
        public int id { get; set; }
    }

    public class CharacterClass_charackter_full_info
    {
        public Key_charackter_full_info key { get; set; }
        public string name { get; set; }
        public int id { get; set; }
    }

    public class ActiveSpec_charackter_full_info
    {
        public Key_charackter_full_info key { get; set; }
        public string name { get; set; }
        public int id { get; set; }
    }

    public class Realm_charackter_full_info
    {
        public Key_charackter_full_info key { get; set; }
        public string name { get; set; }
        public int id { get; set; }
        public string slug { get; set; }
    }

    public class Guild_charackter_full_info
    {
        public Key_charackter_full_info key { get; set; }
        public string name { get; set; }
        public int id { get; set; }
        public Realm_charackter_full_info realm { get; set; }
        public Faction_charackter_full_info faction { get; set; }
    }

    public class Achievements_charackter_full_info
    {
        public string href { get; set; }
    }

    public class Titles_charackter_full_info
    {
        public string href { get; set; }
    }

    public class PvpSummary_charackter_full_info
    {
        public string href { get; set; }
    }

    public class Encounters_charackter_full_info
    {
        public string href { get; set; }
    }

    public class Media_charackter_full_info
    {
        public string href { get; set; }
    }

    public class Specializations_charackter_full_info
    {
        public string href { get; set; }
    }

    public class Statistics_charackter_full_info
    {
        public string href { get; set; }
    }

    public class MythicKeystoneProfile_charackter_full_info
    {
        public string href { get; set; }
    }

    public class Equipment_charackter_full_info
    {
        public string href { get; set; }
    }

    public class Appearance_charackter_full_info
    {
        public string href { get; set; }
    }

    public class Collections_charackter_full_info
    {
        public string href { get; set; }
    }

    public class Reputations_charackter_full_info
    {
        public string href { get; set; }
    }

    public class Quests_charackter_full_info
    {
        public string href { get; set; }
    }

    public class AchievementsStatistics_charackter_full_info
    {
        public string href { get; set; }
    }

    public class Professions_charackter_full_info
    {
        public string href { get; set; }
    }

    public class ChosenCovenant_charackter_full_info
    {
        public Key_charackter_full_info key { get; set; }
        public string name { get; set; }
        public int id { get; set; }
    }

    public class Soulbinds_charackter_full_info
    {
        public string href { get; set; }
    }

    public class CovenantProgress_charackter_full_info
    {
        public ChosenCovenant_charackter_full_info chosen_covenant { get; set; }
        public int renown_level { get; set; }
        public Soulbinds_charackter_full_info soulbinds { get; set; }
    }

    public class Root_charackter_full_info
    {
        public Links_charackter_full_info _links { get; set; }
        public int id { get; set; }
        public string name { get; set; }
        public Gender_charackter_full_info gender { get; set; }
        public Faction_charackter_full_info faction { get; set; }
        public Race_charackter_full_info race { get; set; }
        public CharacterClass_charackter_full_info character_class { get; set; }
        public ActiveSpec_charackter_full_info active_spec { get; set; }
        public Realm_charackter_full_info realm { get; set; }
        public Guild_charackter_full_info guild { get; set; }
        public int level { get; set; }
        public int experience { get; set; }
        public int achievement_points { get; set; }
        public Achievements_charackter_full_info achievements { get; set; }
        public Titles_charackter_full_info titles { get; set; }
        public PvpSummary_charackter_full_info pvp_summary { get; set; }
        public Encounters_charackter_full_info encounters { get; set; }
        public Media_charackter_full_info media { get; set; }
        public long last_login_timestamp { get; set; }
        public int average_item_level { get; set; }
        public int equipped_item_level { get; set; }
        public Specializations_charackter_full_info specializations { get; set; }
        public Statistics_charackter_full_info statistics { get; set; }
        public MythicKeystoneProfile_charackter_full_info mythic_keystone_profile { get; set; }
        public Equipment_charackter_full_info equipment { get; set; }
        public Appearance_charackter_full_info appearance { get; set; }
        public Collections_charackter_full_info collections { get; set; }
        public Reputations_charackter_full_info reputations { get; set; }
        public Quests_charackter_full_info quests { get; set; }
        public AchievementsStatistics_charackter_full_info achievements_statistics { get; set; }
        public Professions_charackter_full_info professions { get; set; }
        public CovenantProgress_charackter_full_info covenant_progress { get; set; }
    }
}
