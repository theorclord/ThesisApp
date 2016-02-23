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

        public Event nodeEvent = new Event();
        
        public NodeStats(Vector3 pos, string titlename, string flavourText)
        {
            Position = pos;
            TitleName = titlename;
            FlavourText = flavourText;
        }

        public NodeStats()
        {
        }


    }
}
