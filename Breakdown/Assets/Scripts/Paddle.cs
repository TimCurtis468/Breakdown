using System;
using UnityEngine;

public class Paddle : MonoBehaviour
{
    #region Singleton
    private static Paddle _instance;
    public static Paddle Instance => _instance;

    private void Awake()
    {
        if (_instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            _instance = this;
        }
    }
    #endregion

    private Camera mainCamera;
    private float paddleInitialY;
    private float leftClamp = 0;
    private float rightClamp = 410;
    private float screenEdgeOffset = 0.1f;
    private float BALL_SPEED_FACTOR = 0.05f;
    private float MAX_BALL_SPEED = 12.0f;
    private SpriteRenderer sr;
    private BoxCollider2D boxCol;

    private Vector2 screenBounds;
    private float objectWidth;

    public static event Action<Paddle, int> OnPaddleHit;

    void Start()
    {
        mainCamera = FindObjectOfType<Camera>();
        paddleInitialY = transform.position.y;
        sr = GetComponent<SpriteRenderer>();
        boxCol = GetComponent<BoxCollider2D>();

        screenBounds = mainCamera.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, mainCamera.transform.position.z));
        objectWidth = sr.bounds.extents.x; //extents = size of width / 2

        leftClamp = -screenBounds.x + (objectWidth + screenEdgeOffset);
        rightClamp = screenBounds.x - (objectWidth + screenEdgeOffset);
    }

    // Update is called once per frame
    void LateUpdate()
    {
        PaddleMovement();
    }

    private void PaddleMovement()
    {
        float mousePositionPixels = Input.mousePosition.x;
        float mousePositionWorldX = mainCamera.ScreenToWorldPoint(new Vector3(mousePositionPixels, 0, 0)).x;
        mousePositionWorldX = Mathf.Clamp(mousePositionWorldX, leftClamp, rightClamp);
        transform.position = new Vector3(mousePositionWorldX, paddleInitialY, 0);
    }


    private void OnCollisionEnter2D(Collision2D coll)
    {
        float speed;
        float vel_x;
        float vel_y;

        if (coll.gameObject.tag == "Ball")
        {
            Rigidbody2D ballRb = coll.gameObject.GetComponent<Rigidbody2D>();
            Vector3 hitPoint = coll.contacts[0].point;
            Vector3 paddleCentre = new Vector3(this.gameObject.transform.position.x, this.gameObject.transform.position.y);

            /* Get speed from x,y velocity */
            speed = Mathf.Sqrt((ballRb.velocity.x * ballRb.velocity.x) + (ballRb.velocity.y * ballRb.velocity.y));
            /* Limit speed to max value */
            if (speed < MAX_BALL_SPEED)
            {
                speed += BALL_SPEED_FACTOR;
            }
            else
            {
                speed = MAX_BALL_SPEED;
            }

            float difference = paddleCentre.x - hitPoint.x;

            /* Calculate X and Y velocities by using position of collision on paddle for X */
            if (hitPoint.x < paddleCentre.x)
            {
                vel_x = -(Mathf.Abs(difference * 200));
            }
            else
            {
                vel_x = Mathf.Abs(difference * 200);
            }
            /* Modify Y to keep speed constant (plus a bit added each time) */
            vel_y = Mathf.Sqrt(Mathf.Abs((vel_x * vel_x) - (speed * speed)));   // ABS it to stop errors when getting -ve number */
            ballRb.AddForce(new Vector2(vel_x, vel_y));

            OnPaddleHit?.Invoke(this, (int)speed);
        }
#if (PI)
        else if (coll.gameObject.tag == "Heart")
        {
            if (BallsManager.Instance.Hearts.Count > 0)
            {
                Heart heart = coll.gameObject.GetComponent<Heart>();
                BallsManager.Instance.Hearts.Remove(heart);
                heart.Catch();
            }
        }
#endif
    }
}
