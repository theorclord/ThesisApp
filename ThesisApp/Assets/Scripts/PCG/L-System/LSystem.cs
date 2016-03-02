using UnityEngine;
using System.Collections;

public class LSystem : MonoBehaviour {

    string axiom { get; set; }
    public IDictionary ruleSets;
    string expanded { get; set; }


    public void expand(int depth)
    {
        string result = axiom;

        // MORE Calculations needed for the nodes.
        for(int i = 0; i < depth; i++)
        {
            char[] currDSA = result.ToCharArray();
            string newString = "";
            for(int j = 0; j < currDSA.Length; j++)
            {
                if (ruleSets.Contains(currDSA[j]))
                {
                    newString += ruleSets[currDSA[j]];
                }
                else
                {
                    newString += currDSA[j];
                }
            }
            result = newString;
        }
        expanded = result;
    }

    public void interpret()
    {
        Stack stack = new Stack();
        char[] chars = expanded.ToCharArray();
        for (int i = 0; i < chars.Length; i++)
        {
            switch (chars[i])
            {
                case 'S':
                    break;
                case '[':
                    stack.Push(new Object());
                    break;
                case ']':
                    Object o = (Object)stack.Pop();
                    break;
            }
        }
    }

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
