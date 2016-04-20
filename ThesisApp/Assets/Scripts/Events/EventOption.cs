    using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Scripts.Utility
{
    public class EventOption
    {
        public Dictionary<Piece, int> Conditions { get; set; }
        public List<EventOutcomeGroup> Results { get; set; }
        public Location locType { get; set; }
        public string locationXmlString { get; set; }

        public EventOption()
        {
            Conditions = new Dictionary<Piece, int>();
            Results = new List<EventOutcomeGroup>();
        }
    }
}
