using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightningBolt : MonoBehaviour
{

    public Vector3 direction;
    public float speed;
    Rigidbody rb;

    public int damage;

    // Start is called before the first frame update
    void Start()
    {
        rb = this.GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        rb.MovePosition(rb.position + direction * speed * Time.fixedDeltaTime);
    }

    private void OnCollisionEnter(Collision collision)
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer != 9 && other.gameObject.layer != 10 && other.gameObject.layer != 13)
        {
            //whatever code would need to happen to the enemy

            if (other.GetComponent<NewBaseEnemy>())
            {
                other.GetComponent<NewBaseEnemy>().health -= damage;
                Destroy(this.gameObject);
            }
            //Destroy(this.gameObject);
        }
    }

}
