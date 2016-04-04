using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Scripts.Utility
{
    public class EventOutcome
    {

        public EventOutcome(Piece basConPiece, int amount)
        {
            this.BoardPiece = basConPiece;
            this.Amount = amount;
        }

        public int Amount { get; set; }

        public Piece BoardPiece { get; set; }

        public int Chance { get; set; }

    }
}
