using UnityEngine;
using System.Collections;
using System.Xml;
using System.IO;
using Assets.Scripts.Utility;
using System.Collections.Generic;

public class Event {
    //XML path
    private string filepath = "assets/scripts/XML/EventNodeMarkup.xml";

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

    //TODO find use for enum
    //private EventSpec eventSpec;

    public List<EventResult> EventConditions
    {
        get; set;
    }

    public Event()
    {
        EventConditions = new List<EventResult>();
    }
    

    public void GenerateEvent(int eventNumber)
    {
        //Add PCG to create the different types of events and rewards
        switch (eventNumber)
        {
            case 0:// gather
                setEventDataFromXml(gather);
                //TODO Set eventText
                eventText = "Gathering Event";
                break;
            case 1://research
                setEventDataFromXml(research);
                //TODO Set eventText
                eventText = "Research Event";
                break;
            case 2://diplomacy
                setEventDataFromXml(diplomacy);
                //TODO Set eventText
                eventText = "Diplomacy Event";
                break;
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
                evres.Result = nodelist[i].SelectSingleNode("outcome").InnerText;
                evres.ResultText = nodelist[i].SelectSingleNode("flavourText").InnerText;
                break;
            }
        }
        return evres;
    }

    private void setEventDataFromXml(string eventType)
    {
        int numEvents = Random.Range(0, 2) + 2;
        XmlDocument xmlDoc = new XmlDocument();
        xmlDoc.Load(filepath);
        XmlNode basic = xmlDoc.SelectSingleNode(eventType + Tbasic);
        XmlNodeList basicList = basic.SelectNodes("event/results/result");

        EventResult evbasic = getEventResult(basicList);
        evbasic.FlavorText = basic.SelectSingleNode("event/eventText").InnerText;
        EventConditions.Add(evbasic);

        //second
        XmlNode second = xmlDoc.SelectSingleNode(eventType + Tsecond);
        XmlNodeList secondEventList = second.SelectNodes("event");
        // random event
        XmlNode tempevent = secondEventList[Random.Range(0, secondEventList.Count)];
        XmlNodeList secresults = tempevent.SelectNodes("results/result");
        EventResult evsec = getEventResult(secresults);
        evsec.FlavorText = tempevent.SelectSingleNode("eventText").InnerText;
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
            evres.FlavorText = tempevent.SelectSingleNode("eventText").InnerText;
            EventConditions.Add(evres);
        }
    }
}
