using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameMasterScript : MonoBehaviour
{
    public int gold = 1000;
    public Text goldText;

    public int lives = 20;
    public Text livesText;

    public int wave = 1;
    public Text waveText;

    public int mobsRemaining = 0;

    public bool upgrading = false;
    public bool precheckingUpgrade = false;

    public bool waveInProgress = false;
    public bool buildingPhase = true;

    AudioSource audioSource;

    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        goldText.text = gold.ToString();
        livesText.text = lives.ToString();
        waveText.text = wave.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        if (upgrading == true && Time.timeScale > 0.02)
        {
            Time.timeScale = Time.timeScale/(1 + (2f * Time.unscaledDeltaTime));           
            Time.fixedDeltaTime = Time.timeScale * 0.02f;
        }

        else if (upgrading == false && Time.timeScale < 1)
        {
            Time.timeScale = Time.timeScale * (1 + (4f * Time.unscaledDeltaTime));
            Time.fixedDeltaTime = Time.timeScale * 0.02f;
            if(Time.timeScale > 1)
            {
                //Debug.Log("Clamping time");
                Time.timeScale = 1;
            }
        }
    }

    public void SetUpgradeingValue(bool upgradeDesire)
    {
        upgrading = upgradeDesire;
    }

    public void ChangePrecheckingUpgradeValue(bool BlurDesire)
    {
        precheckingUpgrade = BlurDesire;
    }

    public bool SubtractGold(int value)
    {
        if (gold - value < 0)
        {
            return false;
        }

        else
        {
            gold = gold - value;
            goldText.text = gold.ToString();
            return true;
        }
    }

    public void AddGold(int value)
    {
        gold = gold + value;
        goldText.text = gold.ToString();
    }

    public void SubtractLives(int value)
    {
        lives = lives - value;
        livesText.text = lives.ToString();
    }

    public void AddLives(int value)
    {
        lives = lives + value;
        livesText.text = lives.ToString();
    }

    public void IncrementWave()
    {
        wave++;
        waveText.text = wave.ToString();
        if (wave == 5)
        {
            EndBuildingPhase();
            Debug.Log("Game over");
        }
    }

    public void PlayUIClick()
    {
        audioSource.Play();
    }

    public void StartWave()
    {
        waveInProgress = true;
        buildingPhase = false;
        Debug.Log("Wave Spawning");
    }

    public void EndWave()
    {
        waveInProgress = false;
        Debug.Log("WaveFinishedSpawning");
    }

    public bool getWaveStatus()
    {
        return waveInProgress;
    }

    public void StartBuildingPhase()
    {
        buildingPhase = true;
        Debug.Log("Building Phase");
    }

    public void EndBuildingPhase()
    {
        buildingPhase = false;
        Debug.Log("Building Phase ended");
    }

    public bool getBuildingPhase()
    {
        return buildingPhase;
    }

}
