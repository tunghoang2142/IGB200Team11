using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
    public Camera playerCamera;
    public GameObject player;
    public GameObject hidenObject;

    public Vector3 offset;


    // Start is called before the first frame update
    void Start()
    {
        if (!playerCamera)
        {
            playerCamera = Camera.main;
        }
        if(offset.magnitude == 0)
        {
            offset = playerCamera.transform.position - player.transform.position;
        }
    }

    // Update is called once per frame
    void Update()
    {
        playerCamera.transform.position = player.transform.position + offset;

        Ray ray = new Ray(playerCamera.transform.position, player.transform.position - playerCamera.transform.position);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            if(hit.collider.gameObject != player && hit.collider.CompareTag("Wall"))
            {
                if (hidenObject)
                {
                    hidenObject.SetActive(true);
                }
                hidenObject = hit.collider.gameObject;
                hidenObject.SetActive(false);
            }
        }
    }
}
