using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class WiringUIManager : UIManager
{
    public TMP_Text timer;

    // will be replaced with a timer bar
    public void DisplayTimer(float second)
    {
        timer.text = "Timer: " + (int)second;
    }
}
