using UnityEngine;
using System.Collections;

public class State : MonoBehaviour {
    public double x {
        get; set; }
    public double y
    {
        get; set;
    }
    public double angle
    {
        get; set;
    }
    public double length
    {
        get; set;
    }
    public double turningAngle
    {
        get; set;
    }

    public State(double x, double y, double startAngle, double length, double turningA)
    {
        this.x = x;
        this.y = y;
        angle = startAngle;
        this.length = length;
        turningAngle = turningA;
    }
}
