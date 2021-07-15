using System;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.ParticleSystem;

public class Brick : MonoBehaviour
{
    private SpriteRenderer sr;
    private BoxCollider2D boxCol;

    public int HitPoints = 1;
    public ParticleSystem DestroyEffect;

    public static event Action<Brick> OnBrickDistruction;
    public static event Action<Brick, int> OnBrickHit;

    private void Awake()
    {
        this.sr = this.GetComponent<SpriteRenderer>();
        this.boxCol = this.GetComponent<BoxCollider2D>();

//        Ball.OnLightningBallEnable += OnLightningBallEnable;
//        Ball.OnLightningBallDisable += OnLightningBallDisable;
    }

#if (PI)
    private void OnLightningBallDisable(Ball obj)
    {
        if (this != null)
        {
            this.boxCol.isTrigger = false;
        }
    }

    private void OnLightningBallEnable(Ball obj)
    {
        if (this != null)
        {
            this.boxCol.isTrigger = true;
        }
    }
#endif
    private void OnCollisionEnter2D(Collision2D collision)
    {
        bool instantKill = false;

        if (collision.collider.tag == "Ball")
        {
            Ball ball = collision.gameObject.GetComponent<Ball>();
//            instantKill = ball.isLightningBall;
        }

        if ((collision.collider.tag == "Ball") || (collision.collider.tag == "Projectile"))
        {
            TakeDamage(instantKill);

        }

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        bool instantKill = false;

        if (collision.gameObject.tag == "Ball")
        {
            Ball ball = collision.gameObject.GetComponent<Ball>();
//            instantKill = ball.isLightningBall;
        }

        if ((collision.gameObject.tag == "Ball") || (collision.gameObject.tag == "Projectile"))
        {
            TakeDamage(instantKill);

        }
    }

    private void TakeDamage(bool instantKill)
    {
        if( HitPoints > 1)
        {
            OnBrickHit?.Invoke(this, HitPoints);
        }
        // Add hitpoints to score

        this.HitPoints--;

        if ((this.HitPoints <= 0) || (instantKill == true))
        {
            BricksManager.Instance.RemainingBricks.Remove(this);
            OnBrickDistruction?.Invoke(this);
//            OnBrickDestructionBuffs();
            SpawnDestroyEffect();
            Destroy(this.gameObject);
        }
        else
        {
//            this.sr.sprite = BricksManager.Instance.Sprites[this.HitPoints - 1];
        }
    }
#if (PI)
    private void OnBrickDestructionBuffs()
    {
        float buffSpawnChance = UnityEngine.Random.Range(0, 100f);
        float debuffSpawnChance = UnityEngine.Random.Range(0, 100f);
        bool alreadySpawned = false;

        if (buffSpawnChance <= CollectablesManager.Instance.BuffChance)
        {
            alreadySpawned = true;
            Collectable newBuff = this.SpawnCollectable(true);
        }
        if ((debuffSpawnChance <= CollectablesManager.Instance.DebuffChance) && (alreadySpawned == false))
        {
            Collectable newBuff = this.SpawnCollectable(false);
        }
    }

    private Collectable SpawnCollectable(bool isBuff)
    {
        List<Collectable> collection;

        if (isBuff == true)
        {
            collection = CollectablesManager.Instance.AvailableBuffs;
        }
        else
        {
            collection = CollectablesManager.Instance.AvailableDebuffs;
        }

        int buffIndex = UnityEngine.Random.Range(0, collection.Count);
        Collectable prefab = collection[buffIndex];
        Collectable newCollectable = Instantiate(prefab, this.transform.position, Quaternion.identity) as Collectable;
        return newCollectable;
    }
#endif
    private void SpawnDestroyEffect()
    {
        Vector3 brickPos = gameObject.transform.position;
        Vector3 spawnPosition = new Vector3(brickPos.x, brickPos.y, brickPos.z - 0.2f);
        GameObject effect = Instantiate(DestroyEffect.gameObject, spawnPosition, Quaternion.identity);

        MainModule mm = effect.GetComponent<ParticleSystem>().main;
        mm.startColor = this.sr.color;
        Destroy(effect, DestroyEffect.main.startLifetime.constant);
    }

    internal void Init(Transform transform, Sprite sprite, Color color, int hitPoints)
    {
        this.transform.SetParent(transform);
        this.sr.sprite = sprite;
        this.sr.color = color;
        this.HitPoints = hitPoints;
    }

    private void OnDisable()
    {
//        Ball.OnLightningBallEnable -= OnLightningBallEnable;
 //       Ball.OnLightningBallDisable -= OnLightningBallDisable;
    }
}
