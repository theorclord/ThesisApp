using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class EventPanel : MonoBehaviour {



	// Use this for initialization
	void Start () {
	
	}

    public void OpenWithText(string titlename, string flavour)
    {
        //Debug.Log(header + ", " + flavour);
        this.gameObject.transform.FindChild("LocationName").GetComponent<Text>().text = titlename;
        this.gameObject.transform.FindChild("EventText").GetComponent<Text>().text = flavour;
        this.gameObject.SetActive(true);
    }
	
    public void CloseButton()
    {
        this.gameObject.SetActive(false);
    }
	// Update is called once per frame
	void Update () {
	
	}
}
