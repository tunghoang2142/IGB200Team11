using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class GameManager : MonoBehaviour
{
    public UIManager UIManager;
    public bool isGamePause = false;
    public bool isGameOver = false;

    // Start is called before the first frame update
    public virtual void Start()
    {
        if (!UIManager)
        {
            UIManager = this.gameObject.GetComponent<UIManager>();
        }

        Time.timeScale = 1;
    }

    public virtual void Pause()
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

    public virtual void GameOver()
    {
        isGameOver = true;
        Time.timeScale = 0;
        UIManager.GameOver();
    }

    public virtual void GameOver(string cause = "")
    {
        isGameOver = true;
        Time.timeScale = 0;
        UIManager.GameOver(cause);
    }

    public virtual void Win()
    {
        isGameOver = true;
        Time.timeScale = 0;
        UIManager.Win();
    }
}
