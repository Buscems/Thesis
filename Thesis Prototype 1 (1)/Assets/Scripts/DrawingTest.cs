using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

public class DrawingTest : MonoBehaviour
{

    bool draw;
    [Tooltip("Makes the player able to draw one continuous line before deleting")]
    public bool singleLine;
    [Tooltip("Allows the player to draw multiple lines that last a little bit")]
    public bool multipleLine;

    public GameObject trailObject;
    GameObject currentObj;

    TrailRenderer tr;

    [Tooltip("Button for drawing")]
    public SteamVR_Action_Boolean drawing;
    [Tooltip("Button for changing drawing styles")]
    public SteamVR_Action_Boolean change;
    public SteamVR_Input_Sources hand = SteamVR_Input_Sources.Any;

    // Start is called before the first frame update
    void Start()
    {
        tr = GetComponent<TrailRenderer>();
        tr.enabled = false;

        multipleLine = true;

    }

    // Update is called once per frame
    void Update()
    {
        //this sets up the button presses for this particular button
        //this one is for the grips
        drawing.AddOnStateDownListener(GripDown, hand);
        drawing.AddOnStateUpListener(GripUp, hand);

        //this is for the triggers
        //change.AddOnStateDownListener(TriggerDown, hand);

        //this is code for if the player is using a singular line to draw

        if (draw)
        {
            tr.enabled = true;
        }
        else
        {
            tr.Clear();
            tr.enabled = false;
        }

        //this is for the multiple line code

        if(currentObj != null)
        {
            currentObj.transform.SetParent(this.transform);
        }

    }

    //this is for when you press down the grip on the controller

    public void GripDown(SteamVR_Action_Boolean fromAction, SteamVR_Input_Sources fromSource)
    {
        if (singleLine)
        {
            draw = true;
        }
        if (multipleLine)
        {
            currentObj = Instantiate(trailObject, transform.position, Quaternion.identity);
        }
    }

    //this is for when you press release the grip on the controller

    public void GripUp(SteamVR_Action_Boolean fromAction, SteamVR_Input_Sources fromSource)
    {
        if (singleLine)
        {
            draw = false;
        }
        if (multipleLine)
        {
            StartCoroutine(currentObj.GetComponent<TrailObject>().TimeUntilDestroy());
            currentObj.transform.parent = null;
            currentObj = null;
        }
    }

    //this is for when you press down the trigger on the controller
    public void TriggerDown(SteamVR_Action_Boolean fromAction, SteamVR_Input_Sources fromSource)
    {
        if (singleLine)
        {
            multipleLine = true;
            singleLine = false;
        }
        else if (multipleLine)
        {
            singleLine = true;
            multipleLine = false;
        }
    }

}
