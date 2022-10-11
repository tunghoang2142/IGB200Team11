using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PipeGameManager : GameManager
{
    public static string PipePrefabName = "Pipe";
    public static string ElbowPrefabName = "PipeElbow";
    public float timer = 30f;

    static PipeGameManager _instance;

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

    public static PipeGameManager Instance { get { return _instance; } }

    // Update is called once per frame
    void Update()
    {
        if (isGamePause || isGameOver)
        {
            return;
        }

        timer -= Time.deltaTime;
        ((PipeUIManager)UIManager).DisplayTimer(timer);
        if (timer < 0)
        {
            GameOver("Time over!");
        }
    }
}
