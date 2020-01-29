using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

public class SpellController : MonoBehaviour
{
    [Header("Hand")]
    [Tooltip("Which hand this script is on")]
    public GameObject thisHand;
    Rigidbody handRb;
    [Header("Camera")]
    public GameObject headCamera;
    public enum CurrentSpell { None, Fireball, LightningBolt, MiniWard, AirBlast }
    public CurrentSpell spell;

    [Header("Fireball")]
    public GameObject fireball;
    public float fireballOffset;
    public float fireballThrowForce;

    [Header("Lightning Bolt")]
    public GameObject lightningBolt;
    public float lightningBoltOffset;

    [Header("Mini Ward")]
    public GameObject miniWard;

    [Header("Air Blast")]
    public GameObject airBlast;

    [Tooltip("Buttons for throwing")]
    public SteamVR_Action_Boolean throwObject;
    public SteamVR_Behaviour_Pose controllerPose;
    public SteamVR_Input_Sources hand = SteamVR_Input_Sources.Any;

    GameObject collidingObj;
    GameObject inHand;

    bool canLevitate;

    bool pressButton;

    // Start is called before the first frame update
    void Start()
    {

        //getting things from the gameobject
        handRb = thisHand.GetComponent<Rigidbody>();

        //checking the buttons for the VR controller
        throwObject.AddOnStateDownListener(TriggerDown, hand);
        throwObject.AddOnStateUpListener(TriggerUp, hand);
        canLevitate = true;
    }

    //for throwing
    void SetCollidingObject(Collider col)
    {
        if(collidingObj || !col.GetComponent<Rigidbody>())
        {
            return;
        }

        collidingObj = col.gameObject;
        //player cannot levitate if they are able to grab something
        canLevitate = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (spell == CurrentSpell.Fireball)
        {
            var obj = Instantiate(fireball, headCamera.transform.position + new Vector3(headCamera.transform.forward.x, 0, headCamera.transform.forward.z) * fireballOffset, Quaternion.Euler(headCamera.transform.eulerAngles));
            spell = CurrentSpell.None;
        }

        if(spell == CurrentSpell.LightningBolt)
        {
            var obj = Instantiate(lightningBolt, headCamera.transform.position + new Vector3(headCamera.transform.forward.x, 0, headCamera.transform.forward.z) * lightningBoltOffset, Quaternion.Euler(headCamera.transform.eulerAngles));
            spell = CurrentSpell.None;
        }

        if(spell == CurrentSpell.MiniWard)
        {
            var obj = Instantiate(miniWard, headCamera.transform.position + new Vector3(headCamera.transform.forward.x, 0, headCamera.transform.forward.z) * lightningBoltOffset, Quaternion.Euler(headCamera.transform.eulerAngles));
            spell = CurrentSpell.None;
        }

        if(spell == CurrentSpell.AirBlast)
        {
            var obj = Instantiate(airBlast, headCamera.transform.position + new Vector3(headCamera.transform.forward.x, 0, headCamera.transform.forward.z) * lightningBoltOffset, Quaternion.Euler(Quaternion.identity.x, headCamera.transform.eulerAngles.y, Quaternion.identity.z));
            spell = CurrentSpell.None;
        }

        if (canLevitate)
        {
            this.GetComponent<NewLevitate>().enabled = true;
        }
        else
        {
            this.GetComponent<NewLevitate>().enabled = false;
        }
    }

    //two different ways of doing the same thing with both vive and index controllers

    public void TriggerDown(SteamVR_Action_Boolean fromAction, SteamVR_Input_Sources fromSource)
    {
        Debug.Log("Press down");
        if (!pressButton)
        {
            if (collidingObj)
            {
                PickUp();
            }
        }
    }

    public void TriggerUp(SteamVR_Action_Boolean fromAction, SteamVR_Input_Sources fromSource)
    {
        if (pressButton)
        {
            pressButton = false;
        }
        if (inHand)
        {
            LetGo();
        }
        
    }

    private void OnTriggerEnter(Collider other)
    {
        SetCollidingObject(other);
    }

    private void OnTriggerStay(Collider other)
    {
        SetCollidingObject(other);
    }

    private void OnTriggerExit(Collider other)
    {
        if (!collidingObj)
        {
            return;
        }

        collidingObj = null;
        //if the player is no longer holding something they can now levitate
        canLevitate = true;
        
    }


    void PickUp()
    {
        inHand = collidingObj;
        collidingObj = null;

        inHand.transform.parent = this.transform;
        inHand.transform.localPosition = new Vector3(.0025f, -.008f, -.1055f);

        var joint = AddFixedJoint();
        joint.connectedBody = inHand.GetComponent<Rigidbody>();
        if (inHand.GetComponent<Shield>())
        {
            inHand.GetComponent<Shield>().stopTimer = true;
        }

    }
    private FixedJoint AddFixedJoint()
    {
        FixedJoint fx = gameObject.AddComponent<FixedJoint>();
        fx.breakForce = 20000;
        fx.breakTorque = 20000;
        return fx;
    }

    void LetGo()
    {
        if (GetComponent<FixedJoint>())
        {
            GetComponent<FixedJoint>().connectedBody = null;
            Destroy(GetComponent<FixedJoint>());
            inHand.transform.parent = null;
            if (inHand.GetComponent<Fireball>())
            {
                inHand.GetComponent<Rigidbody>().velocity = controllerPose.GetVelocity() * fireballThrowForce;
                inHand.GetComponent<Rigidbody>().angularVelocity = controllerPose.GetAngularVelocity();
                inHand.GetComponent<Rigidbody>().useGravity = true;
            }
            if (inHand.GetComponent<LightningBolt>())
            {
                //inHand.GetComponent<Rigidbody>().velocity = controllerPose.GetVelocity() * lightningBoltSpeed;
                inHand.GetComponent<LightningBolt>().direction = controllerPose.GetVelocity();
                inHand.transform.up = inHand.GetComponent<LightningBolt>().direction;
            }
            if (inHand.GetComponent<Shield>())
            {
                inHand.GetComponent<Animator>().SetTrigger("disappear");
            }
        }
        inHand = null;
    }

}
