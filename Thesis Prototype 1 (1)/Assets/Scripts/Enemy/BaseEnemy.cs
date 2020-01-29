using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseEnemy : MonoBehaviour
{
    Pathing path;

    public float range;

    public float attackRate;

    public bool inKnockback;

    public enum EnemyType { Archer, Zombie}
    public EnemyType type;

    public enum EnemyState { Idle, Moving, Attacking}
    public EnemyState state;

    bool isAttacking;

    [Header("Archer Variables")]
    public float attackRange;
    public GameObject arrow;
    public float shootForce;

    // Start is called before the first frame update
    void Start()
    {
        path = this.GetComponent<Pathing>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 direction = path.target.position - this.transform.position;
        if (direction.magnitude < range)
        {
            RaycastHit hit;
            // is the ray hitting something
            if (Physics.Raycast(transform.position, direction.normalized, out hit, attackRange))
            {
                if(hit.collider.tag == "Player" || hit.collider.tag == "EnemyAttack")
                {
                    Debug.DrawRay(transform.position, direction.normalized * hit.distance, Color.yellow);
                    //the enemy has line of sight on the player
                    //stop the attacker's pursuit
                    path.thisObject.isStopped = true;
                    if (!isAttacking)
                    {
                        StartCoroutine(Attack());
                    }
                }
                else
                {
                    //continue on the path until it does hit the player
                    Debug.DrawRay(transform.position, direction.normalized * hit.distance, Color.blue);
                    path.thisObject.isStopped = false;
                }
            }
            else
            {
                path.thisObject.isStopped = false;
                Debug.DrawRay(transform.position, direction.normalized * range, Color.white);
            }
        }
    }

    public IEnumerator Knockback(float time)
    {
        inKnockback = true;
        yield return new WaitForSeconds(time);
        inKnockback = false;
    }

    IEnumerator Attack()
    {
        isAttacking = true;
        Vector3 direction = path.target.position - this.transform.position;
        direction.y = 0;
        var currentArrow = Instantiate(arrow, transform.position, Quaternion.identity);
        currentArrow.GetComponent<Rigidbody>().AddForce(direction.normalized * shootForce);
        currentArrow.transform.forward = direction.normalized;
        yield return new WaitForSeconds(attackRate);
        isAttacking = false;
    }

}
