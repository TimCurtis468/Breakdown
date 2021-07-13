using System;
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

    public static event Action<int> OnLifeLost;
//    public static event Action<int> OnLifeGained;


    public GameObject background;
    public GameObject leftWall;
    public GameObject topWall;
    public GameObject rightWall;

    private int endScore = 0;

    public AudioClip heartCatch;
    private AudioSource audioSource;

    private void Start()
    {
        GameObject obj;
        Transform trans;
        Transform childTrans;

        this.Lives = AvailableLives;
        Ball.OnBallDeath += OnBallDeath;
        Brick.OnBrickDistruction += OnBrickDestruction;

        //        Heart.OnHeartCatch += OnHeartCatch;
        //        Heart.OnHeartDeath += OnHeartDeath;

        // Set background and walls sizes
        trans = background.transform;
        childTrans = trans.Find("Graphics"); 
        obj = childTrans.gameObject;
        Utilities.ResizeSpriteToFullScreen(obj);

        Utilities.ResizeAndPositionSprite(leftWall.gameObject);
        Utilities.ResizeAndPositionSprite(topWall.gameObject);
        Utilities.ResizeAndPositionSprite(rightWall.gameObject);

        audioSource = GetComponentInChildren<AudioSource>();
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


#if (PI)

    private void OnHeartCatch(Heart obj)
    {
        audioSource.PlayOneShot(heartCatch);

        this.Lives++;
        OnLifeGained?.Invoke(this.Lives);

        DeathCheck();
    }

    private void OnHeartDeath(Heart obj)
    {
        DeathCheck();
    }

#endif

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
            }
            else
            {
                OnLifeLost?.Invoke(this.Lives);
                BallsManager.Instance.ResetBalls();
                IsGameStarted = false;
//                BlocksManager.Instance.NewLevel();
            }
        }
    }

    private void OnDisable()
    {
        Ball.OnBallDeath -= OnBallDeath;
        Brick.OnBrickDistruction -= OnBrickDestruction;

#if (PI)
        Heart.OnHeartCatch -= OnHeartCatch;
        Heart.OnHeartDeath -= OnHeartDeath;
#endif
    }

    private void OnBrickDestruction(Brick obj)
    {
        if (BricksManager.Instance.RemainingBricks.Count <= 0)
        {
            BallsManager.Instance.ResetBalls();
            GameManager.Instance.IsGameStarted = false;
            BricksManager.Instance.LoadNextLevel();
        }
    }
#if (PI)
    private void ResizeSpriteRendered(GameObject gameObject)
    {
        var topRightCorner = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, Camera.main.transform.position.z));
        var worldSpaceWidth = topRightCorner.x * 2;
        var worldSpaceHeight = topRightCorner.y * 2;
        var spriteSize = gameObject.GetComponent<SpriteRenderer>().bounds.size;
        var scaleFactorX = worldSpaceWidth / spriteSize.x;
        var scaleFactorY = worldSpaceHeight / spriteSize.y;

        gameObject.transform.localScale = new Vector3(scaleFactorX, scaleFactorY, 1);
    }
#endif
}


