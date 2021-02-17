using System.Collections.Generic;

namespace Warcraft
{
    public class Self_Conduit
    {
        public string href { get; set; }
    }

    public class Links_Conduit
    {
        public Self_Conduit self { get; set; }
    }

    public class Key_Conduit
    {
        public string href { get; set; }
    }

    public class Conduit
    {
        public Key_Conduit key { get; set; }
        public string name { get; set; }
        public int id { get; set; }
    }

    public class Root_Conduit
    {
        public Links_Conduit _links { get; set; }
        public List<Conduit> conduits { get; set; }
    }


    public class Self_conduit_info
    {
        public string href { get; set; }
    }

    public class Links_conduit_info
    {
        public Self self { get; set; }
    }

    public class Key_conduit_info
    {
        public string href { get; set; }
    }

    public class Item_conduit_info
    {
        public Key_conduit_info key { get; set; }
        public string name { get; set; }
        public int id { get; set; }
    }

    public class SocketType_conduit_info
    {
        public string type { get; set; }
        public string name { get; set; }
    }

    public class Spell_conduit_info
    {
        public Key_conduit_info key { get; set; }
        public string name { get; set; }
        public int id { get; set; }
    }

    public class SpellTooltip_conduit_info
    {
        public Spell_conduit_info spell { get; set; }
        public string description { get; set; }
        public string cast_time { get; set; }
    }

    public class Rank_conduit_info
    {
        public int id { get; set; }
        public int tier { get; set; }
        public SpellTooltip_conduit_info spell_tooltip { get; set; }
    }

    public class Root_conduit_info
    {
        public Links_conduit_info _links { get; set; }
        public int id { get; set; }
        public string name { get; set; }
        public Item_conduit_info item { get; set; }
        public SocketType_conduit_info socket_type { get; set; }
        public List<Rank_conduit_info> ranks { get; set; }
    }

    public class Self_conduit_info_spell
    {
        public string href { get; set; }
    }

    public class Links_conduit_info_spell
    {
        public Self_conduit_info_spell self { get; set; }
    }

    public class Key_conduit_info_spell
    {
        public string href { get; set; }
    }

    public class Media_conduit_info_spell
    {
        public Key_conduit_info_spell key { get; set; }
        public int id { get; set; }
    }

    public class Root_conduit_info_spell
    {
        public Links_conduit_info_spell _links { get; set; }
        public int id { get; set; }
        public string name { get; set; }
        public string description { get; set; }
        public Media_conduit_info_spell media { get; set; }
    }

    public class Self_conduit_icon
    {
        public string href { get; set; }
    }

    public class Links_conduit_icon
    {
        public Self self { get; set; }
    }

    public class Asset_conduit_icon
    {
        public string key { get; set; }
        public string value { get; set; }
        public int file_data_id { get; set; }
    }

    public class Root_conduit_icon
    {
        public Links_conduit_icon _links { get; set; }
        public List<Asset_conduit_icon> assets { get; set; }
        public int id { get; set; }
    }

    public class Conduits_list
    {
        public string id { get; set; }
        public string rank { get; set; }
    }
}
