using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class Selectable : MonoBehaviour {

    public GameObject selected;


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

                selected = hit.transform.gameObject;
                //Debug.Log(selected.tag);
                if (selected.tag == "SystemNode")
                {
                    if (selected.gameObject == this.gameObject)
                    {
                        SceneManager.LoadScene("NodeScene");  
                        //showLocationInfo(header, flavour);

                    }
                    //Debug.Log("H: " + header + ", F:" + flavour);
                }
                //selected.GetComponent<NodeNode>().

            }
        }
    }
}
