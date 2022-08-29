using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RigidNoiseFilter : INoiseFilter
{

    private NoiseSettings.RigidNoiseSettings settings;
    private Noise noise = new Noise();

    public RigidNoiseFilter(NoiseSettings.RigidNoiseSettings noiseSettings)
    {
        this.settings = noiseSettings;
    }

    public float Evaluate(Vector3 point)
    {

        float noiseValue = (noise.Evaluate(point * settings.Roughness + settings.Center) + 1) * .5f;
        noiseValue = 0f;
        float amplitude = 1;
        float frequency = settings.BaseRoughness;
        float weight = 1;

        for (int i = 0; i < settings.NumLayers; i++)
        {
            float v = 1-Mathf.Abs(noise.Evaluate(point * frequency + settings.Center));
            v *= v;
            v *= weight;
            weight = Mathf.Clamp01(v * settings.WeightMultiplier);

            noiseValue += v * amplitude;
            frequency *= settings.Roughness;
            amplitude *= settings.Persistence;
        }

        noiseValue = Mathf.Max(0, noiseValue - settings.MinValue);

        return noiseValue * settings.Strength;
    }


}
