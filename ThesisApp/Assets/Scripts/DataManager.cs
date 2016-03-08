﻿using UnityEngine;
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
    
    public void InitializeNodes()
    {
        LSystem ls = new LSystem();
        RuleCollection rc = new RuleCollection();
        rc.GenerateRules();
        int selectedIndex = Random.Range(0, rc.GetCollection().Count);
        LRule r = (LRule)rc.GetCollection()[selectedIndex];
        ls.axiom = r.axiom;
        ls.delta = r.delta;
        // JUST GO THROUGH THE RANDOMLY SELECTED RULE, USE ITS COLLECTION IN THE EXPAND AND INTERPRET
        ls.expand(r.expandingIterations, r.rules);
        ls.interpret();
       /* ls.axiom = "SNN";
        ls.ruleSets.Add("S", "S[-N][+N]");
        ls.ruleSets.Add("N", "N[+N]-N");
        ls.expand(3);
        ls.interpret();  */

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
