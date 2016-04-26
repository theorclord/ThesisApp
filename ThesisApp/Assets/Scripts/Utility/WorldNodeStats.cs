using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts.Utility
{
    public class WorldNodeStats
    {
        public Vector3 Position
        { get; set; }
        public string Name
        { get; set; }
        public string Description
        { get; set; }
        public bool Visited
        { get; set; }
        public NodeType Type
        { get; set; }
        public List<NodeStats> Nodes { get; set; }

        public Faction NodeFaction { get; set; }

        public WorldNodeStats()
        {
            Nodes = new List<NodeStats>();
            Visited = false;
            Type = NodeType.NORMAL;
        }


    }
}
