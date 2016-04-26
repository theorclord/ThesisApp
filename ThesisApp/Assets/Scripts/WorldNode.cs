using UnityEngine;
using System.Collections;
using Assets.Scripts.Utility;

public class WorldNode : MonoBehaviour {

    public string NodeName
    { get; set; }
    public string NodeDesciption
    { get; set; }

    private bool visited;
    public bool Visited
    {
        get
        {
            return visited;
        }
        set
        {
            visited = value;
            setRingId();
        }
    }
    public NodeType Type
    { get; set; }
	
    // Use this for initialization
	void Start () {
        // Changes the halo of the world node
        if (Type == NodeType.START)
        {
            gameObject.transform.GetChild(1).GetComponent<MeshRenderer>().material.SetColor("_Color", new Color(0, 1, 0));
        }
        else if (Type == NodeType.GOAL)
        {
            gameObject.transform.GetChild(1).GetComponent<MeshRenderer>().material.SetColor("_Color", new Color(0, 0, 1));
        }
        else if (Type == NodeType.NORMAL)
        {
            if (Visited)
            {
                gameObject.transform.GetChild(1).GetComponent<MeshRenderer>().material.SetColor("_Color", new Color(1, 0, 1));
            }
            else
            {
                gameObject.transform.GetChild(1).GetComponent<MeshRenderer>().material.SetColor("_Color", new Color(1, 1, 1));
            }
        }
    }
    private void setRingId()
    {
        // Changes the halo of the world node
        if (Type == NodeType.START)
        {
            this.gameObject.transform.GetChild(1).GetComponent<MeshRenderer>().material.SetColor("_Color", new Color(0, 1, 0));
        }
        else if (Type == NodeType.GOAL)
        {
            this.gameObject.transform.GetChild(1).GetComponent<MeshRenderer>().material.SetColor("_Color", new Color(0, 0, 1));
        }
        else if (Type == NodeType.NORMAL)
        {
            if (Visited)
            {
                this.gameObject.transform.GetChild(1).GetComponent<MeshRenderer>().material.SetColor("_Color", new Color(1, 0, 1));
            }
            else
            {
                this.gameObject.transform.GetChild(1).GetComponent<MeshRenderer>().material.SetColor("_Color", new Color(1, 1, 1));
            }
        }
    }
	// Update is called once per frame
	void Update () {
        
	}
}
