using UnityEngine;
using System.Collections;

public class NodeNode : MonoBehaviour {

    public string TitleName
    { get; set; }
    public string FlavourText
    { get; set; }
    private GameObject selected;

    // Use this for initialization
    void Start () {
        //Todo load from generation
	    TitleName = "Test Header";
        FlavourText = "Test Flavour";
    }

	
	// Update is called once per frame
	void Update () {
        
    }
}
