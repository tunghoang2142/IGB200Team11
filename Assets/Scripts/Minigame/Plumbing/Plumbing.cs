using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Plumbing : MonoBehaviour
{
    public GameObject start;
    public GameObject end;
    public float connectDeviance = 0.3f; // How far between connection point can 2 pipes be connected when come into contact
    public float expansionSpeed = 3f; // How fast pipe can be expanded
    public float collisionTimer = 0.1f; // How long collision stay before trigger collision event
    public bool isDragging = false;

    // Delay before new pipe can be created
    public static float createTimer = 0f;
    public static readonly float createDelay = 0.3f;

    public float Diameter { get { return GetDiameter(); } }
    public float Length { get { return GetLength(); } }

    public virtual void Update()
    {
        createTimer -= Time.deltaTime;
    }

    public bool CanCreate()
    {
        if (isDragging && createTimer <= 0f)
        {
            return true;
        }
        return false;
    }

    float GetLength()
    {
        // Matrix respresent this transform
        Matrix4x4 transform = this.transform.localToWorldMatrix;
        // Rotate back to 0 degree
        Matrix4x4 rotation = Matrix4x4.Rotate(this.transform.rotation).inverse;
        // Scale it with forward vector
        Matrix4x4 scale = Matrix4x4.Scale(this.transform.forward);
        transform = transform * rotation * scale;
        // Return the right scale of this transform
        return transform.lossyScale.magnitude;
    }

    float GetDiameter()
    {
        // Matrix respresent this transform
        Matrix4x4 transform = this.transform.localToWorldMatrix;
        // Rotate back to 0 degree
        Matrix4x4 rotation = Matrix4x4.Rotate(this.transform.rotation).inverse;
        // Scale it with right vector
        Matrix4x4 scale = Matrix4x4.Scale(this.transform.right);
        transform = transform * rotation * scale;
        // Return the right scale of this transform
        return transform.lossyScale.magnitude;
    }

    private void OnCollisionStay(Collision collision)
    //private void OnCollisionEnter(Collision collision)
    {
        //print(collision.body.name + ": " + collision.contacts[0].point);

        switch (collision.body.tag)
        {
            case "Pipe":
                if (collisionTimer > 0)
                {
                    collisionTimer -= Time.deltaTime;
                    return;
                }
                PipeGameManager.Instance.GameOver("Pipe hit another pipe!");
                return;
            default:
                break;
        }
    }

    void OnTriggerEnter(Collider other)
    {
        print(other.name);
        switch (other.tag)
        {
            case "Wall":
                print("Pipe hit wall!");
                PipeGameManager.Instance.GameOver("Pipe hit wall!");
                return;
            case "Goal":
                // Check allignment and deviance
                float distance = (other.transform.position - end.transform.position).magnitude;
                if (this.transform.forward == other.transform.forward && distance <= connectDeviance)
                {
                    PipeGameManager.Instance.Win();
                    return;
                }
                else
                {
                    PipeGameManager.Instance.GameOver("Pipes are too far apart!");
                }
                break;
            default:
                break;
        }

    }
}
