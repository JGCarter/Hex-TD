using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PulseBombNoisesScript : MonoBehaviour
{

    AudioSource audioSource;
    public AudioClip pulseSound;
    public AudioClip burstSound;
    float startTime;
    float burst = 1.5f;
    float burstAddition = 1.65f;
    float pitch = 1;

    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        startTime = Time.time;
    }

    // Update is called once per frame
    void Update()
    {
        float timeSinceInitialisation = Time.time - startTime;
        if (timeSinceInitialisation < 10)
        {
            //Debug.Log(timeSinceInitialisation);
            if (timeSinceInitialisation > burst)
            {
                //Debug.Log("playing pulse sound");
                audioSource.PlayOneShot(pulseSound);
                burst = burst + (burstAddition);
                burstAddition = burstAddition / 1.58f + 0.05f;
                audioSource.pitch = pitch;
                pitch = pitch * 1.08f;
            }
        }
        else
        {
            audioSource.pitch = 1;
            audioSource.PlayOneShot(burstSound);
            Destroy(this);
        }
    }
}
