using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Utility;

public class PCGNode {
    public int ID
    {
        get;
        set;
    }
    public PCGNodeType type
    {
        get;
        set;
    }

    public bool visited
    {
        get;
        set;
    }
    public List<PCGNode> Children
    {
        get;
        private set;
    }

    public PCGNode()
    {
        Children = new List<PCGNode>();
    }


}
