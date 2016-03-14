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
        rule1.name = "First";
        rule1.axiom = "ONN";
        rule1.rules.Add("O", "O[--N][+N]");
        rule1.rules.Add("N", "N[-N+N][+N++N]");
        rule1.expandingIterations = 2;
        rule1.delta = Mathf.PI / 7.2f;
        ruleCollection.Add(rule1);

        // Second Rule *STAR FORMATION*
        LRule rule2 = new LRule();
        rule2.name = "Star";
        rule2.axiom = "N--N--N";
        rule2.rules.Add("N", "N+N--N+N");
        rule2.expandingIterations = 1;
        rule2.delta = Mathf.PI / 3;
        ruleCollection.Add(rule2);

        // Third Rule *Grass Straw*
        LRule rule3 = new LRule();
        rule3.name = "Grass Straw";
        rule3.axiom = "N";
        rule3.rules.Add("N", "O[+N][-N]ON");
        rule3.rules.Add("O", "O");
        rule3.expandingIterations = 3;
        rule3.delta = Mathf.PI/ 7.0039f;
        ruleCollection.Add(rule3);

        // Fourth Rule *Box Fractal*
        LRule rule4 = new LRule();
        rule4.name = "Box Fractal";
        rule4.axiom = "N-N-N-N";
        rule4.rules.Add("N", "N-N+N+N-N");
        rule4.expandingIterations = 2;
        rule4.delta = Mathf.PI / 2;
        ruleCollection.Add(rule4);

        // Fifth Rule *Seaweed*
        LRule rule5 = new LRule();
        rule5.name = "Seaweed";
        rule5.axiom = "N";
        rule5.rules.Add("N", "NN-[-N+N+N]+[+N-N-N]");
        rule5.expandingIterations = 2;
        rule5.delta = Mathf.PI / 8.1818f;
        ruleCollection.Add(rule5);

        // Sixth Rule *Rings*
        LRule rule6 = new LRule();
        rule6.name = "Rings";
        rule6.axiom = "N";
        rule6.rules.Add("N", "NN+N+N+N+N+N-N");
        rule6.expandingIterations = 2;
        rule6.delta = Mathf.PI / 2;
        ruleCollection.Add(rule6);

    }
    
}
