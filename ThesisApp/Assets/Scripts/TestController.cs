using UnityEngine;
using System.Collections;

public class TestController : MonoBehaviour {
    public GameObject Sphere;
    public GameObject Cube;

    private float speed = 2;
	// Use this for initialization
	void Start () {
        
	}
	
	// Update is called once per frame
	void Update () {
        Sphere.transform.position = Vector3.MoveTowards(Sphere.transform.position, Cube.transform.position, speed *Time.deltaTime);
    }
}
