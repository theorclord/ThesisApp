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
    private GameObject resultPanel;

    private bool panelOpen;

    // Use this for initialization
    void Start()
    {
        generateNodes();
        nodeInfoPanel = GameObject.FindGameObjectWithTag("MainCanvas").transform.FindChild("NodeInformation").gameObject;
        eventPanel = (GameObject.Find("Canvas").gameObject.transform.FindChild("EventPanel").gameObject);
        resultPanel = GameObject.FindGameObjectWithTag("MainCanvas").transform.FindChild("EventResolution").gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        if (!panelOpen && Input.GetMouseButtonDown(0))
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
        panelOpen = true;
    }

    public void BackButton()
    {
        panelOpen = false;
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
        panelOpen = false;
        panel.SetActive(false);
    }


    public void ExploreNode(GameObject Sender)
    {
        Sender.SetActive(false);
        Event e = selectNode.nodeEvent;
        // Set conditions and choices:
        Transform buttoncont = eventPanel.transform.FindChild("ButtonController");

        for(int i = 0; i<e.EventConditions.Count; i++)
        {
            //Need to copy else the value is only set to the last i
            int tempint = i;
            GameObject button = Instantiate(Resources.Load("Prefabs/ChoiceButton") as GameObject);
            button.transform.SetParent(buttoncont);
            button.GetComponent<Button>().onClick.AddListener(delegate { ResolveEvent(tempint); });
            button.transform.position = new Vector3(buttoncont.position.x, buttoncont.position.y - 35 * i);
            button.transform.GetChild(0).GetComponent<Text>().text = e.EventConditions[tempint].FlavorText;
        }

        // TODO Clean up
        eventPanel.transform.FindChild("EventType").GetComponent<Text>().text = e.eventText + "\n" + e.eventReward;
        string eventtext = "";
        foreach(EventResult evr in e.EventConditions)
        {
            eventtext += evr.FlavorText + "\n";
        }
        eventPanel.transform.FindChild("EventText").GetComponent<Text>().text = eventtext;
        //TODO set event information in own class
        panelOpen = true;
        eventPanel.SetActive(true);
    }

    public void ResolveEvent(int eventnum)
    {
        eventPanel.SetActive(false);
        panelOpen = true;
        resultPanel.SetActive(true);
        resultPanel.transform.FindChild("ResolutionText").GetComponent<Text>().text = selectNode.nodeEvent.EventConditions[eventnum].ResultText;
        resultPanel.transform.FindChild("Outcome").GetComponent<Text>().text = selectNode.nodeEvent.EventConditions[eventnum].Result;
    }
}
