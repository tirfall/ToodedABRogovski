using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToodedAB
{
    public class Toode
    {
        public int Id { get; set; }
        public string Nimetus { get; set; }
        public int Kogus { get; set; }
        public float Hind { get; set; }
        public string Pilt { get; set; }
        public IEnumerable<string> Kategooriad { get; set; }
    }
    public class Kategooria
    {
        public int Id { get; set; }
        public string Kategooria_nimetus { get; set; }
        public string Kirjeldus { get; set; }

    }
}
