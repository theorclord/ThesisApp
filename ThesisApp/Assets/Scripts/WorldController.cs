using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using Assets.Scripts.Utility;

public class WorldController : MonoBehaviour {

    private GameObject playerToken;

    //Camera movement
    public GameObject mainCam;
    private Vector3 lastPosition;
    private float mouseSensitivity = 0.2f;

    // Use this for initialization
    void Start()
    {
        generateWorldNodes();
        GameObject player = Instantiate(Resources.Load("Prefabs/PlayerToken"), DataManager.instance.Player.Position, Quaternion.identity) as GameObject;
        playerToken = player;
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
                if (hit.transform.gameObject.GetComponent<WorldNode>() != null)
                {
                    playerToken.GetComponent<WorldPlayer>().SetTarget(hit.transform.gameObject);
                }
            }
            lastPosition = Input.mousePosition;
        }

        // Move the camera if mouse button is held
        if (Input.GetMouseButton(0))
        {
            Vector3 delta = Input.mousePosition - lastPosition;
            mainCam.transform.Translate(delta.x * mouseSensitivity, delta.y * mouseSensitivity, 0);
            lastPosition = Input.mousePosition;
        }
    }

    /// <summary>
    /// Exit to start menu
    /// </summary>
    public void ExitButton()
    {
        SceneManager.LoadScene("StartMenu");
    }

    private void generateWorldNodes()
    {
        //TODO generate nodes
        foreach(NodeStats stat in DataManager.instance.Nodes)
        {
            GameObject obj = Resources.Load("Prefabs/NodeWorld") as GameObject;
            GameObject node = Instantiate(obj, stat.Position, Quaternion.identity) as GameObject;
            WorldNode wn = node.GetComponent<WorldNode>();
            wn.NodeName = stat.Name;
            wn.NodeDesciption = stat.Description;
            wn.Visited = stat.Visited;
        }
    }
}
