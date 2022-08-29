using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleNoiseFilter : INoiseFilter
{

    private NoiseSettings.SimpleNoiseSettings noiseSettings;
    private Noise noise = new Noise();

    public SimpleNoiseFilter(NoiseSettings.SimpleNoiseSettings noiseSettings)
    {
        this.noiseSettings = noiseSettings;
    }

    public float Evaluate(Vector3 point)
    {

        float noiseValue = (noise.Evaluate(point * noiseSettings.Roughness + noiseSettings.Center) + 1) * .5f;
        noiseValue = 0f;
        float amplitude = 1;
        float frequency = noiseSettings.BaseRoughness;

        for (int i = 0; i < noiseSettings.NumLayers; i++)
        {
            float v = noise.Evaluate(point * frequency + noiseSettings.Center);
            noiseValue += (v + 1f) * .5f * amplitude;
            frequency *= noiseSettings.Roughness;
            amplitude *= noiseSettings.Persistence;
        }

        noiseValue = Mathf.Max(0, noiseValue - noiseSettings.MinValue);

        return noiseValue * noiseSettings.Strength;
    }


}
