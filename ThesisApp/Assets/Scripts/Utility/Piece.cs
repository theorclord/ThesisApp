using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Scripts.Utility
{
    public class Piece
    {
        public string BoardName { get; set; }
        public BoardType Type { get; set; }
        public Piece (string name)
        {
            BoardName = name;
        }
        public override int GetHashCode()
        {
            return BoardName.GetHashCode();
        }
        public override bool Equals(object obj)
        {
            
            return base.Equals(obj) && BoardName .Equals( ((Piece)obj).BoardName);
        }

    }
}
