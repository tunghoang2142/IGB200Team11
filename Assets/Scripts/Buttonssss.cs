using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Buttonssss : MonoBehaviour
{
    public void PlayButton()
    {
        SceneManager.LoadScene("TestMovement");
    }

    public void ControlsButton()
    {

    }

    public void OptionsButton()
    {

    }

    public void PlayPipe()
    {
        SceneManager.LoadScene("Pipe");
    }

    public void MainMenuButton()
    {
        SceneManager.LoadScene("Main Menu");
    }

    public void ExitButton()
    {
        Application.Quit();
    }
}
