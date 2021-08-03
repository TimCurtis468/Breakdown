using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    #region Singleton
    private static GameManager _instance;
    public static GameManager Instance => _instance;


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

    public int AvailableLives = 3;

    public int Lives { get; set; }

    public bool IsGameStarted { get; set; }

    public bool paused = false;

    public static event Action<int> OnLifeLost;
    public static event Action<int> OnLifeGained;

    public GameObject background;
    public GameObject leftWall;
    public GameObject topWall;
    public GameObject rightWall;

    public GameObject gameOver;

    private int endScore = 0;

    private int level = 1;
    private int nextAd = 4;

    private void Start()
    {
        GameObject obj;
        Transform trans;
        Transform childTrans;

        if (gameOver != null)
        {
            gameOver.SetActive(false);
        }

        this.Lives = AvailableLives;
        Ball.OnBallDeath += OnBallDeath;
        Brick.OnBrickDistruction += OnBrickDestruction;

        // Set background and walls sizes
        trans = background.transform;
        childTrans = trans.Find("Graphics"); 
        obj = childTrans.gameObject;
        Utilities.ResizeSpriteToFullScreen(obj);

        Utilities.ResizeAndPositionSprite(leftWall.gameObject);
        Utilities.ResizeAndPositionSprite(topWall.gameObject);
        Utilities.ResizeAndPositionSprite(rightWall.gameObject);

        AdManager.Instance.RequestBanner(GoogleMobileAds.Api.AdPosition.Top);

        paused = false;

        level = 1;
        nextAd = 4;
    }

    public void SetScore(int score)
    {
        endScore = score;
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        this.Lives = AvailableLives;
        level = 1;
        nextAd = 4;
    }

    private void OnBallDeath(Ball obj)
    {
        DeathCheck();
    }

    public void AddLife()
    {
        SoundFxManager.Instance.PlayHeart();
        Lives++;
        OnLifeGained?.Invoke(this.Lives);
    }

    private void DeathCheck()
    {
        if (BallsManager.Instance.Balls.Count <= 0)
        {
            this.Lives--;

            if (this.Lives < 1)
            {
                BallsManager.Instance.DestroyBalls();

                if(gameOver != null)
                {
                    gameOver.SetActive(true);
                    Paddle.Instance.isActive = false;
                    Paddle.Instance.PaddleIsShooting = false;
                    MusicManager.Instance.StopMusic();
                }
            }
            else
            {
                OnLifeLost?.Invoke(this.Lives);
                BallsManager.Instance.ResetBalls();
                IsGameStarted = false;
            }
        }
    }

    public void GameOverWatchVid()
    {
        AdManager.Instance.RequestRewarded();
    }

    public void GameOverExtraLife()
    {
        this.Lives++;
        gameOver.SetActive(false);
        AdManager.Instance.DestroyRewarded();

        OnLifeLost?.Invoke(this.Lives);
        BallsManager.Instance.ResetBalls();
        IsGameStarted = false;
        Paddle.Instance.isActive = true;
        MusicManager.Instance.StartMusic();
    }

    public void MoveToNextScene()
    {
        gameOver.SetActive(false);
        AdManager.Instance.DestroyRewarded();
        AdManager.Instance.DestroyBanner();

        Paddle.Instance.isActive = true;
        EndScreen.score = endScore;
        SceneManager.LoadScene("GameOver");
    }

    private void OnDisable()
    {
        Ball.OnBallDeath -= OnBallDeath;
        Brick.OnBrickDistruction -= OnBrickDestruction;
    }

    private void OnBrickDestruction(Brick obj)
    {
        if (BricksManager.Instance.RemainingBricks.Count <= 0)
        {
            paused = true;
            if (level == nextAd)
            {
                AdManager.Instance.RequestInterstital();
                int rand = UnityEngine.Random.Range(3, 5);
                nextAd += rand;
            }

            // Pause for 1 second 
            StartPause(1);
        }
    }

    public void StartPause(float pause)
    {
        // how many seconds to pause the game
        StartCoroutine(PauseGame(pause));
    }
    public IEnumerator PauseGame(float pauseTime)
    {
        float pauseEndTime = Time.realtimeSinceStartup + pauseTime;
        while (Time.realtimeSinceStartup < pauseEndTime)
        {
            yield return 0;
        }

        PauseEnded();
    }

    public void PauseEnded()
    {
        paused = false;
        level++;
        BallsManager.Instance.ResetBalls();
        GameManager.Instance.IsGameStarted = false;
        BricksManager.Instance.LoadNextLevel();
    }
}


