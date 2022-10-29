using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HouseUIManager : UIManager
{
    public GameObject canvas;
    public readonly Vector3 seclectionBtnOffset = new(0, -150f, 0);

    public override void Win() { return; }
    public override void GameOver(string cause) { return; }
    public override void GameOver() { return; }


}
