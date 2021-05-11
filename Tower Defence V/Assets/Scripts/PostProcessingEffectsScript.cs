using System.Collections;
using System.Collections.Generic;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine;

public class PostProcessingEffectsScript : MonoBehaviour
{

    ColorAdjustments colorAdjustments;

    // Start is called before the first frame update
    void Start()
    {
        VolumeProfile profile = GetComponent<Volume>().profile;
        profile.TryGet(out colorAdjustments);
        //colorAdjustments.postExposure.value = -1f;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void darkenScreenEffect(float duration)
    {
        colorAdjustments.postExposure.value = -3f;
        StartCoroutine(NormaliseTint(duration));
        //NormaliseTint(duration);

    }

    private IEnumerator NormaliseTint(float duration)
    {
        while (colorAdjustments.postExposure.value < 0)
        {
            float adjustment = Time.deltaTime * (2f / duration);
            colorAdjustments.postExposure.value = colorAdjustments.postExposure.value + adjustment;
            yield return 0;
        }
        //colorAdjustments.postExposure.value = 0f;
    }
}
