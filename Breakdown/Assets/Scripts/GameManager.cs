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
    public static event Action<int> OnLifeGained;

#if (PI)
    public GameObject LeftWall;
    public GameObject RightWall;
#endif
    public GameObject background;

    private Vector2 screenBounds;
    private Camera mainCamera;

    private int endScore = 0;


    private float widthFactor;
    private float heightFactor;

    public AudioClip heartCatch;
    private AudioSource audioSource;

    private void Start()
    {
//        float objectWidth;

        GameObject obj;
        Transform trans;
        Transform childTrans;

        widthFactor = Screen.width / 1080.0f;
        heightFactor = Screen.height / 1920.0f;

        this.Lives = AvailableLives;
        Ball.OnBallDeath += OnBallDeath;
//        Heart.OnHeartCatch += OnHeartCatch;
//        Heart.OnHeartDeath += OnHeartDeath;

        /* Set position of walls to match screen resolution */
        mainCamera = FindObjectOfType<Camera>();
        screenBounds = mainCamera.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, mainCamera.transform.position.z));
#if (PI)
        objectWidth = RightWall.GetComponent<SpriteRenderer>().bounds.extents.x;
        RightWall.transform.position = new Vector3(screenBounds.x + (objectWidth / 2), RightWall.transform.position.y, RightWall.transform.position.z);

        objectWidth = LeftWall.GetComponent<SpriteRenderer>().bounds.extents.x;
        LeftWall.transform.position = new Vector3(-(screenBounds.x + (objectWidth / 2)), LeftWall.transform.position.y, LeftWall.transform.position.z);
#endif
        // Graphics text
        trans = background.transform;
        childTrans = trans.Find("Graphics"); 
        obj = childTrans.gameObject;
        ResizeSpriteRendered(obj);

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
//                EndScreen.score = endScore;
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
#if (PI)
        Heart.OnHeartCatch -= OnHeartCatch;
        Heart.OnHeartDeath -= OnHeartDeath;
#endif
    }

    private void ResizeSpriteRendered(GameObject gameObject)
    {
        float objectWidth;
        float objectHeight;
        SpriteRenderer sr;

        sr = gameObject.GetComponent<SpriteRenderer>();
        objectWidth = sr.transform.localScale.x;
        objectHeight = sr.transform.localScale.y;

        objectWidth = objectWidth * widthFactor;
        objectHeight = objectHeight * heightFactor;

        sr.transform.localScale = new Vector2(objectWidth, objectHeight);
    }
}


