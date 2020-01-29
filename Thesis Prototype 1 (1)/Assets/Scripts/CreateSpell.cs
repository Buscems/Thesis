using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

public class CreateSpell : MonoBehaviour
{

    //this is for testing if people enjoy the gameplay of making shapes and casting spells like that
    public GameObject waterDragon;
    public float dragonTime;
    public float dragonOffset;
    public GameObject fireTornado;
    public float tornadoTime;
    public float tornadoOffset;
    public GameObject summonRock;
    public float rockTime;
    public float rockOffset;
    public GameObject waterGun;
    public float gunTime;

    public GameObject hand;

    public GameObject playerCamera;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

        //if(AirSigManager.OnPlayerGestureMatch)

        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            var dragon = Instantiate(waterDragon, waterDragon.transform.position, Quaternion.identity);
            dragon.transform.position += new Vector3(playerCamera.transform.forward.x, 0, playerCamera.transform.forward.z) * dragonOffset;
            StartCoroutine(DestroySpell(dragon, dragonTime));
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            var tornado = Instantiate(fireTornado, fireTornado.transform.position, Quaternion.identity);
            tornado.transform.position += new Vector3(playerCamera.transform.forward.x, 0, playerCamera.transform.forward.z) * tornadoOffset;
            StartCoroutine(DestroySpell(tornado, tornadoTime));
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            var rock = Instantiate(summonRock, summonRock.transform.position, Quaternion.identity);
            rock.transform.position += new Vector3(playerCamera.transform.forward.x, 0, playerCamera.transform.forward.z) * rockOffset;
            //StartCoroutine(DestroySpell(rock, rockTime));
        }
        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            var gun = Instantiate(waterGun, hand.transform.position, Quaternion.identity);
            gun.transform.parent = hand.transform;
            gun.transform.rotation = hand.transform.rotation;
            gun.transform.localPosition = new Vector3(-.075f, 0, 0);
            StartCoroutine(DestroySpell(gun, gunTime));
        }

    }

    IEnumerator DestroySpell(GameObject currentSpell, float time)
    {
        yield return new WaitForSeconds(time);
        Destroy(currentSpell);
    }

}
