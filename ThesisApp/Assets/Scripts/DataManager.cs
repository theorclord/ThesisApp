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

        NodeStats node1 = new NodeStats();
        node1.Name = "Mats";
        node1.Description = "This is mats";
        node1.Position = new Vector3(0f,0f);
        node1.Type = NodeType.START;
        Nodes.Add(node1);

        Player.Position = new Vector3(node1.Position.x, node1.Position.y+1.3f);

        NodeStats node2 = new NodeStats();
        node2.Name = "Mikkel";
        node2.Description = "This is mikkel";
        node2.Position = new Vector3(5f, 0f);
        node2.Type = NodeType.GOAL;
        Nodes.Add(node2);

        for(int i = 0; i<10; i++)
        {
            //Test generate 10 random nodes
            if(i == 0)
            {

            }
            NodeStats newNode = new NodeStats();
        }

	}
	
	// Update is called once per frame
	void Update () {
	
	}

}
