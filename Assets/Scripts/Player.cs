using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Player : MonoBehaviour
{
    public bool isMoveable = true;

    private int layerMask = 1 << 6; // Add layer 6 to layer mask
    NavMeshAgent agent;
    public GameObject cube;

    // TODO: Put this to UI Manager
    public GameObject jobPanel;
    // TODO: Put this to Game Manager
    public string currentJob;
    

    // Start is called before the first frame update
    void Start()
    {
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

        //Debug.DrawRay(ray.origin, ray.direction, Color.red);

        if (Physics.Raycast(ray, out hit))
        {
            print(hit.transform.name);
            Debug.DrawRay(ray.origin, ray.direction, Color.blue);
            print(hit.point);
            agent.SetDestination(hit.point);
            cube.transform.position = hit.point;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        Trigger trigger = other.GetComponent<Trigger>();
        if (trigger)
        {
            isMoveable = false;
            currentJob = trigger.SceneName;
            jobPanel.SetActive(true);
        }
    }

    // TODO: Put this to Game Manager
    public void AcceptJob()
    {
        GameManager.LoadScene(currentJob);
    }

    public void DeclineJob()
    {
        isMoveable = true;
        jobPanel.SetActive(false);
    }
}
