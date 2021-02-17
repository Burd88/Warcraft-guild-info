using System.Collections.Generic;

namespace Warcraft
{
    public class Self_tech_talent
    {
        public string href { get; set; }
    }

    public class Links_tech_talent
    {
        public Self_tech_talent self { get; set; }
    }

    public class Key_tech_talent
    {
        public string href { get; set; }
    }

    public class Talent_tech_talent
    {
        public Key_tech_talent key { get; set; }
        public string name { get; set; }
        public int id { get; set; }
    }

    public class Root_tech_talent
    {
        public Links_tech_talent _links { get; set; }
        public List<Talent_tech_talent> talents { get; set; }
    }

}
