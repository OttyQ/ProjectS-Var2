using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RewardItemModel
{
    public bool IsInBag { get; private set; }

    public void MoveToBag()
    {
        IsInBag = true;
    }

    public void ResetPosition()
    {
        IsInBag = false;
    }
}
