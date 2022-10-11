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

    Texture2D oldTexture;
    float[] layersWeight;
    float widthScale;
    float heightScale;

    // Start is called before the first frame update
    void Start()
    {
        // Calculate weight for each layer
        layersWeight = new float[terrain.terrainData.terrainLayers.Length];
        float[,,] splatmap = terrain.terrainData.GetAlphamaps(0, 0, terrain.terrainData.alphamapWidth, terrain.terrainData.alphamapHeight);
        for (int mapWidth = 0; mapWidth < terrain.terrainData.alphamapWidth; mapWidth++)
        {
            for (int mapHeight = 0; mapHeight < terrain.terrainData.alphamapHeight; mapHeight++)
            {
                for (int layer = 0; layer < terrain.terrainData.terrainLayers.Length; layer++)
                {
                    layersWeight[layer] += splatmap[mapHeight, mapWidth, layer];
                }
            }
        }

        Cursor.visible = false;

        if (!brush)
        {
            brush = this.gameObject;
        }

        widthScale = terrain.terrainData.size.x / terrain.terrainData.alphamapWidth;
        heightScale = terrain.terrainData.size.z / terrain.terrainData.alphamapHeight;
        transform.localScale = new Vector3(brushWidth * widthScale, 1, brushHeight * heightScale);
    }

    // Update is called once per frame
    void Update()
    {
        UpdatePos();
        GlowPaintedArea(1);

        print("Percentage: " + GetPercentage(1) * 100);

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
                brushSize.x = terrain.terrainData.alphamapWidth - mapPoint.x - 1;
            }

            if (mapPoint.y < 0)
            {
                brushSize.y += mapPoint.y;
                mapPoint.y = 0;
            }
            else if (mapPoint.y + brushSize.y > terrain.terrainData.alphamapHeight)
            {
                brushSize.y = terrain.terrainData.alphamapHeight - mapPoint.y - 1;
            }

            PaintMap((int)mapPoint.x, (int)mapPoint.y, (int)brushSize.x, (int)brushSize.y);
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
                //TODO: Check number of layers and select layer for paint
                layersWeight[0] -= splatmap[mapHeight, mapWidth, 0];
                splatmap[mapHeight, mapWidth, 0] = 0;
                layersWeight[1] += 1 - splatmap[mapHeight, mapWidth, 1];
                splatmap[mapHeight, mapWidth, 1] = 1;
            }
        }

        terrain.terrainData.SetAlphamaps(x, y, splatmap);
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
