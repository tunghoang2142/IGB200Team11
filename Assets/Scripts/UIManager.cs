using UnityEngine;

public abstract class UIManager : MonoBehaviour
{
    public GameObject pausePanel;

    public void Pause()
    {
        if (PipeGameManager.Instance.isGamePause)
        {
            pausePanel.SetActive(true);
        }
        else
        {
            pausePanel.SetActive(false);
        }
    }

    public abstract void Win();
}
