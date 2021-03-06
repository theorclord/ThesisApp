﻿using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

/// <summary>
/// Deprecated
/// </summary>
public class GameController : MonoBehaviour {
    //GameObject panel; 
    private List<GameObject> savedObjects;

    private GameObject selected;
    private bool statspressed;

    private bool spawnset;
    // Use this for initialization
    void Start () {
        savedObjects = new List<GameObject>();

    }

    // Update is called once per frame
    void Update () {
        if (Input.GetMouseButtonDown(0) && !spawnset)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, 100))
            {
                if(hit.transform.gameObject.GetComponent<Selectable>() != null)
                {
                    selected = hit.transform.gameObject;
                }
            }
        } else if (spawnset && Input.GetMouseButtonDown(0))
        {
            GameObject temp = Instantiate(selected, Camera.main.ScreenToWorldPoint( Input.mousePosition), Quaternion.identity) as GameObject;
            temp.transform.position = new Vector3(temp.transform.position.x, temp.transform.position.y);
        }
	}

    public void SaveSelectable()
    {
        savedObjects.Add(selected);
    }

    public void SetSpawnLastSelected()
    {
        if (savedObjects.Count > 0)
        {
            spawnset = !spawnset;
            selected = savedObjects[savedObjects.Count-1];
        }
    }

    public void BackButton()
    {
        SceneManager.LoadScene("WorldScene");
    }

    public void NodeViewButton()
    {
        SceneManager.LoadScene("NodeScene");
    }

    public void LocationButton()
    {
        GameObject p = (GameObject.Find("Canvas").gameObject.transform.FindChild("EventPanel").gameObject);
        p.SetActive(true);

    }

    public void ExitButton()
    {
        SceneManager.LoadScene("StartMenu");
    }
    public void ShowStats()
    {
        //panel = GameObject.Find("StatButton").transform.GetChild(1).gameObject;
    }
   
}
