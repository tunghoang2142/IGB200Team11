using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PipeGameManager : MonoBehaviour
{
    public PipeUIManager UIManager;
    public static string PipePrefabName = "Pipe";
    public static string CornerPrefabName = "Coupling";
    public bool isGamePause = false;
    public bool isGameOver = false;
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

    // Start is called before the first frame update
    void Start()
    {
        if (!UIManager)
        {
            UIManager = this.gameObject.GetComponent<PipeUIManager>();
        }

        Time.timeScale = 1;
    }

    // Update is called once per frame
    void Update()
    {
        if(isGamePause || isGameOver)
        {
            return;
        }

        timer -= Time.deltaTime;
        UIManager.DisplayTimer(timer);
        if(timer < 0)
        {
            GameOver("Time over!");
        }
    }

    public void Pause()
    {
        isGamePause = !isGamePause;
        if (isGamePause)
        {
            Time.timeScale = 0;
        }
        else
        {
            Time.timeScale = 1;
        }

        UIManager.Pause();
    }

    public void GameOver(string cause = "")
    {
        isGameOver = true;
        Time.timeScale = 0;
        UIManager.GameOver(cause);
        print("Game Over!");
    }

    public void Win()
    {
        print("Game end.");
        isGameOver = true;
        Time.timeScale = 0;
        UIManager.Win();
    }
}
