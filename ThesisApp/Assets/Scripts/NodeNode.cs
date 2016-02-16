using UnityEngine;
using System.Collections;

public class NodeNode : MonoBehaviour {

    private string header = "Test";
    private string flavour = "Test";
    private GameObject selected;

    // Use this for initialization
    void Start () {
	
	}

    public void setHeader(string h)
    {
        this.header = h;
    }

    public void setFlavour(string f)
    {
        this.flavour = f;
    }

	
	// Update is called once per frame
	void Update () {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, 100))
            {
               
                    selected = hit.transform.gameObject;
                    //Debug.Log(selected.tag);
                    if (selected.tag == "LocationNode")
                    {
                    if(selected.gameObject == this.gameObject)
                    {
                        showLocationInfo(header, flavour);

                    }
                    //Debug.Log("H: " + header + ", F:" + flavour);
                    }
                //selected.GetComponent<NodeNode>().

            }
        }
        
    }
    private void showLocationInfo(string header, string flavour)
    {
        GameObject p = (GameObject.Find("Canvas").gameObject.transform.FindChild("EventPanel").gameObject);
        p.GetComponent<EventPanel>().OpenWithText(header, flavour);
       // p.SetActive(true);
    }
}
