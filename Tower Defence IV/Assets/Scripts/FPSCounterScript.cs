using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FPSCounterScript : MonoBehaviour
{

    public float FPS = 0;
    public Text FPSCounterText;
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        FPS = (1f / Time.smoothDeltaTime);
        FPSCounterText.text = FPS.ToString();
    }
}
