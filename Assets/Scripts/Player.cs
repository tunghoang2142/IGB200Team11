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
    private Animator anim;
    public GameObject avatar;
    // TODO: Put this to UI Manager
    public GameObject jobPanel;
    public GameObject JobsTr1;
    public GameObject JobsTr2;
    public GameObject JobsTr3;
    private Trigger JobTrig1;
    private Trigger JobTrig2;
    private Trigger JobTrig3;

    // Start is called before the first frame update
    public override void Start()
    {
        base.Start();
        layerMask = LayerMask.GetMask("Ground");
        anim = avatar.GetComponent<Animator>();
        JobTrig1 = JobsTr1.GetComponent<Trigger>();
        JobTrig2 = JobsTr2.GetComponent<Trigger>();
        JobTrig3 = JobsTr3.GetComponent<Trigger>();
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
        //anim.SetFloat("Speed", stoppingDistance);
        if (agent.velocity == Vector3.zero)
        {
            anim.SetBool("Walking", false);
        }
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
                anim.SetBool("Walking", false);
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
        anim.SetBool("Walking", true);
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
            trigger.ActivateText();
            
            //anim.SetBool("Walking", false);
        }
    }

    public void AcceptJob()
    {
        SceneController.PlayMinigame();
        JobTrig1.DeactivateText();
        JobTrig2.DeactivateText();
        JobTrig3.DeactivateText();
    }

    public void DeclineJob()
    {
        agent.isStopped = false;
        //isMoveable = true;
        jobPanel.SetActive(false);
        JobTrig1.DeactivateText();
        JobTrig2.DeactivateText();
        JobTrig3.DeactivateText();
    }
}
