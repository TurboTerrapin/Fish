using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Line : MonoBehaviour
{
    public Vector3 gravity = new Vector3(0f, 9.81f, 0f);
    public float drag = 0.01f;
    public float elasticity = 10.0f;

    //The prefab that gets spawned
    public Point pointPrefab = null;
    //The list of all points
    public List<Point> points = new List<Point>();
    //The list of all sticks
    public List<Stick> sticks = new List<Stick>();

    //The number of points along x
    public int width = 0;
    //The number of points along y
    public int height = 0;
    //The distance between points
    public int spacing = 0;

    public float length = 0f;

    public Vector3 startingPos = Vector3.zero;
    public GameObject rodTip = null;
    public GameObject endLine = null;

    // Start is called before the first frame update
    public void Cast()
    {

        Vector3 dir = rodTip.transform.position - endLine.transform.position;

        for (int x = 0; x <= 1 + 1; x++)
        {
            //Create a point gameobject
            Point point = Instantiate<Point>(pointPrefab, transform.position, transform.rotation);
            //Set the position to the starting position
            point.SetPosition(rodTip.transform.position.x + x * spacing * dir.x, dir.y * x * spacing, rodTip.transform.position.z * x * spacing * dir.z);
            //If the point isn't on the leftmost side
            if (x != 0)
            {
                //Find the point to the left of this point
                Point leftPoint = points[this.points.Count - 1];
                //Create a stick between the points
                Stick s = new Stick(point, leftPoint, spacing);
                //Add the stick to both points
                leftPoint.AddStick(s);
                point.AddStick(s);
                //Add the stick to the list of sticks
                sticks.Add(s);
            }
            //If the point is the tallest point and also even
            if (x == 0)
            {
                //Pin the point
                point.PinToObject(endLine);
            }
            else if (x == 2)
            {
                
                point.PinToObject(rodTip);
            }
            //Add the point to the list of points
            points.Add(point);
        }
        length = 2;
    }

    public void Clear()
    {
        sticks.Clear();
        foreach (Point point in points)
        {
            Destroy(point.gameObject);
        }
        points.Clear();
    }

    public void AddPointToEnd()
    {
        if (points.Count >= 1)
        {
            //Create a point gameobject
            Point point = Instantiate<Point>(pointPrefab, transform.position, transform.rotation);
            point.SetPosition(rodTip.transform.position.x, rodTip.transform.position.y, rodTip.transform.position.z);

            //Find the point to the left of this point
            Point leftPoint = points[this.points.Count - 1];
            //Create a stick between the points
            Stick s = new Stick(point, leftPoint, spacing);
            //Add the stick to both points
            leftPoint.AddStick(s);
            point.AddStick(s);
            //Add the stick to the list of sticks
            sticks.Add(s);

            point.PinToObject(rodTip);
            leftPoint.UnPin();

            points.Add(point);
            length++;
        }
    }
    public void RemovePointFromEnd()
    {
        if (points.Count > 2 && sticks.Count > 1)
        {
            //sticks.RemoveAt(this.sticks.Count - 1);

            Point leftPoint = points[this.points.Count - 2];
            //leftPoint.sticks.RemoveAt(0);
            leftPoint.PinToObject(rodTip);

            Destroy(points[points.Count - 1].gameObject);
            points.RemoveAt(this.points.Count - 1);
            length--;
        }
    }


    void Update()
    {
        if (points.Count >= 2)
        {
            if ((points[points.Count - 1].prevPos - points[points.Count - 2].prevPos).magnitude > 1.3 && !endLine.GetComponent<Hook>().landed)
            {
                AddPointToEnd();
            }
        }

        //Get the time between frames
        float deltaTime = Time.deltaTime;
        //For every point in the point list
        foreach (Point p in points)
        {

            Vector3 buoyancy = Vector3.zero;
            if(p.pos.y < 0)
            {
                buoyancy.y = 10f;
            }

            //Update the point's position
            p.UpdatePos(deltaTime, drag, gravity + buoyancy, elasticity);
        }
        //For every stick in the stick list
        foreach (Stick stick in sticks)
        {
            //Update the sticks
            stick.UpdateStick();
        }
    }
}
