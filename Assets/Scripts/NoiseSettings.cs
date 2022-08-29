using UnityEngine;

[System.Serializable]
public class NoiseSettings
{

    public enum FilterType
    {
        Simple, Rigid
    }
    public FilterType Type;

    [ConditionalHide("Type", 0)]

    public SimpleNoiseSettings simpleNoiseSettings;
    [ConditionalHide("Type", 1)]
    public RigidNoiseSettings rigidNoiseSettings;


    [System.Serializable]
    public class SimpleNoiseSettings
    {
        public float Strength = 1f;
        [Range(1, 8)]
        public int NumLayers = 1;
        public float BaseRoughness = 2f;
        public float Roughness = 1f;
        public float Persistence = .5f;
        public Vector3 Center;
        public float MinValue;
    }

    [System.Serializable]
    public class RigidNoiseSettings : SimpleNoiseSettings
    {
        public float WeightMultiplier = 0.8f;
    }

}
