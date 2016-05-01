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
        public Faction()
        {
            BoardName = "";
        }

        public string BoardName { get; set; }
        public override int GetHashCode()
        {
            return BoardName.GetHashCode();
        }
        public override bool Equals(object obj)
        {

            return base.Equals(obj) && BoardName.Equals(((Faction)obj).BoardName);
        }
    }
}
