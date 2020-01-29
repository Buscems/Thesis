using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrailObject : MonoBehaviour
{
    [Tooltip("How long I want the trail to last")]
    public float timeLength;

    TrailRenderer tr;

    // Start is called before the first frame update
    void Start()
    {
        tr = GetComponent<TrailRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //this will make the trail and gameobject disappear when it's time to get rid of the drawing

    public IEnumerator TimeUntilDestroy()
    {
        yield return new WaitForSeconds(timeLength);
        tr.Clear();
        Destroy(this.gameObject);
    }

}
