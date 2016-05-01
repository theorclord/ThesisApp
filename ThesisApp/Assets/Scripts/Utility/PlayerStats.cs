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
        private int maxRep = 150;
        private int minRep = -150;

        
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

        public void AddReputation(Faction fac, int value)
        {
            Debug.Log("inside add rep");
            Debug.Log(FactionRelations.Count);
            Debug.Log(fac.BoardName);
            int preVal = FactionRelations[fac];
            FactionRelations[fac] += value;
            if (FactionRelations[fac] > maxRep)
            {
                FactionRelations[fac] = maxRep;
            }
            else if (FactionRelations[fac] < minRep)
            {
                FactionRelations[fac] = minRep;
            }
            
            if(preVal > 50 && FactionRelations[fac] <= 50)
            {
                DataManager.instance.AllianceChange.Add(fac);
            } else if(preVal < -50 && FactionRelations[fac] >= -50)
            {
                DataManager.instance.AllianceChange.Add(fac);
            } else if(preVal > -50 && FactionRelations[fac] <= -50)
            {
                DataManager.instance.AllianceChange.Add(fac);
            } else if(preVal < 50 && FactionRelations[fac] >= 50)
            {
                DataManager.instance.AllianceChange.Add(fac);
            }
            /*
            foreach (Faction fc in FactionRelations.Keys)
            {
                Debug.Log(fc.BoardName + ", "+ fac.BoardName);
                if(fc.BoardName == fac.BoardName)
                {
                    Debug.Log("Rep Before: " + fc.BoardName + " " + FactionRelations[fc]);
                    FactionRelations[fc] += value;
                    if(FactionRelations[fc] > maxRep)
                    {
                        FactionRelations[fc] = maxRep;
                    }else if(FactionRelations[fc] < minRep)
                    {
                        FactionRelations[fc] = minRep;
                    }
                    Debug.Log("Rep After: " + fc.BoardName + " " + FactionRelations[fc]);
                    break;
                }
            }
            */
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
