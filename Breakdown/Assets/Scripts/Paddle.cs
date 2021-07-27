using System;
using UnityEngine;
using System.Collections;

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
    private float screenEdgeOffset = 0.15f;
    private float shadowWidth = 0.04f;
    private SpriteRenderer sr;
    private BoxCollider2D boxCol;

    private float paddleCentreOffset = 0.1f;

    private float objectWidth;

    public static event Action<Paddle, int> OnPaddleHit;

    public bool PaddleIsTransforming { get; set; }

    public float extendShrinkDuration = 1;
    public float paddleWidth = 2.0f;
    public float paddleHeight = 0.28f;

    private Vector3 topRightCorner;


    void Start()
    {
        screenEdgeOffset = Utilities.ResizeXValue(screenEdgeOffset);
        paddleCentreOffset = Utilities.ResizeXValue(paddleCentreOffset);

        mainCamera = FindObjectOfType<Camera>();
        paddleInitialY = transform.position.y;
        sr = GetComponent<SpriteRenderer>();
        boxCol = GetComponent<BoxCollider2D>();

        topRightCorner = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, Camera.main.transform.position.z));

//        objectWidth = boxCol.bounds.extents.x;
//        objectWidth = Utilities.ResizeXValue(objectWidth);

//        leftClamp = -topRightCorner.x + objectWidth + screenEdgeOffset + paddleCentreOffset;
//        rightClamp = topRightCorner.x - objectWidth - screenEdgeOffset + paddleCentreOffset;

        SetClamps();

        Utilities.ResizeSprite(this.gameObject);
    }

    // Update is called once per frame
    void LateUpdate()
    {
        PaddleMovement();
    }

    private void SetClamps()
    {
        objectWidth = boxCol.bounds.extents.x;
        objectWidth = Utilities.ResizeXValue(objectWidth);

        leftClamp = -topRightCorner.x + objectWidth + screenEdgeOffset + paddleCentreOffset;
//        leftClamp = Utilities.ResizeXValue(leftClamp);
        rightClamp = topRightCorner.x - objectWidth - screenEdgeOffset + paddleCentreOffset + shadowWidth;
//        rightClamp = Utilities.ResizeXValue(rightClamp);
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
        if (coll.gameObject.tag == "Ball")
        {
            Rigidbody2D ballRb = coll.gameObject.GetComponent<Rigidbody2D>();
            Vector3 hitPoint = coll.contacts[0].point;
            Vector3 paddleCentre = new Vector3(this.gameObject.transform.position.x - paddleCentreOffset, this.gameObject.transform.position.y);

            ballRb.velocity = Vector2.zero;

            float difference = paddleCentre.x - hitPoint.x;

            if (hitPoint.x < paddleCentre.x)
            {
                ballRb.AddForce(new Vector2(-(Mathf.Abs(difference * 200)), BallsManager.Instance.initialBallSpeed * 2.0f));
            }
            else
            {
                ballRb.AddForce(new Vector2(Mathf.Abs(difference * 200), BallsManager.Instance.initialBallSpeed * 2.0f));
            }

            OnPaddleHit?.Invoke(this, 0);
        }
    }

    public void StartWidthAnimation(float newWidth)
    {
        StartCoroutine(AnimationPaddleWidth(newWidth));
    }

    private IEnumerator AnimationPaddleWidth(float width)
    {
        this.PaddleIsTransforming = true;
        this.StartCoroutine(ResetPaddleWidthAfterTime(this.extendShrinkDuration));

        if (width > this.sr.size.x)
        {
            float currentWidth = this.sr.size.x;
            while (currentWidth < width)
            {
                currentWidth += Time.deltaTime * 2;
                this.sr.size = new Vector2(currentWidth, paddleHeight);
                boxCol.size = new Vector2(currentWidth, paddleHeight);
                yield return null;
            }
        }
        else
        {
            float currentWidth = this.sr.size.x;
            while (currentWidth > width)
            {
                currentWidth -= Time.deltaTime * 2;
                this.sr.size = new Vector2(currentWidth, paddleHeight);
                boxCol.size = new Vector2(currentWidth, paddleHeight);
                yield return null;
            }
        }
        this.PaddleIsTransforming = false;
        SetClamps();
    }

    private IEnumerator ResetPaddleWidthAfterTime(float seconds)
    {

        yield return new WaitForSeconds(seconds);
        this.StartWidthAnimation(this.paddleWidth);
    }
}
