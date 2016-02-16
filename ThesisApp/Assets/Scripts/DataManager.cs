using UnityEngine;
using System.Collections;

public class DataManager : MonoBehaviour {
    public static DataManager instance;
    public Vector3 playerPos
    {
        get;
        set;
    }

    void Awake()
    {
        if (!instance)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else {
            Destroy(gameObject);
        }
    }

    // Use this for initialization
    void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

}
