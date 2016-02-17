using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

/// <summary>
/// Responsible for moving the player around in the world map
/// </summary>
public class WorldPlayer : MonoBehaviour {

    private float speed = 2;
    private GameObject target;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        if (target != null)
        {
            gameObject.transform.position = Vector3.MoveTowards(gameObject.transform.position, target.transform.position, speed * Time.deltaTime);
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
            other.transform.GetComponent<WorldNode>().GetNodeLayout();
            DataManager.instance.Player.Position = transform.position;
            SceneManager.LoadScene("NodeScene");
        }
    }
}
