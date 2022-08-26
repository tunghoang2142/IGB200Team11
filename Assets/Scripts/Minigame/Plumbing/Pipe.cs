using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pipe : MonoBehaviour
{
    public GameObject child;
    public GameObject start;
    public GameObject end;

    public float connectDeviance = 0.3f; // How far between connection point can 2 pipes be connected when come into contact

    bool isDragging = false;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        //this.transform.position += this.transform.forward * Time.deltaTime;
        if (child)
        {
            Destroy(this);
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

            // Do nothing if mouse pos is behind object
            float angle = Vector3.Angle(end.transform.forward, dragDirection);
            if (angle > 90f)
            {
                return;
            }

            float length = new Vector3(dragDirection.x * end.transform.forward.x, dragDirection.y * end.transform.forward.y, dragDirection.z * end.transform.forward.z).magnitude;

            // TODO check for direction changing

            //

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


        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            if (hit.transform.gameObject == this.gameObject)
            {
                isDragging = true;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        print(other.name);
    }
}
