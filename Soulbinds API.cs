using System.Collections.Generic;

namespace Warcraft
{
    public class Soulbinds_API_Self
    {
        public string href { get; set; }
    }

    public class Soulbinds_API_Links
    {
        public Soulbinds_API_Self self { get; set; }
    }

    public class Soulbinds_API_Key
    {
        public string href { get; set; }
    }

    public class Soulbinds_API_Realm
    {
        public Soulbinds_API_Key key { get; set; }
        public string name { get; set; }
        public int id { get; set; }
        public string slug { get; set; }
    }

    public class Soulbinds_API_Character
    {
        public Soulbinds_API_Key key { get; set; }
        public string name { get; set; }
        public int id { get; set; }
        public Soulbinds_API_Realm realm { get; set; }
    }

    public class Soulbinds_API_ChosenCovenant
    {
        public Soulbinds_API_Key key { get; set; }
        public string name { get; set; }
        public int id { get; set; }
    }

    public class Soulbinds_API_Soulbind2
    {
        public Soulbinds_API_Key key { get; set; }
        public string name { get; set; }
        public int id { get; set; }
    }

    public class Soulbinds_API_Trait2
    {
        public Soulbinds_API_Key key { get; set; }
        public string name { get; set; }
        public int id { get; set; }
    }

    public class Soulbinds_API_Type
    {
        public string type { get; set; }
        public string name { get; set; }
    }

    public class Soulbinds_API_Conduit
    {
        public Soulbinds_API_Key key { get; set; }
        public string name { get; set; }
        public int id { get; set; }
    }

    public class Soulbinds_API_Socket
    {
        public Soulbinds_API_Conduit conduit { get; set; }
        public int rank { get; set; }
    }

    public class Soulbinds_API_ConduitSocket
    {
        public Soulbinds_API_Type type { get; set; }
        public Soulbinds_API_Socket socket { get; set; }
    }

    public class Soulbinds_API_Trait
    {
        public Soulbinds_API_Trait2 trait { get; set; }
        public int tier { get; set; }
        public int display_order { get; set; }
        public Soulbinds_API_ConduitSocket conduit_socket { get; set; }
    }

    public class Soulbinds_API_Soulbind
    {
        public Soulbinds_API_Soulbind2 soulbind { get; set; }
        public List<Soulbinds_API_Trait> traits { get; set; }
        public bool is_active { get; set; }
    }

    public class Soulbinds_API
    {
        public Soulbinds_API_Links _links { get; set; }
        public Soulbinds_API_Character character { get; set; }
        public Soulbinds_API_ChosenCovenant chosen_covenant { get; set; }
        public int renown_level { get; set; }
        public List<Soulbinds_API_Soulbind> soulbinds { get; set; }
    }


}
