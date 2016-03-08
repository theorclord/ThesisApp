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

        int numberOfPeople = 0;
        int numberOfPeopleAlt = 0;
        int numberOfPower = 0;
        int numberOfPowerAlt = 0;
        int numberOfResearch = 0;
        int numberOfResearchAlt = 0;

        switch (eventNumber)
        {
            case 0:// gather
                eventText = root.SelectSingleNode(basic + gathering + header).InnerText;
                eventReward = "Reward for completion: " + Random.Range(1, 3) + " " + root.SelectSingleNode(basic + gathering + rewards + scrap).InnerText;

                //Should have people in gathering machine + power
                numberOfPower = Random.Range(0, 3); //0, 1 or 2
                numberOfPeople = 2-numberOfPower;
                conditionOne = numberOfPeople + " available workers + " + numberOfPower + " power.";
                if (numberOfPeople != numberOfPower) {
                    conditionTwo = numberOfPower + " available workers + " + numberOfPeople + " power.";
                }
                else {
                    conditionTwo = "";
                }
                conditionThree = "1 Research token available";


                break;
            case 1://research
                eventText = root.SelectSingleNode(basic + research + header).InnerText;
                eventReward = "Reward for completion: " + Random.Range(1, 3) + " " + root.SelectSingleNode(basic + research + rewards + scrap).InnerText;

                // should have people + research token
                numberOfResearch = Random.Range(0, 3); //0, 1 or 2
                numberOfPeople = 2 - numberOfResearch;
                conditionOne = numberOfPeople + " available workers + " + numberOfResearch + " research tokens.";
                if (numberOfPeople != numberOfResearch)
                {
                    conditionTwo = numberOfResearch + " available workers + " + numberOfPeople + " research tokens.";
                }
                else
                {
                    conditionTwo = "";
                }
                
                conditionThree = "2 power tokens available";
                break;
            case 2://diplomacy
                eventText = root.SelectSingleNode(basic + exploration + header).InnerText;
                eventReward = "Reward for completion: " + Random.Range(1, 3) + " " + root.SelectSingleNode(basic + exploration + rewards + scrap).InnerText;

                //should have more people available
                numberOfPeople = Random.Range(1, 3); //1 or 2
                numberOfResearch = 2 - numberOfPeople;
                conditionOne = numberOfPeople + " available workers + " + numberOfResearch + " power.";
                if (numberOfPeople != numberOfResearch)
                {
                    conditionTwo = numberOfResearch + " available workers + " + numberOfPeople + " power.";
                    }
                else
                {
                    conditionTwo = "";
                }
               
                conditionThree = "2 power tokens available";
                break;

        }

        //Add PCG to create the different types of events and rewards
      //  eventText = root.SelectSingleNode("Events/Basic/Gathering/Header").InnerText;
       // eventReward = "Reward for completion: " + Random.Range(1,3) +" "+root.SelectSingleNode("Events/Basic/Gathering/Rewards/Scrap").InnerText;
    }
}
