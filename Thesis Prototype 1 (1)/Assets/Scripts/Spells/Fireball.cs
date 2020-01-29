using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fireball : MonoBehaviour
{

    public GameObject explosionEffect;

    public float radius;

    public float force;

    public int damage;

    // Start is called before the first frame update
    void Start()
    {
        if(this.transform.parent != null)
        {
            this.transform.parent = null;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer != 10)
        {
            Explode();
        }
    }

    //this is a reference from one of Brackey's videos
    void Explode()
    {
        //making sure that there is an actual explosion effect to add
        if (explosionEffect != null)
        {
            Instantiate(explosionEffect, transform.position, Quaternion.identity);
        }

        Collider[] colliders = Physics.OverlapSphere(transform.position, radius);

        foreach (Collider nearbyObject in colliders)
        {
            if (nearbyObject.gameObject.layer != 9 && nearbyObject.gameObject.layer != 10)
            {
                Rigidbody rb = nearbyObject.GetComponent<Rigidbody>();
                if (rb != null)
                {
                    rb.AddExplosionForce(force, transform.position, radius);
                }
                if (nearbyObject.gameObject.GetComponent<BaseEnemy>())
                {
                    StartCoroutine(nearbyObject.gameObject.GetComponent<BaseEnemy>().Knockback(3f));
                }
                if (nearbyObject.GetComponent<NewBaseEnemy>())
                {
                    nearbyObject.GetComponent<NewBaseEnemy>().health -= damage;
                }
            }
        }

        Destroy(this.gameObject);

    }

}
