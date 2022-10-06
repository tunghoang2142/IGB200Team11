using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class PipeUIManager : UIManager
{
    public GameObject winPanel;
    public GameObject gameoverPanel;

    public TMP_Text timer;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public override void Win()
    {
        winPanel.SetActive(true);
    }

    public void GameOver(string cause = "")
    {
        // Do something
        gameoverPanel.SetActive(true);

        if (cause != "")
        {
            gameoverPanel.GetComponentInChildren<TMPro.TextMeshProUGUI>().text = cause;
        }
    }
    
    // will be replaced with a timer bar
    public void DisplayTimer(float second)
    {
        timer.text = "Timer: " + (int)second;
    }
}
