using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Utility;

public class LSystem {
    
    public string axiom { get; set; }
    //public IDictionary ruleSets = new Dictionary<string, string>();
    public string expanded { get; set; }
    public float delta { get; set; }
    float distance = 9.0f;

    public void expand(int depth, IDictionary ruleSets)
    {
        string result = axiom;
        //Debug.Log(depth);
        //Debug.Log("Length of Rules:" + ruleSets.Count);
        // MORE Calculations needed for the nodes.
        for (int i = 0; i < depth; i++)
        {
            char[] currDSA = result.ToCharArray();
           // Debug.Log("Length of Currdsa:" + currDSA.Length);
            string newString = "";
            for(int j = 0; j < currDSA.Length; j++)
            {
                if (ruleSets.Contains(currDSA[j].ToString()))
                {
                    newString += ruleSets[currDSA[j].ToString()];
                }
                else
                {
                    newString += currDSA[j];
                }
            }
            
            result = newString;
        }
        if (!result.Contains("s"))
        {
            for(int i = 0; i < result.ToCharArray().Length; i++)
            {
                if (result.ToCharArray()[i] == 'N')
                {
                    char[] tmp = result.ToCharArray();
                    tmp[i] = 'S';
                    string a = tmp.ToString();
                    break;
                }
            }
        }
        result += "E";
        expanded = result;
        Debug.Log(result);
    }

    public void interpret()
    {
        Stack statestack = new Stack();
        State s = new State(Random.Range(0.0f, 20.0f), Random.Range(0.0f, 20.0f), 0,distance,delta);
        double x, y, a;
        char[] chars = expanded.ToCharArray();
        int numNodes = 0;
        for (int i = 0; i < chars.Length; i++)
        {
            switch (chars[i])
            {
                case 'S'://start node
                    double preX = s.x;
                    double preY = s.y;
                    x = s.x + (int)(s.length * Mathf.Cos((float)s.angle));
                    y = s.y + (int)(s.length * Mathf.Sin((float)s.angle));
                    s = new State(x, y, s.angle, s.length, s.turningAngle);

                    //place world node at position
                    Vector3 position = new Vector3((float)x, (float)y);
                    string name = "Node " + numNodes;
                    string description = "Description of node " + numNodes;
                    NodeType type = NodeType.START;
                    //Test generate 10 random nodes
                    //Set player start based on start node
                    
                    DataManager.instance.Player.Position = new Vector3(position.x, position.y + 1.3f);

                    //NodeStats (World Node)
                    WorldNodeStats newNode = new WorldNodeStats(position, name, description);
                    newNode.Type = type;
                    generateNodeStats(newNode);
                    numNodes++;

                    break;
                case 'N'://Normal World Node
                    preX = s.x;
                    preY = s.y;
                    //Debug.Log("y->Sin " + s.angle + ": " + Mathf.Sin((float)s.angle));
                    x = s.x + (int)(s.length * Mathf.Cos((float)s.angle));
                    y = s.y + (int)(s.length * Mathf.Sin((float)s.angle));
                    s = new State(x, y, s.angle, s.length, s.turningAngle);
                    //place normal world node
                    //place world node at position
                    position = new Vector3((float)x, (float)y);
                    name = "Node " + numNodes;
                    description = "Description of node " + numNodes;
                    type = NodeType.NORMAL;
                    //Test generate 10 random nodes
                    //Set player start based on start node

                    //Player.Position = new Vector3(position.x, position.y + 1.3f);

                    //NodeStats (World Node)
                    newNode = new WorldNodeStats(position, name, description);
                    newNode.Type = type;
                    generateNodeStats(newNode);
                    numNodes++;
                    break;
                case 'E'://End Node
                    preX = s.x;
                    preY = s.y;
                    
                    x = s.x + (int)(s.length * Mathf.Cos((float)s.angle));
                    y = s.y + (int)(s.length * Mathf.Sin((float)s.angle));
                    s = new State(x, y, s.angle, s.length, s.turningAngle);
                    //place world node at position
                    position = new Vector3((float)x, (float)y);
                    name = "Node " + numNodes;
                    description = "Description of node " + numNodes;
                    type = NodeType.GOAL;
                    //Test generate 10 random nodes
                    //Set player start based on start node

                    //Player.Position = new Vector3(position.x, position.y + 1.3f);

                    //NodeStats (World Node)
                    newNode = new WorldNodeStats(position, name, description);
                    newNode.Type = type;
                    generateNodeStats(newNode);
                    numNodes++;
                    break;
                case '-':
                    //Debug.Log("Before: " + s.angle);
                    s = new State(s.x, s.y, s.angle - s.turningAngle, s.length, s.turningAngle);
                    //Debug.Log("After: " + s.angle);
                    break;
                case '+':
                    s = new State(s.x, s.y, s.angle + s.turningAngle, s.length, s.turningAngle);
                    break;
                case '[':
                    statestack.Push(s);
                    break;
                case ']':
                    s = (State)statestack.Pop();
                    break;
            }
        }
    }
    
    private void generateNodeStats(WorldNodeStats newNode)
    {
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
            ns.setEventType(eventNumber);
            newNode.Nodes.Add(ns);
        }
        DataManager.instance.Nodes.Add(newNode);
    }
}
