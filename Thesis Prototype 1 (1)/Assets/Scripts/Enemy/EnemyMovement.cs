using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{

    Rigidbody rb;

    public float speed;

    Vector3 direction;

    NewBaseEnemy baseEnemy;

    // Start is called before the first frame update
    void Start()
    {

        rb = GetComponent<Rigidbody>();

        rb.constraints = RigidbodyConstraints.FreezeRotation;

        baseEnemy = GetComponent<NewBaseEnemy>();

    }

    // Update is called once per frame
    void Update()
    {
        direction = baseEnemy.direction;
    }

    private void FixedUpdate()
    {
        rb.MovePosition(rb.position + direction * speed * Time.fixedDeltaTime);
    }

}
