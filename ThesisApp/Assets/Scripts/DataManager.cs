using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Utility;

public class DataManager : MonoBehaviour {
    public static DataManager instance;
    
    public PlayerStats Player
    { get; set; }
    public List<WorldNodeStats> Nodes
    { get; private set; }

    public WorldNodeStats ActiveNode
    {
        get; set;
    }
    void Awake()
    {
        if (!instance)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else {
            Destroy(gameObject);
        }
    }

    // Use this for initialization
    void Start () {
        Player = new PlayerStats();
        Nodes = new List<WorldNodeStats>();
        //Load world
        //TODO generate using PCG
        //Set player start based on start node
        
        int numnodes = 10;
        for(int i = 0; i< numnodes; i++)
        {
            Vector3 position = new Vector3(Random.Range(0.0f, 20.0f), Random.Range(0.0f, 20.0f));
            string name = "Node " + i;
            string description = "Description of node " + i;
            NodeType type = NodeType.NORMAL;
            //Test generate 10 random nodes
            if(i == 0)
            {
                type = NodeType.START;
                Player.Position = new Vector3(position.x, position.y + 1.3f);
            } else if (i == numnodes - 1) {
                type = NodeType.GOAL;
            }
            
            WorldNodeStats newNode = new WorldNodeStats(position, name, description);
            newNode.Type = type;
            int numIntNodes = Random.Range(1, 4);
            for(int j= 0; j<numIntNodes; j++)
            {
                Vector3 intNodePos = new Vector3(-5 + j * 5, 0);
                string tempname = "Internal node " + j;
                string flavText = "The node " + j + " was freaking awesome";
                newNode.Nodes.Add(new NodeStats(intNodePos, tempname, flavText));
            }
            Nodes.Add(newNode);
        }

	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void clearNodes()
    {
        Nodes.Clear();
    }

    public void InitializeNodes()
    {
        //TODO pcg generation of new node map
      /*  WorldNodeStats node1 = new WorldNodeStats();
        node1.Name = "Mats";
        node1.Description = "This is mats";
        node1.Position = new Vector3(0f, 0f);
        node1.Type = NodeType.START;
        Nodes.Add(node1);

        Player.Position = new Vector3(node1.Position.x, node1.Position.y + 1.3f);

        WorldNodeStats node2 = new WorldNodeStats();
        node2.Name = "Mikkel";
        node2.Description = "This is mikkel";
        node2.Position = new Vector3(5f, 0f);
        node2.Type = NodeType.GOAL;
        Nodes.Add(node2);*/

        int numnodes = 10;
        for (int i = 0; i < numnodes; i++)
        {
            Vector3 position = new Vector3(Random.Range(0.0f, 20.0f), Random.Range(0.0f, 20.0f));
            string name = "Node " + i;
            string description = "Description of node " + i;
            NodeType type = NodeType.NORMAL;
            //Test generate 10 random nodes
            if (i == 0)
            {
                type = NodeType.START;
                Player.Position = new Vector3(position.x, position.y + 1.3f);
            }
            else if (i == numnodes - 1)
            {
                type = NodeType.GOAL;
            }

            WorldNodeStats newNode = new WorldNodeStats(position, name, description);
            newNode.Type = type;
            Nodes.Add(newNode);
        }
    }

}
