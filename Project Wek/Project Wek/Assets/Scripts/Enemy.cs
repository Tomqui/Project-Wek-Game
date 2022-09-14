using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
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

    //fronfish
    [SerializeField] bool isFish;
    private bool chargeCD;
    private float chargeTime;
    private float chargeTimeLimit;
    private int chargeDMG;
    private float chargeSpeed;

    float xVariance;
    float yVariance;

    void Start()
    {
        moving = true;
        touchCooldown = false;
        chargeCD = false;
        timer = 0;

        chargeSpeed = 10f;
        chargeTime = 0f;
        chargeTimeLimit = 5f;

        player = GameObject.Find("Player");
        rb = GetComponent<Rigidbody2D>();
        //Physics2D.IgnoreCollision(player.GetComponent<BoxCollider2D>(), GetComponent<BoxCollider2D>());
        particle = GetComponentInChildren<ParticleSystem>();

        xVariance = Random.Range(-0.5f, 0.5f);
        yVariance = Random.Range(-0.5f, 0.5f);
    }

    public void GetHit(int dmg)
    {
        GetComponent<SimpleFlash>().Flash();
        currentHP -= dmg;

        if (currentHP <= 0)
        {
            Die();
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

        player.GetComponent<Player>().Heal(player.GetComponent<Player>().lifesteal);
        player.GetComponent<Player>().GetEXP(exp);

        yield return new WaitForSeconds(particle.main.startLifetime.constantMax);
        Destroy(gameObject);
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        //touch damage
        if (collision.gameObject.CompareTag("Player"))
        {
            if (!touchCooldown)
            {
                touchCooldown = true;
                timer = 0;
                collision.gameObject.GetComponent<Player>().GetHit(attack);
                
            }
        }
    }

    private IEnumerator Charge()
    {
        
        Vector2 dif = player.transform.position-transform.position;
        yield return new WaitForSeconds(1f);
        dif = dif.normalized * chargeSpeed;
        rb.AddForce(dif, ForceMode2D.Impulse);
        yield return new WaitForSeconds(0.75f);
        rb.velocity = Vector2.zero;

        moving = true;
        
    }

    private void FixedUpdate()
    {
        if (Vector2.Distance(transform.position,player.transform.position) <= 4)
        {
            if (!chargeCD)
            {
                chargeCD = true;
                moving = false;
                StartCoroutine(Charge());
            }
            
        }

        if (moving)
        {
            float step = movementSpeed * Time.fixedDeltaTime;
            Vector3 variance = new Vector3(xVariance,yVariance,0);   
            transform.position = Vector2.MoveTowards(transform.position, player.transform.position+variance, step);
            
            if (gameObject.transform.position.x > player.transform.position.x)
            {
                transform.rotation = Quaternion.Euler(0, 0, 0);
            }
            else
            {
                transform.rotation = Quaternion.Euler(0, 180, 0);
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

}
