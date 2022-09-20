using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WiringGameManager : MonoBehaviour
{
    public Wire goal1;
    public Wire goal2;

    public bool isGamePause = false;
    public bool isGameOver = false;
    public float timer = 30f;

    private int layerMask;

    // Start is called before the first frame update
    void Start()
    {
        layerMask = LayerMask.GetMask("Ground");
    }

    // Update is called once per frame
    void Update()
    {
        if (isGameOver || isGameOver)
        {
            return;
        }

        if (goal1.isPowered && goal2.isPowered)
        {
            End();
            return;
        }

        timer -= Time.deltaTime;
        //UIManager.DisplayTimer(timer);
        if (timer < 0)
        {
            //GameOver();
        }

        if (Input.GetMouseButtonUp(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, Mathf.Infinity, layerMask))
            {
                TurnTile(hit.collider.gameObject);
            }
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

        //UIManager.Pause();
    }

    public void GameOver()
    {
        isGameOver = true;
        Time.timeScale = 0;
        //UIManager.GameOver();
        print("Game Over!");
    }

    public void End()
    {
        print("Game end.");
        isGameOver = true;
        Time.timeScale = 0;
        //UIManager.End();
        SceneManager.LoadScene("Win screen W");
    }

    void TurnTile(GameObject tile)
    {
        Wire[] wires = tile.GetComponentsInChildren<Wire>();

        // Turn off power of wire in tile before turning
        foreach (Wire wire in wires)
        {
            wire.PowerOff();
        }

        Vector3 rotation = this.transform.up * 90f;
        tile.transform.Rotate(rotation);

    }
}
