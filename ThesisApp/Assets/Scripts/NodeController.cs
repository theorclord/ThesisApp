using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using Assets.Scripts.Utility;
using UnityEngine.UI;

public class NodeController : MonoBehaviour {

    private GameObject selected;

    private GameObject nodeInfoPanel;
    private GameObject eventPanel;
    private NodeNode selectNode;

    // Use this for initialization
    void Start()
    {
        generateNodes();
        nodeInfoPanel = GameObject.FindGameObjectWithTag("MainCanvas").transform.FindChild("NodeInformation").gameObject;
        eventPanel = (GameObject.Find("Canvas").gameObject.transform.FindChild("EventPanel").gameObject);
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
                        showLocationInfo(selected);
                        //showLocationInfo(selected.GetComponent<NodeNode>().nodeEvent, selected.GetComponent<NodeNode>().TitleName, selected.GetComponent<NodeNode>().FlavourText);
                    }
                }
            }
        }

    }

    private void showLocationInfo(GameObject selected)
    {
        selectNode = selected.GetComponent<NodeNode>();
        nodeInfoPanel.SetActive(true);
        nodeInfoPanel.transform.Find("LocationName").GetComponent<Text>().text = selectNode.TitleName;
        nodeInfoPanel.transform.Find("FlavorText").GetComponent<Text>().text = selectNode.FlavourText;
    }

    public void BackButton()
    {
        SceneManager.LoadScene("WorldScene");
    }

    private void generateNodes()
    {
        WorldNodeStats stats = DataManager.instance.ActiveNode;
        foreach( NodeStats nodestat in stats.Nodes)
        {
            GameObject nodeObj = Instantiate(Resources.Load("Prefabs/NodeNode"), nodestat.Position, Quaternion.identity) as GameObject;
            NodeNode node = nodeObj.GetComponent<NodeNode>();
            node.FlavourText = nodestat.FlavourText;
            node.TitleName = nodestat.TitleName;
            //nodestat.nodeEvent.getXml();
            node.nodeEvent = nodestat.nodeEvent;
            node.type = nodestat.type;
            switch (node.type)
            {
                case EventSpec.GATHER:
                    nodeObj.transform.GetComponent<MeshRenderer>().material = Resources.Load("Materials/GatherMaterial") as Material;
                    nodeObj.transform.FindChild("Gathering").gameObject.SetActive(true);
                    break;
                case EventSpec.RESEARCH:
                    nodeObj.transform.GetComponent<MeshRenderer>().material = Resources.Load("Materials/ResearchMaterial") as Material;
                    nodeObj.transform.FindChild("Research").gameObject.SetActive(true);
                    break;
                case EventSpec.DIPLOMACY:
                    nodeObj.transform.GetComponent<MeshRenderer>().material = Resources.Load("Materials/DiplomacyMaterial") as Material;
                    nodeObj.transform.FindChild("Diplomacy").gameObject.SetActive(true);
                    break;
            }
            
        }
    }

    public void ClosePanel(GameObject panel)
    {
        panel.SetActive(false);
    }


    public void ExploreNode(GameObject Sender)
    {
        Sender.SetActive(false);
        Event e = selectNode.nodeEvent;
        // Set conditions and choices:
        Transform buttoncont = eventPanel.transform.FindChild("ButtonController");

        int conditionCount = 0;
        GameObject button = Instantiate (Resources.Load("Prefabs/ChoiceButton") as GameObject);
        button.transform.SetParent(buttoncont);
        button.GetComponent<Button>().onClick.AddListener(delegate { ResolveEvent(e.conditionOne); });
        button.transform.position = new Vector3(buttoncont.position.x, buttoncont.position.y - 35 * conditionCount);
        conditionCount++;

        button = Instantiate(Resources.Load("Prefabs/ChoiceButton") as GameObject);
        button.transform.SetParent(buttoncont);
        button.GetComponent<Button>().onClick.AddListener(delegate { ResolveEvent(e.conditionTwo); });
        button.transform.position = new Vector3(buttoncont.position.x, buttoncont.position.y - 35 * conditionCount);
        conditionCount++;

        button = Instantiate(Resources.Load("Prefabs/ChoiceButton") as GameObject);
        button.transform.SetParent(buttoncont);
        button.GetComponent<Button>().onClick.AddListener(delegate { ResolveEvent(e.conditionThree); });
        button.transform.position = new Vector3(buttoncont.position.x, buttoncont.position.y - 35 * conditionCount);
        conditionCount++;

        eventPanel.transform.FindChild("EventType").GetComponent<Text>().text = e.eventText + "\n" + e.eventReward;
        eventPanel.transform.FindChild("EventText").GetComponent<Text>().text = e.conditionOne + "\n" + e.conditionTwo + "\n" + e.conditionThree;
        eventPanel.SetActive(true);
    }

    public void ResolveEvent(string result)
    {
        Debug.Log(result);
        eventPanel.SetActive(false);
        SceneManager.LoadScene("WorldScene");
    }
}
