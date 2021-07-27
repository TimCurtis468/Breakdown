using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExtendOrShrink : Collectable
{
    public float NewWidth = 2.5f;
    protected override void ApplyEffect()
    {
        if ((Paddle.Instance != null) && (Paddle.Instance.PaddleIsTransforming == false))
        {
            Paddle.Instance.StartWidthAnimation(NewWidth);
        }
    }
}
