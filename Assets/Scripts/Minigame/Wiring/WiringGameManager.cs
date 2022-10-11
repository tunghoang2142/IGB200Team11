using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WiringGameManager : GameManager
{
    public Wire goal1;
    public Wire goal2;

    public float timer = 30f;

    private int layerMask;

    // Start is called before the first frame update
    public override void Start()
    {
        base.Start();
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
            Win();
            return;
        }

        timer -= Time.deltaTime;
        ((WiringUIManager) UIManager).DisplayTimer(timer);
        if (timer < 0)
        {
            GameOver("Time over!");
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
