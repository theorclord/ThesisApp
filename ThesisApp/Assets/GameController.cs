using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour {
    GameObject panel; 
    // Use this for initialization
    void Start () {
	    panel = GameObject.Find("StatButton").transform.GetChild(1).gameObject;
    }
    public bool pressedD = false;
    public bool pressedU = false;

    // Update is called once per frame
    void Update () {
        
	}

    public void ExitButton()
    {
        SceneManager.LoadScene("StartMenu");
    }
    public void ShowStats()
    {
        pressedD = !pressedD;
        //GameObject panel = GameObject.Find("StatButton").transform.GetChild(1).gameObject;
        //ButtonDOWNpresser();
        panel.SetActive(pressedD);
    }
   


    IEnumerator Wait()
    {
        yield return new WaitForSeconds(3.0f);
    }

    public void ButtonDOWNpresser() //Called from the OnClick() section of the GUI button
    {
        pressedD = true;
        StartCoroutine(Wait());
        pressedD = false;

    }

    public void ButtonUPpresser()
    {
        pressedU = true;
        StartCoroutine(Wait());
        pressedU = false;
    }
}
