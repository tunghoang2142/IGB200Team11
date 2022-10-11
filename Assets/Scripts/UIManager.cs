using UnityEngine;

public abstract class UIManager : MonoBehaviour
{
    public GameObject pausePanel;
    public GameObject winPanel;
    public GameObject gameoverPanel;

    public virtual void Pause()
    {
        if (SceneController.Instance.isGamePause)
        {
            pausePanel.SetActive(true);
        }
        else
        {
            pausePanel.SetActive(false);
        }
    }

    public virtual void Win()
    {
        winPanel.SetActive(true);
    }

    public virtual void GameOver(string cause)
    {
        gameoverPanel.SetActive(true);

        if (cause != "")
        {
            gameoverPanel.GetComponentInChildren<TMPro.TextMeshProUGUI>().text = cause;
        }
    }

    public virtual void GameOver()
    {
        gameoverPanel.SetActive(true);
    }
}
