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
    }

    private void OnCollisionEnter2D(Collision2D coll)
    {
        int numFx;

        if ((coll.gameObject.tag == "Ball") || (coll.gameObject.tag == "Paddle"))
        {
            numFx = UnityEngine.Random.Range(0, soundFX.Length);
            audioSource.PlayOneShot(soundFX[numFx]);
        }
        else if (coll.gameObject.tag == "Untagged")
        {
            numFx = UnityEngine.Random.Range(0, loFX.Length);
            audioSource.PlayOneShot(loFX[numFx]);
        }
        else if (coll.gameObject.tag == "Block")
        {
            numFx = UnityEngine.Random.Range(0, loFX.Length);
            audioSource.PlayOneShot(loFX[numFx]);
        }
        else if (coll.gameObject.tag == "Walls")
        {
            numFx = UnityEngine.Random.Range(0, loFX.Length);
            audioSource.PlayOneShot(loFX[numFx]);
            Rigidbody2D ballRb = this.GetComponent<Rigidbody2D>();
            if(( ballRb.velocity.y > 0.0f ) && 
               ( ballRb.velocity.y < 1.0f))
            {
                ballRb.velocity = new Vector2( ballRb.velocity.x, ballRb.velocity.y + 0.1f);
                Debug.Log("Ball collision Added velocity" + coll.gameObject.tag);
            }
            else if ((ballRb.velocity.y < 0.0f) &&
               (ballRb.velocity.y > -1.0f))
            {
                ballRb.velocity = new Vector2(ballRb.velocity.x, ballRb.velocity.y - 0.1f);
                Debug.Log("Ball collision Subtracted velocity" + coll.gameObject.tag);
            }


            //            ballRb.AddForce(new Vector2(0.0f, 4.0f));
        }
    }

    public void Die()
    {
        OnBallDeath?.Invoke(this);
        Destroy(this.gameObject);
    }
}
