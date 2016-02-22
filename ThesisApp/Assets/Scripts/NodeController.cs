using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using Assets.Scripts.Utility;

public class NodeController : MonoBehaviour {

    private GameObject selected;

    // Use this for initialization
    void Start()
    {
        generateNodes();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, 100))
            {
                
                selected = hit.transform.gameObject;
                if (selected.tag == "LocationNode")
                {
                    if (selected.GetComponent<NodeNode>() != null)
                    {
                        Debug.Log("Hit");
                        showLocationInfo(selected.GetComponent<NodeNode>().TitleName, selected.GetComponent<NodeNode>().FlavourText);
                    }
                }
            }
        }

    }
    private void showLocationInfo(string titlename, string flavour)
    {
        GameObject panel = (GameObject.Find("Canvas").gameObject.transform.FindChild("EventPanel").gameObject);
        panel.GetComponent<EventPanel>().OpenWithText(titlename, flavour);
    }

    public void BackButton()
    {
        SceneManager.LoadScene("WorldScene");
    }

    private void generateNodes()
    {
        //WorldNodeStats stats = DataManager.instance.ActiveNode;
        //GameObject node = Instantiate(Resources.Load("Prefabs/NodeNode"), stats.Position, Quaternion.identity) as GameObject;
    }
}
