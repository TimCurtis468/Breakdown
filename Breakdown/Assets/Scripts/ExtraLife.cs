using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExtraLife : Collectable
{
    protected override void ApplyEffect()
    {
        GameManager.Instance.AddLife();
    }
}
