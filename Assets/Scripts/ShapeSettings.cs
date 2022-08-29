using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu()]
public class ShapeSettings : ScriptableObject
{
    public float PlanetRadius = 1f;
    public NoiseLayer[] NoiseLayers;

    [System.Serializable]
    public class NoiseLayer
    {

        public bool Enabled = true;
        public bool UseFirstLayerAsMask = false;
        public NoiseSettings NoiseSettings;

    }

}
