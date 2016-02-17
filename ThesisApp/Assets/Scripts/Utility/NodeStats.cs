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
        public string Name
        { get; set; }
        public string Description
        { get; set; }

        public bool Visited
        { get; set; }

        public NodeType Type
        { get; set; }
        public NodeStats(Vector3 pos, string name, string description)
        {
            Position = pos;
            Name = name;
            Description = description;
            Visited = false;
            Type = NodeType.NORMAL;
        }

        public NodeStats()
        {
        }
    }
}
