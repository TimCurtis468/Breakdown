using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour
{
    public AudioClip[] soundFX;
    public AudioClip[] loFX;

    private AudioSource audioSource;

    private SpriteRenderer sr;

    public static event Action<Ball> OnBallDeath;


    private void Awake()
    {
        this.sr = GetComponentInChildren<SpriteRenderer>();
        audioSource = GetComponentInChildren<AudioSource>();
        Utilities.ResizeSprite(this.gameObject);
    }

    private void OnCollisionEnter2D(Collision2D coll)
    {
        int numFx;

        switch(coll.gameObject.tag)
        {
            case "Ball":
            case "Paddle":
                numFx = UnityEngine.Random.Range(0, soundFX.Length);
                audioSource.PlayOneShot(soundFX[numFx]);
                break;
            case "Brick":
                numFx = UnityEngine.Random.Range(0, loFX.Length);
                audioSource.PlayOneShot(loFX[numFx]);

                Rigidbody2D ballRbx = this.GetComponent<Rigidbody2D>();

                /* Check if low X velocity and speed it up if it is */
                if ((ballRbx.velocity.x >= 0.0f) &&
                   (ballRbx.velocity.x < 0.4f))
                {
                    ballRbx.velocity = new Vector2(ballRbx.velocity.x + 0.4f, ballRbx.velocity.y);
                    Debug.Log("Ball collision Added X velocity" + coll.gameObject.tag);
                }
                else if ((ballRbx.velocity.x <= 0.0f) &&
                         (ballRbx.velocity.x > -0.4f))
                {
                    ballRbx.velocity = new Vector2(ballRbx.velocity.x - 0.4f, ballRbx.velocity.y);
                    Debug.Log("Ball collision Subtracted X velocity" + coll.gameObject.tag);
                }

                break;
            case "Block":
                numFx = UnityEngine.Random.Range(0, loFX.Length);
                audioSource.PlayOneShot(loFX[numFx]);
                break;
            case "Walls":
                numFx = UnityEngine.Random.Range(0, loFX.Length);
                audioSource.PlayOneShot(loFX[numFx]);

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
}
