using UnityEngine;
using System.Collections;
using Assets.Scripts.Utility;
using UnityEngine.SceneManagement;

/// <summary>
/// Responsible for moving the player around in the world map
/// </summary>
public class WorldPlayer : MonoBehaviour {
    private GameObject target;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        if (target != null)
        {
            gameObject.transform.position = Vector3.MoveTowards(gameObject.transform.position, target.transform.position, DataManager.instance.Player.Speed * Time.deltaTime);
        }
    }

    public void SetTarget(GameObject tar)
    {
        target = tar;
    }

    void OnTriggerEnter(Collider other)
    {
        if(other.transform.gameObject == target)
        {
            //TODO: Should initialize the node layout for next scene

            bool canGo = true;
            if (target.GetComponent<WorldNode>().Visited)
            {
                canGo = false;
            }
            foreach(WorldNodeStats nodestats in DataManager.instance.Nodes)
            {
                if(nodestats.Position == target.transform.position)
                {
                    nodestats.Visited = true;
                    DataManager.instance.ActiveNode = nodestats;
                }
            }
            
            target.GetComponent<WorldNode>().Visited = true;
            other.transform.GetComponent<WorldNode>().GetNodeLayout();
            DataManager.instance.Player.Position = transform.position;
            if(target.GetComponent<WorldNode>().Type == NodeType.GOAL)
            {
                DataManager.instance.clearNodes();
                SceneManager.LoadScene("ScoreScene");
            }
            else
            {
                if (canGo)
                {
                    SceneManager.LoadScene("NodeScene");
                }
            }
        }
    }
}
