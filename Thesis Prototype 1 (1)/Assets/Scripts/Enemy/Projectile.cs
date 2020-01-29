using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{

    public float despawnTimer;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(DespawnTimer());
    }

    // Update is called once per frame
    void Update()
    {
        transform.up = this.GetComponent<Rigidbody>().velocity;
    }

    private void OnTriggerEnter(Collider other)
    {
        Destroy(this.gameObject);
    }

    IEnumerator DespawnTimer()
    {
        yield return new WaitForSeconds(despawnTimer);
        Destroy(this.gameObject);
    }

}
