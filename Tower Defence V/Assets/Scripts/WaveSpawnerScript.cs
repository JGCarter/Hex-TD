using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class WaveSpawnerScript : MonoBehaviour
{
    GameMasterScript gameMasterScript;

    public Transform mobPrefab;
    //public Transform bossMobPrefab;

    public Transform[] spawnPoints;
    int spawnpointNumber;
    int spawnpointIndex = 1;

    public float timeBetweenWaves = 5f;
    public Text waveCoundownText;
    private float countdown = 0.5f;
    //private Vector3 BossHeightAdjust = new Vector3(0f, 0.5f, 0f);

    private int WaveIndex = 0;

    private void Start()
    {
        spawnpointNumber = spawnPoints.Length;
        gameMasterScript = GetComponent<GameMasterScript>();
    }

    void Update()
    {
        if (countdown <= 0f )
        {
            StartCoroutine(SpawnWave());
            countdown = timeBetweenWaves;
            gameMasterScript.IncrementWave();
        }

        countdown -= Time.deltaTime;

        waveCoundownText.text = Mathf.Ceil(countdown).ToString();
    }

    IEnumerator SpawnWave()
    {
        //Debug.Log("Wave Incoming");
        WaveIndex++;
        //if (WaveIndex % 5 == 1)
            //SpawnBossMob();
        for (int i = 0; i < WaveIndex; i++)
        {
            SpawnMob();
            yield return new WaitForSeconds(0.2f);
        }
    }

    void SpawnMob()
    {       
        Instantiate(mobPrefab, spawnPoints[spawnpointIndex % spawnpointNumber].position - new Vector3(0f, 1f, 0f), Quaternion.Euler(0, 0, 0));
        spawnpointIndex++;
    }

    //void SpawnBossMob()
    //{
    //    Instantiate(bossMobPrefab, spawnPoint.position + BossHeightAdjust, spawnPoint.rotation);
    //}
}
