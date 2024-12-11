using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Point : MonoBehaviour
{
    //The sticks that connect to this point
    public List<Stick> sticks = new List<Stick>();

    //The positions of the objects
    public Vector3 pos = Vector2.zero;
    public Vector3 prevPos = Vector2.zero;
    public Vector3 initPos = Vector2.zero;

    bool isPinned = false;
    bool isSelected = false;

    public GameObject pinnedObject = null;

    //Starts the points at the correct values
    void Start()
    {
        transform.position = pos;
        prevPos = pos;
        initPos = pos;
    }

    //Keeps the point in view of the player
    public void KeepInsideView(int width, int height)
    {
        if (pos.y >= height) pos.y = height;
        if (pos.x >= width) pos.x = width;
        if (pos.y < 0) pos.y = 0;
        if (pos.x < 0) pos.x = 0;
    }
    //Adds a stick to the point's stick list
    public void AddStick(Stick stick)
    {
        sticks.Add(stick);
    }
    //Returns the position of the point
    public Vector3 GetPosition()
    {
        return pos;
    }
    //Set the position of the point
    public void SetPosition(float x, float y, float z)
    {
        pos.x = x;
        pos.y = y;
        pos.z = z;
    }
    //Pin the point in place
    public void Pin()
    {
        isPinned = true;
    }
    public void UnPin()
    {
        isPinned = false;
        pinnedObject = null;
    }
    public void PinToObject(GameObject pin)
    {
        isPinned = true;
        pinnedObject = pin;
    }
    //Update the position of the point
    public void UpdatePos(float deltaTime, float drag, Vector3 acceleration, float elasticity)
    {
        
        //For each stick in the list of sticks
        foreach (Stick stick in sticks)
        {
            //If there is a stick
            if (stick != null)
            {
                //Set it as selected
                stick.SetIsSelected(isSelected);
            }
        }
        //If the left mouse button is held and the point is selected
        /*
        if (mouse.leftButtonDown && isSelected)
        {
            
        }
        */
        //Keep a pinned point in place
        if(pinnedObject)
        {
            pos = pinnedObject.transform.position;
            prevPos = pinnedObject.transform.position;
            return;
        }
        if (isPinned)
        {
            pos = initPos;
            return;
        }

        //Find the new position of the point
        Vector3 newPos = pos + (pos - prevPos) * (1.0f - drag) + acceleration * (1.0f - drag) * deltaTime * deltaTime;
        //Set the old position to the current one
        prevPos = pos;
        //Set the current position to the new one
        pos = newPos;

        //Move the point in unity
        transform.position = pos;
    }
}
