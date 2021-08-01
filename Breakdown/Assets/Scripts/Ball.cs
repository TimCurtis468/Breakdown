using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour
{
    public bool isLightningBall;
    private SpriteRenderer sr;

    public ParticleSystem lightningBallEffect;

    public float lightningBallDuration = 10;

    public static event Action<Ball> OnBallDeath;
    public static event Action<Ball> OnLightningBallEnable;
    public static event Action<Ball> OnLightningBallDisable;

    private void Awake()
    {
        this.sr = GetComponentInChildren<SpriteRenderer>();
        Utilities.ResizeSprite(this.gameObject);
    }

    private void OnCollisionEnter2D(Collision2D coll)
    {
        switch(coll.gameObject.tag)
        {
            case "Ball":
            case "Paddle":
                SoundFxManager.Instance.PlaySoundFx(false);
                break;
            case "Brick":
                SoundFxManager.Instance.PlaySoundFx(true);
                Rigidbody2D ballRbx = this.GetComponent<Rigidbody2D>();

                /* Check if low X velocity and speed it up if it is */
                if ((ballRbx.velocity.x >= 0.0f) &&
                   (ballRbx.velocity.x < 0.4f))
                {
                    ballRbx.velocity = new Vector2(ballRbx.velocity.x + 0.4f, ballRbx.velocity.y);
//                    Debug.Log("Ball collision Added X velocity" + coll.gameObject.tag);
                }
                else if ((ballRbx.velocity.x <= 0.0f) &&
                         (ballRbx.velocity.x > -0.4f))
                {
                    ballRbx.velocity = new Vector2(ballRbx.velocity.x - 0.4f, ballRbx.velocity.y);
//                    Debug.Log("Ball collision Subtracted X velocity" + coll.gameObject.tag);
                }

                break;
            case "Block":
                SoundFxManager.Instance.PlaySoundFx(true);
                break;
            case "Walls":
                SoundFxManager.Instance.PlaySoundFx(true);

                Rigidbody2D ballRb = this.GetComponent<Rigidbody2D>();

                /* Check if low Y velocity and speed it up if it is */
                if(( ballRb.velocity.y >= 0.0f ) && 
                   ( ballRb.velocity.y < 1.0f))
                {
                    ballRb.velocity = new Vector2( ballRb.velocity.x, ballRb.velocity.y + 0.1f);
    //                Debug.Log("Ball collision Added velocity" + coll.gameObject.tag);
                }
                else if ((ballRb.velocity.y < 0.0f) &&
                   (ballRb.velocity.y > -1.0f))
                {
                    ballRb.velocity = new Vector2(ballRb.velocity.x, ballRb.velocity.y - 0.1f);
    //                Debug.Log("Ball collision Subtracted velocity" + coll.gameObject.tag);
                }
                break;
            default:
                break;


            //            ballRb.AddForce(new Vector2(0.0f, 4.0f));
        }
    }

    public void Die()
    {
        OnBallDeath?.Invoke(this);
        Destroy(this.gameObject);
    }

    internal void StartLightningBall()
    {
        if (this.isLightningBall == false)
        {
            this.isLightningBall = true;
            this.sr.enabled = false;
            lightningBallEffect.gameObject.SetActive(true);
            StartCoroutine(StopLightningBallAfterTime(this.lightningBallDuration));

            OnLightningBallEnable?.Invoke(this);

        }
    }

    private IEnumerator StopLightningBallAfterTime(float seconds)
    {
        yield return new WaitForSeconds(seconds);

        StopLightingBall();
    }

    public void StopLightingBall()
    {
        if (this.isLightningBall == true)
        {
            this.isLightningBall = false;
            this.sr.enabled = true;
            lightningBallEffect.gameObject.SetActive(false);

            OnLightningBallDisable?.Invoke(this);
        }
    }
}
