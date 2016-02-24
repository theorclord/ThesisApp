using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class ScoreController : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void RestartButton()
    {
        DataManager.instance.clearNodes();
        SceneManager.LoadScene("StartMenu");
    }
}
