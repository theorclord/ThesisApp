using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts.Utility
{
    public class PlayerStats
    {
        public Vector3 Position
        { get; set; }
        private float speed = 10;
        public float Speed
        {
            get { return speed; }
            set { speed = value; }
        }

    }
}
