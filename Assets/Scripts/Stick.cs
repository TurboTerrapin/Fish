using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stick
{
    //The points the stick connects
    public Point p0 = null;
    public Point p1 = null;
    //The length of the stick
    public float length = 0f;

    public bool isActive = true;
    public bool isSelected = false;

    public Color color = new Color(255, 0, 0, 0);
    public Color colorWhenSelected = new Color(255, 204, 00, 00);

    //Constructor
    public Stick(Point p0, Point p1, float length)
    {
        this.p0 = p0;
        this.p1 = p1;
        this.length = length;
    }
    //Set the stick as currently selected
    public void SetIsSelected(bool value)
    {
        isSelected = value;
    }
    //Break the connection between the points
    public void Break()
    {
        isActive = false;
    }
    //Update the position of the points based on the stick
    public void UpdateStick()
    {
        //Skip if the stick is broken
        if (!isActive)
            return;
        //Get the positions of both points
        Vector3 p0Pos = p0.GetPosition();
        Vector3 p1Pos = p1.GetPosition();

        //Get the difference between the points
        Vector3 diff = p0Pos - p1Pos;
        //Get the distance between the points
        float dist = Mathf.Sqrt(diff.x * diff.x + diff.y * diff.y + diff.z * diff.z);
        //Get the ratio of how different the current distance is from the set distance
        float diffFactor = (length - dist) / dist;
        //Find the offset that the points should be at
        Vector3 offset = diff * diffFactor * 0.5f;

        //Set the position of the points
        p0.SetPosition(p0Pos.x + offset.x, p0Pos.y + offset.y, p0Pos.z + offset.z);
        p1.SetPosition(p1Pos.x - offset.x, p1Pos.y - offset.y, p1Pos.z - offset.z);
    }
}
