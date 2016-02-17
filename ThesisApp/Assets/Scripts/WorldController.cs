using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class WorldController : MonoBehaviour {
    private List<GameObject> savedObjects;

    private GameObject selected;
    private GameObject playerToken;

    //Camera movement
    public GameObject mainCam;
    private Vector3 lastPosition;
    private float mouseSensitivity = 0.2f;

	private GameObject target = null;
    // Use this for initialization
    void Start()
    {
        playerToken = GameObject.Find("PlayerToken");
        if (DataManager.instance.playerPos != Vector3.zero)
        {
            playerToken.transform.position = DataManager.instance.playerPos;
        }
        savedObjects = new List<GameObject>();
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
										
					target = hit.transform.gameObject;
					Vector3 pos = hit.transform.position;
					GameObject.Find("Canvas").transform.FindChild("DestinationPanel").gameObject.transform.position = mainCam.GetComponent<Camera>().WorldToScreenPoint(pos);
					GameObject.Find("Canvas").transform.FindChild("DestinationPanel").gameObject.SetActive(true);
                    //playerToken.GetComponent<WorldPlayer>().SetTarget(hit.transform.gameObject);
                }
            }
            lastPosition = Input.mousePosition;
        }

        if (Input.GetMouseButton(0))
        {
            Vector3 delta = Input.mousePosition - lastPosition;
            mainCam.transform.Translate(delta.x * mouseSensitivity, delta.y * mouseSensitivity, 0);
            lastPosition = Input.mousePosition;
        }
    }


	public void CloseButton()
	{
		GameObject.Find("Canvas").transform.FindChild("DestinationPanel").gameObject.SetActive(false);
	}

	public void AcceptButton()
	{
		GameObject.Find("Canvas").transform.FindChild("DestinationPanel").gameObject.SetActive(false);
		playerToken.GetComponent<WorldPlayer>().SetTarget(target);
	}

    public void ExitButton()
    {
        SceneManager.LoadScene("StartMenu");
    }
}
