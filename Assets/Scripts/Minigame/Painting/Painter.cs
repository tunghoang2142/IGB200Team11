using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Painter : MonoBehaviour
{
    public GameObject brush;
    public Texture2D glowingTexture;
    public Terrain terrain; // Only work on normalize rotation
    public int brushWidth = 50;
    public int brushHeight = 100;
    public float paintAmount = 15f;

    // Set layer to ignore
    public int[] ignoreLayers;

    // Additional paint lost on painting on ignore layer
    public float painPenalty = 1f;

    Texture2D oldTexture;
    float[] layersWeight;
    float widthScale;
    float heightScale;

    readonly int painLayer = 1;
    int layerMask;

    // Start is called before the first frame update
    void Start()
    {
        foreach (int layer in ignoreLayers)
        {
            AddIgnoreLayer(layer);
        }

        // Calculate weight for each layer
        layersWeight = new float[terrain.terrainData.terrainLayers.Length];
        float[,,] splatmap = terrain.terrainData.GetAlphamaps(0, 0, terrain.terrainData.alphamapWidth, terrain.terrainData.alphamapHeight);

        for (int mapWidth = 0; mapWidth < terrain.terrainData.alphamapWidth; mapWidth++)
        {
            for (int mapHeight = 0; mapHeight < terrain.terrainData.alphamapHeight; mapHeight++)
            {
                for (int layer = 0; layer < terrain.terrainData.terrainLayers.Length; layer++)
                {
                    // Check if ignored layer
                    if (layerMask == (layerMask | (1 << layer)))
                    {
                        if (splatmap[mapHeight, mapWidth, layer] > 0)
                        {
                            splatmap[mapHeight, mapWidth, layer] = 1;
                            continue;
                        }
                    }

                    layersWeight[layer] += splatmap[mapHeight, mapWidth, layer];
                }
            }
        }

        terrain.terrainData.SetAlphamaps(0, 0, splatmap);

        if (!brush)
        {
            brush = this.gameObject;
        }

        widthScale = terrain.terrainData.size.x / terrain.terrainData.alphamapWidth;
        heightScale = terrain.terrainData.size.z / terrain.terrainData.alphamapHeight;
        brush.transform.localScale = new Vector3(brushWidth * widthScale, 1, brushHeight * heightScale);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            foreach (var weight in layersWeight)
            {
                print(weight);
            }
        }

        if (PaintingGameManager.Instance.isGamePause || PaintingGameManager.Instance.isGameOver)
        {
            return;
        }

        UpdatePos();
        GlowPaintedArea(painLayer);

        if (Input.GetMouseButton(0))
        {
            Vector3 terrainPoint = GetMousePos() - terrain.GetPosition();
            Vector2 mapPoint = new(Mathf.FloorToInt(terrainPoint.x / widthScale), Mathf.FloorToInt(terrainPoint.z / heightScale));
            Vector2 brushSize = new(brushWidth, brushHeight);

            if (mapPoint.x > terrain.terrainData.alphamapWidth || mapPoint.x + brushSize.x < 0
                || mapPoint.y >= terrain.terrainData.alphamapHeight || mapPoint.y + brushSize.y <= 0)
            {
                return;
            }

            if (mapPoint.x < 0)
            {
                brushSize.x += mapPoint.x;
                mapPoint.x = 0;
            }
            else if (mapPoint.x + brushSize.x > terrain.terrainData.alphamapWidth)
            {
                brushSize.x = terrain.terrainData.alphamapWidth - mapPoint.x;
            }

            if (mapPoint.y < 0)
            {
                brushSize.y += mapPoint.y;
                mapPoint.y = 0;
            }
            else if (mapPoint.y + brushSize.y > terrain.terrainData.alphamapHeight)
            {
                brushSize.y = terrain.terrainData.alphamapHeight - mapPoint.y;
            }

            PaintMap((int)mapPoint.x, (int)mapPoint.y, (int)brushSize.x, (int)brushSize.y);
            paintAmount -= Time.deltaTime;
        }
    }

    Vector3 GetMousePos()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        Plane terrainPlane = new(terrain.transform.up, terrain.transform.position);
        terrainPlane.Raycast(ray, out float distance);
        return ray.GetPoint(distance);
    }

    void UpdatePos()
    {
        // Only work on orthographic projection
        transform.position = GetMousePos() + transform.lossyScale / 2f;
    }

    void GlowPaintedArea(int layer)
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            oldTexture = Terrain.activeTerrain.terrainData.terrainLayers[layer].diffuseTexture;
            Terrain.activeTerrain.terrainData.terrainLayers[layer].diffuseTexture = glowingTexture;
        }
        else if (Input.GetKeyUp(KeyCode.Space))
        {
            Terrain.activeTerrain.terrainData.terrainLayers[layer].diffuseTexture = oldTexture;
        }
    }

    void PaintMap(int x, int y, int width, int height)
    {
        float[,,] splatmap = terrain.terrainData.GetAlphamaps(x, y, width, height);

        for (int mapWidth = 0; mapWidth < width; mapWidth++)
        {
            for (int mapHeight = 0; mapHeight < height; mapHeight++)
            {
                bool isIgnored = false;

                for (int i = 0; i < terrain.terrainData.terrainLayers.Length; i++)
                {
                    // Check if is ignored layer
                    if (layerMask == (layerMask | (1 << i)))
                    {
                        if (splatmap[mapHeight, mapWidth, i] > 0)
                        {
                            // Apply paint penalty
                            paintAmount -= splatmap[mapHeight, mapWidth, painLayer] / (brushWidth * brushHeight) * painPenalty * Time.deltaTime;
                            isIgnored = true;
                            break;
                        }
                    }
                }

                if (isIgnored)
                {
                    continue;
                }

                //TODO: Check number of layers and select layer for painting
                layersWeight[0] -= splatmap[mapHeight, mapWidth, 0];
                splatmap[mapHeight, mapWidth, 0] = 0;
                layersWeight[painLayer] += 1 - splatmap[mapHeight, mapWidth, painLayer];
                splatmap[mapHeight, mapWidth, painLayer] = 1;
            }
        }

        terrain.terrainData.SetAlphamaps(x, y, splatmap);
    }

    void AddIgnoreLayer(int layer)
    {
        layerMask |= (1 << layer);
    }

    public float GetPercentage()
    {
        return GetPercentage(painLayer) * 100;
    }

    float GetPercentage(int layer)
    {
        float total = 0f;
        foreach (float weight in layersWeight)
        {
            total += weight;
        }

        return Mathf.InverseLerp(0, total, layersWeight[layer]);
    }
}
