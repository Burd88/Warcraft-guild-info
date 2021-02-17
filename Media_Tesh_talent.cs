using System.Collections.Generic;

namespace Warcraft
{
    public class Self_Media_Tesh_talent
    {
        public string href { get; set; }
    }

    public class Links_Media_Tesh_talent
    {
        public Self_Media_Tesh_talent self { get; set; }
    }

    public class Asset_Media_Tesh_talent
    {
        public string key { get; set; }
        public string value { get; set; }
        public int file_data_id { get; set; }
    }




    class Media_Tesh_talent
    {
        public Links_Media_Tesh_talent _links { get; set; }
        public List<Asset_Media_Tesh_talent> assets { get; set; }
    }



    public class Self_tech_info
    {
        public string href { get; set; }
    }

    public class Links_tech_info
    {
        public Self_tech_info self { get; set; }
    }

    public class Key_tech_info
    {
        public string href { get; set; }
    }

    public class TalentTree_tech_info
    {
        public Key_tech_info key { get; set; }
        public string name { get; set; }
        public int id { get; set; }
    }

    public class Spell_tech_info
    {
        public Key_tech_info key { get; set; }
        public string name { get; set; }
        public int id { get; set; }
    }

    public class SpellTooltip_tech_info
    {
        public Spell_tech_info spell { get; set; }
        public string description { get; set; }
        public string cast_time { get; set; }
    }

    public class Media_tech_info
    {
        public Key_tech_info key { get; set; }
        public int id { get; set; }
    }

    public class Telent_tech_info
    {
        public Links_tech_info _links { get; set; }
        public int id { get; set; }
        public TalentTree_tech_info talent_tree { get; set; }
        public string name { get; set; }
        public string description { get; set; }
        public SpellTooltip_tech_info spell_tooltip { get; set; }
        public int tier { get; set; }
        public int display_order { get; set; }
        public Media_tech_info media { get; set; }
    }

}
