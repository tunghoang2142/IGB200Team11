using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Plumbing : MonoBehaviour
{
    public GameObject start;
    public GameObject end;
    public float connectDeviance = 0.3f; // How far between connection point can 2 pipes be connected when come into contact

    public float Diameter { get { return GetDiameter(); } }
    public float Length { get { return GetLength(); } }

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
        print(collision.contacts[0].point);
        
        switch (collision.body.tag)
        {
            case "Pipe":
                print("Pipe hit another pipe!");
                break;
            default:
                return;
        }
        PipeGameManager.Instance.GameOver();
    }

    void OnTriggerEnter(Collider other)
    {
        print(other.name);
        switch (other.tag)
        {
            case "Wall":
                print("Pipe hit wall!");
                break;
            case "Goal":
                // Check allignment and deviance
                float distance = (other.transform.position - end.transform.position).magnitude;
                if (this.transform.forward == other.transform.forward && distance <= connectDeviance)
                {
                    PipeGameManager.Instance.End();
                    return;
                }
                break;
            default:
                return;
        }
        PipeGameManager.Instance.GameOver();
    }
}
