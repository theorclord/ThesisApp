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
    public float cameraZoom = 5.0f;

    private float angleSpread = Mathf.PI/2;

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
        InitializeNodes();

    }
	
	// Update is called once per frame
	void Update () {
	
	}

    public void clearNodes()
    {
        Nodes.Clear();
    }

    public void GraphTest()
    {
        // Test Nodes, should be made with PCG
        PCGNode root = new PCGNode();
        root.type = PCGNodeType.S;
        PCGNode rootc1 = new PCGNode();
        PCGNode rootc2 = new PCGNode();
        PCGNode rootc3 = new PCGNode();
        root.Children.Add(rootc1);
        root.Children.Add(rootc2);
        root.Children.Add(rootc3);
        PCGNode c4 = new PCGNode();
        rootc1.Children.Add(c4);
        rootc2.Children.Add(c4);
        PCGNode c5 = new PCGNode();
        rootc3.Children.Add(c5);
        c4.Children.Add(c5);
        
        PCGNode end = new PCGNode();
        end.type = PCGNodeType.E;
        c5.Children.Add(end);

        // Visualization 
        bool endNotPlaced = true;
        PCGNode currentNode = root;
        Vector3 parentPos = Vector3.zero;
        PCGNode parent = null;
        // Take root node
        // Place children at radius $speed
        // Split into a cone of 90 degress
        // Repeat for child nodes
        while (endNotPlaced)
        {
            Vector3 curPos = Vector3.zero;
            if(currentNode.type == PCGNodeType.S)
            {
                curPos = Vector3.zero;
                string name = "Start Node";
                string description = "This is the first node";
                NodeType type = NodeType.START;
                Player.Position = new Vector3(curPos.x, curPos.y + 1.3f);
                WorldNodeStats newNode = new WorldNodeStats(curPos, name, description);
                newNode.Type = type;
                Nodes.Add(newNode);
            } else
            {

            }
            int curcount = currentNode.Children.Count;
            if (curcount == 1)
            {
                Vector3 placementPostion = new Vector3(curPos.x + Player.Speed, curPos.y);
                string name = "A node";
                string description = "This is a node";
                NodeType type = NodeType.NORMAL;
                WorldNodeStats newNode = new WorldNodeStats(placementPostion, name, description);
                newNode.Type = type;
                Nodes.Add(newNode);
            } else
            {
                float placementAngle = angleSpread / curcount;
                for( int i= 0; i<curcount; i++)
                {
                    float tranX = Player.Speed * Mathf.Cos((angleSpread / 2)-placementAngle*i);
                    float tranY = Player.Speed * Mathf.Sin((angleSpread / 2) - placementAngle * i);
                    Vector3 placementPostion = new Vector3(curPos.x +tranX, curPos.y+ tranY);
                    string name = "A node " + i;
                    string description = "This is a node";
                    NodeType type = NodeType.NORMAL;
                    WorldNodeStats newNode = new WorldNodeStats(placementPostion, name, description);
                    newNode.Type = type;
                    Nodes.Add(newNode);
                }
            }
            parent = currentNode;
            currentNode = currentNode.Children[0];
            endNotPlaced = false;
        }

    }

    public void InitializeNodes()
    {
        LSystem ls = new LSystem();
        ls.axiom = "SNN";
        ls.ruleSets.Add("S", "S[-N][+N]");
       /* int a = Random.Range(0, 2);
        if (a < 1)
        {
        ls.ruleSets.Add("N", "[-N]N[++N]");

        }
        else
        {*/
        ls.ruleSets.Add("N", "N[+N]-N");

        //}
       // Debug.Log(ls.ruleSets["N"]);
        ls.expand(3);
        ls.interpret();
        //TODO pcg generation of new node map
        //TODO generate using PCG
        /*int numnodes = 10;
        for (int i = 0; i < numnodes; i++)
        {
            Vector3 position = new Vector3(Random.Range(0.0f, 20.0f), Random.Range(0.0f, 20.0f));
            string name = "Node " + i;
            string description = "Description of node " + i;
            NodeType type = NodeType.NORMAL;
            //Test generate 10 random nodes
            if (i == 0)
            {
                //Set player start based on start node
                type = NodeType.START;
                Player.Position = new Vector3(position.x, position.y + 1.3f);
            }
            else if (i == numnodes - 1)
            {
                type = NodeType.GOAL;
            }

            WorldNodeStats newNode = new WorldNodeStats(position, name, description);
            newNode.Type = type;
            int numIntNodes = Random.Range(1, 4);
            for (int j = 0; j < numIntNodes; j++)
            {
                Vector3 intNodePos = new Vector3(-5 + j * 5, 0);
                string tempname = "Internal node " + j;
                string flavText = "The node " + j + " was freaking awesome";
                Event ev = new Event();
                int eventNumber = Random.Range(0, 3);
                ev.getXml(eventNumber);
                NodeStats ns = new NodeStats(intNodePos, tempname, flavText, ev);
                ns.generateEventType(eventNumber);
                newNode.Nodes.Add(ns);
            }
            Nodes.Add(newNode);
        }*/
    }

}
