using System.Collections;
using UnityEngine;

public class WaveSpawnerTheRavine001 : BaseWaveSpawnerScript
{
    protected override IEnumerator SpawnWave()
    {
        int spawnPointIndex = 0;

        //Debug.Log("Wave Incoming");

        for (int i = 0; i < WaveIndex * 10; i++)
        {
            SpawnMob(0, spawnPointIndex);
            yield return new WaitForSeconds(0.2f);
        }


        if (WaveIndex % 3 == 2)
            SpawnMob(1, spawnPointIndex);

        WaveIndex++;

        gameMasterScript.EndWave();
    }
}
