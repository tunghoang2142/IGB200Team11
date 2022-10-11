using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PaintingUIManager : UIManager
{
    public TMP_Text percentageText;
    public TMP_Text paintText;

    public void DisplayPercentage(float percentage)
    {
        percentageText.text = "Percentage: " + percentage + "%";
    }

    public void DisplayPaint(float amount)
    {
        paintText.text = "Paint: " + (int)amount;
    }
}
