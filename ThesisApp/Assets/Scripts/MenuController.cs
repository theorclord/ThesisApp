using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void StartButton()
    {
        //DataManager.instance.clearNodes();
        //DataManager.instance.InitializeNodes();

        SceneManager.LoadScene("WorldScene");

    }

    public void RestartButton()
    {
        DataManager.instance.clearNodes();
        SceneManager.LoadScene("StartMenu");
    }
}
