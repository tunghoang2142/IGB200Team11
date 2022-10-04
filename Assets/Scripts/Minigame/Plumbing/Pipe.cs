using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pipe : Plumbing
{
    public float minTurningDistance = 1f;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (PipeGameManager.Instance.isGameOver || PipeGameManager.Instance.isGamePause)
        {
            return;
        }

        Drag();
    }

    void Drag()
    {
        if (!Input.GetMouseButton(0))
        {
            isDragging = false;
            return;
        }

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (isDragging)
        {
            float distance = Camera.main.transform.position.y - end.transform.position.y;
            Vector3 mousePos = ray.GetPoint(distance);
            Vector3 dragDirection = mousePos - end.transform.position;

            float height = new Vector3(dragDirection.x * end.transform.right.x, dragDirection.y * end.transform.right.y, dragDirection.z * end.transform.right.z).magnitude;

            // Turning the pipe
            if (height > Diameter && height > minTurningDistance)
            {
                GameObject corner = Resources.Load<GameObject>(LocalPath.prefabs + PipeGameManager.CornerPrefabName);
                corner = Instantiate(corner, end.transform.position, end.transform.rotation);
                PipeCorner script = corner.GetComponent<PipeCorner>();
                script.isDragging = true;
                StrechPipe(-script.Length / 2);

                // Rotate the corner depend on the direction of the mouse
                float turningAngle = Vector3.Angle(end.transform.right, dragDirection);
                if (turningAngle > 90)
                {
                    corner.GetComponent<PipeCorner>().Flip();
                }

                Destroy(this);
                return;
            }

            float angle = Vector3.Angle(end.transform.forward, dragDirection);

            // Do nothing if mouse is behind object
            if (angle > 90f)
            {
                return;
            }

            float length = new Vector3(dragDirection.x * end.transform.forward.x, dragDirection.y * end.transform.forward.y, dragDirection.z * end.transform.forward.z).magnitude;
            float expansionLimit = expansionSpeed * Time.deltaTime;

            if (length > expansionLimit)
            {
                length = expansionLimit;
            }

            StrechPipe(length);

            return;
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

    public void StrechPipe(float length)
    {
        // Normalization of rotation before scaling
        Quaternion rotation = end.transform.rotation;
        this.transform.rotation = new Quaternion();

        // Scale and rotate back
        Vector3 scale = end.transform.forward * length;
        this.transform.localScale += scale;
        this.transform.rotation = rotation;

        // Translate object to new pos after scaling
        this.transform.position += end.transform.forward * length / 2;
    }
}
