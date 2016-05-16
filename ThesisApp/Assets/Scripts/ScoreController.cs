using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using System.Xml;
using UnityEngine.UI;

public class ScoreController : MonoBehaviour {

    private string eventstructurepath = "assets/scripts/XML/EventStructure.xml";
    // Use this for initialization
    void Start () {
        XmlDocument xmlDoc = new XmlDocument();
        TextAsset textAsset = Resources.Load("XML/EventStructure", typeof(TextAsset)) as TextAsset;
        xmlDoc.LoadXml(textAsset.text);
        //XmlDocument xmlDoc = new XmlDocument();
        //xmlDoc.Load(eventstructurepath);
        XmlNodeList list = xmlDoc.SelectNodes("eventstructure/endflavor/flavor");
        int r = Random.Range(0, list.Count);
        string endflavor = list[r].InnerText;
        GameObject.FindGameObjectWithTag("MainCanvas").transform.FindChild("Panel").gameObject.transform.Find("Endtext").GetComponent<Text>().text = endflavor;
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
