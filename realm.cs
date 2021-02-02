using System.Collections.Generic;

namespace Warcraft
{
    public class Self_realm
    {
        public string href { get; set; }
    }

    public class Links_realm
    {
        public Self_realm self { get; set; }
    }

    public class Status_realm
    {
        public string type { get; set; }
        public string name { get; set; }
    }

    public class Population_realm
    {
        public string type { get; set; }
        public string name { get; set; }
    }

    public class Key_realm
    {
        public string href { get; set; }
    }

    public class Region_realm
    {
        public Key_realm key { get; set; }
        public string name { get; set; }
        public int id { get; set; }
    }

    public class ConnectedRealm_realm
    {
        public string href { get; set; }
    }

    public class Type_realm
    {
        public string type { get; set; }
        public string name { get; set; }
    }

    public class Realm_realm
    {
        public int id { get; set; }
        public Region_realm region { get; set; }
        public ConnectedRealm_realm connected_realm { get; set; }
        public string name { get; set; }
        public string category { get; set; }
        public string locale { get; set; }
        public string timezone { get; set; }
        public Type_realm type { get; set; }
        public bool is_tournament { get; set; }
        public string slug { get; set; }
    }

    public class MythicLeaderboards_realm
    {
        public string href { get; set; }
    }

    public class Auctions_realm
    {
        public string href { get; set; }
    }

    public class Root_realm
    {
        public Links_realm _links { get; set; }
        public int id { get; set; }
        public bool has_queue { get; set; }
        public Status_realm status { get; set; }
        public Population_realm population { get; set; }
        public List<Realm_realm> realms { get; set; }
        public MythicLeaderboards_realm mythic_leaderboards { get; set; }
        public Auctions_realm auctions { get; set; }
    }
    class realm
    {
    }
}
