using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class LightDimmer : MonoBehaviour
{
    public float minIntensityGlow = 2.0f;
    public float maxIntensityGlow = 4.5f;

    public Color emmisionColor = Color.white;
    public Material materialGlow;

    public Light2D light;
    public float minLightIntensity = 1.0f;
    public float maxLightIntensity = 3.0f;

    public float timeInterval = 2.0f;

    private Color maxIntensity;
    private Color minIntensity;

    private void Start()
    {
        if(light != null)
            light.color = emmisionColor;

        float maxFactor = Mathf.Pow(maxIntensityGlow, 2);
        maxIntensity = emmisionColor;
        maxIntensity = new Color(maxIntensity.r * maxFactor, maxIntensity.g * maxFactor, maxIntensity.b * maxFactor);

        float minFactor = Mathf.Pow(minIntensityGlow, 2);
        minIntensity = emmisionColor;
        minIntensity = new Color(minIntensity.r * minFactor, minIntensity.g * minFactor, minIntensity.b * minFactor);
    }
    private void Update()
    {
        materialGlow.SetColor("_GlowColor", Color.Lerp(minIntensity, maxIntensity, Mathf.Sin(Time.time * timeInterval)));

        if (light != null)
            light.intensity = Mathf.Lerp(minLightIntensity, maxLightIntensity,Mathf.Sin(Time.time * timeInterval));
    }
}
