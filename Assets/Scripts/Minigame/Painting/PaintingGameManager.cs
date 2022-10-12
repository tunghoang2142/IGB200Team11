using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PaintingGameManager : GameManager
{
    public Painter painter;
    static PaintingGameManager _instance;
    float[,,] splatmap;

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

    public static PaintingGameManager Instance { get { return _instance; } }

    public override void Start()
    {
        splatmap = painter.terrain.terrainData.GetAlphamaps(0, 0, painter.terrain.terrainData.alphamapWidth, painter.terrain.terrainData.alphamapHeight);
        base.Start();
        Cursor.visible = false;
    }

    private void Update()
    {
        if (isGamePause || isGameOver)
        {
            return;
        }

        float percentage = painter.GetPercentage();
        ((PaintingUIManager)UIManager).DisplayPercentage(percentage);
        if (percentage > 99)
        {
            Win();
            return;
        }

        ((PaintingUIManager)UIManager).DisplayPaint(painter.paintAmount);
        if (painter.paintAmount < 0)
        {
            GameOver("You run out of paint!");
        }
    }

    public override void Win()
    {
        base.Win();
        Cursor.visible = true;
    }

    public void ResetMap()
    {
        painter.terrain.terrainData.SetAlphamaps(0, 0, splatmap);
    }
}
