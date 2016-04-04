using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Scripts.Utility
{
    public class EventOption
    {
        public Dictionary<Piece, int> Conditions { get; set; }
        public List<EventOutcome> Results { get; set; }

        public string FlavorText { get; set; }

        public EventOption()
        {
            Conditions = new Dictionary<Piece, int>();
            Results = new List<EventOutcome>();
        }
    }
}
