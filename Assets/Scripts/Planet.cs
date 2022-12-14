using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Planet : MonoBehaviour
{

    [Range(2, 256)]
    public int resolution = 10;
    public bool autoUpdate = true;

    public enum FaceRenderMask { All,Top,Bottom,Left,Right,Front,Back};

    public FaceRenderMask faceRenderMask = FaceRenderMask.All;

    [SerializeField,HideInInspector]
    private MeshFilter[] meshFilters;

    private TerrainFace[] terrainFaces;

    public ShapeSettings ShapeSettings;

    public ColorSettings ColorSettings;

    [HideInInspector]
    public bool ShapeSettingsFoldout;

    [HideInInspector]
    public bool ColorSettingsFoldout;

    private ShapeGenerator shapeGenerator = new();
    private ColorGenerator colorGenerator = new();


    private void OnValidate()
    {
        GeneratePlanet();
    }

    public void OnShapeSettingsUpdated()
    {
        if (!autoUpdate) return;

        Initialize();
        GenerateMesh();
    }
    public void OnColorSettingsUpdated()
    {
        if (!autoUpdate) return;

        Initialize();
        GenerateColors();
    }

    private void Initialize()
    {

        shapeGenerator.UpdateSettings(ShapeSettings);
        colorGenerator.UpdateSettings(ColorSettings);

        if (meshFilters == null || meshFilters.Length == 0)
        {
            meshFilters = new MeshFilter[6];
        }

        
        terrainFaces = new TerrainFace[6];

        Vector3[] directions = { Vector3.up, Vector3.down, Vector3.left, Vector3.right, Vector3.forward, Vector3.back };

        for (int i = 0; i < 6; i++)
        {

            if(meshFilters[i] == null)
            {
                GameObject meshObj = new GameObject("mesh");
                meshObj.transform.parent = transform;

                meshObj.AddComponent<MeshRenderer>();
                meshFilters[i] = meshObj.AddComponent<MeshFilter>();
                meshFilters[i].sharedMesh = new Mesh();
            }

            meshFilters[i].GetComponent<MeshRenderer>().sharedMaterial = ColorSettings.PlanetMaterial;

            terrainFaces[i] = new TerrainFace(shapeGenerator,meshFilters[i].sharedMesh, resolution, directions[i]);
            bool renderFace = faceRenderMask == FaceRenderMask.All || (int)faceRenderMask - 1 == i;
            meshFilters[i].gameObject.SetActive(renderFace);

        }

    }

    public void GeneratePlanet()
    {
        Initialize();
        GenerateMesh();
        GenerateColors();
    }

    private void GenerateMesh()
    {
        for (int i = 0; i < 6; i++)
        {
            if (!meshFilters[i].gameObject.activeSelf) continue;

            terrainFaces[i].ConstructMesh();

        }

        colorGenerator.UpdateElevation(shapeGenerator.ElevationMinMax);
    }

    private void GenerateColors()
    {
        colorGenerator.UpdateColors();
    }

}
