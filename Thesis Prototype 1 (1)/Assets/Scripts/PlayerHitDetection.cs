using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHitDetection : MonoBehaviour
{

    public bool gameOver;

    public float timeUntilRestart;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (gameOver)
        {
            //restarting the scene causes unknown errors
            //StartCoroutine(RestartGame());
            gameOver = false;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "EnemyAttack")
        {
            gameOver = true;
        }    
    }

    IEnumerator RestartGame()
    {
        yield return new WaitForSeconds(.2f);
        Time.timeScale = 0;
        yield return new WaitForSecondsRealtime(timeUntilRestart);
        UnityEngine.SceneManagement.SceneManager.LoadScene("SampleScene");
    }

}
