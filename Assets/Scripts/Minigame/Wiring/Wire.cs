using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wire : MonoBehaviour
{
    public Material onMaterial;
    public Material offMaterial;

    public bool isPowered;

    public ArrayList neighbours = new();
    public Wire child;

    // Start is called before the first frame update
    void Start()
    {
        if (!onMaterial)
        {
            onMaterial = Resources.Load<Material>(LocalPath.materials + this.tag + "On");
            offMaterial = Resources.Load<Material>(LocalPath.materials + this.tag + "Off");
        }

        if (isPowered)
        {
            foreach (Renderer renderer in GetComponentsInChildren<Renderer>())
            {
                renderer.material = onMaterial;
            }
        }
        else
        {
            foreach (Renderer renderer in GetComponentsInChildren<Renderer>())
            {
                renderer.material = offMaterial;
            }
        }
    }

    public void ConnectWire(Wire wire)
    {
        //print(gameObject.GetComponentInParent<Transform>().gameObject.name + ": " + "Connect ");
        if (!neighbours.Contains(wire))
        {
            neighbours.Add(wire);

            if (isPowered && !wire.isPowered)
            {
                if (child)
                {
                    child.PowerOff();
                }
                child = wire;
                wire.PowerOn();
            }
        }
    }

    public void DisconnectWire(Wire wire)
    {
        //print(gameObject.GetComponentInParent<Transform>().gameObject.name + ": " + "Disconnect ");
        if (neighbours.Contains(wire))
        {
            neighbours.Remove(wire);

            if (wire == child)
            {
                child = null;
                wire.PowerOff();
            }
            else if (wire.child == this)
            {
                wire.child = null;
                PowerOff();
            }
        }
    }

    public void PowerOn()
    {
        //print(gameObject.GetComponentInParent<Transform>().gameObject.name + ": " + "On");
        isPowered = true;

        foreach (Renderer renderer in GetComponentsInChildren<Renderer>())
        {
            renderer.material = onMaterial;
        }

        if (child)
        {
            child.PowerOn();
        }
        else
        {
            // Check one unpowered neighbour
            foreach (Wire wire in neighbours)
            {
                if (!wire.isPowered)
                {
                    child = wire;
                    wire.PowerOn();
                    return;
                }
            }
        }
    }

    public void PowerOff()
    {
        //print(gameObject.GetComponentInParent<Transform>().gameObject.name + ": " + "Off");
        isPowered = false;
        foreach (Renderer renderer in GetComponentsInChildren<Renderer>())
        {
            renderer.material = offMaterial;
        }

        if (child)
        {
            child.PowerOff();
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        //print(gameObject.GetComponentInParent<Transform>().gameObject.name + ": " + "Enter");
        if (collision.gameObject.tag == this.tag)
        {
            Wire wire = collision.gameObject.GetComponent<Wire>();
            ConnectWire(wire);
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        //print(GetComponentInParent<Transform>().gameObject.name + ": " + "Exist");
        if (collision.gameObject.tag == this.tag)
        {
            Wire wire = collision.gameObject.GetComponent<Wire>();
            DisconnectWire(wire);
        }
    }
}
