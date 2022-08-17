using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Player : MonoBehaviour
{
    private int layerMask = 1 << 6; // Add layer 6 to layer mask
    NavMeshAgent agent;
    public GameObject cube;

    // Start is called before the first frame update
    void Start()
    {
        agent = this.GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
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
            print(hit.transform.position);
            Debug.DrawRay(ray.origin, ray.direction, Color.blue);
            print(hit.point);
            agent.SetDestination(hit.point);
            cube.transform.position = hit.point;
        }
    }
}
