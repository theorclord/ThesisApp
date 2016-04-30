using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Utility;
using System.Xml;

public class LSystem
{

    public string axiom { get; set; }
    //public IDictionary ruleSets = new Dictionary<string, string>();
    public string expanded { get; set; }
    public float delta { get; set; }
    float distance = 9.0f;

    public void expand(int depth, IDictionary ruleSets)
    {
        string result = axiom;
        for (int i = 0; i < depth; i++)
        {
            char[] currDSA = result.ToCharArray();
            string newString = "";
            for (int j = 0; j < currDSA.Length; j++)
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

        //bool replaced = false;
        char[] resList = result.ToCharArray();
        string tmpS = "";
        int index = resList.Length-1;
        bool found = false;

        while (!found)
        {
            if (resList[index] == '[' || resList[index] == ']' || resList[index] == '-' || resList[index] == '+')
            {
                index--;
            }
            else
            {
                found = true;
            }
        }

        for (int i = 0; i < resList.Length; i++)
        {
            if (i == index)
            {
                tmpS += "E";
            }
            else
            {
                tmpS += resList[i].ToString();
            }
        }
        result = tmpS;
        
        if (!result.Contains("S"))
        {
            string b = "S" + result;
            result = b;
        }
        expanded = result;

        Debug.Log(result);
    }

    public string addPOIs(string str)
    {
        string res = "";
        bool replaced = false;
        char[] resList = str.ToCharArray();
        int a = Random.Range(resList.Length / 2, resList.Length);
        while (!replaced)
        {
            // adds Trading post
            if (resList[a] != '[' && resList[a] != ']' && resList[a] != '-' && resList[a] != '+' && resList[a] != 'E')
            {
                // Debug.Log("Found candidate: " + resList[a].ToString());
                string tmpRes = "";
                for (int i = 0; i < a; i++)
                {
                    tmpRes += resList[i].ToString();
                }
                tmpRes += "T";
                for (int i = a + 1; i < resList.Length; i++)
                {
                    tmpRes += resList[i].ToString();
                }
                res = tmpRes;
                replaced = true;
            }
        }
        return res;
    }

    public void interpret()
    {
        Stack statestack = new Stack();
        State s = new State(Random.Range(0.0f, 20.0f), Random.Range(0.0f, 20.0f), 0, distance, delta);
        double x, y;
        char[] chars = expanded.ToCharArray();
        int numNodes = 0;
        for (int i = 0; i < chars.Length; i++)
        {
            switch (chars[i])
            {
                case 'S'://start node
                    x = s.x + (int)(s.length * Mathf.Cos((float)s.angle));
                    y = s.y + (int)(s.length * Mathf.Sin((float)s.angle));
                    s = new State(x, y, s.angle, s.length, s.turningAngle);

                    //place world node at position
                    Vector3 position = new Vector3((float)x, (float)y);
                    NodeType type = NodeType.START;
                    //Set player start based on start node
                    DataManager.instance.Player.Position = new Vector3(position.x, position.y + 1.3f);

                    //NodeStats (World Node)
                    generateNodeStats(position, type);
                    numNodes++;

                    break;
                case 'N'://Normal World Node
                    x = s.x + (int)(s.length * Mathf.Cos((float)s.angle));
                    y = s.y + (int)(s.length * Mathf.Sin((float)s.angle));
                    s = new State(x, y, s.angle, s.length, s.turningAngle);
                    //place normal world node
                    //place world node at position
                    position = new Vector3((float)x, (float)y);
                    type = NodeType.NORMAL;

                    //NodeStats (World Node)
                    generateNodeStats(position, type);
                    numNodes++;
                    break;
                case 'O'://Normal World Node
                    x = s.x + (int)(s.length * Mathf.Cos((float)s.angle));
                    y = s.y + (int)(s.length * Mathf.Sin((float)s.angle));
                    s = new State(x, y, s.angle, s.length, s.turningAngle);
                    //place normal world node
                    //place world node at position
                    position = new Vector3((float)x, (float)y);

                    type = NodeType.NORMAL;
                    generateNodeStats(position, type);
                    numNodes++;
                    break;
                case 'E'://End Node
                    x = s.x + (int)(s.length * Mathf.Cos((float)s.angle));
                    y = s.y + (int)(s.length * Mathf.Sin((float)s.angle));
                    s = new State(x, y, s.angle, s.length, s.turningAngle);
                    //place world node at position
                    position = new Vector3((float)x, (float)y);
                    type = NodeType.GOAL;
                    generateNodeStats(position, type);
                    numNodes++;
                    break;

                case 'R'://Random place
                    break;
                case 'T'://Trading post
                    x = s.x + (int)(s.length * Mathf.Cos((float)s.angle));
                    y = s.y + (int)(s.length * Mathf.Sin((float)s.angle));
                    s = new State(x, y, s.angle, s.length, s.turningAngle);
                    //place normal world node
                    //place world node at position
                    position = new Vector3((float)x, (float)y);
                    type = NodeType.TRADING;
                    
                    generateNodeStats(position, type);
                    numNodes++;
                    break;
                case 'D':// Distress signal
                    break;
                case '-':
                    s = new State(s.x, s.y, s.angle - s.turningAngle, s.length, s.turningAngle);
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

    /// <summary>
    /// Generates the stats for the individual nodes in the world node
    /// </summary>
    /// <param name="newNode"></param>
    private void generateNodeStats(Vector3 position, NodeType type)
    {
        WorldNodeStats wns = new WorldNodeStats();
        // Set position and type
        wns.Position = position;
        wns.Type = type;
        // Set name and description
        XmlDocument worldNodesDoc = new XmlDocument();
        worldNodesDoc.Load("assets/scripts/XML/WorldNode.xml");
        XmlNodeList worldNodes = worldNodesDoc.SelectNodes("WorldNodes/WorldNode");
        int[] nameOrder = DataManager.randomArray(worldNodes.Count);

        wns.WorldName = worldNodes[nameOrder[0]].SelectSingleNode("name").InnerText;
        wns.Description = worldNodes[nameOrder[0]].SelectSingleNode("description").InnerText;

        //check for duplications
        bool duplicate = false;

        foreach (WorldNodeStats wn in DataManager.instance.Nodes)
        {
            if (wn.Position == wns.Position)
            {
                if (wns.Type == NodeType.GOAL && wn.Type != NodeType.GOAL)
                {
                    wn.Type = NodeType.GOAL;
                }
                //Debug.Log("Duplicate found and removed");
                duplicate = true;
                break;
            }
        }

        // if there is no duplicate
        if (!duplicate)
        {
            // Assign faction
            int[] ranFacArr = DataManager.randomArray(DataManager.instance.Factions.Count);
            int countFac = 0;
            foreach (KeyValuePair<string, Faction> pair in DataManager.instance.Factions)
            {
                if (ranFacArr[0] == countFac)
                {
                    wns.NodeFaction = pair.Value;
                }
                countFac++;
            }
            // set the events
            int[] eventTypeOrder = DataManager.randomArray(2);
            int numIntNodes = eventTypeOrder.Length;

            for (int j = 0; j < numIntNodes; j++)
            {
                // The position of the nodes are currently just at 3 different points
                Vector3 intNodePos = new Vector3(-5 + j * 5, 0);
                NodeStats ns = new NodeStats();
                Event ev = new Event();
                ns.Position = intNodePos;
                ns.nodeEvent = ev;
                ns.setEventType(eventTypeOrder[j]);
                wns.Nodes.Add(ns);
                switch (ns.type)
                {
                    case EventSpec.GATHER:
                        setNameAndFlavor("Gather", ns);
                        break;/*
                    case EventSpec.DIPLOMACY:
                        setNameAndFlavor("Diplomacy", ns);
                        break;*/
                    case EventSpec.RESEARCH:
                        setNameAndFlavor("Research", ns);
                        break;
                }
            }
            DataManager.instance.Nodes.Add(wns);
        }
    }

    private void setNameAndFlavor(string type, NodeStats ns)
    {
        // Get name of nodes from xml
        XmlDocument nodeNameCollection = new XmlDocument();
        nodeNameCollection.Load("assets/scripts/XML/NodeNode.xml");
        XmlNodeList nameList = nodeNameCollection.SelectNodes("NodeNode/" + type + "/nodeFlavour");
        int selectedTitle = Random.Range(0, 5);
        ns.nodeEvent.SetLocType(selectedTitle);
        /*switch (ns.nodeEvent.locType)
        {
            case Location.MINE:
                selectedTitle = 0;
                break;
            case Location.QUARRY:
                selectedTitle = 1;
                break;
            case Location.WRECKAGE:
                selectedTitle = 2;
                break;
            case Location.FACTORY:
                selectedTitle = 3;
                break;
            case Location.VILLAGE:
                selectedTitle = 4;
                break;
            case Location.FOREST:
                selectedTitle = 0;
                break;
            case Location.ROCKFORMATION:
                selectedTitle = 1;
                break;
            case Location.MAGICSITE:
                selectedTitle = 2;
                break;
            case Location.LAKE:
                selectedTitle = 3;
                break;
            case Location.RUINS:
                selectedTitle = 4;
                break;

        }
        */
        string titlename = nameList.Item(selectedTitle).SelectSingleNode("name").InnerText;
        string flavour = nameList.Item(selectedTitle).SelectSingleNode("flavour").InnerText;
        ns.TitleName = titlename;
        //ns.islandName = DataManager.instance.ActiveNode.islandName;
        ns.FlavourText = flavour;
    }
}
