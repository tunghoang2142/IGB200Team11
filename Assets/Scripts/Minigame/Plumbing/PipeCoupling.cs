using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PipeCoupling : Plumbing
{
    public float minPipeLength = 0.5f;
    float bufferLength = 0.01f;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (!Input.GetMouseButton(0) || PipeGameManager.Instance.isGameOver)
        {
            return;
        }

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        float distance = Camera.main.transform.position.y - end.transform.position.y;
        Vector3 mousePos = ray.GetPoint(distance);
        Vector3 dragDirection = mousePos - end.transform.position;

        if (dragDirection.magnitude > minPipeLength)
        {
            GameObject pipe = Resources.Load<GameObject>(LocalPath.prefabs + "Pipe");
            pipe = Instantiate(pipe);
            Pipe script = pipe.GetComponent<Pipe>();
            pipe.transform.rotation = end.transform.rotation;
            pipe.transform.position += end.transform.position - script.start.transform.position;

            if (script.Length > minPipeLength)
            {
                script.StrechPipe(minPipeLength - script.Length + bufferLength);
            }
            else
            {
                script.StrechPipe(script.Length - minPipeLength + bufferLength);
            }
            Destroy(this);
        }
    }

    public void Flip()
    {
        Vector3 rotation = this.transform.up * 180f;
        this.transform.Rotate(rotation);
    }
}
