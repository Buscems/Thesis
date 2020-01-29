using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForcePush : MonoBehaviour
{
    [Tooltip("How strong the blast initially is")]
    public float pushForce;
    [Tooltip("How much percent to decrease the blast by per second")]
    [Range(0,1)]
    public float decreasingValue;

    float maxForce;

    // Start is called before the first frame update
    void Start()
    {
        maxForce = pushForce;
        decreasingValue *= pushForce; 
    }

    // Update is called once per frame
    void Update()
    {
        //decrease the strength of the blast as it goes on        
        pushForce -= decreasingValue * Time.deltaTime;
    }

    private void OnTriggerEnter(Collider other)
    {
        Rigidbody rb = other.GetComponent<Rigidbody>();
        rb.AddForce(pushForce * (other.transform.position - this.transform.position).normalized);
    }

    public void DeactivateBlast()
    {
        pushForce = maxForce;
        this.gameObject.SetActive(false);
    }

}
