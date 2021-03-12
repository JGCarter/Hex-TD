using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonFadeInScript : MonoBehaviour
{

    Material material;
    Color colour;

    // Start is called before the first frame update
    void Start()
    {
        material = gameObject.GetComponent<Image>().material;
        colour = material.color;
        colour.a = 0f;
        material.color = colour;
    }

    // Update is called once per frame
    void Update()
    {
        if (colour.a < 0.8)
        {
            colour.a = colour.a + (2f * Time.unscaledDeltaTime);
            material.color = colour;
        }
    }
}
