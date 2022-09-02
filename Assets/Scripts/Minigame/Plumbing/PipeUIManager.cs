using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class PipeUIManager : MonoBehaviour
{
    public TMP_Text timer;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Pause()
    {
        if (PipeGameManager.Instance.isGamePause)
        {
            // Do something
        }
        else
        {
            // Do something
        }
    }

    public void End()
    {
        // Do something
        SceneManager.LoadScene("Win screen");
    }

    public void GameOver()
    {
        // Do something
        SceneManager.LoadScene("Game over");
    }

    public void DisplayTimer(float second)
    {
        timer.text = "Timer: " + (int)second;
    }
}
