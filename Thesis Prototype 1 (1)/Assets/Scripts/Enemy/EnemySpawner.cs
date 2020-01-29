using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{

    public GameObject enemy;
    public GameObject[] spawnPoint;

    public float timeBetweenSpawns;

    bool spawning;

    public GameObject enemyActive;
    public GameObject enemyInactive;

    [HideInInspector]
    public bool startEnemies;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //this is for testing
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Instantiate(enemy, spawnPoint[Random.Range(0, spawnPoint.Length)].transform.position, Quaternion.identity);
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            Instantiate(enemy, spawnPoint[Random.Range(0, spawnPoint.Length)].transform.position, Quaternion.identity);
        }

        if (startEnemies)
        {
            enemyInactive.SetActive(false);
            enemyActive.SetActive(true);
            if (!spawning)
            {
                //StartCoroutine(EnemySpawning());
                StartCoroutine(SpawnEnemy());
            }
            
        }
        else
        {
            enemyActive.SetActive(false);
            enemyInactive.SetActive(true);
        }

    }

    IEnumerator SpawnEnemy()
    {
        spawning = true;
        Instantiate(enemy, spawnPoint[Random.Range(0, spawnPoint.Length)].transform.position, Quaternion.identity);
        yield return new WaitForSeconds(.5f);
        startEnemies = false;
        spawning = false;
    }

    IEnumerator EnemySpawning()
    {
        spawning = true;
        Instantiate(enemy, spawnPoint[Random.Range(0, spawnPoint.Length)].transform.position, Quaternion.identity);
        yield return new WaitForSeconds(timeBetweenSpawns);
        spawning = false;
    }

}
