using UnityEngine;
using System.Collections;
using System.Xml;

public class SpecialEvent : MonoBehaviour {

    private XmlDocument specEvDoc;
    private string eventstructurepath = "assets/scripts/XML/SpecialEvents.xml";


    // Use this for initialization
    void Start () {

        specEvDoc.Load(eventstructurepath);


	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
