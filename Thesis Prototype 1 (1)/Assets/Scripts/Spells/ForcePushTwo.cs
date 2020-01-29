using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForcePushTwo : MonoBehaviour
{
    [Tooltip("How strong the blast initially is")]
    public float pushForce;
    [Tooltip("How much percent to decrease the blast by per second")]
    [Range(0, 1)]
    public float decreasingValue;

    [Tooltip("How fast this goes forward")]
    public float speed;

    float maxForce;

    Rigidbody rb;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        maxForce = pushForce;
        decreasingValue *= pushForce;
    }

    // Update is called once per frame
    void Update()
    {
        //decrease the strength of the blast as it goes on        
        pushForce -= decreasingValue * Time.deltaTime;
    }

    private void FixedUpdate()
    {
        rb.MovePosition(rb.position + transform.forward * speed * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        try
        {
            Rigidbody otherRb = other.GetComponent<Rigidbody>();
            otherRb.AddForce(pushForce * transform.forward.normalized);
        }
        catch { }
    }

    public void DeactivateBlast()
    {
        Destroy(this.gameObject);
    }

}
