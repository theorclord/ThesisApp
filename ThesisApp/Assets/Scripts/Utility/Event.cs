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

    public List<EventResult> EventConditions
    {
        get; set;
    }

    public Event()
    {
        EventConditions = new List<EventResult>();
    }
    

    public void GenerateEvent(EventSpec eventType)
    {
        //Add PCG to create the different types of events and rewards
        switch (eventType)
        {
            case EventSpec.GATHER:// gather
                setDataFromXml(EventSpec.GATHER);
                setEventDataFromXml(gather);
                //TODO Set eventText
                eventText = "Gathering Event";
                break;
            case EventSpec.RESEARCH://research
                setDataFromXml(EventSpec.RESEARCH);
                setEventDataFromXml(research);
                //TODO Set eventText
                eventText = "Research Event";
                break;
            case EventSpec.DIPLOMACY://diplomacy
                setDataFromXml(EventSpec.DIPLOMACY);
                setEventDataFromXml(diplomacy);
                //TODO Set eventText
                eventText = "Diplomacy Event";
                break;
            /*case 3:
                eventText = "Trading Post";
                break;*/
        }
    }
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
    /// Gets the data for the event based on its type
    /// </summary>
    /// <param name="eventType"></param>
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

    private void setDataFromXml(EventSpec eventtype)
    {
        XmlDocument xmlDoc = new XmlDocument();
        xmlDoc.Load(eventxmlpath);
        EventOption opt = new EventOption();
        //Get conditions
        XmlNodeList conditionList = xmlDoc.SelectNodes("eventSystem/eventConditions/condition");
        int conSelect = Random.Range(0, conditionList.Count);
        Piece tempPiece = DataManager.instance.BoardPieces[conditionList[conSelect].SelectSingleNode("piece").InnerText];
        opt.Conditions.Add(tempPiece, int.Parse( conditionList[conSelect].SelectSingleNode("amount").InnerText));

        //Get outcomes
        XmlNodeList outcomeList = xmlDoc.SelectNodes("eventSystem/eventOutcome/outcome");
        int outSelect = Random.Range(0, outcomeList.Count);

        //get pieces?
        generateConditionFlavor(xmlDoc, conSelect);

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
