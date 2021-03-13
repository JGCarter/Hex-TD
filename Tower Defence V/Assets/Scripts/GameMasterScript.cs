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

    public bool upgrading = false;
    public bool precheckingUpgrade = false;

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
                Debug.Log("Clamping time");
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
    }

    public void PlayUIClick()
    {
        audioSource.Play();
    }

}
