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

    private Ball initialBall;

    public float initialBallSpeed = 250;

    public int numBalls = 1;

    private bool mouseButtonLatch = false;

    public List<Ball> Balls { get; set; }
    public List<Rigidbody2D> BallRbs { get; set; }

    private void Start()
    {
        Balls = new List<Ball>();
        BallRbs = new List<Rigidbody2D>();
        InitBall();
        mouseButtonLatch = false;
    }

    private void Update()
    {
        if((GameManager.Instance != null) &&(!GameManager.Instance.IsGameStarted))
        {
            Vector3 paddlePosition = Paddle.Instance.gameObject.transform.position;
            Vector3 ballPosition = new Vector3(paddlePosition.x - 0.046f, paddlePosition.y + 0.27f, 0);
            Balls[0].transform.position = ballPosition;

            // Has mouse button been pressed down?
            if(Input.GetMouseButtonDown(0) == true)
            {
                mouseButtonLatch = true;
            }

            // Mas mouse button been released
            if (mouseButtonLatch == true)
            {
                if ((Input.GetMouseButtonUp(0) == true) &&
                   (AdManager.Instance.isInterstialClosed() == true))
                {
                    float y_speed = UnityEngine.Random.Range(250.0f, 350.0f);

                    BallRbs[0].isKinematic = false;
                    BallRbs[0].AddForce(new Vector2(0, y_speed));
                    GameManager.Instance.IsGameStarted = true;
                    mouseButtonLatch = false;
                }
            }
        }
    }

    public void SpawnBalls(Vector3 position, int count, bool isLightningBall)
    {
        for (int i = 0; i < count; i++)
        {
            Ball spawnedBall = Instantiate(ballPrefab, position, Quaternion.identity);
            if (isLightningBall == true)
            {
                spawnedBall.StartLightningBall();
            }
            Rigidbody2D spawnedBallRb = spawnedBall.GetComponent<Rigidbody2D>();
            spawnedBallRb.isKinematic = false;
            spawnedBallRb.AddForce(new Vector2(i, initialBallSpeed * 2));
            this.Balls.Add(spawnedBall);
        }
    }

    public void DestroyBalls()
    {
        int numBalls = Balls.Count - 1;
        for (int idx = numBalls; idx >= 0; idx--)
        {
            var ball = Balls[idx];
            ball.StopLightingBall();
            Destroy(ball.gameObject);
        }
    }


    public void ResetBalls()
    {
        int numBalls = Balls.Count - 1;
        for (int idx = numBalls; idx >= 0; idx--)
        {
            var ball = Balls[idx];
            ball.StopLightingBall();
            Destroy(ball.gameObject);
        }

        InitBall();
    }

    private void InitBall()
    {
        Rigidbody2D newBallRb;

        Balls.Clear();
        BallRbs.Clear();

        Vector3 paddlePosition = Paddle.Instance.gameObject.transform.position;
        Vector3 startingPosition = new Vector3(paddlePosition.x, paddlePosition.y + 0.27f, 0);
        initialBall = Instantiate(ballPrefab, startingPosition, Quaternion.identity);
        newBallRb = initialBall.GetComponent<Rigidbody2D>();

        Balls.Add(initialBall);
        BallRbs.Add(newBallRb);
    }
}
