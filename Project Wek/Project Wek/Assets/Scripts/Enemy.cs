using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;
using UnityEngine.UI;

public class Enemy : MonoBehaviour
{

    public float currentHP = 10;
    public float maxHP = 10;
    public float movementSpeed;
    public int attack;
    GameObject player;

    private ParticleSystem particle;

    public GameObject shadow;

    private bool moving;
    private bool touchCooldown;
    private float timeLimit = 0.5f;
    private float timer = 0;
    public int exp = 1;

    private Rigidbody2D rb;

    //frontfish
    [SerializeField] bool isFish;
    private bool chargeCD;
    private float chargeTime;
    private float chargeTimeLimit;
    private int chargeDMG;
    private float chargeSpeed;
    private float chargeRange;

    //sand
    [SerializeField] bool isSand;
    [SerializeField] GameObject bombPrefab;
    private bool bombCD;
    private float bombTime;
    private float bombTimeLimit;
    private int bombDMG;
    private float bombDetonate;

    //laser
    [SerializeField] bool isYMDN;
    private bool laserCD;
    private float laserTime;
    private float lasterTimeLimit;
    private int laserDMG;

    public bool dead;

    float xVariance;
    float yVariance;


    [SerializeField] SimpleFlash hitFlash;
    [SerializeField] SimpleFlash atkFlash;

    [SerializeField] AudioSource FishDashSound;
    [SerializeField] AudioSource SandThrowSound;

    float resetTime;
    float resetTimeLimit;

    [SerializeField] GameObject atkBoost;
    [SerializeField] GameObject spdBoost;

    [SerializeField] TrailRenderer tr;
    void Start()
    {
        moving = true;
        touchCooldown = false;
        chargeCD = false;
        timer = 0;

        chargeTime = 0f;
        chargeTimeLimit = 5f;

        bombCD = false;
        bombTime = 0f;
        bombTimeLimit = 7.5f;

        resetTime = 0;
        resetTimeLimit = 10;

        player = GameObject.Find("Player");
        rb = GetComponent<Rigidbody2D>();
        //Physics2D.IgnoreCollision(player.GetComponent<BoxCollider2D>(), GetComponent<BoxCollider2D>());
        particle = GetComponentInChildren<ParticleSystem>();

        xVariance = Random.Range(-0.5f, 0.5f);
        yVariance = Random.Range(-0.5f, 0.5f);
    }

    public void SetUpFish(int atk, int hp, float move, float chargeSPD, float range)
    {
        attack = atk;
        maxHP = hp;
        currentHP = hp;
        movementSpeed = move;
        chargeSpeed = chargeSPD;
        chargeRange = range;
    }

    public void SetUpSand(int atk,int hp,float move, int bd,float detonate)
    {
        attack = atk;
        maxHP = hp;
        currentHP = hp;
        bombDMG = bd;
        movementSpeed = move;
        bombDetonate = detonate;
    }

    public void GetHit(int dmg)
    {
        if (!dead)
        {
            hitFlash.Flash();
            currentHP -= dmg;

            if (currentHP <= 0 && !dead)
            {
                Die();
            }
        }
    }

    public void Die() {
        StartCoroutine(Break());
    }

    private IEnumerator Break()
    {
        moving = false;
        particle.Play();
        gameObject.GetComponent<SpriteRenderer>().enabled = false;
        gameObject.GetComponent<Collider2D>().enabled = false;
        shadow.SetActive(false);
        dead = true;
        rb.velocity = Vector2.zero;

        player.GetComponent<Player>().Heal(player.GetComponent<Player>().lifesteal);
        player.GetComponent<Player>().GetEXP(exp);
        GameObject.Find("Enemies").GetComponent<EnemySystem>().enemyCount--;

        if (Random.Range(0, 15) == 0)
        {
            if(Random.Range(0, 2) == 1)
            {
                GameObject powerup = Instantiate(spdBoost, transform.position, Quaternion.identity);
            }
            else
            {
                GameObject powerup = Instantiate(atkBoost, transform.position, Quaternion.identity);
            }
        }

        yield return new WaitForSeconds(particle.main.startLifetime.constantMax+1f);
        Destroy(gameObject);
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (!dead)
        {
            

            

            //touch damage
            if (collision.gameObject.CompareTag("Player"))
            {
                if (!touchCooldown)
                {
                    touchCooldown = true;
                    timer = 0;
                    collision.gameObject.GetComponent<Player>().GetHit(attack);
                    moving = false;
                }
            }
        }
    }

    private IEnumerator Charge()
    {
        
        Vector2 dif = player.transform.position-transform.position;
        atkFlash.Flash();

        yield return new WaitForSeconds(0.6f);
        FishDashSound.Play();
        rb.velocity = Vector2.zero;
        tr.emitting = true;
        rb.AddForce(dif.normalized * (movementSpeed+chargeSpeed), ForceMode2D.Impulse);
        yield return new WaitForSeconds(0.7f);
        tr.emitting = false;
        rb.velocity = Vector2.zero;

        moving = true;
        
    }

    private IEnumerator ThrowBomb()
    {
        GameObject newBomb = Instantiate(bombPrefab,transform.position,Quaternion.identity);
        newBomb.GetComponent<Bomb>().SetDmg(bombDMG);
        newBomb.GetComponent<Bomb>().timeLimit = bombDetonate;
        SandThrowSound.Play();
        yield return new WaitForSeconds(0.5f);
        moving = true;
    }

    private void FixedUpdate()
    {
        if (dead)
        {
            rb.velocity = Vector2.zero;
        }
        else
        {
            if (resetTime > resetTimeLimit)
            {
                if (Vector2.Distance(transform.position, player.transform.position) >= 15)
                {
                    resetTime = 0;
                    rb.velocity = Vector2.zero;
                }
                if (Vector2.Distance(transform.position, player.transform.position) >= 75)
                {
                    Destroy(gameObject);
                }

            }
            resetTime += Time.fixedDeltaTime;


            if (isFish)
            {
                if (Vector2.Distance(transform.position, player.transform.position) <= chargeRange && moving)
                {
                    if (!chargeCD)
                    {
                        chargeCD = true;
                        StartCoroutine(Charge());
                    }
                }

                if (chargeCD)
                {
                    chargeTime += Time.fixedDeltaTime;
                    if (chargeTime >= chargeTimeLimit)
                    {
                        chargeCD = false;
                        chargeTime = 0;
                    }
                }
            }

            if (isSand)
            {
                if (!bombCD && Vector2.Distance(transform.position, player.transform.position) <= 8.5)
                {
                    bombCD = true;
                    StartCoroutine(ThrowBomb());
                }
                if (bombCD)
                {
                    bombTime += Time.fixedDeltaTime;
                    if (bombTime >= bombTimeLimit)
                    {
                        bombCD = false;
                        bombTime = 0;
                    }
                }
            }

            if (moving)
            {
                if (isSand)
                {
                    if (gameObject.transform.position.x > player.transform.position.x)
                    {
                        transform.rotation = Quaternion.Euler(0, 180, 0);
                    }
                    else
                    {
                        transform.rotation = Quaternion.Euler(0, 0, 0);
                    }
                }
                if (isSand && (Vector2.Distance(transform.position, player.transform.position) <= 4))
                {
                    float step = movementSpeed * Time.fixedDeltaTime;
                    Vector3 variance = new Vector3(xVariance, yVariance, 0);

                    //Vector3 target = transform.position-player.transform.position;
                    transform.position = Vector2.MoveTowards(transform.position, player.transform.position+ variance, -step/0.75f);

                    
                }
                
                else
                {
                    if (!isSand || Vector2.Distance(transform.position, player.transform.position) >= 7.5)
                    {

                        float step = movementSpeed * Time.fixedDeltaTime;
                        Vector3 variance = new Vector3(xVariance, yVariance, 0);
                        transform.position = Vector2.MoveTowards(transform.position, player.transform.position + variance, step);
                    }
                    

                    if (isFish)
                    {
                        if (gameObject.transform.position.x > player.transform.position.x)
                        {
                            transform.rotation = Quaternion.Euler(0, 0, 0);
                        }
                        else
                        {
                            transform.rotation = Quaternion.Euler(0, 180, 0);
                        }
                    }
                    else
                    {
                        if (gameObject.transform.position.x > player.transform.position.x)
                        {
                            transform.rotation = Quaternion.Euler(0, 180, 0);
                        }
                        else
                        {
                            transform.rotation = Quaternion.Euler(0, 0, 0);
                        }
                    }
                    
                }
            }

            if (touchCooldown)
            {
                timer += Time.fixedDeltaTime;
                if (timer >= timeLimit)
                {
                    touchCooldown = false;
                    moving = true;
                    timer = 0;
                }
            }

        }
    }
}
