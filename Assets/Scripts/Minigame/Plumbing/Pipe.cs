using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pipe : Plumbing
{
    //public float Diameter { get { return GetDiameter(); } }
    //public float Length { get { return GetLength(); } }

    public float minTurningDistance = 1f;
    public bool isDragging = false;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        //Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        //Debug.DrawLine(this.transform.position, ray.GetPoint(Camera.main.transform.position.y - end.transform.position.y), Color.red);

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
                GameObject coupling = Resources.Load<GameObject>(LocalPath.prefabs + "Coupling");
                coupling = Instantiate(coupling);
                coupling.transform.rotation = end.transform.rotation;
                coupling.transform.position = end.transform.position;
                StrechPipe(-coupling.GetComponent<PipeCoupling>().Length / 2);

                // Rotate the coupling depend on the direction of the mouse
                float turningAngle = Vector3.Angle(end.transform.right, dragDirection);
                if (turningAngle > 90)
                {
                    coupling.GetComponent<PipeCoupling>().Flip();
                }

                Destroy(this);
                return;
            }

            float angle = Vector3.Angle(end.transform.forward, dragDirection);

            // Do nothing if mouse pos is behind object
            if (angle > 90f)
            {
                return;
            }

            float length = new Vector3(dragDirection.x * end.transform.forward.x, dragDirection.y * end.transform.forward.y, dragDirection.z * end.transform.forward.z).magnitude;
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

    //float GetLength()
    //{
    //    // Matrix respresent this transform
    //    Matrix4x4 transform = this.transform.localToWorldMatrix;
    //    // Rotate back to 0 degree
    //    Matrix4x4 rotation = Matrix4x4.Rotate(this.transform.rotation).inverse;
    //    // Scale it with forward vector
    //    Matrix4x4 scale = Matrix4x4.Scale(this.transform.forward);
    //    transform = transform * rotation * scale;
    //    // Return the right scale of this transform
    //    return transform.lossyScale.magnitude;
    //}

    //float GetDiameter()
    //{
    //    // Matrix respresent this transform
    //    Matrix4x4 transform = this.transform.localToWorldMatrix;
    //    // Rotate back to 0 degree
    //    Matrix4x4 rotation = Matrix4x4.Rotate(this.transform.rotation).inverse;
    //    // Scale it with right vector
    //    Matrix4x4 scale = Matrix4x4.Scale(this.transform.right);
    //    transform = transform * rotation * scale;
    //    // Return the right scale of this transform
    //    return transform.lossyScale.magnitude;
    //}
}
