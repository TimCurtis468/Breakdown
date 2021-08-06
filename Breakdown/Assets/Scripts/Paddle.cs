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
    private float shadowWidth;
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

    // Shooting
    public bool PaddleIsShooting { get; set; } = false;
    public GameObject leftMuzzle;
    public GameObject rightMuzzle;
    public Projectile bulletPrefab;

    public bool isActive;


    void Start()
    {
        leftMuzzle.SetActive(false);
        rightMuzzle.SetActive(false);

        screenEdgeOffset = Utilities.ResizeXValue(screenEdgeOffset);
        paddleCentreOffset = Utilities.ResizeXValue(paddleCentreOffset);

        mainCamera = FindObjectOfType<Camera>();
        paddleInitialY = transform.position.y;
        sr = GetComponent<SpriteRenderer>();
        boxCol = GetComponent<BoxCollider2D>();

        topRightCorner = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, Camera.main.transform.position.z));

        // Shadow doesn't change size when the paddle gets bigger/smaller, so keep this a contant value
        shadowWidth = sr.bounds.extents.x;
        shadowWidth = Utilities.ResizeXValue(shadowWidth);
        shadowWidth = shadowWidth / 5.0f;

        SetClamps(true);

        Utilities.ResizeSprite(this.gameObject);

        isActive = true;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        if (isActive == true)
        {
            PaddleMovement();
            UpdateMuzzlePosition();
        }
    }
    private void UpdateMuzzlePosition()
    {
        leftMuzzle.transform.position = new Vector3(this.transform.position.x - (this.sr.size.x / 2) + 0.2f, this.transform.position.y + 0.4f, this.transform.position.z);
        rightMuzzle.transform.position = new Vector3(this.transform.position.x + (this.sr.size.x / 2) - 0.253f, this.transform.position.y + 0.4f, this.transform.position.z);
    }

    private void SetClamps(bool init)
    {
        objectWidth = sr.bounds.extents.x;
        if (init == true)
        {
            objectWidth = Utilities.ResizeXValue(objectWidth);
        }
        leftClamp = -topRightCorner.x + objectWidth + (topRightCorner.x/20.0f);   
        rightClamp = topRightCorner.x - objectWidth + shadowWidth - (topRightCorner.x / 20.0f); 
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
        SetClamps(false);
    }

    private IEnumerator ResetPaddleWidthAfterTime(float seconds)
    {

        yield return new WaitForSeconds(seconds);
        this.StartWidthAnimation(this.paddleWidth);
    }

    public void StartShooting()
    {
        if (this.PaddleIsShooting == false)
        {
            this.PaddleIsShooting = true;
            StartCoroutine(StartShootingRoutine());
            GameManager.Instance.buffActive = true;
        }
    }

    public IEnumerator StartShootingRoutine()
    {
        float fireCooldown = 0.5f;
        float fireCooldownleft = 0.0f;

        float shootingDuration = 20.0f;
        float shootingDurationLeft = shootingDuration;

        //        Debug.Log("START SHOOTING");

        while ((shootingDurationLeft >= 0.0) && (PaddleIsShooting == true))
        {
            fireCooldownleft -= Time.deltaTime;
            shootingDurationLeft -= Time.deltaTime;

            if (fireCooldownleft <= 0.0f)
            {
                this.Shoot();
                fireCooldownleft = fireCooldown;
                //                Debug.Log("Shoot at {time.time}");
            }

            yield return null;
        }

        //        Debug.Log("STOPPED SHOOTING");
        this.PaddleIsShooting = false;
        leftMuzzle.SetActive(false);
        rightMuzzle.SetActive(false);
        GameManager.Instance.buffActive = false;
    }

    private void Shoot()
    {
        leftMuzzle.SetActive(false);
        rightMuzzle.SetActive(false);

        leftMuzzle.SetActive(true);
        rightMuzzle.SetActive(true);

        this.SpawnBullet(leftMuzzle);
        this.SpawnBullet(rightMuzzle);
    }

    private void SpawnBullet(GameObject muzzle)
    {
        Vector3 spawnPosition = new Vector3(muzzle.transform.position.x, muzzle.transform.position.y + 0.2f, muzzle.transform.position.y);
        Projectile bullet = Instantiate(bulletPrefab, spawnPosition, Quaternion.identity);
        Rigidbody2D bulletRb = bullet.GetComponent<Rigidbody2D>();
        bulletRb.AddForce(new Vector2(0, 450.0f));

    }
}
