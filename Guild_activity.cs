using System.Collections.Generic;

namespace Warcraft
{

    public class Self
    {
        public string href { get; set; }
    }

    public class Links
    {
        public Self self { get; set; }
    }

    public class Key
    {
        public string href { get; set; }
    }

    public class Realm
    {
        public Key key { get; set; }
        public string name { get; set; }
        public int id { get; set; }
        public string slug { get; set; }
    }

    public class Faction
    {
        public string type { get; set; }
        public string name { get; set; }
    }

    public class Guild
    {
        public Key key { get; set; }
        public string name { get; set; }
        public int id { get; set; }
        public Realm realm { get; set; }
        public Faction faction { get; set; }
    }

    public class Encounter
    {
        public Key_encou key { get; set; }
        public string name { get; set; }
        public int id { get; set; }
    }
    public class Key_encou
    {
        public string href { get; set; }
    }
    public class Mode
    {
        public string type { get; set; }
        public string name { get; set; }
    }

    public class EncounterCompleted
    {
        public Encounter encounter { get; set; }
        public Mode mode { get; set; }
    }

    public class Activity2
    {
        public string type { get; set; }
    }

    public class Key_charakter
    {
        public string href { get; set; }
    }
    public class Character
    {
        public Key_charakter key { get; set; }
        public string name { get; set; }
        public int id { get; set; }
        public Realm_char realm { get; set; }
    }
    public class Realm_char
    {
        public Key_realm_achiv key { get; set; }
        public string name { get; set; }
        public int id { get; set; }
        public string slug { get; set; }
    }
    public class Key_realm_achiv
    {
        public string href { get; set; }
    }
    public class Key_achiv
    {
        public string href { get; set; }
    }
    public class Achievement
    {
        public Key_achiv key { get; set; }
        public string name { get; set; }
        public int id { get; set; }
    }

    public class CharacterAchievement
    {
        public Character character { get; set; }
        public Achievement achievement { get; set; }
    }

    public class Activity
    {
        public EncounterCompleted encounter_completed { get; set; }
        public Activity2 activity { get; set; }
        public string timestamp { get; set; }
        public CharacterAchievement character_achievement { get; set; }
    }

    public class Activity_all
    {
        public Links _links { get; set; }
        public Guild guild { get; set; }
        public List<Activity> activities { get; set; }
    }




}
