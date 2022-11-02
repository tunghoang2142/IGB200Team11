using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PipeElbow : Plumbing
{
    public static float minPipeLength = 0.3f;
    float bufferLength = 0.01f;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    public override void Update()
    {
        base.Update();
        if (!Input.GetMouseButton(0) || PipeGameManager.Instance.isGameOver || PipeGameManager.Instance.isGamePause)
        {
            return;
        }

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (CanCreate())
        {
            float distance = Camera.main.transform.position.y - end.transform.position.y;
            Vector3 mousePos = ray.GetPoint(distance);
            Vector3 dragDirection = mousePos - end.transform.position;

            if (dragDirection.magnitude > minPipeLength)
            {
                createTimer = createDelay;
                GameObject pipe = Resources.Load<GameObject>(LocalPath.prefabs + PipeGameManager.PipePrefabName);
                pipe = Instantiate(pipe);
                Pipe script = pipe.GetComponent<Pipe>();
                script.isDragging = true;
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

        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            if (hit.transform.gameObject == this.gameObject)
            {
                isDragging = true;
            }
        }

    }

    public void Flip()
    {
        Vector3 rotation = this.transform.up * 180f;
        this.transform.Rotate(rotation);
    }
}
