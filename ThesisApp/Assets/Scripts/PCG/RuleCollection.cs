using UnityEngine;
using System.Collections;

public class RuleCollection : MonoBehaviour {


    ArrayList ruleCollection = new ArrayList();

    public ArrayList GetCollection()
    {
        return ruleCollection;
    }

    public void GenerateRules()
    {
        // First Rule
        LRule rule1 = new LRule();
        rule1.axiom = "N-N-N-N";
        //rule1.rules.Add("S", "S[-N][+N]");
        rule1.rules.Add("N", "N-N+N+NN-N-N+N");
        rule1.expandingIterations = 1;
        rule1.delta = Mathf.PI / 2;
        ruleCollection.Add(rule1);

        // Second Rule *STAR FORMATION*
        LRule rule2 = new LRule();
        rule2.axiom = "N--N--N";
        rule2.rules.Add("N", "N+N--N+N");
        rule2.expandingIterations = 1;
        rule2.delta = Mathf.PI / 3;
        ruleCollection.Add(rule2);
    }

    // Use this for initialization
    void Start () {
       
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
