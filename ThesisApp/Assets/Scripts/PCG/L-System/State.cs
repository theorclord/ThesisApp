using UnityEngine;
using System.Collections;

public class State : MonoBehaviour {
    double x;
    double y;
    double angle;
    double length;
    double turningAngle;

    public State(double x, double y, double startAngle, double length, double turningA)
    {
        this.x = x;
        this.y = y;
        angle = startAngle;
        this.length = length;
        turningAngle = turningA;
    }
}
