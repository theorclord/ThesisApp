using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Scripts.Utility
{
    public class EventOutcome
    {
        public Dictionary<Piece, int[]> Pieces { get; set; }

        public EventOutcome()
        {
            Pieces = new Dictionary<Piece, int[]>();
        }

        public int Chance { get; set; }
        public EventOutcomeType Type { get; set; }
        public void AddPiece(Piece piece, int[] range)
        {
            if (Pieces.ContainsKey(piece))
            {
                Pieces[piece][0] += range[0];
                Pieces[piece][1] += range[1];
            } else
            {
                Pieces.Add(piece, range);
            }
        }
        
    }
}
