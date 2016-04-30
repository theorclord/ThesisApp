using UnityEngine;
using System.Collections;
using System.Xml;
using System.IO;
using Assets.Scripts.Utility;
using System.Collections.Generic;

public class Event {
    //XML path
    private string eventstructurepath = "assets/scripts/XML/EventStructure.xml";
    private string eventconditions = "eventstructure/conditions/condition";
    public int locType { get; set;}  
    // Crit val
    private int crit = 75;
    //XML gather values
    //private string gather = "Events/Gathering";
    //private string research = "Events/Research";
    //private string diplomacy = "Events/Diplomacy";

    //XML type events
    //private string Tbasic = "/basic";
    //private string Tsecond = "/second";
    //private string Tthird = "/third";

    //Variables
    public string eventText = "";
    public string eventReward = "";
    public string entryFlavor = "";
    public string locationXmlString { get; set; }

    public List<EventOption> EventOptions { get; set; }
    public void SetLocType(int loc)
    {
        locType = loc;
    }
    public Event()
    {
        EventOptions = new List<EventOption>();
    }

    public void GenerateEvent(EventSpec eventType, int NumberOptions)
    {
        //Add PCG to create the different types of events and rewards
        switch (eventType)
        {
            case EventSpec.GATHER:// gather
                //TODO Set eventText
                eventText = "Gathering Event";
                buildDataFromXml("gathering", NumberOptions);
                break;
            case EventSpec.RESEARCH://research
                //TODO Set eventText
                eventText = "Research Event";
                buildDataFromXml("research", NumberOptions);
                //buildDataFromXml("gathering");
                break;
                /*
            case EventSpec.DIPLOMACY://diplomacy
                //TODO Set eventText
                eventText = "Diplomacy Event";
                buildDataFromXml("gathering");
                break;
                */
        }
    }
    
    /// <summary>
    /// Builds events based on creation rules
    /// </summary>
    /// <param name="eventtype">string type, related to the xml file</param>
    private void buildDataFromXml(string eventtype, int numOpt)
    {
        for(int i = 0; i < numOpt; i++)
        {
            EventOptions.Add(generateEvent(i, i+1, eventtype, locType));
        }
    }
    /// <summary>
    /// Generates the content of the events based on the condition
    /// </summary>
    /// <param name="conType">Type of conditions, 0 for required, 1 for basic, 2 for advanced</param>
    /// <param name="numCon">Number of conditions</param>
    /// <param name="eventtype">The type of event, gathering, diplomacy, research</param>
    /// <returns></returns>
    private EventOption generateEvent(int conType, int numCon, string eventtype, int location)
    {
        // test vars
        //int conType = 0;
        //int numCon = 1;
        //int numOut = 1;
        //string eventtype = "gathering";
        //string eventLevel = "basic";
        XmlDocument xmlDoc = new XmlDocument();
        xmlDoc.Load(eventstructurepath);
        XmlNodeList conditionList = xmlDoc.SelectNodes(eventconditions);

        //Event options
        EventOption evOpt = new EventOption();

        // add crew member condition
        for (int i = 0; i < conditionList.Count; i++)
        {
            XmlNode xn = conditionList[i];
            if (int.Parse(xn.SelectSingleNode("type").InnerText) <= 0)
            {
                Piece basPiece = DataManager.instance.BoardPieces[xn.SelectSingleNode("piece").InnerText];
                if (evOpt.Conditions.ContainsKey(basPiece))
                {
                    evOpt.Conditions[basPiece] += int.Parse(xn.SelectSingleNode("amount").InnerText);
                }
                else
                {
                    evOpt.Conditions.Add(basPiece, int.Parse(xn.SelectSingleNode("amount").InnerText));
                }
                break;
            }
        }
        bool roomAdded = false;
        // Select event conditions
        for (int j = 0; j < numCon-1; j++)
        {   
            int[] conSelect = DataManager.randomArray(conditionList.Count);
            for (int i = 0; i < conditionList.Count; i++)
            {
                bool roomExist = false;
                XmlNode xn = conditionList[conSelect[i]];
                //Select a condition which is limited by the condition type parameter   
                if (int.Parse(xn.SelectSingleNode("type").InnerText) <= conType)
                {
                    Piece basPiece = DataManager.instance.BoardPieces[xn.SelectSingleNode("piece").InnerText];
                    if( !roomAdded && conType == 2 && basPiece.Type != BoardType.ROOM )
                    {
                        continue;
                    }
                    roomAdded = true;
                    // ensures there is only one room.
                    if(basPiece.Type == BoardType.ROOM)
                    {
                        foreach(KeyValuePair<Piece, int> pieceNum in evOpt.Conditions)
                        {
                            if(pieceNum.Key.Type == BoardType.ROOM)
                            {
                                roomExist = true;
                            }
                        }
                        if (roomExist)
                        {
                            continue;
                        }
                    }
                    if (evOpt.Conditions.ContainsKey(basPiece))
                    {
                        evOpt.Conditions[basPiece] += int.Parse(xn.SelectSingleNode("amount").InnerText);
                    }
                    else
                    {
                        evOpt.Conditions.Add(basPiece, int.Parse(xn.SelectSingleNode("amount").InnerText));
                    }
                    break;
                }
            }
        }
        int conSelectnum = numCon;
        int addedCons = 0;

        Dictionary<Piece, int> usedConds = new Dictionary<Piece, int>();

        //Select outcomes
        while (addedCons < numCon )
        {
            //numRuns++;
            string numberString = "";
            switch (conSelectnum)
            {
                case 2:
                    numberString = "/two";
                    break;
                case 1:
                    numberString = "/one";
                    break;
                case 3:
                    numberString = "/three";
                    break;
            }
            //numberString = "/one";

            XmlNodeList Outcome = xmlDoc.SelectNodes("eventstructure/" + eventtype + numberString + "/outcome");
            int[] outSelect = DataManager.randomArray(Outcome.Count);
            for (int i = 0; i < outSelect.Length; i++)
            {
                XmlNode xn = Outcome[outSelect[i]];
                XmlNodeList xnConlist = xn.SelectNodes("conditions/condition");
                bool found = true;

                for (int j = 0; j < xnConlist.Count; j++)
                {
                    XmlNode xnCon = xnConlist[j];

                    Piece conPiece = DataManager.instance.BoardPieces[xnCon.SelectSingleNode("piece").InnerText];
                    if (!evOpt.Conditions.ContainsKey(conPiece) ||
                        evOpt.Conditions[conPiece] < int.Parse(xnCon.SelectSingleNode("amount").InnerText))
                    {
                        found = false;
                        break;
                    }
                    if(usedConds.ContainsKey(conPiece) && !(evOpt.Conditions[conPiece]-usedConds[conPiece] >= int.Parse(xnCon.SelectSingleNode("amount").InnerText)))
                    {
                        found = false;
                        break;
                    }
                }
                if (found)
                {
                    for(int j = 0; j<xnConlist.Count; j++)
                    {
                        XmlNode xnCon = xnConlist[j];
                        addedCons = addedCons + int.Parse(xnCon.SelectSingleNode("amount").InnerText);
                        Piece conPiece = DataManager.instance.BoardPieces[xnCon.SelectSingleNode("piece").InnerText];
                        if (usedConds.ContainsKey(conPiece))
                        {
                            usedConds[conPiece] += int.Parse(xnCon.SelectSingleNode("amount").InnerText);
                        } else
                        {
                            usedConds.Add(conPiece, int.Parse(xnCon.SelectSingleNode("amount").InnerText));
                        }
                    }
                    
                    string critString = "/normal";
                    if ((Random.Range(0, 100) + 1) > crit)
                    {
                        critString = "/critical";
                    }
                    EventOutcomeGroup evgroup = new EventOutcomeGroup();
                    evgroup.Outcomes.Add(generateOutcome(xn, "success", critString, EventOutcomeType.SUCCESS));
                    evgroup.Outcomes.Add(generateOutcome(xn, "neutral", critString, EventOutcomeType.NEUTRAL));
                    evgroup.Outcomes.Add(generateOutcome(xn, "failure", critString, EventOutcomeType.FAILURE));
                    evOpt.Results.Add(evgroup);
                    break;
                }
            }
            //Debug.Log("Selected part " + conSelectnum);
            //Debug.Log("Added conditions " +addedCons);
            if( conSelectnum != 1)
            {
                conSelectnum--;
            }
        }
        //Set flavors
        switch (eventtype)
        {
            case "gathering":
                switch (location)
                {
                    case 0:
                        locationXmlString = "mine";
                        evOpt.locType = Location.MINE;
                        break;
                    case 1:
                        locationXmlString = "quarry";
                        evOpt.locType = Location.QUARRY;
                        break;
                    case 2:
                        locationXmlString = "wreckage";
                        evOpt.locType = Location.WRECKAGE;
                        break;
                    case 3:
                        locationXmlString = "factory";
                        evOpt.locType = Location.FACTORY;
                        break;
                    case 4:
                        locationXmlString = "village";
                        evOpt.locType = Location.VILLAGE;
                        break;
                }
                break;

            case "research":
                switch (location)
                {
                    case 0:
                        locationXmlString = "forest";
                        evOpt.locType = Location.FOREST;
                        break;
                    case 1:
                        locationXmlString = "rockformation";
                        evOpt.locType = Location.ROCKFORMATION;
                        break;
                    case 2:
                        locationXmlString = "magicsite";
                        evOpt.locType = Location.MAGICSITE;
                        break;
                    case 3:
                        locationXmlString = "lake";
                        evOpt.locType = Location.LAKE;
                        break;
                    case 4:
                        locationXmlString = "ruins";
                        evOpt.locType = Location.RUINS;
                        break;
                }
                break;
        }
        evOpt.locationXmlString = locationXmlString;
        XmlNodeList entryflavs = xmlDoc.SelectNodes("eventstructure/introflavor/" + eventtype + "/"+ locationXmlString + "/flavor");
        int[] flSel = DataManager.randomArray(entryflavs.Count);
        entryFlavor = entryflavs[flSel[0]].InnerText;
        return evOpt;
    }

    private EventOutcome generateOutcome(XmlNode selectedNode, string typepath, string crit, EventOutcomeType type)
    {
        EventOutcome evo = new EventOutcome();
        
        XmlNodeList outOptions = selectedNode.SelectNodes(typepath + crit + "/option");
        int[] optArr = DataManager.randomArray(outOptions.Count);
        //Debug.Log(selectedNode.LocalName+typepath + crit+"/option");//"size of Array(research):" + optArr.Length);
        XmlNodeList pieces = outOptions[optArr[0]].SelectNodes("piece");
        evo.Chance = int.Parse(selectedNode.SelectSingleNode(typepath).Attributes["chance"].Value);
        evo.Type = type;
        foreach (XmlNode piece in pieces)
        {
            Piece outPiece = DataManager.instance.BoardPieces[piece.SelectSingleNode("name").InnerText];
            string[] rangestr = piece.SelectSingleNode("range").InnerText.Split(',');
            int[] range = new int[] { int.Parse(rangestr[0]), int.Parse(rangestr[1]) };
            evo.AddPiece(outPiece, range);
        }

        // Fetching Result Flavor text
       /* XmlNodeList resFlavors = outOptions[optArr[0]].SelectSingleNode("flavors").SelectNodes("flavor");
        int[] flavArr = DataManager.randomArray(resFlavors.Count);
        string resFlavText = resFlavors[flavArr[0]].InnerText;
        evo.outcomeFlavor = resFlavText;*/

        return evo;
    }
}
