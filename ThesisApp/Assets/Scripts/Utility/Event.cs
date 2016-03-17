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
    //XML Difficulties
    //string basic = "Events/Basic";
    //string intermediate = "Events/Intermediate";
    //string hard = "Events/Hard";
    //XML Header
    //string header = "/Header";
    //XML Types
    //string gathering = "/Gathering";
    //string exploration = "/Diplomacy";
    //string research = "/Research";
    //XML Rewards
    //string rewards = "/Rewards";
    //string scrap = "/Scrap";
    //XML Conditions
    //string conditions = "/Conditions";
    //string crew = "/Crew-Members";
    //string engineRoom = "/Room-types/Engine";
    //string diplomacyRoom = "/Room-types/Diplomacy";
    //string exploreRoom = "/Room-types/Exploration";
    //string scienceRoom = "/Room-types/Science";
    //string energy = "/Energy";
    //string researchPoints = "/Research";
    //XML
    public string localizedStringsFile = "assets/scripts/XML/EventValues.xml";
    string language;
    string grouping;
    XmlDocument root;

    //Variables
    public string eventText = "Basic Gathering Event";
    public string eventReward = "Successful event yields 3 scraps";

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
    

    public void getXml(int eventNumber)
    {
        int numEvents = Random.Range(0, 2) + 2;
        EventResult evbasic = null;
        XmlNodeList secondEventList = null;
        XmlNode tempevent = null;
        XmlNodeList secresults = null;
        EventResult evsec = null;
        //Add PCG to create the different types of events and rewards
        switch (eventNumber)
        {
            case 0:// gather
                //basic
                XmlDocument gatherSection = new XmlDocument();
                gatherSection.Load(filepath);
                XmlNode gatherBasic = gatherSection.SelectSingleNode(gather+Tbasic);
                XmlNodeList gatherBasicList = gatherBasic.SelectNodes("event/results/result");

                evbasic = getEventResult(gatherBasicList);
                evbasic.FlavorText = gatherBasic.SelectSingleNode("event/eventText").InnerText;
                EventConditions.Add(evbasic);
                
                //second
                XmlNode gatherSecond = gatherSection.SelectSingleNode(gather + Tsecond);
                secondEventList = gatherSecond.SelectNodes("event");
                // random event
                tempevent = secondEventList[Random.Range(0, secondEventList.Count)];
                secresults = tempevent.SelectNodes("results/result");
                evsec = getEventResult(secresults);
                evsec.FlavorText = tempevent.SelectSingleNode("eventText").InnerText;
                EventConditions.Add(evsec);

                //third
                if (numEvents == 3)
                {
                    
                    XmlNode gatherThird = gatherSection.SelectSingleNode(gather + Tthird);
                    XmlNodeList thirdEventList = gatherThird.SelectNodes("event");
                    // random event
                    tempevent = thirdEventList[Random.Range(0, thirdEventList.Count)];
                    XmlNodeList thiresults = tempevent.SelectNodes("results/result");
                    EventResult evres = getEventResult(thiresults);
                    evres.FlavorText = tempevent.SelectSingleNode("eventText").InnerText;
                    EventConditions.Add(evres);
                }
                //TODO Set eventText
                //eventText
                break;
            case 1://research
                //basic
                XmlDocument researchSection = new XmlDocument();
                researchSection.Load(filepath);
                XmlNode researchBasic = researchSection.SelectSingleNode(research + Tbasic);
                XmlNodeList researchBasicList = researchBasic.SelectNodes("event/results/result");

                evbasic = getEventResult(researchBasicList);
                evbasic.FlavorText = researchBasic.SelectSingleNode("event/eventText").InnerText;
                EventConditions.Add(evbasic);

                //second
                XmlNode researchSecond = researchSection.SelectSingleNode(research + Tsecond);
                secondEventList = researchSecond.SelectNodes("event");
                // random event
                tempevent = secondEventList[Random.Range(0, secondEventList.Count)];
                secresults = tempevent.SelectNodes("results/result");
                evsec = getEventResult(secresults);
                evsec.FlavorText = tempevent.SelectSingleNode("eventText").InnerText;
                EventConditions.Add(evsec);

                //third
                if (numEvents == 3)
                {

                    XmlNode diplomacyThird = researchSection.SelectSingleNode(research + Tthird);
                    XmlNodeList thirdEventList = diplomacyThird.SelectNodes("event");
                    // random event
                    tempevent = thirdEventList[Random.Range(0, thirdEventList.Count)];
                    XmlNodeList thiresults = tempevent.SelectNodes("results/result");
                    EventResult evres = getEventResult(thiresults);
                    evres.FlavorText = tempevent.SelectSingleNode("eventText").InnerText;
                    EventConditions.Add(evres);
                }
                //TODO Set eventText
                //eventText
                break;
            case 2://diplomacy
                //basic
                XmlDocument diplomacySection = new XmlDocument();
                diplomacySection.Load(filepath);
                XmlNode diplomacyBasic = diplomacySection.SelectSingleNode(diplomacy + Tbasic);
                XmlNodeList diplomacyBasicList = diplomacyBasic.SelectNodes("event/results/result");

                evbasic = getEventResult(diplomacyBasicList);
                evbasic.FlavorText = diplomacyBasic.SelectSingleNode("event/eventText").InnerText;
                EventConditions.Add(evbasic);

                //second
                XmlNode diplomacySecond = diplomacySection.SelectSingleNode(diplomacy + Tsecond);
                secondEventList = diplomacySecond.SelectNodes("event");
                // random event
                tempevent = secondEventList[Random.Range(0, secondEventList.Count)];
                secresults = tempevent.SelectNodes("results/result");
                evsec = getEventResult(secresults);
                evsec.FlavorText = tempevent.SelectSingleNode("eventText").InnerText;
                EventConditions.Add(evsec);

                //third
                if (numEvents == 3)
                {

                    XmlNode diplomacyThird = diplomacySection.SelectSingleNode(diplomacy + Tthird);
                    XmlNodeList thirdEventList = diplomacyThird.SelectNodes("event");
                    // random event
                    tempevent = thirdEventList[Random.Range(0, thirdEventList.Count)];
                    XmlNodeList thiresults = tempevent.SelectNodes("results/result");
                    EventResult evres = getEventResult(thiresults);
                    evres.FlavorText = tempevent.SelectSingleNode("eventText").InnerText;
                    EventConditions.Add(evres);
                }
                //TODO Set eventText
                //eventText
                break;
        }
    }
    private EventResult getEventResult(XmlNodeList nodelist)
    {
        int chance = Random.Range(1, 101);
        Debug.Log("The chance value " + chance);
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
}
