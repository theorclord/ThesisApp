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
            // Changes the halo of the world node
            if (Type == NodeType.START)
            {
                setAllHaloInactive();
                this.gameObject.transform.GetChild(0).gameObject.SetActive(true);
            }
            else if (Type == NodeType.GOAL)
            {
                setAllHaloInactive();
                this.gameObject.transform.GetChild(3).gameObject.SetActive(true);
            }
            else if (Type == NodeType.NORMAL)
            {
                if (Visited)
                {
                    setAllHaloInactive();
                    this.gameObject.transform.GetChild(1).gameObject.SetActive(true);
                }
                else
                {
                    setAllHaloInactive();
                    this.gameObject.transform.GetChild(2).gameObject.SetActive(true);
                }
            }
        }
    }
    public NodeType Type
    { get; set; }
	
    // Use this for initialization
	void Start () {
        // Changes the halo of the world node
        if (Type == NodeType.START)
        {
            setAllHaloInactive();
            this.gameObject.transform.GetChild(0).gameObject.SetActive(true);
        }
        else if (Type == NodeType.GOAL)
        {
            setAllHaloInactive();
            this.gameObject.transform.GetChild(3).gameObject.SetActive(true);
        }
        else if (Type == NodeType.NORMAL)
        {
            if (Visited)
            {
                setAllHaloInactive();
                this.gameObject.transform.GetChild(1).gameObject.SetActive(true);
            }
            else
            {
                setAllHaloInactive();
                this.gameObject.transform.GetChild(2).gameObject.SetActive(true);
            }
        }
    }

    void setAllHaloInactive()
    {
        this.gameObject.transform.GetChild(0).gameObject.SetActive(false);
        this.gameObject.transform.GetChild(1).gameObject.SetActive(false);
        this.gameObject.transform.GetChild(2).gameObject.SetActive(false);
        this.gameObject.transform.GetChild(3).gameObject.SetActive(false);
    }
	
	// Update is called once per frame
	void Update () {
        
	}

    public void GetNodeLayout()
    {
        //TODO return the layout needed
    }
}
