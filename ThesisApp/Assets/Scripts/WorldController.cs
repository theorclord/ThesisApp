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
				    GameObject dp = GameObject.Find("Canvas").transform.FindChild("DestinationPanel").gameObject;
                    RectTransform rt = dp.GetComponent<RectTransform>();
                    target = hit.transform.gameObject;
                    mainCam.transform.position = new Vector3(target.transform.position.x, target.transform.position.y, mainCam.transform.position.z);
					Vector3 pos = new Vector3(hit.transform.position.x + (hit.transform.localScale.x/2), hit.transform.position.y - (hit.transform.localScale.y / 2), hit.transform.position.z);
                    pos = new Vector3(mainCam.GetComponent<Camera>().WorldToScreenPoint(pos).x + (rt.rect.width / 2), mainCam.GetComponent<Camera>().WorldToScreenPoint(pos).y , mainCam.GetComponent<Camera>().WorldToScreenPoint(pos).z);
					dp.transform.position = pos ;
					dp.SetActive(true);
                    //playerToken.GetComponent<WorldPlayer>().SetTarget(hit.transform.gameObject);
                }
            }
            else
            {
                if (GameObject.Find("Canvas").transform.FindChild("DestinationPanel").gameObject.active)
                {
                    //GameObject.Find("Canvas").transform.FindChild("DestinationPanel").gameObject.SetActive(false);
                }
            }
            lastPosition = Input.mousePosition;
        }

        if (Input.GetMouseButton(0))
        {
            Vector3 delta = Input.mousePosition - lastPosition;
            mainCam.transform.Translate((-1)*delta.x * mouseSensitivity, (-1) * delta.y * mouseSensitivity, 0);
            if (GameObject.Find("Canvas").transform.FindChild("DestinationPanel").gameObject.active && lastPosition != Input.mousePosition)
            {
                GameObject.Find("Canvas").transform.FindChild("DestinationPanel").gameObject.SetActive(false);
            }
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
