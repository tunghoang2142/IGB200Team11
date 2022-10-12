using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{
    public static GameObject currentTrigger;
    static SceneController _instance;
    public bool isGamePause;

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

    public static SceneController Instance { get { return _instance; } }

    public static void PlayMinigame()
    {
        Trigger trigger = currentTrigger.GetComponent<Trigger>();
        if (trigger)
        {
            HouseManager.Instance.SetBrokenMaterials(trigger);
            LoadScene(trigger.ScenesName[Random.Range(0, trigger.ScenesName.Length)]);
        }
        else
        {
            Debug.LogError("GameManager - PlayeMinigame: Trigger not found!");
        }
    }

    public void Win()
    {
        Time.timeScale = 1;
        HouseManager.Repair();
        LoadScene(LocalPath.houseLevel);
    }

    public void GameOver()
    {
        Time.timeScale = 1;
        LoadScene(LocalPath.houseLevel);
    }

    public static void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName, LoadSceneMode.Single);
    }

    public static void LoadScene(int sceneNumber)
    {
        SceneManager.LoadScene(sceneNumber, LoadSceneMode.Single);
    }

    public static void Menu()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(0, LoadSceneMode.Single);
    }

    public static void StartGame()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(1, LoadSceneMode.Single);
    }

    public static void Replay()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name, LoadSceneMode.Single);
    }

    public static void Quit()
    {
        Application.Quit();
    }
}
