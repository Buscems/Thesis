using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

public class PlayerMovement : MonoBehaviour
{

    public SteamVR_Action_Vector2 direction;

    public float speed;

    Rigidbody rb;

    // Start is called before the first frame update
    void Start()
    {
        rb = this.GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Debug.Log(direction.axis);
        rb.MovePosition(rb.position + new Vector3(direction.axis.x, 0, direction.axis.y) * speed * Time.fixedDeltaTime);
    }
}
