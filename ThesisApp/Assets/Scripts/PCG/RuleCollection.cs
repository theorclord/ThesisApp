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
        rule1.delta = Mathf.PI / 7.2f;
        ruleCollection.Add(rule1);

        // Second Rule *STAR FORMATION*
        LRule rule2 = new LRule();
        rule2.axiom = "N--N--N";
        rule2.rules.Add("N", "N+N--N+N");
        rule2.expandingIterations = 1;
        rule2.delta = Mathf.PI / 3;
        ruleCollection.Add(rule2);

        // Third Rule *Grass Straw*
        LRule rule3 = new LRule();
        rule3.axiom = "N";
        rule3.rules.Add("N", "O[+N][-N]ON");
        rule3.rules.Add("O", "O");
        rule3.expandingIterations = 3;
        rule3.delta = Mathf.PI/ 7.0039f;
        ruleCollection.Add(rule3);
    }
    
}
