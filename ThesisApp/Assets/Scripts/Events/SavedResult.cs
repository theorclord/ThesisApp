using Assets.Scripts.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Scripts.Events
{
    public class SavedResult
    {
        public Location Location { get; set; }
        public EventOutcomeType OutcomeType { get; set; }

        //Event conditions
        public Dictionary<Piece, int> Conditions { get; set; }
        //Event Outcome
        public Dictionary<Piece, int> Outcomes { get; set; }

        public Faction Alligance { get; set; }

        public SavedResult()
        {
            Conditions = new Dictionary<Piece, int>();
            Outcomes = new Dictionary<Piece, int>();
        }
    }
}
