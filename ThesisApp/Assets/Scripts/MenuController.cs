using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour {

	// Use this for initialization
	void Start () {
        DataManager.instance.ClearAll();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void StartButton()
    {
        SceneManager.LoadScene("WorldScene");
    }

    public void ExitButton()
    {
        Application.Quit();
    }
}
