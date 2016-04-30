using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using Assets.Scripts.Utility;
using UnityEngine.UI;

public class WorldController : MonoBehaviour {

    private GameObject playerToken;

    //Camera movement
    public GameObject mainCam;
    private Vector3 lastPosition;
    private float mouseSensitivity = 0.2f;

    private float maxDistance = 10.0f;

    //Zooming
    private float zoomSpeed = 0.5f;
    private float timeForDoubleClick = 0;
    private bool oneClick = false;
    private float doubleDelay = 0.5f;
    private float maxZoom = 20f;
    private float minZoom = 1f;


    private GameObject target = null;
    // Use this for initialization
    void Start()
    {
        if(DataManager.instance.Nodes.Count == 0)
        {
            DataManager.instance.InitializeNodes();
        }
        spawnWorldNodes();
        GameObject player = Instantiate(Resources.Load("Prefabs/PlayerToken"), DataManager.instance.Player.Position, Quaternion.identity) as GameObject;
        playerToken = player;
        // Set player movement indicator
        //Gets the movement quad as first child
        float playerspeed = DataManager.instance.Player.Speed;
        float ringSize = playerspeed * 2;
        float innerRadius = 0.0005f * ringSize + 0.485f;
        playerToken.transform.GetChild(0).GetComponent<MeshRenderer>().material.SetFloat("_InnerRadius", innerRadius);

        // Move camera to player
        mainCam.transform.position = new Vector3(DataManager.instance.Player.Position.x,DataManager.instance.Player.Position.y,-10f);
        mainCam.GetComponent<Camera>().orthographicSize = DataManager.instance.cameraZoom;
    }

    // Update is called once per frame
    void Update()
    {
        // Select World node
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, 100))
            {
                if (hit.transform.gameObject.GetComponent<WorldNode>() != null)
                {
                    // Spawns a panel next to the selected object
                    GameObject destpanel = GameObject.Find("Canvas").transform.FindChild("DestinationPanel").gameObject;
                    RectTransform rt = destpanel.GetComponent<RectTransform>();

                    float distanceToTarget = Vector3.Distance(hit.transform.position, playerToken.transform.position);
                    //Debug.Log("Distance = " + distanceToTarget);

                    if(distanceToTarget <= maxDistance && !destpanel.activeSelf)
                    {
                        target = hit.transform.gameObject;
                    mainCam.transform.position = new Vector3(target.transform.position.x, target.transform.position.y, mainCam.transform.position.z);
                    Vector3 pos = new Vector3(hit.transform.position.x + (hit.transform.localScale.x / 2), hit.transform.position.y - (hit.transform.localScale.y / 2), hit.transform.position.z);
                    pos = new Vector3(mainCam.GetComponent<Camera>().WorldToScreenPoint(pos).x + (rt.rect.width / 2), mainCam.GetComponent<Camera>().WorldToScreenPoint(pos).y, mainCam.GetComponent<Camera>().WorldToScreenPoint(pos).z);
                    destpanel.transform.position = pos;
                    destpanel.transform.FindChild("NamePanel").FindChild("LocationName").GetComponent<Text>().text = target.GetComponent<WorldNode>().NodeName;
                    destpanel.transform.FindChild("DescriptionPanel").FindChild("LocationDescription").GetComponent<Text>().text = target.GetComponent<WorldNode>().NodeDesciption;
                    destpanel.SetActive(true);
                    }
                }
            }
        }

        #region Camera controls
        if (Input.GetMouseButtonDown(0))
        {
            //For mouse movement
            lastPosition = Input.mousePosition;
        }
        // Move the camera if mouse button is held
        if (Input.GetMouseButton(0))
        {
            Vector3 delta = Input.mousePosition - lastPosition;
            mainCam.transform.Translate((-1) * delta.x * mouseSensitivity, (-1) * delta.y * mouseSensitivity, 0);
            //Sets the destination panel to inactive if the camera moves
            if (GameObject.Find("Canvas").transform.FindChild("DestinationPanel").gameObject.activeSelf && lastPosition != Input.mousePosition)
            {
                GameObject.Find("Canvas").transform.FindChild("DestinationPanel").gameObject.SetActive(false);
            }
            lastPosition = Input.mousePosition;
        }

        //Zoom features
        // Reset zoom with double click
        if (Input.GetMouseButtonDown(0))
        {
            if (!oneClick) // first click no previous clicks
            {
                oneClick = true;

                timeForDoubleClick = Time.time; // save the current time
            }
            else
            {
                //Double click
                oneClick = false;
                Debug.Log("Double click");
                mainCam.GetComponent<Camera>().orthographicSize = 5 ;
                mainCam.transform.position = new Vector3(DataManager.instance.Player.Position.x,DataManager.instance.Player.Position.y,-10);
            }
        }
        if (oneClick)
        {
            // Checks if more time have passed than the delay 
            if ((Time.time - timeForDoubleClick) > doubleDelay)
            {
                // Reset double click
                oneClick = false;
            }
        }
        float mouseDelta = Input.GetAxis("Mouse ScrollWheel");
        if (mouseDelta > 0f)
        {
            //Zoom in
            mainCam.GetComponent<Camera>().orthographicSize = Mathf.Clamp( mainCam.GetComponent<Camera>().orthographicSize - 1 * zoomSpeed,minZoom, maxZoom);
            DataManager.instance.cameraZoom = mainCam.GetComponent<Camera>().orthographicSize;
        }
        else if (mouseDelta < 0f)
        {
            //Zoom out
            mainCam.GetComponent<Camera>().orthographicSize = Mathf.Clamp(mainCam.GetComponent<Camera>().orthographicSize + 1 * zoomSpeed, minZoom, maxZoom);
            
            DataManager.instance.cameraZoom = mainCam.GetComponent<Camera>().orthographicSize;
        }
        #endregion
    }

    /// <summary>
    /// Closes the panel with destination information
    /// </summary>
    public void CloseButton()
	{
		GameObject.Find("Canvas").transform.FindChild("DestinationPanel").gameObject.SetActive(false);
	}

    /// <summary>
    /// Sets the destination for the player and closes the panel
    /// </summary>
	public void AcceptButton()
	{
		GameObject.Find("Canvas").transform.FindChild("DestinationPanel").gameObject.SetActive(false);
		playerToken.GetComponent<WorldPlayer>().SetTarget(target);
	}

    /// <summary>
    /// Exit to start menu
    /// </summary>
    public void ExitButton()
    {
        SceneManager.LoadScene("StartMenu");
    }

    private void spawnWorldNodes()
    {
        foreach(WorldNodeStats stat in DataManager.instance.Nodes)
        {
            if(stat.Type != NodeType.GOAL)
            {
                GameObject obj = Resources.Load("Prefabs/NodeWorldNormal") as GameObject;
                GameObject node = Instantiate(obj, stat.Position, Quaternion.identity) as GameObject;
                WorldNode wn = node.GetComponent<WorldNode>();
                wn.NodeName = stat.Name;
                wn.NodeDesciption = stat.Description;
                wn.Visited = stat.Visited;
                wn.Type = stat.Type;
                if (wn.Visited)
                {
                    // Transform t = node.gameObject.transform.FindChild("Sprite");
                    node.GetComponent<SpriteRenderer>().sprite = DataManager.instance.ConqueredSprite;//Resources.Load("Sprites/Conquered", typeof(Sprite)) as Sprite;
                }
            }
            else
            {
                GameObject obj = Resources.Load("Prefabs/NodeWorldGoal") as GameObject;
                GameObject node = Instantiate(obj, stat.Position, Quaternion.identity) as GameObject;
                WorldNode wn = node.GetComponent<WorldNode>();
                wn.NodeName = stat.Name;
                wn.NodeDesciption = stat.Description;
                wn.Visited = stat.Visited;
                wn.Type = stat.Type;
            }
           
           
        }
    }
}
