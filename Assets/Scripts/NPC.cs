using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC : Human
{
    public bool isTalking;
    public GameObject follow;
    public Vector3 stationaryPos;
    private Animator anim;
    public GameObject avatar;
    //TMPro.TMP_Text text;

    // Start is called before the first frame update
    public override void Start()
    {
        base.Start();
        stationaryPos = transform.position;
        anim = avatar.GetComponent<Animator>();
    }

    public void Update()
    {
        if (agent.isStopped)
        {
            anim.SetBool("Walking", true);
        }
        else 
        {
            anim.SetBool("Walking", false);
        }
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
