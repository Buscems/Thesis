using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

public class NewLevitate : MonoBehaviour
{

    public EnemySpawner enemySpawn;

    public GameObject objectToMove;

    public GameObject referencePoint;

    public float speed;

    public float levitateRange;

    public float throwThreshold;
    public float throwForce;

    public float pushPullSpeed;

    bool triggerIsDown;
    bool push, pull;

    Vector3 controllerVelocity;
    Vector3 previousPos;

    int layerMask = 1 << 11;

    public Vector3 levitateDirection;
    public GameObject levitateCursor;

    [Tooltip("Buttons for levitating")]
    public SteamVR_Action_Boolean levitate;
    public SteamVR_Action_Boolean pushPull;
    public SteamVR_Input_Sources hand = SteamVR_Input_Sources.Any;

    public bool isLevitating;

    // Start is called before the first frame update
    void Start()
    {

        levitateCursor.SetActive(false);

        //checking the buttons for the VR controller
        levitate.AddOnStateDownListener(TriggerDown, hand);
        levitate.AddOnStateUpListener(TriggerUp, hand);

        pushPull.AddOnStateDownListener(GripDown, hand);
        pushPull.AddOnStateUpListener(GripUp, hand);

    }

    // Update is called once per frame
    void Update()
    {
        controllerVelocity = (transform.position - previousPos) / Time.deltaTime;
        previousPos = transform.position;
        

        if (isLevitating)
        {
            Levitating(objectToMove);
        }
        if (push)
        {
            Vector3 moveDirection = (referencePoint.transform.position).normalized - transform.position;

            var rb = referencePoint.GetComponent<Rigidbody>();
            referencePoint.transform.localPosition += (moveDirection * pushPullSpeed);

        }

        if (pull)
        {
            Vector3 moveDirection = (referencePoint.transform.position).normalized - transform.position;

            var rb = referencePoint.GetComponent<Rigidbody>();
            referencePoint.transform.localPosition -= (moveDirection * pushPullSpeed);
        }

        RaycastHit hit;
        // Does the ray intersect any objects excluding the player layer
        if (Physics.Raycast(transform.position, transform.TransformDirection(levitateDirection), out hit, levitateRange, layerMask))
        {
            if (!isLevitating)
            {
                objectToMove = hit.collider.gameObject;
                levitateCursor.SetActive(true);
                levitateCursor.transform.position = hit.transform.position;
            }
            Debug.DrawRay(transform.position, transform.TransformDirection(levitateDirection) * hit.distance, Color.yellow);
        }
        else
        {
            if (!isLevitating)
            {
                objectToMove = null;
                levitateCursor.SetActive(false);
            }
            Debug.DrawRay(transform.position, transform.TransformDirection(levitateDirection) * levitateRange, Color.white);
        }

    }

    //this is what will happen when the player presses this trigger down
    public void TriggerDown(SteamVR_Action_Boolean fromAction, SteamVR_Input_Sources fromSource)
    {
        if (!triggerIsDown)
        {
            if (objectToMove != null)
            {
                if (objectToMove.name == "Start")
                {
                    if (!enemySpawn.startEnemies)
                    {
                        enemySpawn.startEnemies = true;
                    }
                    else if (enemySpawn.startEnemies)
                    {
                        enemySpawn.startEnemies = false;
                    }
                }
                
                else if (!isLevitating)
                {
                    referencePoint.transform.position = objectToMove.transform.position;
                    isLevitating = true;
                }
            }
            triggerIsDown = true;
        }
    }

    public void TriggerUp(SteamVR_Action_Boolean fromAction, SteamVR_Input_Sources fromSource)
    {
        if (triggerIsDown)
        {
            if (isLevitating)
            {
                if (controllerVelocity.magnitude > throwThreshold)
                {
                    objectToMove.GetComponent<Rigidbody>().AddForce(controllerVelocity.normalized * throwForce);
                }
                objectToMove.GetComponent<Rigidbody>().useGravity = true;
                isLevitating = false;
            }
            triggerIsDown = false;
        }
    }

    public void GripDown(SteamVR_Action_Boolean fromAction, SteamVR_Input_Sources fromSource)
    {
        if (!push)
        {
            push = true;
        }
    }

    public void GripUp(SteamVR_Action_Boolean fromAction, SteamVR_Input_Sources fromSource)
    {
        push = false;
    }

    void Levitating(GameObject currentObj)
    {
        currentObj.GetComponent<Rigidbody>().useGravity = false;
        currentObj.GetComponent<Rigidbody>().position = Vector3.Lerp(currentObj.transform.position, referencePoint.transform.position, speed);
        levitateCursor.SetActive(false);
    }

    void StopLevitating(GameObject currentObj)
    {
        currentObj.GetComponent<Rigidbody>().useGravity = true;
    }

}
