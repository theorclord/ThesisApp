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

    //XML type events
    private string Tbasic = "/basic";
    private string Tsecond = "/second";
    private string Tthird = "/third";
    //XML Difficulties
    string basic = "Events/Basic";
    //string intermediate = "Events/Intermediate";
    //string hard = "Events/Hard";
    //XML Header
    string header = "/Header";
    //XML Types
    string gathering = "/Gathering";
    string exploration = "/Diplomacy";
    string research = "/Research";
    //XML Rewards
    string rewards = "/Rewards";
    string scrap = "/Scrap";
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

    public List<string> EventConditions
    {
        get; set;
    }

    public Event()
    {
        EventConditions = new List<string>();
    }
    

    public void getXml(int eventNumber)
    {
        root = new XmlDocument();
        root.Load(localizedStringsFile);

        int numberOfPeople = 0;
        int numberOfPower = 0;
        int numberOfResearch = 0;

        switch (eventNumber)
        {
            case 0:// gather
                int numEvents = Random.Range(0, 2)+2;
                XmlDocument gatherSection = new XmlDocument();
                gatherSection.Load(filepath);
                XmlNode gatherBasic = gatherSection.SelectSingleNode(gather+Tbasic);
                EventConditions.Add(gatherBasic.SelectSingleNode("event/eventText").InnerText);
                
                XmlNode gatherSecond = gatherSection.SelectSingleNode(gather + Tsecond);
                XmlNodeList secondEventList = gatherSecond.SelectNodes("event");
                EventConditions.Add(secondEventList[Random.Range(0, secondEventList.Count)].SelectSingleNode("eventText").InnerText);
                if (numEvents == 3)
                {
                    XmlNode gatherThird = gatherSection.SelectSingleNode(gather + Tthird);
                    XmlNodeList thirdEventList = gatherThird.SelectNodes("event");
                    EventConditions.Add(thirdEventList[Random.Range(0, thirdEventList.Count)].SelectSingleNode("eventText").InnerText);
                }
                eventText = root.SelectSingleNode(basic + gathering + header).InnerText;
                eventReward = "Reward for completion: " + Random.Range(1, 3) + " " + root.SelectSingleNode(basic + gathering + rewards + scrap).InnerText;

                //Should have people in gathering machine + power
                numberOfPower = Random.Range(0, 3); //0, 1 or 2
                numberOfPeople = 2-numberOfPower;
                //EventConditions.Add(numberOfPeople + " available workers + " + numberOfPower + " power.");
                if (numberOfPeople != numberOfPower) {
                    //EventConditions.Add(numberOfPower + " available workers + " + numberOfPeople + " power.");
                }
                //EventConditions.Add("1 Research token available");


                break;
            case 1://research
                eventText = root.SelectSingleNode(basic + research + header).InnerText;
                eventReward = "Reward for completion: " + Random.Range(1, 3) + " " + root.SelectSingleNode(basic + research + rewards + scrap).InnerText;

                // should have people + research token
                numberOfResearch = Random.Range(0, 3); //0, 1 or 2
                numberOfPeople = 2 - numberOfResearch;
                EventConditions.Add(numberOfPeople + " available workers + " + numberOfResearch + " research tokens.");
                if (numberOfPeople != numberOfResearch)
                {
                    EventConditions.Add(numberOfResearch + " available workers + " + numberOfPeople + " research tokens.");
                }

                EventConditions.Add("2 power tokens available");
                break;
            case 2://diplomacy
                eventText = root.SelectSingleNode(basic + exploration + header).InnerText;
                eventReward = "Reward for completion: " + Random.Range(1, 3) + " " + root.SelectSingleNode(basic + exploration + rewards + scrap).InnerText;

                //should have more people available
                numberOfPeople = Random.Range(1, 3); //1 or 2
                numberOfResearch = 2 - numberOfPeople;
                EventConditions.Add(numberOfPeople + " available workers + " + numberOfResearch + " power.");
                if (numberOfPeople != numberOfResearch)
                {
                    EventConditions.Add(numberOfResearch + " available workers + " + numberOfPeople + " power.");
                }

                EventConditions.Add("2 power tokens available");
                break;

        }

        //Add PCG to create the different types of events and rewards
    }
}
