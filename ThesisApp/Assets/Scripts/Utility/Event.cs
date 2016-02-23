using UnityEngine;
using System.Collections;
using Assets.Scripts.Utility;

public class Event : MonoBehaviour {

    public string eventText = "Basic Gathering Event";
    public string eventReward = "Successful event yields 3 scraps";
    public string conditionOne = "Activated research center + 1 energy";
    public string conditionTwo = "2 available workers";
    public string conditionThree = "1 energy + 1 worker available";

    EventSpec eventSpec = EventSpec.GATHER;

    // Use this for initialization
    void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
