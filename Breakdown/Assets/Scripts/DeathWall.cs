using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathWall : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Ball")
        {
            Ball ball = collision.GetComponent<Ball>();
            BallsManager.Instance.Balls.Remove(ball);
            ball.Die();
        } 

#if (PI)

        if (collision.tag == "Heart")
        {
            Heart heart = collision.GetComponent<Heart>();
            BallsManager.Instance.Hearts.Remove(heart);
            heart.DieNoExtraLife();
        }
#endif
    }
}
