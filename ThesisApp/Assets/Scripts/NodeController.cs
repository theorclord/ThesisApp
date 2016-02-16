using UnityEngine;
using System.Collections;

public class NodeController : MonoBehaviour {

    private GameObject selected;

    // Use this for initialization
    void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, 100))
            {
                if (hit.transform.gameObject.GetComponent<Selectable>() != null)
                {
                    selected = hit.transform.gameObject;
                }
            }
        }
        if(selected.tag == "LocationNode")
        {
            //showLocationInfo(selected.GetComponent<NodeNode>().header, selected.GetComponent<NodeNode>().flavour);
        }
       /* else if (spawnset && Input.GetMouseButtonDown(0))
        {
            GameObject temp = Instantiate(selected, Camera.main.ScreenToWorldPoint(Input.mousePosition), Quaternion.identity) as GameObject;
            temp.transform.position = new Vector3(temp.transform.position.x, temp.transform.position.y);
        }*/
    }

    public void showLocationInfo(string header, string flavour)
    {
        GameObject p = (GameObject.Find("Canvas").gameObject.transform.FindChild("EventPanel").gameObject);
        p.SetActive(true);
        p.GetComponent<EventPanel>().OpenWithText(header, flavour);
    }
}
