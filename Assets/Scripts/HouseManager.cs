using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HouseManager : GameManager
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

    public override void Start()
    {
        base.Start();

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
                assetStates[i] = environmentAssets[i].GetComponent<Renderer>().sharedMaterial;
            }
        }
        else
        {
            // Update state
            for (int i = 0; i < environmentAssets.Count; i++)
            {
                Renderer renderer = environmentAssets[i].GetComponent<Renderer>();
                if (assetStates[i] != renderer.sharedMaterial)
                {
                    renderer.material = assetStates[i];
                }
            }
        }
    }

    public void SetBrokenMaterials(Trigger trigger)
    {
        GameObject[] brokenObjects = trigger.brokenObjects;
        brokenMaterials = new Dictionary<Material, Material>();

        for (int i = 0; i < brokenObjects.Length; i++)
        {
            Material key = brokenObjects[i].GetComponent<Renderer>().sharedMaterial;

            string repairedPrefabName = brokenObjects[i].name;

            repairedPrefabName = repairedPrefabName.Substring(0, repairedPrefabName.Length - broken.Length);
            repairedPrefabName += repaired;
            Material value = Resources.Load<GameObject>(LocalPath.repairedEnvironmentAssets + repairedPrefabName).GetComponent<Renderer>().sharedMaterial;

            brokenMaterials.Add(key, value);
        }
    }

    public void SetBrokenMaterials(GameObject[] brokenObjects)
    {
        brokenMaterials = new Dictionary<Material, Material>();

        for (int i = 0; i < brokenObjects.Length; i++)
        {
            Material key = brokenObjects[i].GetComponent<Renderer>().sharedMaterial;

            string repairedPrefabName = brokenObjects[i].name;
            repairedPrefabName = repairedPrefabName.Substring(0, repairedPrefabName.Length - broken.Length);
            repairedPrefabName += repaired;
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

    public override void Win() { return; }
    public override void GameOver(string cause) { return; }
    public override void GameOver() { return; }
}
