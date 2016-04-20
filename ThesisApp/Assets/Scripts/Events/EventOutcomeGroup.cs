using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Scripts.Utility
{
    public class EventOutcomeGroup
    {
        public List<EventOutcome> Outcomes { get; set; }
        public EventOutcomeGroup()
        {
            Outcomes = new List<EventOutcome>();
        }
    }
}
