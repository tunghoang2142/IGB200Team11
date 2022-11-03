using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trigger : MonoBehaviour
{
    public string[] ScenesName;
    public GameObject[] brokenObjects;
    public GameObject TutText;
    public void ActivateText()
    {
        TutText.SetActive(true);
    }
    public void DeactivateText()
    {
        TutText.SetActive(false);
    }
}

