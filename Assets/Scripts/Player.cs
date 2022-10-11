using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Player : MonoBehaviour
{
    public bool isMoveable = true;

    private int layerMask;
    NavMeshAgent agent;

    // TODO: Put this to UI Manager
    public GameObject jobPanel;

    // Start is called before the first frame update
    void Start()
    {
        layerMask = LayerMask.GetMask("Ground");
        agent = this.GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0) && isMoveable)
        {
            Move();
        }
    }

    void Move()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, Mathf.Infinity, layerMask))
        {
            agent.SetDestination(hit.point);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        Trigger trigger = other.GetComponent<Trigger>();
        if (trigger)
        {
            isMoveable = false;
            agent.SetDestination(other.gameObject.transform.position);
            SceneController.currentTrigger = other.gameObject;
            jobPanel.SetActive(true);
        }
    }

    public void AcceptJob()
    {
        SceneController.PlayMinigame();
    }

    public void DeclineJob()
    {
        isMoveable = true;
        jobPanel.SetActive(false);
    }
}
