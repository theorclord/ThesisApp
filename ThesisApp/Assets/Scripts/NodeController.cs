﻿using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using Assets.Scripts.Utility;
using UnityEngine.UI;
using System.Collections.Generic;

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
            nodestat.nodeEvent.GenerateEvent(nodestat.type);
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

        //EventOption button section
        for (int i = 0; i < e.EventOptions.Count; i++)
        {
            //Need to copy else the value is only set to the last i
            int tempint = i;
            GameObject button = Instantiate(Resources.Load("Prefabs/ChoiceButton") as GameObject);
            button.transform.SetParent(buttoncont);
            button.GetComponent<Button>().onClick.AddListener(delegate { ResolveEvent(tempint); });
            button.transform.position = new Vector3(buttoncont.position.x, buttoncont.position.y - 35 * i);
            string buttonText = "";
            int numCon = 0;
            foreach(KeyValuePair<Piece,int> pair in e.EventOptions[tempint].Conditions)
            {
                buttonText += pair.Key.BoardName + " " + pair.Value; 
                numCon++;
                if(e.EventOptions[tempint].Conditions.Count != numCon)
                {
                    buttonText += ", ";
                }
            }
            button.transform.GetChild(0).GetComponent<Text>().text = buttonText;
        }

        // TODO Clean up
        // TODO Set flavor text based on eventOption
        eventPanel.transform.FindChild("EventType").GetComponent<Text>().text = e.eventText + "\n";
        string eventtext = "";
        eventPanel.transform.FindChild("EventText").GetComponent<Text>().text = e.entryFlavor;
        panelOpen = true;
        eventPanel.SetActive(true);
    }

    public void ResolveEvent(int eventnum)
    {
        Event curEvent = selectNode.nodeEvent;
        eventPanel.SetActive(false);
        panelOpen = true;
        resultPanel.SetActive(true);
        //Old function
        //resultPanel.transform.FindChild("ResolutionText").GetComponent<Text>().text = selectNode.nodeEvent.EventConditions[eventnum].ResultText;
        //resultPanel.transform.FindChild("Outcome").GetComponent<Text>().text = selectNode.nodeEvent.EventConditions[eventnum].Result;

        //New function
        string resultString = "";
        string resFlavor = "";
        int chance = Random.Range(0, 100) + 1;
        int accumChance = 0;
        for (int i = 0; i < curEvent.EventOptions[eventnum].Results.Count; i++)
        {
            EventOutcome eo = curEvent.EventOptions[eventnum].Results[i];
            if(eo.Chance >= chance-accumChance)
            {
                resFlavor = eo.outcomeFlavor;
                int tempCount = 0;
                foreach(KeyValuePair<Piece,int[]> pair in eo.Pieces)
                {
                    tempCount++;
                    resultString += pair.Key.BoardName + " " + Random.Range(pair.Value[0], pair.Value[1] + 1);
                    if (eo.Pieces.Count != tempCount)
                    {
                        resultString += ", ";
                    }
                }
                break;
            } else
            {
                accumChance += eo.Chance;
            }
        }
        resultPanel.transform.FindChild("Outcome").GetComponent<Text>().text = resultString;
        // This should be the flavor text
        resultPanel.transform.FindChild("ResolutionText").GetComponent<Text>().text = resFlavor;
    }
}
