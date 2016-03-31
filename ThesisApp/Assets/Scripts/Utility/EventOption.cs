using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Scripts.Utility
{
    public class EventOption
    {
        public Dictionary<Piece,int> Conditions { get; set; }
        public Dictionary<Piece,int> Results { get; set; }

        public string FlavorText { get; set; }

        public EventOption()
        {
            Conditions = new Dictionary<Piece, int>();
            Results = new Dictionary<Piece, int>();
        }
    }
}
