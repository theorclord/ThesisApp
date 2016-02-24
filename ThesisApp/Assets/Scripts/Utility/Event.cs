using UnityEngine;
using System.Collections;
using System.Xml;
using System.IO;
using Assets.Scripts.Utility;

public class Event {


    //XML Difficulties
    string basic = "Events/Basic";
    string intermediate = "Events/Intermediate";
    string hard = "Events/Hard";
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
    string conditions = "/Conditions";
    string crew = "/Crew-Members";
    string engineRoom = "/Room-types/Engine";
    string diplomacyRoom = "/Room-types/Diplomacy";
    string exploreRoom = "/Room-types/Exploration";
    string scienceRoom = "/Room-types/Science";
    string energy = "/Energy";
    string researchPoints = "/Research";
    //XML
    public string localizedStringsFile = "assets/scripts/utility/EventValues.xml";
    string language;
    string grouping;
    XmlDocument root;

    //Variables
    public string eventText = "Basic Gathering Event";
    public string eventReward = "Successful event yields 3 scraps";
    public string conditionOne = "Activated research center + 1 energy";
    public string conditionTwo = "2 available workers";
    public string conditionThree = "1 energy + 1 worker available";

    EventSpec eventSpec = EventSpec.GATHER;

    public void getXml(int eventNumber)
    {
        root = new XmlDocument();
        root.Load(localizedStringsFile);

        switch (eventNumber)
        {
            case 0:// gather
                eventText = root.SelectSingleNode(basic + gathering + header).InnerText;
                eventReward = "Reward for completion: " + Random.Range(1, 3) + " " + root.SelectSingleNode(basic + gathering + rewards + scrap).InnerText;
                break;
            case 1://research
                eventText = root.SelectSingleNode(basic + research + header).InnerText;
                eventReward = "Reward for completion: " + Random.Range(1, 3) + " " + root.SelectSingleNode(basic + research + rewards + scrap).InnerText;
                break;
            case 2://diplomacy
                eventText = root.SelectSingleNode(basic + exploration + header).InnerText;
                eventReward = "Reward for completion: " + Random.Range(1, 3) + " " + root.SelectSingleNode(basic + exploration + rewards + scrap).InnerText;
                break;

        }

        //Add PCG to create the different types of events and rewards
      //  eventText = root.SelectSingleNode("Events/Basic/Gathering/Header").InnerText;
       // eventReward = "Reward for completion: " + Random.Range(1,3) +" "+root.SelectSingleNode("Events/Basic/Gathering/Rewards/Scrap").InnerText;
    }
}
