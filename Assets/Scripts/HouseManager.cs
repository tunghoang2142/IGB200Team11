using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HouseManager : MonoBehaviour
{
    readonly static string broken = "damaged";
    readonly static string repaired = "repaired";

    public Renderer[] environmentAssets;
    public static Material[] assetStates;
    static HouseManager _instance;

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

    // Start is called before the first frame update
    void Start()
    {
        if (assetStates == null)
        {
            assetStates = new Material[environmentAssets.Length];

            for (int i = 0; i < environmentAssets.Length; i++)
            {
                assetStates[i] = environmentAssets[i].GetComponent<Renderer>().material;
            }
            return;
        }

        for (int i = 0; i < environmentAssets.Length; i++)
        {
            if (assetStates[i] != environmentAssets[i])
            {
                environmentAssets[i].GetComponent<Renderer>().material = assetStates[i] ;
            }
        }
    }

    public static void Repair(Material brokenMaterial)
    {
        print(brokenMaterial.name);
        for (int i = 0; i < assetStates.Length; i++)
        {
            if (assetStates[i] == brokenMaterial)
            {
                string materialName = brokenMaterial.name;
                materialName = materialName.Substring(0, materialName.Length - repaired.Length);
                materialName += broken;
                print(materialName);
                assetStates[i] = Resources.Load<Material>(LocalPath.repairedEnvironmentAssets + materialName);
                return;
            }
        }
    }
}
