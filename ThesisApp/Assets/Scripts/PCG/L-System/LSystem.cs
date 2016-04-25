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
        //Debug.Log(depth);
        //Debug.Log("Length of Rules:" + ruleSets.Count);
        // MORE Calculations needed for the nodes.
        for (int i = 0; i < depth; i++)
        {
            char[] currDSA = result.ToCharArray();
            // Debug.Log("Length of Currdsa:" + currDSA.Length);
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

        bool replaced = false;
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
        /*

        for (int s = resList.Length - 1; s > 0; s--)
        {
            if (resList[s] != '[' && resList[s] != ']' && resList[s] != '-' && resList[s] != '+' && !replaced)
            {
                tmpS += "E";
                replaced = true;
            }
            else
            {
                tmpS += resList[s].ToString();
            }
        }*/
        //tmpS += resList[0].ToString();
        result = tmpS;/*
        while (!replaced)
        {

            int x = resList.Length;
            int a = Random.Range(resList.Length / 2, resList.Length);
            if (resList[a] != '[' && resList[a] != ']' && resList[a] != '-' && resList[a] != '+')
            {
                // Debug.Log("Found candidate: " + resList[a].ToString());
                string tmpRes = "";
                for (int i = 0; i < a; i++)
                {
                    tmpRes += resList[i].ToString();
                }
                tmpRes += "E";
                for (int i = a + 1; i < resList.Length; i++)
                {
                    tmpRes += resList[i].ToString();
                }
                result = tmpRes;
                replaced = true;
            }
        }*/
        // result = addPOIs(result);
        if (!result.Contains("S"))
        {
            string b = "S" + result;
            result = b;
            //Debug.Log("does not contain S");
            /*  for(int i = 0; i < result.ToCharArray().Length; i++)
              {
                  if (result.ToCharArray()[i] == 'N')
                  {
                      char[] tmp = result.ToCharArray();
                      tmp[i] = 'S';
                      string a = tmp.ToString();
                      break;
                  }
              }*/
        }
        // result += "E";
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
                    //double preX = s.x;
                    //double preY = s.y;
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
                case 'O'://Normal World Node
                    x = s.x + (int)(s.length * Mathf.Cos((float)s.angle));
                    y = s.y + (int)(s.length * Mathf.Sin((float)s.angle));
                    s = new State(x, y, s.angle, s.length, s.turningAngle);
                    //place normal world node
                    //place world node at position
                    position = new Vector3((float)x, (float)y);
                    name = "Node " + numNodes;
                    description = "Description of node " + numNodes;
                    type = NodeType.NORMAL;
                    newNode = new WorldNodeStats(position, name, description);
                    newNode.Type = type;
                    generateNodeStats(newNode);
                    numNodes++;
                    break;
                case 'E'://End Node
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

                case 'R'://Random place
                    break;
                case 'T'://Trading post
                    x = s.x + (int)(s.length * Mathf.Cos((float)s.angle));
                    y = s.y + (int)(s.length * Mathf.Sin((float)s.angle));
                    s = new State(x, y, s.angle, s.length, s.turningAngle);
                    //place normal world node
                    //place world node at position
                    position = new Vector3((float)x, (float)y);
                    name = "Node " + numNodes;
                    description = "Description of node " + numNodes;
                    type = NodeType.TRADING;
                    //Test generate 10 random nodes
                    //Set player start based on start node

                    //Player.Position = new Vector3(position.x, position.y + 1.3f);

                    //NodeStats (World Node)
                    newNode = new WorldNodeStats(position, name, description);
                    newNode.Type = type;
                    generateNodeStats(newNode);
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
    private void generateNodeStats(WorldNodeStats newNode)
    {
        bool duplicate = false;

        foreach (WorldNodeStats wn in DataManager.instance.Nodes)
        {
            if (wn.Position == newNode.Position)
            {
                if (newNode.Type == NodeType.GOAL && wn.Type != NodeType.GOAL)
                {
                    wn.Type = NodeType.GOAL;
                }
                //Debug.Log("Duplicate found and removed");
                duplicate = true;
                break;
            }
        }

        // Assign faction
        int[] ranFacArr = DataManager.randomArray(DataManager.instance.Factions.Count);
        int countFac = 0;
        foreach (KeyValuePair<string, Faction> pair in DataManager.instance.Factions)
        {
            if (ranFacArr[0] == countFac)
            {
                newNode.NodeFaction = pair.Value;
            }
            countFac++;
        }

        if (!duplicate)
        {
            int numIntNodes = Random.Range(1, 4);
            int[] eventTypeOrder = new int[] { 0, 1, 2 };
            for (int i = 0; i < eventTypeOrder.Length; i++)
            {
                int random = Random.Range(0, eventTypeOrder.Length);
                int temp = eventTypeOrder[random];
                eventTypeOrder[random] = eventTypeOrder[i];
                eventTypeOrder[i] = temp;
            }

            for (int j = 0; j < numIntNodes; j++)
            {
                // The position of the nodes are currently just at 3 different points
                Vector3 intNodePos = new Vector3(-5 + j * 5, 0);
                NodeStats ns = new NodeStats();
                Event ev = new Event();
                ns.Position = intNodePos;
                ns.nodeEvent = ev;
                ns.setEventType(eventTypeOrder[j]);
                newNode.Nodes.Add(ns);
                switch (ns.type)
                {
                    case EventSpec.GATHER:
                        setNameAndFlavor("Gather", ns);
                        break;
                    case EventSpec.DIPLOMACY:
                        setNameAndFlavor("Diplomacy", ns);
                        break;
                    case EventSpec.RESEARCH:
                        setNameAndFlavor("Research", ns);
                        break;
                }
            }
            DataManager.instance.Nodes.Add(newNode);
        }
    }
    private void setNameAndFlavor(string type, NodeStats ns)
    {
        // Get name of nodes from xml
        XmlDocument nodeNameCollection = new XmlDocument();
        nodeNameCollection.Load("assets/scripts/XML/NodeNode.xml");
        XmlNodeList nameList = nodeNameCollection.SelectNodes("NodeNode/" + type + "/nodeFlavour");
        int selectedTitle = Random.Range(0, nameList.Count);
        string titlename = nameList.Item(selectedTitle).SelectSingleNode("name").InnerText;
        string flavour = nameList.Item(selectedTitle).SelectSingleNode("flavourText").InnerText;
        ns.TitleName = titlename;
        ns.FlavourText = flavour;
    }
}
