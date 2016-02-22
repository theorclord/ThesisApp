using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Utility;

public class DataManager : MonoBehaviour {
    public static DataManager instance;
    
    public PlayerStats Player
    { get; set; }
    public List<NodeStats> Nodes
    { get; private set; }

    public NodeStats ActiveNode
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
        Nodes = new List<NodeStats>();
        //Load world
        //TODO generate using PCG
        //Set player start based on start node

        /*NodeStats node1 = new NodeStats();
        node1.Name = "Mats";
        node1.Description = "This is mats";
        node1.Position = new Vector3(0f,0f);
        node1.Type = NodeType.START;
        Nodes.Add(node1);*/

       // Player.Position = new Vector3(node1.Position.x, node1.Position.y+1.3f);

        /*NodeStats node2 = new NodeStats();
        node2.Name = "Mikkel";
        node2.Description = "This is mikkel";
        node2.Position = new Vector3(5f, 0f);
        node2.Type = NodeType.GOAL;
        Nodes.Add(node2);*/

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
            
            NodeStats newNode = new NodeStats(position, name, description);
            newNode.Type = type;
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
        NodeStats node1 = new NodeStats();
        node1.Name = "Mats";
        node1.Description = "This is mats";
        node1.Position = new Vector3(0f, 0f);
        node1.Type = NodeType.START;
        Nodes.Add(node1);

        Player.Position = new Vector3(node1.Position.x, node1.Position.y + 1.3f);

        NodeStats node2 = new NodeStats();
        node2.Name = "Mikkel";
        node2.Description = "This is mikkel";
        node2.Position = new Vector3(5f, 0f);
        node2.Type = NodeType.GOAL;
        Nodes.Add(node2);

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

            NodeStats newNode = new NodeStats(position, name, description);
            newNode.Type = type;
            Nodes.Add(newNode);
        }
    }

}
