using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PipeCoupling : MonoBehaviour
{
    public GameObject start;
    public GameObject end;
    public float minDistance = 0.5f; //TODO: Rename this

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (!Input.GetMouseButton(0))
        {
            return;
        }

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        float distance = Camera.main.transform.position.y - end.transform.position.y;
        Vector3 mousePos = ray.GetPoint(distance);
        Vector3 dragDirection = mousePos - end.transform.position;
        print(dragDirection.magnitude);

        if (dragDirection.magnitude > minDistance)
        {
            GameObject pipe = Resources.Load<GameObject>(LocalPath.prefabs + "Pipe");
            pipe = Instantiate(pipe);
            Pipe script = pipe.GetComponent<Pipe>();
            pipe.transform.rotation = end.transform.rotation;
            pipe.transform.position = end.transform.position;
            pipe.transform.position += end.transform.position - script.start.transform.position;

            if (script.Length > minDistance)
            {
                script.StrechPipe(minDistance - script.Length);
            }
            else
            {
                script.StrechPipe(script.Length - minDistance);
            }
            Destroy(this);
        }
    }
}
