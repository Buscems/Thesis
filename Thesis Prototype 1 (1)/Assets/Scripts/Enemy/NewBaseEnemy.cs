using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewBaseEnemy : MonoBehaviour
{

    Transform target;

    public float range;

    public float attackRate;

    bool canMove;

    [HideInInspector]
    public Vector3 direction;

    public enum EnemyType { Archer, Zombie }
    public EnemyType type;

    public enum EnemyState { Idle, Moving, Attacking, Death }
    public EnemyState state;

    bool isAttacking;

    [Header("Archer Variables")]
    public float attackRange;
    public GameObject arrow;
    public float shootForce;

    public int maxHealth;
    //[HideInInspector]
    public int health;

    bool startDeath;

    // Start is called before the first frame update
    void Start()
    {
        target = GameObject.Find("Main Camera").transform;

        canMove = true;

        health = maxHealth;

    }

    // Update is called once per frame
    void Update()
    {
        if (state != EnemyState.Death)
        {
            direction = (target.position - this.transform.position).normalized;
            if (canMove)
            {
                this.GetComponent<EnemyMovement>().enabled = true;
            }
            else
            {
                this.GetComponent<EnemyMovement>().enabled = false;
            }

            if (type == EnemyType.Archer)
            {
                Archer();
            }

            if (health <= 0)
            {
                if (!startDeath)
                {
                    StartCoroutine(Death());
                }
            }
        }

    }

    void Archer()
    {
        if (direction.magnitude < range)
        {
            if (!isAttacking)
            {
                StartCoroutine(ArcherAttack());
            }
            canMove = false;
        }
    }

    IEnumerator ArcherAttack()
    {
        isAttacking = true;
        //direction = target.position - this.transform.position;
        direction.y = 0.1f;
        var currentArrow = Instantiate(arrow, transform.position, Quaternion.identity);
        currentArrow.GetComponent<Rigidbody>().AddForce(direction.normalized * shootForce);
        currentArrow.transform.forward = direction.normalized;
        yield return new WaitForSeconds(attackRate);
        isAttacking = false;
    }

    IEnumerator Death()
    {
        startDeath = true;
        state = EnemyState.Death;
        yield return new WaitForSeconds(3);
        Destroy(this.gameObject);
    }

}
