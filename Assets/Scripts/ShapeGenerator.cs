using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShapeGenerator
{
    private ShapeSettings settings;
    private INoiseFilter[] noiseFilters;
    public MinMax ElevationMinMax;

    public void UpdateSettings(ShapeSettings shapeSettings)
    {
        this.settings = shapeSettings;
        noiseFilters = new INoiseFilter[settings.NoiseLayers.Length];
        for (int i = 0; i < noiseFilters.Length; i++)
        {
            noiseFilters[i] = NoiseFilterFactory.CreateNoiseFilter(settings.NoiseLayers[i].NoiseSettings);
        }
        ElevationMinMax = new();
    }

    public Vector3 CalculatePointOnPlanet(Vector3 pointOnUnitSphere)
    {

        float firstLayerValue = 0;
        float elevation = 0f;

        if(noiseFilters.Length > 0)
        {
            firstLayerValue = noiseFilters[0].Evaluate(pointOnUnitSphere);
            if (settings.NoiseLayers[0].Enabled)
            {
                elevation = firstLayerValue;
            }
        }

        for (int i = 1; i < noiseFilters.Length; i++)
        {
            if (!settings.NoiseLayers[i].Enabled) continue;

            float mask = (settings.NoiseLayers[i].UseFirstLayerAsMask) ? firstLayerValue : 1;
            elevation += noiseFilters[i].Evaluate(pointOnUnitSphere) * mask;
        }

        elevation = settings.PlanetRadius * (1 + elevation);
        ElevationMinMax.AddValue(elevation);

        return pointOnUnitSphere * elevation;
    }
}
