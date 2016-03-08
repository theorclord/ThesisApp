using UnityEngine;
using System.Collections;

public class RuleCollection{


    ArrayList ruleCollection = new ArrayList();

    public ArrayList GetCollection()
    {
        return ruleCollection;
    }

    public void GenerateRules()
    {
        // First Rule
        LRule rule1 = new LRule();
        rule1.axiom = "ONN";
        rule1.rules.Add("O", "O[--N][+N]");
        rule1.rules.Add("N", "N[-N+N][+N++N]");
        rule1.expandingIterations = 2;
        rule1.delta = (float)(Mathf.PI / (7.2));
        ruleCollection.Add(rule1);

        // Second Rule *STAR FORMATION*
        LRule rule2 = new LRule();
        rule2.axiom = "N--N--N";
        rule2.rules.Add("N", "N+N--N+N");
        rule2.expandingIterations = 1;
        rule2.delta = Mathf.PI / 3;
        ruleCollection.Add(rule2);
    }
    
}
