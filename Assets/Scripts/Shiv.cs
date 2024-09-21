using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shiv : CardAction
{
    public override void CardActionActivate()
    {
        base.CardActionActivate();

        Debug.Log("Using Shiv");
    }
}
