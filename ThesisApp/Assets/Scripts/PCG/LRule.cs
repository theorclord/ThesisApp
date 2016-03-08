using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LRule {

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
}
