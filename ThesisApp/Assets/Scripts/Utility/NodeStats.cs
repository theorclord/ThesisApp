using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts.Utility
{
    public class NodeStats
    {
        public Vector3 Position
        { get; set; }
        public string TitleName
        { get; set; }
        public string FlavourText
        { get; set; }

        public Event nodeEvent
        { get; set; }

        public EventSpec type { get; set; }

        public void setEventType(int typeNumber)
        {

            switch (typeNumber)
            {
                case 0:
                    type = EventSpec.GATHER;
                    break;
                case 1:
                    type = EventSpec.RESEARCH;
                    break;
                    /*
                case 2:
                    type = EventSpec.DIPLOMACY;
                    break;
                    */
            }
        }

        public NodeStats()
        {
        }


    }
}
