using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Scripts.Utility
{    
    public class Faction
    {
        private string innerText;

        public Faction(string name)
        {
            BoardName = name;
        }

        public string BoardName { get; set; }
    }
}
