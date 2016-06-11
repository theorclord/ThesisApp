using UnityEngine;
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

    public SpecialResults fight = new SpecialResults();
    private bool specEv = false;

    //private SpecialResults sr = new SpecialResults();
    private string allegiance1 = "";
    private string allegiance2 = "";
    private string locationButton1 = "";
    private string locationButton2 = "";
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
        
        GameObject.Find("FactionPanel").transform.FindChild("Faction").FindChild("FactionIcon").GetComponent<Image>().sprite = 
            Resources.Load("BoardMarkers/"+DataManager.instance.ActiveNode.NodeFaction.BoardName, typeof(Sprite)) as Sprite;

        GameObject.Find("FactionPanel").transform.FindChild("TextBox").FindChild("FactionText").GetComponent<Text>().text = 
            DataManager.instance.ActiveNode.NodeFaction.BoardName + " controlled";
        // Loading preevents if conditions are right
        // Either Nomad:Wreckage/Village, Human:Factory/Mine, Highbourne:Forest/MagicSite

        if (DataManager.instance.ActiveDiplomaticEvent != null && 
            DataManager.instance.TurnCounter >= DataManager.instance.ActiveDiplomaticEvent.TurnCount + 2)
        {
            Debug.Log("Special hit");
            if (CheckFactionLocation())
            {
                Debug.Log("Special event");
                InitSpecialEvent();
                DataManager.instance.specialActive = true;

            }
            else
            {
                int a = UnityEngine.Random.Range(0, 5);
                if (a == 0)
                {
                    Debug.Log("Other event");
                    pickOtherSpecial();
                    DataManager.instance.specialActive = true;

                }
            }
        }
    }

    private void pickOtherSpecial()
    {
        Debug.Log("Picking Fight Event");
        fight = new SpecialResults();
        specEv = true;
        specialPanel.SetActive(true);
        XmlDocument doc = new XmlDocument();
        TextAsset textAsset = Resources.Load("XML/SpecialEvents", typeof(TextAsset)) as TextAsset;
        doc.LoadXml(textAsset.text);
        XmlNodeList fights = doc.SelectNodes("specialEvents/fightevent");
        int a = UnityEngine.Random.Range(0, fights.Count);
        string faction1 = fights[a].SelectSingleNode("faction1").InnerText;
        string faction2 = fights[a].SelectSingleNode("faction2").InnerText;
        string intro = fights[a].SelectSingleNode("intro").InnerText;
        string body = fights[a].SelectSingleNode("body").InnerText;
        string extra = fights[a].SelectSingleNode("extra").InnerText;
        string eventflavor = intro + faction1 + body + faction2 + extra;
        string button1text = fights[a].SelectSingleNode("button").InnerText + faction1;
        string button2text = fights[a].SelectSingleNode("button").InnerText + faction2;
        string button3text = "Leave the island and let them settle their own fight.";
        string resultflavor = fights[a].SelectSingleNode("text").InnerText;
        int repvalue = int.Parse(fights[a].SelectSingleNode("value").InnerText);
        fight.neutral = int.Parse(fights[a].SelectSingleNode("neutral").InnerText);
        fight.reputations.Add(faction1, repvalue);
        fight.reputations.Add(faction2, repvalue);
        fight.resultflavor = resultflavor;
        // set the flavors
        specialPanel.transform.FindChild("FlavourScreen").FindChild("EventText").GetComponent<Text>().text = eventflavor;
        specialPanel.transform.FindChild("ButtonController").GetChild(0).FindChild("Text").GetComponent<Text>().text = button1text;
        specialPanel.transform.FindChild("ButtonController").GetChild(1).FindChild("Text").GetComponent<Text>().text = button2text;
        specialPanel.transform.FindChild("ButtonController").GetChild(2).gameObject.SetActive(true);
        specialPanel.transform.FindChild("ButtonController").GetChild(2).FindChild("Text").GetComponent<Text>().text = button3text;
        
    }

    void InitSpecialEvent()
    {
        Debug.Log("The Special event runs");
        //Load special event scene
        specialPanel.SetActive(true);
        //LOAD XML
        XmlDocument doc = new XmlDocument();
        TextAsset textAsset = Resources.Load("XML/SpecialEvents", typeof(TextAsset)) as TextAsset;
        doc.LoadXml(textAsset.text);

        //sr = new SpecialResults();

        /**
        get the event
            name, locationtype, faction
        */
        string PrevLocName = DataManager.instance.ActiveDiplomaticEvent.IslandName;
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
        locationButton1 = "specialEvents/combination[@type='" + combination + "']/options/one" + superAllegiance + "/results/result";
        locationButton2 = "specialEvents/combination[@type='" + combination + "']/options/two" + superAllegiance + "/results/result";
        Debug.Log("size of combination: " + combs.Count);
        foreach (XmlNode xn in combs)
        {
            intro = xn.SelectSingleNode("intro").InnerText;
            body = xn.SelectSingleNode("body").InnerText;
            extra = xn.SelectSingleNode("extra").InnerText;
            button1 = xn.SelectSingleNode("options/one/button").InnerText;
            button2 = xn.SelectSingleNode("options/two/button").InnerText;
            allegiance1 = xn.SelectSingleNode("options/one" + superAllegiance + "/text").InnerText;
            allegiance2 = xn.SelectSingleNode("options/two" + superAllegiance + "/text").InnerText;
        }
        
        specialPanel.transform.FindChild("FlavourScreen").FindChild("EventText").GetComponent<Text>().text = intro + PrevLocName + body + extra;
        specialPanel.transform.FindChild("ButtonController").GetChild(0).FindChild("Text").GetComponent<Text>().text = button1;
        specialPanel.transform.FindChild("ButtonController").GetChild(1).FindChild("Text").GetComponent<Text>().text = button2;


    }

    public void buttonOneClicked()
    {
        SpecialResults sr2 = new SpecialResults();
        XmlDocument doc = new XmlDocument();
        TextAsset textAsset = Resources.Load("XML/SpecialEvents", typeof(TextAsset)) as TextAsset;
        doc.LoadXml(textAsset.text);
        GameObject[] nodes = GameObject.FindGameObjectsWithTag("LocationNode");
        for (int i = 0; i < nodes.Length; i++)
        {
            nodes[i].SetActive(false);
        }
        specialPanel.SetActive(false);
        resultPanel.SetActive(true);
        if (!specEv)
        {
            //GameObject.Find("Canvas").gameObject.transform.FindChild("EventPanel").gameObject;
            resultPanel.transform.FindChild("ResolutionScreen").FindChild("ResolutionText").GetComponent<Text>().text = allegiance1;
            XmlNodeList results = doc.SelectNodes(locationButton1);
            foreach (XmlNode xn in results)
            {
                int value = int.Parse(xn.SelectSingleNode("value").InnerText);
                string name = xn.SelectSingleNode("name").InnerText;
                Debug.Log(xn.SelectSingleNode("type").InnerText);
                switch (xn.SelectSingleNode("type").InnerText)
                {
                    case "reputation":
                        sr2.reputations.Add(name, value);
                        break;
                    case "resource":
                        sr2.resources.Add(name, value);
                        break;
                    case "room":
                        sr2.rooms.Add(name, value);
                        break;
                }
            }
            resultPanel.transform.FindChild("OutcomeScreen").FindChild("Outcome").GetComponent<Text>().text = getFlavor(sr2);

        }
        else
        {
            
            string outcomeText = "";
            int index = 0;
            foreach (string s in fight.reputations.Keys)
            {
                Debug.Log(s);
                if (index == 0)
                {
                    outcomeText += s + ": +" + fight.reputations[s] + "\n";
                    DataManager.instance.Player.AddReputation(DataManager.instance.Factions[s], fight.reputations[s]);
                    resultPanel.transform.FindChild("ResolutionScreen").FindChild("ResolutionText").GetComponent<Text>().text = "The "+ s + " " +fight.resultflavor;
                    index++;
                }
                else
                {
                    DataManager.instance.Player.AddReputation(DataManager.instance.Factions[s], (-1)*fight.reputations[s]);
                    outcomeText += s + ": -" + fight.reputations[s];
                    index++;
                }
            }

            resultPanel.transform.FindChild("OutcomeScreen").FindChild("Outcome").GetComponent<Text>().text = outcomeText;
            fight = null;
        }
        DataManager.instance.ActiveDiplomaticEvent = null;// new SavedResult();
        DataManager.instance.specialActive = false;
    }

    public void buttonTwoClicked()
    {
        SpecialResults sr2 = new SpecialResults();
        XmlDocument doc = new XmlDocument();
        TextAsset textAsset = Resources.Load("XML/SpecialEvents", typeof(TextAsset)) as TextAsset;
        doc.LoadXml(textAsset.text);
        GameObject[] nodes = GameObject.FindGameObjectsWithTag("LocationNode");
        for (int i = 0; i < nodes.Length; i++)
        {
            nodes[i].SetActive(false);
        }
        specialPanel.SetActive(false);
        resultPanel.SetActive(true);
        if (!specEv)
        {
            resultPanel.transform.FindChild("ResolutionScreen").FindChild("ResolutionText").GetComponent<Text>().text = allegiance2;
            XmlNodeList results = doc.SelectNodes(locationButton2);
            Debug.Log("Size of results: " + results.Count);
            foreach (XmlNode xn in results)
            {
                int value = int.Parse(xn.SelectSingleNode("value").InnerText);
                string name = xn.SelectSingleNode("name").InnerText;
                Debug.Log(xn.SelectSingleNode("type").InnerText);
                switch (xn.SelectSingleNode("type").InnerText)
                {
                    case "reputation":
                        sr2.reputations.Add(name, value);
                        break;
                    case "resource":
                        sr2.resources.Add(name, value);
                        break;
                    case "room":
                        sr2.rooms.Add(name, value);
                        break;
                }
            }
            resultPanel.transform.FindChild("OutcomeScreen").FindChild("Outcome").GetComponent<Text>().text = getFlavor(sr2);
            
        }
        else
        {
            resultPanel.transform.FindChild("ResolutionScreen").FindChild("ResolutionText").GetComponent<Text>().text = fight.resultflavor;
            string outcomeText = "";
            int index = 0;
            foreach (string s in fight.reputations.Keys)
            {
                if (index == 0)
                {
                    outcomeText += s + ": -" + fight.reputations[s] + "\n";
                    DataManager.instance.Player.AddReputation(DataManager.instance.Factions[s], (-1)*fight.reputations[s]);
                    index++;
                }
                else
                {
                    outcomeText += s + ": +" + fight.reputations[s];
                    DataManager.instance.Player.AddReputation(DataManager.instance.Factions[s], fight.reputations[s]);
                    resultPanel.transform.FindChild("ResolutionScreen").FindChild("ResolutionText").GetComponent<Text>().text = "The " + s + " " + fight.resultflavor;
                    index++;
                }
            }

            resultPanel.transform.FindChild("OutcomeScreen").FindChild("Outcome").GetComponent<Text>().text = outcomeText;
            fight = null;
        }
        DataManager.instance.ActiveDiplomaticEvent = null;// new SavedResult() ;
        DataManager.instance.specialActive = false;
    }

    public void leaveButtonClicked()
    {
        SpecialResults sr2 = new SpecialResults();
        XmlDocument doc = new XmlDocument();
        TextAsset textAsset = Resources.Load("XML/SpecialEvents", typeof(TextAsset)) as TextAsset;
        doc.LoadXml(textAsset.text);
        GameObject[] nodes = GameObject.FindGameObjectsWithTag("LocationNode");
        for (int i = 0; i < nodes.Length; i++)
        {
            nodes[i].SetActive(false);
        }
        specialPanel.SetActive(false);
        resultPanel.SetActive(true);
        if (!specEv)
        {
            resultPanel.transform.FindChild("ResolutionScreen").FindChild("ResolutionText").GetComponent<Text>().text = allegiance2;
            XmlNodeList results = doc.SelectNodes(locationButton2);
            Debug.Log("Size of results: " + results.Count);
            foreach (XmlNode xn in results)
            {
                int value = int.Parse(xn.SelectSingleNode("neutral").InnerText);
                string name = xn.SelectSingleNode("name").InnerText;
                Debug.Log(xn.SelectSingleNode("type").InnerText);
                switch (xn.SelectSingleNode("type").InnerText)
                {
                    case "reputation":
                        sr2.reputations.Add(name, value);
                        break;
                    case "resource":
                        sr2.resources.Add(name, value);
                        break;
                    case "room":
                        sr2.rooms.Add(name, value);
                        break;
                }
            }
            resultPanel.transform.FindChild("OutcomeScreen").FindChild("Outcome").GetComponent<Text>().text = getFlavor(sr2);

        }
        else
        {
            resultPanel.transform.FindChild("ResolutionScreen").FindChild("ResolutionText").GetComponent<Text>().text = fight.resultflavor;
            string outcomeText = "";
            foreach (string s in fight.reputations.Keys)
            {
                outcomeText += s + ": -" + fight.neutral + "\n";
                DataManager.instance.Player.AddReputation(DataManager.instance.Factions[s], (-1) * fight.neutral);
            }
            resultPanel.transform.FindChild("ResolutionScreen").FindChild("ResolutionText").GetComponent<Text>().text = "You leave without interfering.";
            resultPanel.transform.FindChild("OutcomeScreen").FindChild("Outcome").GetComponent<Text>().text = outcomeText;
            fight = null;
        }
        DataManager.instance.ActiveDiplomaticEvent = null;// new SavedResult() ;
        DataManager.instance.specialActive = false;
    }

    private string getFlavor(SpecialResults sr)
    {
        string res = "";
        foreach (string k in sr.reputations.Keys)
        {
            res += "Reptutation change: " + k + ": " + sr.reputations[k] + "\n";
            DataManager.instance.Player.AddReputation(DataManager.instance.Factions[k], sr.reputations[k]);
        }
        foreach (string k in sr.resources.Keys)
        {
            if (k == "crewMember")
            {
                switch (sr.resources[k])
                {
                    case 1:
                        res += "Crew gained: injured." + "\n";
                        break;
                    case 10:
                        res += "Crew gained: healthy" + "\n";
                        break;
                    case -1:
                        res += "Crew injured.\n";
                        break;
                    case -10:
                        res += "Castle crew dead.\n";
                        break;
                }
            }
            else
            {
                string name = "";
                switch (k)
                {
                    case "sciencePoint":
                        name = "Alchemy Point";
                        break;
                    case "energyPoint":
                        name = "Crystal Charge";
                        break;
                    case "scrap":
                        name = "Building Material";
                        break;
                }
                res += "Resource gained: " + name + ": " + sr.resources[k] + "\n";
            }
        }
        foreach (string k in sr.rooms.Keys)
        {
            string roomName = "";
            string roomRes = "";
            if (k == "randomRoom")
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
                Piece outPiece = tempRooms[roomOrder[0]];
                roomName = outPiece.BoardName;
            }
            else if (k == "labRoom")
            {
                roomName = "Alchemy Lab";
            }
            switch (sr.rooms[k])
            {
                case 1:
                    roomRes = " Created.";
                    break;
                case -1:
                    roomRes = " Damaged.";
                    break;
                case -10:
                    roomRes = " Destroyed.";
                    break;
            }

            res += "Room change: " + roomName + roomRes + "\n";
        }
        return res;
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
            if (hit.collider != null && !DataManager.instance.specialActive)
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
        }

    }

    private void showLocationInfo(GameObject selected)
    {
        selectNode = selected.GetComponent<NodeNode>();
        switch (selectNode.type)
        {
            case EventSpec.GATHER:
                nodeInfoPanel.transform.FindChild("TypeIcon").GetComponent<Image>().sprite = Resources.Load("BoardMarkers/GatherCardIcon", typeof(Sprite)) as Sprite;
                break;
            case EventSpec.RESEARCH:
                nodeInfoPanel.transform.FindChild("TypeIcon").GetComponent<Image>().sprite = Resources.Load("BoardMarkers/AlchemyCardIcon", typeof(Sprite)) as Sprite;
                break;
        }
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
        int thirdChance = 50;
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
            TextAsset textAsset = Resources.Load("XML/Location", typeof(TextAsset)) as TextAsset;
            LocationData.LoadXml(textAsset.text);
            XmlNodeList locations = LocationData.SelectNodes("locations/location");
            // Set information of location based on nodes
            foreach (XmlNode location in locations)
            {
                if (location.SelectSingleNode("name").InnerText == locationString)
                {
                    //Visual node 
                    string[] coord = location.SelectSingleNode("coordinates").InnerText.Split(',');
                    Vector3 locPos = new Vector3(float.Parse(coord[0]), float.Parse(coord[1]));
                    GameObject VisualnodeObj = Instantiate(Resources.Load("Prefabs/Node2D"), locPos, Quaternion.identity) as GameObject;
                    Vector3 nodeObjPos = new Vector3(locPos.x, locPos.y + 1.5f);
                    string[] scale = location.SelectSingleNode("size").InnerText.Split(',');
                    VisualnodeObj.transform.localScale = new Vector3(float.Parse(scale[0]),float.Parse(scale[1]));
                    VisualnodeObj.GetComponent<SpriteRenderer>().sprite = Resources.Load("LocationSprite/" + location.SelectSingleNode("file").InnerText, typeof (Sprite)) as Sprite;
                    //Selection node obj
                    nodeObj = Instantiate(Resources.Load("Prefabs/NodeIcon"), nodeObjPos, Quaternion.identity) as GameObject;
                    string iconString = "";
                    switch (nodestat.type)
                    {
                        case EventSpec.GATHER:
                            iconString = "ExploreIcon";
                            break;
                        case EventSpec.RESEARCH:
                            iconString = "AlchemyIcon";
                            break;
                    }
                    nodeObj.GetComponent<SpriteRenderer>().sprite = Resources.Load("BoardMarkers/" + iconString, typeof(Sprite)) as Sprite;
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
        for(int i = 0; i < buttoncont.childCount; i++)
        {
            buttoncont.GetChild(i).gameObject.SetActive(false);
        }
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
            TextAsset textAsset = Resources.Load("XML/EventStructure", typeof(TextAsset)) as TextAsset;
            xmlDoc.LoadXml(textAsset.text);
            int conditionCount = 0;
            foreach (KeyValuePair<Piece, int> pair in e.EventOptions[tempint].Conditions)
            {
                for(int k =0; k < pair.Value; k++)
                {
                    //Instantiate image of condition. height same as button. start pos 415
                    GameObject resourceIcon = Instantiate(Resources.Load("Prefabs/ResourcesIcon") as GameObject);
                    resourceIcon.transform.SetParent(buttoncont);
                    resourceIcon.transform.localPosition = new Vector3(415f + 65f * (conditionCount), -55 * i);
                    switch (pair.Key.BoardName)
                    {
                        case "Castle Crew":
                            resourceIcon.GetComponent<Image>().sprite = Resources.Load("BoardMarkers/CM generic", typeof(Sprite)) as Sprite;
                            break;
                        case "Building Material":
                            resourceIcon.GetComponent<Image>().sprite = Resources.Load("BoardMarkers/scrap", typeof(Sprite)) as Sprite;
                            break;
                        case "Crystal Charge":
                            resourceIcon.GetComponent<Image>().sprite = Resources.Load("BoardMarkers/CrystalPiece", typeof(Sprite)) as Sprite;
                            break;
                        case "Alchemical Material":
                            resourceIcon.GetComponent<Image>().sprite = Resources.Load("BoardMarkers/AlchemyPoint", typeof(Sprite)) as Sprite;
                            break;
                        case "Miners Guild":
                            resourceIcon.GetComponent<Image>().sprite = Resources.Load("BoardMarkers/MineMaster", typeof(Sprite)) as Sprite;
                            break;
                        case "Cleric Quarters":
                            resourceIcon.GetComponent<Image>().sprite = Resources.Load("BoardMarkers/PriestMaster", typeof(Sprite)) as Sprite;
                            break;
                        case "Workshop":
                            resourceIcon.GetComponent<Image>().sprite = Resources.Load("BoardMarkers/MechMaster", typeof(Sprite)) as Sprite;
                            break;
                        case "Alchemical Lab":
                            resourceIcon.GetComponent<Image>().sprite = Resources.Load("BoardMarkers/AlchemyMaster", typeof(Sprite)) as Sprite;
                            break;
                        default:
                            break;
                    }
                    conditionCount++;
                }

                // Make button text
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
                //Debug.Log("Location: " + e.EventOptions[tempint].locationXmlString);
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
        savRes.IslandName = DataManager.instance.ActiveNode.WorldName;// TitleName;
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
        TextAsset textAsset = Resources.Load("XML/EventStructure", typeof(TextAsset)) as TextAsset;
        xmlDoc.LoadXml(textAsset.text);
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
        //DataManager.instance.ActiveDiplomaticEvent == null && 
        if (savNumCon == 3 && DataManager.instance.ActiveDiplomaticEvent == null)
        {
            DataManager.instance.ActiveDiplomaticEvent = savRes;
        }
        DataManager.instance.SavedEvents.Add(savRes);
        resultPanel.transform.FindChild("ResolutionScreen").transform.FindChild("ResolutionText").GetComponent<Text>().text = resultflavortext;
    }
}
