using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Multiball : Collectable
{
    protected override void ApplyEffect()
    {
        try
        {
            int numBalls = BallsManager.Instance.Balls.Count - 1;
            for (int idx = numBalls; idx >= 0; idx--)
            //            foreach (Ball ball in BallsManager.Instance.Balls)
            {
                // Limit number of balls
                if (BallsManager.Instance.Balls.Count < 10)
                {
                    var ball = BallsManager.Instance.Balls[idx];
                    BallsManager.Instance.SpawnBalls(ball.gameObject.transform.position, 2, ball.isLightningBall);
                }
            }
        }
        catch
        {

        }
    }
}
