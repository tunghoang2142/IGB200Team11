using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HouseManager : MonoBehaviour
{
    readonly static string broken = "damaged";
    readonly static string repaired = "repaired";
    static HouseManager _instance;

    List<GameObject> environmentAssets;
    static Material[] assetStates;

    // TODO: Make both repaired and broken material name systematic will simplify the code
    static Dictionary<Material, Material> brokenMaterials;

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this;
        }
    }

    public static HouseManager Instance { get { return _instance; } }

    private void Start()
    {
        if (environmentAssets == null)
        {
            environmentAssets = new List<GameObject>();
            foreach (Trigger trigger in GameObject.FindObjectsOfType<Trigger>())
            {
                foreach (GameObject brokenObject in trigger.brokenObjects)
                {
                    environmentAssets.Add(brokenObject);
                }
            }
        }

        if (assetStates == null)
        {
            assetStates = new Material[environmentAssets.Count];
            for (int i = 0; i < environmentAssets.Count; i++)
            {
                print(i);
                assetStates[i] = environmentAssets[i].GetComponent<Renderer>().material;
            }
        }

        foreach (Material material in assetStates)
        {
            print(material.name);
        }

        // Update state
        for (int i = 0; i < environmentAssets.Count; i++)
        {
            Renderer renderer = environmentAssets[i].GetComponent<Renderer>();
            if (assetStates[i] != renderer.material)
            {
                renderer.material = assetStates[i];
            }
        }
    }

    public void SetBrokenMaterials(Trigger trigger)
    {
        GameObject[] brokenObjects = trigger.brokenObjects;
        brokenMaterials = new Dictionary<Material, Material>();

        for (int i = 0; i < brokenObjects.Length; i++)
        {
            Material key = brokenObjects[i].GetComponent<Renderer>().material;

            string repairedPrefabName = brokenObjects[i].name;
            print(repairedPrefabName);
            print(repairedPrefabName.Length);
            print(broken.Length);
            repairedPrefabName = repairedPrefabName.Substring(0, repairedPrefabName.Length - broken.Length);

            repairedPrefabName += repaired;
            print(repairedPrefabName);
            print(LocalPath.repairedEnvironmentAssets + repairedPrefabName);
            Material value = Resources.Load<GameObject>(LocalPath.repairedEnvironmentAssets + repairedPrefabName).GetComponent<Renderer>().sharedMaterial;

            brokenMaterials.Add(key, value);
        }
    }

    public void SetBrokenMaterials(GameObject[] brokenObjects)
    {
        brokenMaterials = new Dictionary<Material, Material>();

        for (int i = 0; i < brokenObjects.Length; i++)
        {
            Material key = brokenObjects[i].GetComponent<Renderer>().material;

            string repairedPrefabName = brokenObjects[i].name;
            print(repairedPrefabName);
            print(repairedPrefabName.Length);
            print(broken.Length);
            repairedPrefabName = repairedPrefabName.Substring(0, repairedPrefabName.Length - broken.Length);

            repairedPrefabName += repaired;
            print(repairedPrefabName);
            print(LocalPath.repairedEnvironmentAssets + repairedPrefabName);
            Material value = Resources.Load<GameObject>(LocalPath.repairedEnvironmentAssets + repairedPrefabName).GetComponent<Renderer>().sharedMaterial;

            brokenMaterials.Add(key, value);
        }
    }

    public static void Repair()
    {
        for (int i = 0; i < assetStates.Length; i++)
        {
            if (brokenMaterials.ContainsKey(assetStates[i]))
            {
                assetStates[i] = brokenMaterials[assetStates[i]];
            }
        }
    }
}
