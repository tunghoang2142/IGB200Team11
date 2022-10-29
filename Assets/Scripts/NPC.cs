using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC : Human
{
    public bool isTalking;
    public GameObject follow;
    public Vector3 stationaryPos;
    //TMPro.TMP_Text text;

    // Start is called before the first frame update
    public override void Start()
    {
        base.Start();
        stationaryPos = transform.position;
    }

    public override bool Moveable()
    {
        if (!agent.isStopped)
        {
            return true;
        }
        return false;
    }
}
