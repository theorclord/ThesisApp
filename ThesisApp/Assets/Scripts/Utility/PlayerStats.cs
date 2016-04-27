using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts.Utility
{
    public class PlayerStats
    {

        private int Fthresh = 50;
        private int Ethresh = -50;
        
        public Vector3 Position
        { get; set; }
        private float speed = 10;
        public float Speed
        {
            get { return speed; }
            set { speed = value; }
        }

        public Dictionary<Faction,int> FactionRelations { get; set; }

        public PlayerStats()
        {
            FactionRelations = new Dictionary<Faction, int>();
        }

        public Standing getStanding(Faction f)
        {
            int a = FactionRelations[f];
            if(a >= Fthresh)
            {
                return Standing.FRIENDLY;
            }else if(a <= Ethresh)
            {
                return Standing.ENEMY;
            }
            else
            {
                return Standing.NEUTRAL;
            }
        }

    }
}
