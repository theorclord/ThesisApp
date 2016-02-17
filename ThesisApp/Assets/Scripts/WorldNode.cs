using UnityEngine;
using System.Collections;

public class WorldNode : MonoBehaviour {

    public string NodeName
    {
        get;
        set;
    }
    public string NodeDesciption
    {
        get;
        set;
    }

    public bool Visited
    {
        get;
        set;
    }

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void GetNodeLayout()
    {
        //TODO return the layout needed
    }
}
