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
            //GetComponent<Renderer>().material = onMaterial;
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
            //GetComponentsInChildren<Renderer>().material = offMaterial;
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void ConnectWire(Wire wire)
    {
        if (!neighbours.Contains(wire))
        {
            neighbours.Add(wire);
        }

        if (isPowered && !wire.isPowered)
        {
            child = wire;
            wire.PowerOn();
        }
    }

    public void DisconnectWire(Wire wire)
    {
        neighbours.Remove(wire);

        if (wire == child)
        {
            child = null;
            wire.PowerOff();
        }
    }

    public void PowerOn()
    {
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
        if (collision.gameObject.tag == this.tag)
        {
            Wire wire = collision.gameObject.GetComponent<Wire>();
            ConnectWire(wire);
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.tag == this.tag)
        {
            Wire wire = collision.gameObject.GetComponent<Wire>();
            DisconnectWire(wire);
        }
    }
}
