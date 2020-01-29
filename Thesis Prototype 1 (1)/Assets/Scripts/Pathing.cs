using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Pathing : MonoBehaviour
{
    [HideInInspector]
    public NavMeshAgent thisObject;

    public Transform target;

    BaseEnemy baseEnemy;

    // Start is called before the first frame update
    void Start()
    {
        thisObject = this.GetComponent<NavMeshAgent>();
        baseEnemy = this.GetComponent<BaseEnemy>();
        target = GameObject.Find("Main Camera").transform;
    }

    // Update is called once per frame
    void Update()
    {
        if (!baseEnemy.inKnockback)
        {
            thisObject.enabled = true;
            try
            {
                thisObject.SetDestination(target.position);
            }
            catch
            {
                //this is when there is no target
            }
        }
        else
        {
            thisObject.enabled = false;
        }

    }
}
