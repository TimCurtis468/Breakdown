using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BallsManager : MonoBehaviour
{
    #region Singleton
    private static BallsManager _instance;

    public static BallsManager Instance => _instance;

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
    [SerializeField]
    private Ball ballPrefab;
//    public Heart heartPrefab;

    private Ball initialBall;

    private Rigidbody2D initialBallRb;

    private Camera mainCamera;
    private Vector2 screenBounds;


    public float initialBallSpeed = 250;

    public int numBalls = 1;

    public List<Ball> Balls { get; set; }
    public List<Rigidbody2D> BallRbs { get; set; }

//    public List<Heart> Hearts { get; set; }


    private void Start()
    {
        mainCamera = FindObjectOfType<Camera>();
        screenBounds = mainCamera.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, mainCamera.transform.position.z));

        Balls = new List<Ball>();
        BallRbs = new List<Rigidbody2D>();
//        Hearts = new List<Heart>();

        InitBall();
    }

    private void Update()
    {

        if (!GameManager.Instance.IsGameStarted)
        {
            Vector3 paddlePosition = Paddle.Instance.gameObject.transform.position;
            Vector3 ballPosition = new Vector3(paddlePosition.x, paddlePosition.y + 0.27f, 0);
            Balls[0].transform.position = ballPosition;

            if (Input.GetMouseButtonDown(0))
            {
                float x_speed = UnityEngine.Random.Range(50.0f, 100.0f);
                float y_speed = UnityEngine.Random.Range(250.0f, 450.0f);
                int direction = UnityEngine.Random.Range(1, 3);
                if (direction == 1)
                {
                    x_speed = -x_speed;
                }

                BallRbs[0].isKinematic = false;
                BallRbs[0].AddForce(new Vector2(x_speed, y_speed));
                GameManager.Instance.IsGameStarted = true;
            }
        }
    }

    public void DestroyBalls()
    {
        foreach (var ball in this.Balls.ToList())
        {
            Destroy(ball.gameObject);
        }
#if (PI)
        foreach (var heart in this.Hearts.ToList())
        {
            Destroy(heart.gameObject);
        }
#endif
    }


    public void ResetBalls()
    {
        foreach (var ball in this.Balls.ToList())
        {
            Destroy(ball.gameObject);
        }
#if (PI)

        foreach (var heart in this.Hearts.ToList())
        {
            Destroy(heart.gameObject);
        }

#endif

        InitBall();
    }

    private void InitBall()
    {
        Rigidbody2D newBallRb;
//        Heart newHeart;

        int numDrumsticks = UnityEngine.Random.Range(0, 2);

        Balls.Clear();
        BallRbs.Clear();


        Vector3 paddlePosition = Paddle.Instance.gameObject.transform.position;
        Vector3 startingPosition = new Vector3(paddlePosition.x, paddlePosition.y + 0.27f, 0);
        initialBall = Instantiate(ballPrefab, startingPosition, Quaternion.identity);
        newBallRb = initialBall.GetComponent<Rigidbody2D>();

        Balls.Add(initialBall);
        BallRbs.Add(newBallRb);


//        Vector3 heartPosition = new Vector3(0.8f, -screenBounds.y / 2, 0);
//        newHeart = Instantiate(heartPrefab, heartPosition, Quaternion.identity);
//        Hearts.Add(newHeart);
    }
}
