using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Frenzy : Collectable
{
    // Start is called before the first frame update
    protected override void ApplyEffect()
    {
        if ((Paddle.Instance != null) && (Paddle.Instance.PaddleIsTransforming == false))
        {
            Paddle.Instance.StartWidthAnimation(2.5f);
        }
        foreach (var ball in BallsManager.Instance.Balls)
        {
            ball.StartLightningBall();
        }
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
                    BallsManager.Instance.SpawnBalls(ball.gameObject.transform.position, 5, ball.isLightningBall);
                }
            }
        }
        catch
        {

        }
    }
}
