using UnityEngine;
using System.Collections;
using System.Xml;
using System.IO;
using Assets.Scripts.Utility;

public class Event {

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

    public void getXml()
    {
        root = new XmlDocument();
        root.Load(localizedStringsFile);
        //Add PCG to create the different types of events and rewards
        eventText = root.SelectSingleNode("Events/Basic/Gathering/Header").InnerText;
        eventReward = "Reward for completion: " + Random.Range(1,3) +" "+root.SelectSingleNode("Events/Basic/Gathering/Rewards/Scrap").InnerText;
    }
}
