using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Scripts.Utility
{
    public class Piece
    {
        public string BoardName { get; set; }
        public Piece (string name)
        {
            BoardName = name;
        }
    }
}
