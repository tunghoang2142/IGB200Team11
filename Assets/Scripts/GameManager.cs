using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameObject currentTrigger;
    static GameManager _instance;
    static Material[] brokenMaterials;
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

    public static GameManager Instance { get { return _instance; } }

    public static void PlayMinigame()
    {
        Trigger trigger = currentTrigger.GetComponent<Trigger>();
        if (trigger)
        {
            brokenMaterials = new Material[trigger.brokenObjects.Length];
            for(int i = 0; i < trigger.brokenObjects.Length; i++)
            {
                brokenMaterials[i] = trigger.brokenObjects[i].GetComponent<Renderer>().material;
            }
            LoadScene(trigger.SceneName);
        }
        else
        {
            Debug.LogError("GameManager - PlayeMinigame: Trigger not found!");
        }
    }

    public void Win()
    {
        print("A");
        foreach (Material material in brokenMaterials)
        {
            print("B");
            HouseManager.Repair(material);
            print("C");
        }
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
