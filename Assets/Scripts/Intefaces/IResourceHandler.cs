using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IResourceHandler
{
    void UseShovel();

    bool CanDig();

    void AddGold();
}
