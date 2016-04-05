using UnityEngine;
using System.Collections;
using System.Xml;
using System.IO;
using Assets.Scripts.Utility;
using System.Collections.Generic;

public class Event {
    //XML path
    private string filepath = "assets/scripts/XML/EventNodeMarkup.xml";
    private string eventxmlpath = "assets/scripts/XML/EventSystem.xml";

    //XML gather values
    private string gather = "Events/Gathering";
    private string research = "Events/Research";
    private string diplomacy = "Events/Diplomacy";

    //XML type events
    private string Tbasic = "/basic";
    private string Tsecond = "/second";
    private string Tthird = "/third";

    //Variables
    public string eventText = "";
    public string eventReward = "";

    public List<EventOption> EventOptions { get; set; }
    public List<EventResult> EventConditions
    {
        get; set;
    }

    public Event()
    {
        EventConditions = new List<EventResult>();

        // Test option
        EventOptions = new List<EventOption>();
    }

    public void GenerateEvent(EventSpec eventType)
    {
        //Add PCG to create the different types of events and rewards
        switch (eventType)
        {
            case EventSpec.GATHER:// gather
                //setEventDataFromXml(gather);
                //TODO Set eventText
                eventText = "Gathering Event";
                //Testoption
                //setDataFromXml(EventSpec.GATHER);
                setDataFromXml("gathering");

                break;
            case EventSpec.RESEARCH://research
                //setEventDataFromXml(research);
                //TODO Set eventText
                eventText = "Research Event";
                //Testoption
                //setDataFromXml(EventSpec.RESEARCH);
                setDataFromXml("research");
                break;
            case EventSpec.DIPLOMACY://diplomacy
                //setEventDataFromXml(diplomacy);
                //TODO Set eventText
                eventText = "Diplomacy Event";
                //Testoption
                //setDataFromXml(EventSpec.DIPLOMACY);
                setDataFromXml("diplomacy");
                break;
        }
    }
    /// <summary>
    /// Selects the result of events which are based on an eventlist
    /// </summary>
    /// <param name="nodelist"></param>
    /// <returns></returns>
    private EventResult getEventResult(XmlNodeList nodelist)
    {
        int chance = Random.Range(1, 101);
        EventResult evres = new EventResult();
        int accChance = 0;
        for (int i = 0; i < nodelist.Count; i++)
        {
            int eventchance = int.Parse(nodelist[i].SelectSingleNode("chance").InnerText);
            accChance += eventchance;
            if (accChance >= chance)
            {
                XmlNodeList outcomes = nodelist[i].SelectNodes("eventOutcome/outcome");
                string resString ="";
                for(int j = 0; j<outcomes.Count;j++)
                {
                    string[] ranges = outcomes[j].SelectSingleNode("range").InnerText.Split(',');
                    int amount = Random.Range(int.Parse(ranges[0]), int.Parse(ranges[1]) + 1);
                    resString = amount + " " + renameString(outcomes[j].SelectSingleNode("piece").InnerText);
                    resString += " " + outcomes[j].SelectSingleNode("extraFlavor").InnerText;
                    if(j != outcomes.Count - 1)
                    {
                        resString += ", ";
                    }
                }

                evres.Result = resString; //renameString(nodelist[i].SelectSingleNode("outcome").InnerText);
                evres.ResultText = renameString(nodelist[i].SelectSingleNode("flavourText").InnerText);
                break;
            }
        }
        return evres;
    }

    /// <summary>
    /// Renames boardgame pieces based on their enum name
    /// </summary>
    /// <param name="text"></param>
    /// <returns></returns>
    private string renameString(string text)
    {
        string alterString = text;
        foreach(KeyValuePair<string,Piece> pair in DataManager.instance.BoardPieces)
        {
            alterString = alterString.Replace(pair.Key, pair.Value.BoardName);
        }
        return alterString;
    }

    /// <summary>
    /// Builds events from predefined events
    /// </summary>
    /// <param name="eventType">used for selecting the xml path</param>
    private void setEventDataFromXml(string eventType)
    {
        int numEvents = Random.Range(0, 2) + 2;
        XmlDocument xmlDoc = new XmlDocument();
        xmlDoc.Load(filepath);
        XmlNode basic = xmlDoc.SelectSingleNode(eventType + Tbasic);
        XmlNodeList basicList = basic.SelectNodes("event/results/result");

        EventResult evbasic = getEventResult(basicList);
        evbasic.FlavorText = renameString(basic.SelectSingleNode("event/eventText").InnerText);
        
        EventConditions.Add(evbasic);

        //second
        XmlNode second = xmlDoc.SelectSingleNode(eventType + Tsecond);
        XmlNodeList secondEventList = second.SelectNodes("event");
        // random event
        XmlNode tempevent = secondEventList[Random.Range(0, secondEventList.Count)];
        XmlNodeList secresults = tempevent.SelectNodes("results/result");
        EventResult evsec = getEventResult(secresults);
        evsec.FlavorText = renameString(tempevent.SelectSingleNode("eventText").InnerText);
        EventConditions.Add(evsec);

        //third
        if (numEvents == 3)
        {

            XmlNode gatherThird = xmlDoc.SelectSingleNode(eventType + Tthird);
            XmlNodeList thirdEventList = gatherThird.SelectNodes("event");
            // random event
            tempevent = thirdEventList[Random.Range(0, thirdEventList.Count)];
            XmlNodeList thiresults = tempevent.SelectNodes("results/result");
            EventResult evres = getEventResult(thiresults);
            evres.FlavorText = renameString(tempevent.SelectSingleNode("eventText").InnerText);
            EventConditions.Add(evres);
        }
    }

    /// <summary>
    /// Builds events from blocks of outcomes and conditions
    /// </summary>
    /// <param name="eventtype"></param>
    private void setDataFromXml(EventSpec eventtype)
    {
        XmlDocument xmlDoc = new XmlDocument();
        xmlDoc.Load(eventxmlpath);
        string[] rangeArr;

        // Basic event
        EventOption basic = new EventOption();
        // Send a crew member, basic event
        basic.Conditions.Add(DataManager.instance.BoardPieces["crewMember"], 1);
        //Select common outcome
        XmlNodeList outcom = xmlDoc.SelectNodes("eventSystem/eventOutcome/common/outcome");
        int[] basoutSelect = randomArray(outcom.Count);
        Piece basConPiece = DataManager.instance.BoardPieces[outcom[basoutSelect[0]].SelectSingleNode("piece").InnerText];
        rangeArr = outcom[basoutSelect[0]].SelectSingleNode("range").InnerText.Split(',');
        basic.Results.Add(new EventOutcome(basConPiece, Random.Range(int.Parse(rangeArr[0]), int.Parse(rangeArr[1]))));
        //Select uncommon outcome
        XmlNodeList outuncom = xmlDoc.SelectNodes("eventSystem/eventOutcome/uncommon/outcome");
        basoutSelect = randomArray(outuncom.Count);
        Piece basUnConPiece = DataManager.instance.BoardPieces[outcom[basoutSelect[0]].SelectSingleNode("piece").InnerText];
        rangeArr = outcom[basoutSelect[0]].SelectSingleNode("range").InnerText.Split(',');
        basic.Results.Add(new EventOutcome( basUnConPiece, Random.Range(int.Parse(rangeArr[0]), int.Parse(rangeArr[1]))));

        EventOptions.Add(basic);

        // Second event
        EventOption opt = new EventOption();
        //Get conditions
        //TODO select condition amount based on turns and internal logic

        int numCon = Random.Range(1,4);
        XmlNodeList conditionList = xmlDoc.SelectNodes("eventSystem/eventConditions/condition");
        int[] conSelect = randomArray(conditionList.Count);
        for (int i = 0; i < numCon; i++)
        {
            Piece tempConPiece = DataManager.instance.BoardPieces[conditionList[conSelect[i]].SelectSingleNode("piece").InnerText];
            opt.Conditions.Add(tempConPiece, int.Parse(conditionList[conSelect[i]].SelectSingleNode("amount").InnerText));
        }

        //Get outcomes
        int numOut = Random.Range(1, 4);
        XmlNodeList outcomeList = xmlDoc.SelectNodes("eventSystem/eventOutcome/outcome");
        int[] outSelect = randomArray(outcomeList.Count);
        for (int i = 0; i < numOut; i++)
        {
            Piece tempOutPiece = DataManager.instance.BoardPieces[outcomeList[outSelect[i]].SelectSingleNode("piece").InnerText];
            rangeArr = outcomeList[outSelect[i]].SelectSingleNode("range").InnerText.Split(',');
            opt.Results.Add(new EventOutcome( tempOutPiece, Random.Range(int.Parse(rangeArr[0]), int.Parse(rangeArr[1]))));
        }
        EventOptions.Add(opt);
    }

    /// <summary>
    /// !!!Not working yet!!!
    /// Builds events from chosing conditions then matching outcomes
    /// </summary>
    /// <param name="eventtype"></param>
    private void setDataFromXml(string eventtype)
    {
        XmlDocument xmlDoc = new XmlDocument();
        xmlDoc.Load(eventxmlpath);
        XmlNodeList conditionList = xmlDoc.SelectNodes("eventSystem/eventConditions/condition");
        XmlNodeList outcomesList = xmlDoc.SelectNodes("eventSystem/Outcomes/outcome");

        string[] rangeArr;

        // Basic event
        EventOption basic = new EventOption();
        // Select single event condition
        int[] conBasSelect = randomArray(conditionList.Count);
        Piece basConPiece = DataManager.instance.BoardPieces[conditionList[conBasSelect[0]].SelectSingleNode("piece").InnerText];
        basic.Conditions.Add(basConPiece, int.Parse(conditionList[conBasSelect[0]].SelectSingleNode("amount").InnerText));

        //Select common outcome
        int[] basoutSelect = randomArray(outcomesList.Count);
        for(int i =0; i < basoutSelect.Length; i++)
        {
            XmlNode xn = outcomesList[basoutSelect[i]];
            
            if (xn.SelectSingleNode("type").InnerText == eventtype && xn.SelectSingleNode("rarity").InnerText == "common")
            {
                Piece basOutPiece = DataManager.instance.BoardPieces[xn.SelectSingleNode("piece").InnerText];
                rangeArr = xn.SelectSingleNode("range").InnerText.Split(',');
                basic.Results.Add(new EventOutcome(basOutPiece, Random.Range(int.Parse(rangeArr[0]), int.Parse(rangeArr[1]))));
                break;
            }
        }
        
        //Select uncommon outcome
        basoutSelect = randomArray(outcomesList.Count);
        for (int i = 0; i < basoutSelect.Length; i++)
        {
            XmlNode xn = outcomesList[basoutSelect[i]];
            if (xn.SelectSingleNode("type").InnerText == eventtype && xn.SelectSingleNode("rarity").InnerText == "uncommon")
            {
                Piece basOutPiece = DataManager.instance.BoardPieces[xn.SelectSingleNode("piece").InnerText];
                rangeArr = xn.SelectSingleNode("range").InnerText.Split(',');
                basic.Results.Add(new EventOutcome(basOutPiece, Random.Range(int.Parse(rangeArr[0]), int.Parse(rangeArr[1]))));
                break;
            }
        }

        //################WORK FROM HERE#####################
        EventOptions.Add(basic);

        // Second event
        EventOption opt = new EventOption();
        //Get conditions
        //TODO select condition amount based on turns and internal logic

        int numCon = Random.Range(1, 4);
        int[] conSelect = randomArray(conditionList.Count);
        for (int i = 0; i < numCon; i++)
        {
            Piece tempConPiece = DataManager.instance.BoardPieces[conditionList[conSelect[i]].SelectSingleNode("piece").InnerText];
            opt.Conditions.Add(tempConPiece, int.Parse(conditionList[conSelect[i]].SelectSingleNode("amount").InnerText));
        }

        //Get outcomes
        int numOut = Random.Range(1, 4);
        XmlNodeList outcomeList = xmlDoc.SelectNodes("eventSystem/eventOutcome/outcome");
        int[] outSelect = randomArray(outcomeList.Count);
        for (int i = 0; i < numOut; i++)
        {
            Piece tempOutPiece = DataManager.instance.BoardPieces[outcomeList[outSelect[i]].SelectSingleNode("piece").InnerText];
            rangeArr = outcomeList[outSelect[i]].SelectSingleNode("range").InnerText.Split(',');
            opt.Results.Add(new EventOutcome(tempOutPiece, Random.Range(int.Parse(rangeArr[0]), int.Parse(rangeArr[1]))));
        }
        EventOptions.Add(opt);
    }

    /// <summary>
    /// Randomizes the array 
    /// </summary>
    /// <param name="size"></param>
    /// <returns></returns>
    private int[] randomArray(int size)
    {
        int[] randomArr = new int[size];
        for(int i=0; i < randomArr.Length; i++)
        {
            randomArr[i] = i;
        }
        for(int i = 0; i < randomArr.Length; i++)
        {
            int random = Random.Range(0, randomArr.Length);
            int temp = randomArr[random];
            randomArr[random] = randomArr[i];
            randomArr[i] = temp;
        }
        return randomArr;
    }

    private void generateConditionFlavor(XmlDocument doc, int condNum)
    {
        string flav = "";
        XmlNodeList condList = doc.SelectNodes("eventSystem/eventConditions/condition");
        XmlNodeList flavList = condList[condNum].SelectNodes("flavors");
        int r = Random.Range(0, flavList.Count);
        flav = flavList[r].InnerText;
        Debug.Log("Flavor text found: " + flav);
    }
}
