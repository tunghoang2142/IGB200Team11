using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public abstract class Human : MonoBehaviour
{
    public NavMeshAgent agent;

    GameObject textbox;

    //TODO: Quick fix clipping issue
    float offsetDistance = 5f;
    //

    // Start is called before the first frame update
    public virtual void Start()
    {
        agent = this.GetComponent<NavMeshAgent>();
    }

    public void StopMoving()
    {
        Rigidbody rigidbody = GetComponent<Rigidbody>();
        rigidbody.velocity = Vector3.zero;
        rigidbody.angularDrag = 0f;
        agent.velocity = Vector3.zero;
    }

    public void Talk(string text)
    {
        textbox = Instantiate(Resources.Load<GameObject>(LocalPath.prefabs + "Textbox"), this.gameObject.transform);
        textbox.GetComponentInChildren<TMPro.TMP_Text>().text = text;

        //TODO: Quick fix clipping issue
        Ray ray = new Ray(Camera.main.transform.position, textbox.transform.position - Camera.main.transform.position);
        textbox.transform.position = ray.GetPoint(offsetDistance);
        //textbox.transform.position = textbox.transform.localPosition + ray.GetPoint(offsetDistance);
        //
    }

    public void FinishTalking()
    {
        Destroy(textbox);
    }

    public abstract bool Moveable();
}
