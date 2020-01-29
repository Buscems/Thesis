using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shield : MonoBehaviour
{

    public float destroyTimer;

    [HideInInspector]
    public bool stopTimer;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!stopTimer)
        {
            destroyTimer -= Time.deltaTime;
        }

        if(destroyTimer <= 0)
        {
            this.GetComponent<Animator>().SetTrigger("disappear");
        }

    }

    public void DestroyThis()
    {
        Destroy(this.gameObject);
    }

}
