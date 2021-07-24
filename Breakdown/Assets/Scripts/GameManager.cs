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

    private int endScore = 0;

    private void Start()
    {
        GameObject obj;
        Transform trans;
        Transform childTrans;

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

        paused = false;
    }

    public void SetScore(int score)
    {
        endScore = score;
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        this.Lives = AvailableLives;
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
                EndScreen.score = endScore;
                SceneManager.LoadScene("GameOver");
                MusicManager.Instance.StopMusic();
            }
            else
            {
                OnLifeLost?.Invoke(this.Lives);
                BallsManager.Instance.ResetBalls();
                IsGameStarted = false;
            }
        }
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
        BallsManager.Instance.ResetBalls();
        GameManager.Instance.IsGameStarted = false;
        BricksManager.Instance.LoadNextLevel();
    }
}


