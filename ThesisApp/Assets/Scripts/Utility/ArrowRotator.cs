using UnityEngine;
using System.Collections;

public class ArrowRotator : MonoBehaviour
{


    //values that will be set in the Inspector
    public Transform Target;
    public float RotationSpeed;

    //values for internal use
    private Quaternion _lookRotation;
    private Vector3 _direction;

    // Update is called once per frame
    void Update()
    {
        if(GameObject.Find("NodeWorldGoal(Clone)").transform != null)
        {
            Target = GameObject.Find("NodeWorldGoal(Clone)").transform;
            Vector3 lp = Camera.main.WorldToScreenPoint(Target.position);
            Vector3 lookPos = Camera.main.ScreenToWorldPoint(Target.position);
            lookPos = lp - transform.position;
            float angle = Mathf.Atan2(lookPos.y, lookPos.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.AngleAxis(angle,  Vector3.forward);
        }
        
    }

}
