using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC : Human
{
    public bool isTalking;
    public Vector3 stationaryPos;
    //TMPro.TMP_Text text;

    // Start is called before the first frame update
    public override void Start()
    {
        base.Start();
        stationaryPos = transform.position;
        //textbox = Instantiate(Resources.Load<GameObject>(LocalPath.prefabs + "Textbox"), this.transform);
        //text = textbox.GetComponentInChildren<TMPro.TMP_Text>();
    }


    //public void SetText(string text)
    //{
    //    this.text.text = text;
    //}

    public override bool Moveable()
    {
        if (!agent.isStopped)
        {
            return true;
        }
        return false;
    }
}
