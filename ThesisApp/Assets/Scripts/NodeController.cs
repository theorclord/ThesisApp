﻿using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using Assets.Scripts.Utility;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Xml;
using Assets.Scripts.Events;
using System;

public class NodeController : MonoBehaviour
{

    private GameObject selected;

    private GameObject nodeInfoPanel;
    private GameObject eventPanel;
    private NodeNode selectNode;
    private GameObject resultPanel;
    private GameObject specialPanel;

    private string allegiance1 = "";
    private string allegiance2 = "";

    private bool panelOpen;
    private string eventstructurepath = "assets/scripts/XML/EventStructure.xml";
    private string specEvPath = "assets/scripts/XML/SpecialEvents.xml";
    // Use this for initialization
    void Start()
    {
        generateNodes();
        nodeInfoPanel = GameObject.FindGameObjectWithTag("MainCanvas").transform.FindChild("NodeInformation").gameObject;
        eventPanel = (GameObject.Find("Canvas").gameObject.transform.FindChild("EventPanel").gameObject);
        resultPanel = GameObject.FindGameObjectWithTag("MainCanvas").transform.FindChild("EventResolution").gameObject;
        specialPanel = (GameObject.Find("Canvas").gameObject.transform.FindChild("PremadeEventPanel").gameObject);

        // Loading preevents if conditions are right
        // Either Nomad:Wreckage/Village, Human:Factory/Mine, Highbourne:Forest/MagicSite

        if (DataManager.instance.ActiveDiplomaticEvent != null && DataManager.instance.TurnCounter >= DataManager.instance.ActiveDiplomaticEvent.TurnCount + 2)
        {
            if (CheckFactionLocation())
            {
                InitSpecialEvent();
            }
        }
    }

    void InitSpecialEvent()
    {
        Debug.Log("The Special event runs");
        //Load special event scene
        specialPanel.SetActive(true);
        //LOAD XML
        XmlDocument doc = new XmlDocument();
        doc.Load(specEvPath);

        /**
        get the event
            name, locationtype, faction
        */
        string name = DataManager.instance.ActiveDiplomaticEvent.IslandName;
        string location = DataManager.instance.ActiveDiplomaticEvent.Location.ToString();
        string faction = DataManager.instance.ActiveDiplomaticEvent.Alligance.BoardName;
        //
        string combination = faction + location;
        //Debug.Log(combination);
        string intro = "";
        string body = "";
        string extra = "";
        string button1 = "";
        string button2 = "";
        string superAllegiance = "";

        switch (DataManager.instance.Player.getStanding(DataManager.instance.ActiveDiplomaticEvent.Alligance))
        {
            case Standing.ENEMY:
                superAllegiance = "/enemy";
                break;
            case Standing.FRIENDLY:
                superAllegiance = "/ally";
                break;
            case Standing.NEUTRAL:
                superAllegiance = "/neutral";
                break;
        }
        //DataManager.instance.ActiveDiplomaticEvent.
        XmlNodeList combs = doc.SelectNodes("specialEvents/combination[@type='" + combination + "']");
        Debug.Log("size of combination: " + combs.Count);
        foreach (XmlNode xn in combs)
        {
            intro = xn.SelectSingleNode("intro").InnerText;
            body = xn.SelectSingleNode("body").InnerText;
            extra = xn.SelectSingleNode("extra").InnerText;
            button1 = xn.SelectSingleNode("options/one/button").InnerText;
            button2 = xn.SelectSingleNode("options/two/button").InnerText;
            allegiance1 = xn.SelectSingleNode("options/one" + superAllegiance).InnerText;
            allegiance2 = xn.SelectSingleNode("options/two" + superAllegiance).InnerText;
        }

        specialPanel.transform.FindChild("FlavourScreen").FindChild("EventText").GetComponent<Text>().text = intro + name + body + extra;
        specialPanel.transform.FindChild("ButtonController").GetChild(0).FindChild("Text").GetComponent<Text>().text = button1;
        specialPanel.transform.FindChild("ButtonController").GetChild(1).FindChild("Text").GetComponent<Text>().text = button2;

    }

    public void buttonOneClicked()
    {
        specialPanel.SetActive(false);
        resultPanel.SetActive(true);
        resultPanel.transform.FindChild("ResolutionScreen").FindChild("ResolutionText").GetComponent<Text>().text = allegiance1;
    }
    public void buttonTwoClicked()
    {
        specialPanel.SetActive(false);
        resultPanel.SetActive(true);
        resultPanel.transform.FindChild("ResolutionScreen").FindChild("Text").GetComponent<Text>().text = allegiance2;
    }


    bool CheckFactionLocation()
    {
        // Either Nomad:Wreckage/Village, Human:Factory/Mine, Highbourne:Forest/MagicSite
        bool match = false;
        string bn = DataManager.instance.ActiveDiplomaticEvent.Alligance.BoardName;
        switch (bn)
        {
            case "Nomads":
                if (DataManager.instance.ActiveDiplomaticEvent.Location.ToString() == "WRECKAGE" || DataManager.instance.ActiveDiplomaticEvent.Location.ToString() == "VILLAGE")
                {
                    match = true;
                }
                break;
            case "Highbournes":
                if (DataManager.instance.ActiveDiplomaticEvent.Location.ToString() == "FOREST" || DataManager.instance.ActiveDiplomaticEvent.Location.ToString() == "MAGICSITE")
                {
                    match = true;
                }
                break;
            case "Humans":
                if (DataManager.instance.ActiveDiplomaticEvent.Location.ToString() == "MINE" || DataManager.instance.ActiveDiplomaticEvent.Location.ToString() == "FACTORY")
                {
                    match = true;
                }
                break;
        }

        return match;
    }

    // Update is called once per frame
    void Update()
    {
        if (!panelOpen && Input.GetMouseButtonDown(0))
        {
            
            RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
            Debug.Log(hit);
            Debug.Log(hit.collider);
            if (hit.collider != null)
            {
                Debug.Log("Collider hit");
                selected = hit.transform.gameObject;
                if (selected.tag == "LocationNode")
                {
                    if (selected.GetComponent<NodeNode>() != null)
                    {
                        showLocationInfo(selected);
                    }
                }
            }/*
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, 100))
            {

                selected = hit.transform.gameObject;
                if (selected.tag == "LocationNode")
                {
                    if (selected.GetComponent<NodeNode>() != null)
                    {
                        showLocationInfo(selected);
                    }
                }
            }
            */
        }

    }

    private void showLocationInfo(GameObject selected)
    {
        selectNode = selected.GetComponent<NodeNode>();
        nodeInfoPanel.SetActive(true);
        nodeInfoPanel.transform.Find("LocationName").GetComponent<Text>().text = selectNode.TitleName;
        nodeInfoPanel.transform.Find("FlavorText").GetComponent<Text>().text = selectNode.FlavourText;
        panelOpen = true;
    }

    public void BackButton()
    {
        panelOpen = false;
        SceneManager.LoadScene("WorldScene");
    }

    private void generateNodes()
    {
        // Number of options available for the players. 
        // Random determine if 2 or 3. 
        // Third option should be semi rare
        int thirdChance = 100;
        int procent = UnityEngine.Random.Range(0, 100) + 1;
        int numOption = 2;
        if (procent <= thirdChance)
        {
            numOption = 3;
        }
        WorldNodeStats stats = DataManager.instance.ActiveNode;
        foreach (NodeStats nodestat in stats.Nodes)
        {
            //GameObject nodeObj = Instantiate(Resources.Load("Prefabs/NodeNode"), nodestat.Position, Quaternion.identity) as GameObject;
            GameObject nodeObj = null;
            nodestat.nodeEvent.GenerateEvent(nodestat.type, numOption);
            string locationString = nodestat.nodeEvent.locationXmlString;
            //Load location data
            XmlDocument LocationData = new XmlDocument();
            LocationData.Load("assets/scripts/XML/Location.xml");
            XmlNodeList locations = LocationData.SelectNodes("locations/location");
            // Set information of location based on nodes
            foreach(XmlNode location in locations)
            {
                if(location.SelectSingleNode("name").InnerText == locationString)
                {
                    string[] coord = location.SelectSingleNode("coordinates").InnerText.Split(',');
                    Vector3 locPos = new Vector3(float.Parse(coord[0]),float.Parse(coord[1]));
                    nodeObj = Instantiate(Resources.Load("Prefabs/Node2D"), locPos, Quaternion.identity) as GameObject;
                    string[] scale = location.SelectSingleNode("size").InnerText.Split(',');
                    nodeObj.transform.localScale = new Vector3(float.Parse(scale[0]),float.Parse(scale[1]));
                    nodeObj.GetComponent<SpriteRenderer>().sprite = Resources.Load("LocationSprite/" + location.SelectSingleNode("file").InnerText, typeof (Sprite)) as Sprite;
                    break;
                }
            }
            
            NodeNode node = nodeObj.GetComponent<NodeNode>();
            node.FlavourText = nodestat.FlavourText;
            node.TitleName = nodestat.TitleName;
            node.nodeEvent = nodestat.nodeEvent;
            node.type = nodestat.type;
        }
    }

    public void ClosePanel(GameObject panel)
    {
        panelOpen = false;
        panel.SetActive(false);
    }

    public void ExploreNode(GameObject Sender)
    {
        Sender.SetActive(false);
        Event e = selectNode.nodeEvent;
        // Set conditions and choices:
        Transform buttoncont = eventPanel.transform.FindChild("ButtonController");

        //EventOption button section
        for (int i = 0; i < e.EventOptions.Count; i++)
        {
            //Need to copy else the value is only set to the last i
            int tempint = i;
            GameObject button = Instantiate(Resources.Load("Prefabs/ChoiceButton") as GameObject);
            button.transform.SetParent(buttoncont);
            button.GetComponent<Button>().onClick.AddListener(delegate { ResolveEvent(tempint); });
            button.transform.position = new Vector3(buttoncont.position.x, buttoncont.position.y - 55 * i);
            string buttonText = "";
            int numCon = 0;
            string btx = "";
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(eventstructurepath);
            foreach (KeyValuePair<Piece, int> pair in e.EventOptions[tempint].Conditions)
            {
                string pieceName = getPieceNameForXml(pair.Key.BoardName);
                //Basic first, then add the location based for the piece.
                XmlNodeList butFlav = xmlDoc.SelectNodes("eventstructure/conditionflavor/basics" + pieceName + "/flavor");

                int[] flSel = DataManager.randomArray(butFlav.Count);
                if (pieceName == "/crew" || pieceName == "/material" || pieceName == "/crystal" || pieceName == "/alchemy")
                {
                    btx += xmlDoc.SelectSingleNode("eventstructure/conditionflavor/basics" + pieceName + "/flavor/first").InnerText;
                    btx += pair.Value;
                    btx += xmlDoc.SelectSingleNode("eventstructure/conditionflavor/basics" + pieceName + "/flavor/second").InnerText;
                }
                else
                {
                    btx += butFlav[flSel[0]].InnerText;
                }
                Debug.Log("Location: " + e.EventOptions[tempint].locationXmlString);
                butFlav = xmlDoc.SelectNodes("eventstructure/conditionflavor/" + e.EventOptions[tempint].locationXmlString + pieceName + "/flavor");
                flSel = DataManager.randomArray(butFlav.Count);
                btx += butFlav[flSel[0]].InnerText;

                buttonText += pair.Key.BoardName + " " + pair.Value;
                numCon++;
                if (e.EventOptions[tempint].Conditions.Count != numCon)
                {
                    buttonText += ", ";
                    btx += " ";
                }
            }
            button.transform.GetChild(0).GetComponent<Text>().text = btx;
        }

        // TODO Clean up
        // TODO Set flavor text based on eventOption
        eventPanel.transform.FindChild("NameScreen").gameObject.SetActive(true);
        eventPanel.transform.FindChild("NameScreen").transform.FindChild("EventType").GetComponent<Text>().text = selectNode.TitleName;//e.eventText + "\n";
        eventPanel.transform.FindChild("FlavourScreen").gameObject.SetActive(true);
        eventPanel.transform.FindChild("FlavourScreen").transform.FindChild("EventText").GetComponent<Text>().text = e.entryFlavor;
        panelOpen = true;
        eventPanel.SetActive(true);
    }

    public string getPieceNameForXml(string name)
    {
        string outS = "";
        switch (name)
        {
            case "Castle Crew":
                outS = "/crew";
                break;
            case "Building Material":
                outS = "/material";
                break;
            case "Crystal Charge":
                outS = "/crystal";
                break;
            case "Miners Guild":
                outS = "/minersguild";
                break;
            case "Workshop":
                outS = "/workshop";
                break;
            case "Alchemical Material":
                outS = "/alchemy";
                break;
            case "Alchemical Lab":
                outS = "/alchemylab";
                break;
            case "Cleric Quarters":
                outS = "/cleric";
                break;
            case "Ambassadors Quarters":
                outS = "/ambassador";
                break;
        }
        return outS;
    }

    public void ResolveEvent(int eventnum)
    {
        // Saved results
        SavedResult savRes = new SavedResult();
        Event curEvent = selectNode.nodeEvent;
        //Save conditions
        savRes.Conditions = curEvent.EventOptions[eventnum].Conditions;
        savRes.TurnCount = DataManager.instance.TurnCounter;
        savRes.IslandName = selectNode.TitleName;
        eventPanel.SetActive(false);
        panelOpen = true;
        resultPanel.SetActive(true);
        //Find outcome
        //check player faction relation
        //100 points for each state
        //-150 -> -50 enemy
        // -50 -> 50 neutral
        // 50 -> 150 friendly
        int facRelVal = 0;
        Dictionary<Faction, int> facRel = DataManager.instance.Player.FactionRelations;
        Faction activeFac = DataManager.instance.ActiveNode.NodeFaction;
        // Hard code faction for mines for special events
        //TODO remove
        if (curEvent.EventOptions[eventnum].locationXmlString == "/mine")
        {
            activeFac = DataManager.instance.Factions["human"];
        }
        
        if (facRel.ContainsKey(activeFac))
        {
            // get relation
            facRelVal = facRel[activeFac];
        }
        else
        {
            // Set new faction
            facRel.Add(activeFac, 0);
            facRelVal = 0;
        }
        // Save faction
        Debug.Log(activeFac.BoardName);
        savRes.Alligance = activeFac;
        int succesChange = 0;
        int failureChange = 0;
        int neutralChange = 0;

        if (facRelVal >= 50)
        {
            //faction is allied

            switch (eventnum)
            {
                case 0:
                    succesChange += 10;
                    neutralChange += -10;
                    break;
                case 1:
                    succesChange += 15;
                    neutralChange += -10;
                    failureChange += -5;
                    break;
                case 2:
                    succesChange += 5;
                    failureChange += -5;
                    break;
                default:
                    break;
            }
        }
        else if (facRelVal <= -50)
        {
            //faction is enemy
            switch (eventnum)
            {
                case 0:
                    succesChange += -10;
                    neutralChange += -5;
                    failureChange += 15;
                    break;
                case 1:
                    succesChange += -10;
                    failureChange += 10;
                    break;
                case 2:
                    succesChange += -10;
                    neutralChange += 5;
                    failureChange += 5;
                    break;
                default:
                    break;
            }
        }
        // Save location
        savRes.Location = curEvent.EventOptions[eventnum].locType;
        int chance = UnityEngine.Random.Range(0, 100) + 1;
        int accumChance = 0;
        Dictionary<Piece, int> outcomePairs = new Dictionary<Piece, int>();
        for (int k = 0; k < curEvent.EventOptions[eventnum].Results.Count; k++)
        {
            EventOutcomeGroup eog = curEvent.EventOptions[eventnum].Results[k];
            for (int i = 0; i < eog.Outcomes.Count; i++)
            {
                EventOutcome eo = eog.Outcomes[i];
                int modChance = eo.Chance;
                switch (eo.Type)
                {
                    case EventOutcomeType.SUCCESS:
                        modChance += succesChange;
                        savRes.OutcomeType = EventOutcomeType.SUCCESS;
                        break;
                    case EventOutcomeType.NEUTRAL:
                        modChance += neutralChange;
                        savRes.OutcomeType = EventOutcomeType.NEUTRAL;
                        break;
                    case EventOutcomeType.FAILURE:
                        modChance += failureChange;
                        savRes.OutcomeType = EventOutcomeType.FAILURE;
                        break;
                    default:
                        break;
                }
                if (modChance >= chance - accumChance)
                {
                    foreach (KeyValuePair<Piece, int[]> pair in eo.Pieces)
                    {
                        Piece outPiece = null;
                        //Handle random room
                        if (pair.Key.BoardName == "RandomRoom")
                        {
                            List<Piece> tempRooms = new List<Piece>();
                            foreach (KeyValuePair<string, Piece> piecePair in DataManager.instance.BoardPieces)
                            {
                                if (piecePair.Value.Type == BoardType.ROOM)
                                {
                                    tempRooms.Add(piecePair.Value);
                                }
                            }
                            int[] roomOrder = DataManager.randomArray(tempRooms.Count);
                            outPiece = tempRooms[roomOrder[0]];
                        }
                        else
                        {
                            outPiece = pair.Key;
                        }
                        //If the faction is friendly, take the max val, if enemy min val
                        int num = 0;
                        if (facRelVal >= 50)
                        {
                            num = pair.Value[1] + 1;
                        }
                        else if (facRelVal <= -50)
                        {
                            num = pair.Value[0];
                        }
                        else
                        {
                            num = UnityEngine.Random.Range(pair.Value[0], pair.Value[1] + 1);
                        }
                        if (outcomePairs.ContainsKey(outPiece))
                        {
                            outcomePairs[outPiece] = outcomePairs[outPiece] + num;
                        }
                        else
                        {
                            outcomePairs.Add(outPiece, num);
                        }
                    }
                    break;
                }
                else
                {
                    accumChance += modChance;
                }
            }
        }
        // Saving the outcomes
        savRes.Outcomes = outcomePairs;
        //Build outcome string
        ArrayList resPieces = new ArrayList();
        ArrayList resPieceNumbs = new ArrayList();
        string resultString = "";
        int tempCount = 0;
        foreach (KeyValuePair<Piece, int> pieceNum in outcomePairs)
        {
            tempCount++;

            if (pieceNum.Key.Type == BoardType.ROOM) // || outPiece.BoardName == "Castle Crew")
            {
                resultString += pieceNum.Key.BoardName + " ";
                switch (pieceNum.Value)
                {
                    case 1:
                        resultString += "Created";
                        break;
                    case -1:
                        resultString += "Damaged";
                        break;
                    case -10:
                        resultString += "Destroyed";
                        break;
                }
            }
            else if (pieceNum.Key.BoardName == "Castle Crew")
            {
                switch (pieceNum.Value)
                {
                    case 10:
                        resultString += pieceNum.Key.BoardName + " ";
                        resultString += "Recruited";
                        break;
                    case 1:
                        resultString += "Injured ";
                        resultString += pieceNum.Key.BoardName + " ";
                        resultString += "Recruited";
                        break;
                    case -1:
                        resultString += pieceNum.Key.BoardName + " ";
                        resultString += "Injured";
                        break;
                    case -10:
                        resultString += pieceNum.Key.BoardName + " ";
                        resultString += "Dead";
                        break;
                }
            }
            else
            {
                resultString += pieceNum.Key.BoardName + " ";
                if (pieceNum.Value > 0) resultString += "+" + pieceNum.Value;
                if (pieceNum.Value < 0) resultString += pieceNum.Value;

            }
            resPieces.Add(pieceNum.Key.BoardName);
            resPieceNumbs.Add(pieceNum.Value);
            if (outcomePairs.Count != tempCount)
            {
                resultString += ", ";
            }
        }
        resultPanel.transform.FindChild("OutcomeScreen").transform.FindChild("Outcome").GetComponent<Text>().text = resultString;


        // This should be the flavor text
        string resultflavortext = "";
        XmlDocument xmlDoc = new XmlDocument();
        xmlDoc.Load(eventstructurepath);
        XmlNodeList list;
        for (int i = 0; i < resPieces.Count; i++)
        {
            string s = (string)resPieces[i];
            int r;
            switch (s)
            {

                case "Castle Crew":
                    if ((int)resPieceNumbs[i] == -10)
                    {
                        list = xmlDoc.SelectNodes("eventstructure/resultflavor/crewdead/flavor");
                        r = UnityEngine.Random.Range(0, list.Count);
                        resultflavortext += list[r].InnerText;
                    }
                    else if ((int)resPieceNumbs[i] == -1)
                    {
                        list = xmlDoc.SelectNodes("eventstructure/resultflavor/crewinjured/flavor");
                        r = UnityEngine.Random.Range(0, list.Count);
                        resultflavortext += list[r].InnerText;
                    }
                    else if ((int)resPieceNumbs[i] == 1)
                    {
                        list = xmlDoc.SelectNodes("eventstructure/resultflavor/crewrecruitedinjured/flavor");
                        r = UnityEngine.Random.Range(0, list.Count);
                        resultflavortext += list[r].InnerText;
                    }
                    else if ((int)resPieceNumbs[i] == 10)
                    {
                        list = xmlDoc.SelectNodes("eventstructure/resultflavor/crewrecruited/flavor");
                        r = UnityEngine.Random.Range(0, list.Count);
                        resultflavortext += list[r].InnerText;
                    }
                    break;
                case "Building Material":
                    list = xmlDoc.SelectNodes("eventstructure/resultflavor/scrap/flavor");
                    r = UnityEngine.Random.Range(0, list.Count);
                    resultflavortext += list[r].InnerText;
                    break;
                case "Crystal Charge":
                    if ((int)resPieceNumbs[i] < 0)
                    {
                        list = xmlDoc.SelectNodes("eventstructure/resultflavor/energylost/flavor");
                        r = UnityEngine.Random.Range(0, list.Count);
                        resultflavortext += list[r].InnerText;
                    }
                    else
                    {
                        list = xmlDoc.SelectNodes("eventstructure/resultflavor/energygained/flavor");
                        r = UnityEngine.Random.Range(0, list.Count);
                        resultflavortext += list[r].InnerText;
                    }
                    break;
                case "Miners Guild":
                    if ((int)resPieceNumbs[i] == -10)
                    {
                        list = xmlDoc.SelectNodes("eventstructure/resultflavor/roomdestroyed/flavor");
                        r = UnityEngine.Random.Range(0, list.Count);
                        resultflavortext += list[r].InnerText;
                    }
                    else if ((int)resPieceNumbs[i] == -1)
                    {
                        list = xmlDoc.SelectNodes("eventstructure/resultflavor/roomdamaged/flavor");
                        r = UnityEngine.Random.Range(0, list.Count);
                        resultflavortext += list[r].InnerText;
                    }
                    else if ((int)resPieceNumbs[i] == 1)
                    {
                        list = xmlDoc.SelectNodes("eventstructure/resultflavor/roomgained/flavor");
                        r = UnityEngine.Random.Range(0, list.Count);
                        resultflavortext += list[r].InnerText;
                    }
                    break;
                case "Workshop":
                    if ((int)resPieceNumbs[i] == -10)
                    {
                        list = xmlDoc.SelectNodes("eventstructure/resultflavor/roomdestroyed/flavor");
                        r = UnityEngine.Random.Range(0, list.Count);
                        resultflavortext += list[r].InnerText;
                    }
                    else if ((int)resPieceNumbs[i] == -1)
                    {
                        list = xmlDoc.SelectNodes("eventstructure/resultflavor/roomdamaged/flavor");
                        r = UnityEngine.Random.Range(0, list.Count);
                        resultflavortext += list[r].InnerText;
                    }
                    else if ((int)resPieceNumbs[i] == 1)
                    {
                        list = xmlDoc.SelectNodes("eventstructure/resultflavor/roomgained/flavor");
                        r = UnityEngine.Random.Range(0, list.Count);
                        resultflavortext += list[r].InnerText;
                    }
                    break;
                case "Alchemical Material":
                    list = xmlDoc.SelectNodes("eventstructure/resultflavor/alchemy/flavor");
                    r = UnityEngine.Random.Range(0, list.Count);
                    resultflavortext += list[r].InnerText;
                    break;
                case "Alchemical Lab":
                    if ((int)resPieceNumbs[i] == -10)
                    {
                        list = xmlDoc.SelectNodes("eventstructure/resultflavor/roomdestroyed/flavor");
                        r = UnityEngine.Random.Range(0, list.Count);
                        resultflavortext += list[r].InnerText;
                    }
                    else if ((int)resPieceNumbs[i] == -1)
                    {
                        list = xmlDoc.SelectNodes("eventstructure/resultflavor/roomdamaged/flavor");
                        r = UnityEngine.Random.Range(0, list.Count);
                        resultflavortext += list[r].InnerText;
                    }
                    else if ((int)resPieceNumbs[i] == 1)
                    {
                        list = xmlDoc.SelectNodes("eventstructure/resultflavor/roomgained/flavor");
                        r = UnityEngine.Random.Range(0, list.Count);
                        resultflavortext += list[r].InnerText;
                    }
                    break;
                case "Cleric Quarters":
                    if ((int)resPieceNumbs[i] == -10)
                    {
                        list = xmlDoc.SelectNodes("eventstructure/resultflavor/roomdestroyed/flavor");
                        r = UnityEngine.Random.Range(0, list.Count);
                        resultflavortext += list[r].InnerText;
                    }
                    else if ((int)resPieceNumbs[i] == -1)
                    {
                        list = xmlDoc.SelectNodes("eventstructure/resultflavor/roomdamaged/flavor");
                        r = UnityEngine.Random.Range(0, list.Count);
                        resultflavortext += list[r].InnerText;
                    }
                    else if ((int)resPieceNumbs[i] == 1)
                    {
                        list = xmlDoc.SelectNodes("eventstructure/resultflavor/roomgained/flavor");
                        r = UnityEngine.Random.Range(0, list.Count);
                        resultflavortext += list[r].InnerText;
                    }
                    break;
                case "Ambassadors Quarters":
                    if ((int)resPieceNumbs[i] == -10)
                    {
                        list = xmlDoc.SelectNodes("eventstructure/resultflavor/roomdestroyed/flavor");
                        r = UnityEngine.Random.Range(0, list.Count);
                        resultflavortext += list[r].InnerText;
                    }
                    else if ((int)resPieceNumbs[i] == -1)
                    {
                        list = xmlDoc.SelectNodes("eventstructure/resultflavor/roomdamaged/flavor");
                        r = UnityEngine.Random.Range(0, list.Count);
                        resultflavortext += list[r].InnerText;
                    }
                    else if ((int)resPieceNumbs[i] == 1)
                    {
                        list = xmlDoc.SelectNodes("eventstructure/resultflavor/roomgained/flavor");
                        r = UnityEngine.Random.Range(0, list.Count);
                        resultflavortext += list[r].InnerText;
                    }
                    break;
                case "RandomRoom":
                    if ((int)resPieceNumbs[i] == -10)
                    {
                        list = xmlDoc.SelectNodes("eventstructure/resultflavor/roomdestroyed/flavor");
                        r = UnityEngine.Random.Range(0, list.Count);
                        resultflavortext += list[r].InnerText;
                    }
                    else if ((int)resPieceNumbs[i] == -1)
                    {
                        list = xmlDoc.SelectNodes("eventstructure/resultflavor/roomdamaged/flavor");
                        r = UnityEngine.Random.Range(0, list.Count);
                        resultflavortext += list[r].InnerText;
                    }
                    else if ((int)resPieceNumbs[i] == 1)
                    {
                        list = xmlDoc.SelectNodes("eventstructure/resultflavor/roomgained/flavor");
                        r = UnityEngine.Random.Range(0, list.Count);
                        resultflavortext += list[r].InnerText;
                    }
                    break;
            }
        }
        int savNumCon = 0;
        foreach (KeyValuePair<Piece, int> pieceNum in savRes.Conditions)
        {
            savNumCon += pieceNum.Value;
        }
        if (savNumCon == 3)
        {
            DataManager.instance.ActiveDiplomaticEvent = savRes;
        }
        DataManager.instance.SavedEvents.Add(savRes);
        resultPanel.transform.FindChild("ResolutionScreen").transform.FindChild("ResolutionText").GetComponent<Text>().text = resultflavortext;
    }
}
