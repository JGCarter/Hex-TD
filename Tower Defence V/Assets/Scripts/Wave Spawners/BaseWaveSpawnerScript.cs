using System.Collections;
using UnityEngine;
using UnityEngine.UI;


public class BaseWaveSpawnerScript : MonoBehaviour
{
    protected GameMasterScript gameMasterScript;

    public Transform[] mobPrefabs;

    public Transform[] spawnPoints;
    int spawnpointNumber;

    public float timeBetweenWaves = 1000f;
    public Text waveCoundownText;
    protected float countdown = 0.5f;

    protected int WaveIndex = 1;



    protected void Start()
    {
        spawnpointNumber = spawnPoints.Length;
        gameMasterScript = GetComponent<GameMasterScript>();
    }



    protected virtual void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && (gameMasterScript.getBuildingPhase()))
        {
            gameMasterScript.StartWave();
            StartCoroutine(SpawnWave());
        }
        
        if (!gameMasterScript.waveInProgress && gameMasterScript.mobsRemaining <= 0 && !gameMasterScript.getBuildingPhase())
        {
            gameMasterScript.StartBuildingPhase();
            gameMasterScript.IncrementWave();
        }

        countdown -= Time.deltaTime;

        waveCoundownText.text = Mathf.Ceil(countdown).ToString();
    }



    protected virtual IEnumerator SpawnWave()
    {
        int spawnPointIndex = 0;

        //Debug.Log("Wave Incoming");

        for (int i = 0; i < WaveIndex*10; i++)
        {
            SpawnMob(0, spawnPointIndex);
            yield return new WaitForSeconds(0.2f);
        }

        WaveIndex++;

        gameMasterScript.EndWave();
    }



    protected virtual void SpawnMob(int mobIndex, int spawnPointIndex)
    {
        Instantiate(mobPrefabs[mobIndex], spawnPoints[spawnPointIndex].position + new Vector3(0f, 1f, 0f), Quaternion.Euler(0, 0, 0));
        spawnPointIndex++;
        gameMasterScript.mobsRemaining++;

    }


}
