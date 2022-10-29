using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class Player : Human
{
    //public bool isMoveable = true;

    private int layerMask;
    //NavMeshAgent agent;

    // TODO: Put this to UI Manager
    public GameObject jobPanel;

    // Start is called before the first frame update
    public override void Start()
    {
        base.Start();
        layerMask = LayerMask.GetMask("Ground");
        //FinishTalking();
        //Talk("A");
    }

    // Update is called once per frame
    void Update()
    {
        //print(agent.pathStatus);
        //print(agent.stoppingDistance);
        //if (Input.GetMouseButtonDown(0) && isMoveable)
        //{
        //    Move();
        //}
        
    }

    public override bool Moveable()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        // TODO: Limit layer mask to ground and NPC
        if (Physics.Raycast(ray, out hit))
        {
            if(hit.collider.CompareTag("NPC"))
            {
                return false;
            }
        }

        if (Input.GetMouseButton(0) && !agent.isStopped)
        {
            return true;
        }

        return false;
    }

    public void Move(float stoppingDistance = 0f)
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, Mathf.Infinity, layerMask))
        {
            agent.stoppingDistance = stoppingDistance;
            agent.SetDestination(hit.point);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        Trigger trigger = other.GetComponent<Trigger>();
        if (trigger)
        {
            agent.isStopped = true;
            //isMoveable = false;
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
        agent.isStopped = false;
        //isMoveable = true;
        jobPanel.SetActive(false);
    }
}
