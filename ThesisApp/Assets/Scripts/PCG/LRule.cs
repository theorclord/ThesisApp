using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LRule : MonoBehaviour {

    public string axiom = "";
    public IDictionary rules = new Dictionary<string, string>();
    public int expandingIterations = 1;
    public float delta = 0.0f;

    public LRule(string ax)
    {
        axiom = ax;
    }

    public LRule()
    {

    }


    public void expand(int depth)
    {
        string result = axiom;
        //Debug.Log(depth);
        //Debug.Log("Length of Rules:" + ruleSets.Count);
        // MORE Calculations needed for the nodes.
        for (int i = 0; i < depth; i++)
        {
            char[] currDSA = result.ToCharArray();
            // Debug.Log("Length of Currdsa:" + currDSA.Length);
            string newString = "";
            for (int j = 0; j < currDSA.Length; j++)
            {
                if (rules.Contains(currDSA[j].ToString()))
                {
                    newString += rules[currDSA[j].ToString()];
                }
                else
                {
                    newString += currDSA[j];
                }
            }

            result = newString;
        }
        result += "E";
        //expanded = result;
        Debug.Log(result);
    }

    // Use this for initialization
    void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
