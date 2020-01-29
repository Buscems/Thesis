using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;
using TMPro;
using AOT;
using System;
using System.Runtime.InteropServices;

public class CreatingGestures : MonoBehaviour
{

    [Header("Hit Detection")]
    public PlayerHitDetection hitDetect;

    [Header("Player Scripts")]
    public SpellController sc;



    public GestureRecognition gr = null;

    private GameObject active_controller = null;

    public int record_gesture_id = -1;

    private double last_performance_report = 0;

    List<string> stroke = new List<string>();

    public int stroke_index = 0;

    private TextMeshProUGUI HUDText;

    public GCHandle me;

    public SteamVR_Behaviour_Skeleton handAction;

    [Header("Hand Configurations")]
    [Tooltip("0 = thumb, 1 = index, 2 = middle, 3 = ring, 4 = pinky")]
    public float[] fireHand;

    public string file_load_gestures = "Assets/GestureRecognition/Sample_OneHanded_MyGestures.dat";

    //which element are we using
    bool fire, lightning;

    public CreatingGestures() : base()
    {
        me = GCHandle.Alloc(this);
    }

    // Start is called before the first frame update
    void Start()
    {

        // Set the welcome message.
        HUDText = GameObject.Find("HUDText").GetComponent<TextMeshProUGUI>();
        HUDText.text = "Welcome to Spell-Smith\n"
                      + "You can cast spells by drawing shapes,\n"
                      + "Draw a circle for fireballs, lightning bolt for a lightning bolt, and a square for a shield\n"
                      + "";

        me = GCHandle.Alloc(this);

        gr = new GestureRecognition();

        gr.setTrainingUpdateCallback(CreatingGestures.trainingUpdateCallback);
        gr.setTrainingUpdateCallbackMetadata((IntPtr)me);
        gr.setTrainingFinishCallback(CreatingGestures.trainingFinishCallback);
        gr.setTrainingFinishCallbackMetadata((IntPtr)me);

        bool success = gr.loadFromFile(file_load_gestures);
        if (!success)
        {
            HUDText.text = "Failed";
        }
        //Debug.Log((success ? "Gesture file loaded successfully" : "[ERROR] Failed to load gesture file."));

    }

    // Update is called once per frame
    void Update()
    {

        if (hitDetect.gameOver)
        {
            //HUDText.text = "You lost the game";
        }

        float trigger_left = Input.GetAxis("LeftControllerTrigger");
        float trigger_right = Input.GetAxis("RightControllerTrigger");

        // Single Gesture recognition / 1-handed operation
        if (this.gr != null)
        {
            // If the user is not yet dragging (pressing the trigger) on either controller, he hasn't started a gesture yet.
            if (active_controller == null)
            {
                // If the user presses either controller's trigger, we start a new gesture.
                if (handAction.skeletonAction.fingerCurls[0] > fireHand[0] && handAction.skeletonAction.fingerCurls[1] < fireHand[1] && handAction.skeletonAction.fingerCurls[2] < fireHand[2] && handAction.skeletonAction.fingerCurls[3] 
                    > fireHand[3] && handAction.skeletonAction.fingerCurls[4] > fireHand[4])
                {
                    // Right controller trigger pressed.
                    active_controller = GameObject.Find("Controller (right)");
                }
                else if (trigger_left > 0.8)
                {
                    // Left controller trigger pressed.
                    active_controller = GameObject.Find("Controller (left)");
                }
                else
                {
                    // If we arrive here, the user is pressing neither controller's trigger:
                    // nothing to do.
                    return;
                }
                // If we arrive here: either trigger was pressed, so we start the gesture.
                GameObject hmd = GameObject.Find("Main Camera"); // alternative: Camera.main.gameObject
                Vector3 hmd_p = hmd.transform.localPosition;
                Quaternion hmd_q = hmd.transform.localRotation;
                gr.startStroke(hmd_p, hmd_q, record_gesture_id);
                return;
            }

            // If we arrive here, the user is currently dragging with one of the controllers.
            // Check if the user is still dragging or if he let go of the trigger button.
            if (trigger_left > 0.3 || 
                handAction.skeletonAction.fingerCurls[0] > fireHand[0] && handAction.skeletonAction.fingerCurls[1] < fireHand[1] && handAction.skeletonAction.fingerCurls[2] < fireHand[2] && handAction.skeletonAction.fingerCurls[3]
                    > fireHand[3] && handAction.skeletonAction.fingerCurls[4] > fireHand[4])
            {
                // The user is still dragging with the controller: continue the gesture.
                Vector3 p = active_controller.transform.position;
                Quaternion q = active_controller.transform.rotation;
                gr.contdStroke(p, q);
                addToStrokeTrail(p);
                return;
            }
            // else: if we arrive here, the user let go of the trigger, ending a gesture.
            active_controller = null;

            // Delete the objectes that we used to display the gesture.
            foreach (string star in stroke)
            {
                Destroy(GameObject.Find(star));
                stroke_index = 0;
            }

            double similarity = 0; // This will receive the similarity value (0~1)
            Vector3 pos = Vector3.zero; // This will receive the position where the gesture was performed.
            double scale = 0; // This will receive the scale at which the gesture was performed.
            Vector3 dir0 = Vector3.zero; // This will receive the primary direction in which the gesture was performed (greatest expansion).
            Vector3 dir1 = Vector3.zero; // This will receive the secondary direction of the gesture.
            Vector3 dir2 = Vector3.zero; // This will receive the minor direction of the gesture (direction of smallest expansion).
            int gesture_id = gr.endStroke(ref similarity, ref pos, ref scale, ref dir0, ref dir1, ref dir2);

            // If we are currently recording samples for a custom gesture, check if we have recorded enough samples yet.
            if (record_gesture_id >= 0)
            {
                // Currently recording samples for a custom gesture - check how many we have recorded so far.
                //HUDText.text = "Recorded a gesture sample for " + gr.getGestureName(record_gesture_id) + ".\n"
                      //+ "Total number of recorded samples for this gesture: " + gr.getGestureNumberOfSamples(record_gesture_id) + ".\n"
                      //+ "You can stop recording samples in the Inspector for the XR rig.\n";
                return;
            }
            // else: if we arrive here, we're not recording new samples,
            // but instead have identified a gesture.
            if (gesture_id < 0)
            {
                // Error trying to identify any gesture
                //HUDText.text = "Failed to identify gesture.";
            }
            else
            {
                string gesture_name = gr.getGestureName(gesture_id);
                //HUDText.text = "Identified gesture " + gesture_name + "(" + gesture_id + ")\n(Similarity: " + similarity + ")";
                switch (gesture_name)
                {
                    case "Basic Fire":
                        sc.spell = SpellController.CurrentSpell.Fireball;
                        if (HUDText.enabled)
                        {
                            HUDText.enabled = false;
                        }
                        break;
                    case "Basic Lightning":
                        sc.spell = SpellController.CurrentSpell.LightningBolt;
                        if (HUDText.enabled)
                        {
                            HUDText.enabled = false;
                        }
                        break;
                    case "Basic Ward":
                        sc.spell = SpellController.CurrentSpell.MiniWard;
                        if (HUDText.enabled)
                        {
                            HUDText.enabled = false;
                        }
                        break;
                    case "Basic Air":
                        sc.spell = SpellController.CurrentSpell.AirBlast;
                        if (HUDText.enabled)
                        {
                            HUDText.enabled = false;
                        }
                        break;
                }
            }
            return;
        }

    }

    // Callback function to be called by the gesture recognition plug-in during the learning process.
    public static void trainingUpdateCallback(double performance, IntPtr ptr)
    {
        if (ptr.ToInt32() == 0)
        {
            return;
        }
        // Get the script/scene object back from metadata.
        GCHandle obj = (GCHandle)ptr;
        CreatingGestures me = (obj.Target as CreatingGestures);
        // Update the performance indicator with the latest estimate.
        me.last_performance_report = performance;
    }

    // Callback function to be called by the gesture recognition plug-in when the learning process was finished.
    public static void trainingFinishCallback(double performance, IntPtr ptr)
    {
        if (ptr.ToInt32() == 0)
        {
            return;
        }
        // Get the script/scene object back from metadata.
        GCHandle obj = (GCHandle)ptr;
        CreatingGestures me = (obj.Target as CreatingGestures);
        // Update the performance indicator with the latest estimate.
        me.last_performance_report = performance;
    }

    // Helper function to add a new star to the stroke trail.
    public void addToStrokeTrail(Vector3 p)
    {
        GameObject star_instance = Instantiate(GameObject.Find("star"));
        GameObject star = new GameObject("stroke_" + stroke_index++);
        star_instance.name = star.name + "_instance";
        star_instance.transform.SetParent(star.transform, false);
        System.Random random = new System.Random();
        star.transform.localPosition = new Vector3((float)random.NextDouble() / 80, (float)random.NextDouble() / 80, (float)random.NextDouble() / 80) + p;
        star.transform.localRotation = new Quaternion((float)random.NextDouble() - 0.5f, (float)random.NextDouble() - 0.5f, (float)random.NextDouble() - 0.5f, (float)random.NextDouble() - 0.5f).normalized;
        //star.transform.localRotation.Normalize();
        float star_scale = (float)random.NextDouble() + 0.3f;
        star.transform.localScale = new Vector3(star_scale, star_scale, star_scale);
        stroke.Add(star.name);
    }

}
