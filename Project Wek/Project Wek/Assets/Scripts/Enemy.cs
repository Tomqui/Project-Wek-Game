using System.Collections;
using System.Collections.Generic;
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

    void Start()
    {
        moving = true;
        touchCooldown = false;
        timer = 0;
        
        player = GameObject.Find("Player");
        //Physics2D.IgnoreCollision(player.GetComponent<BoxCollider2D>(), GetComponent<BoxCollider2D>());
        particle = GetComponentInChildren<ParticleSystem>();
        
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

    private void FixedUpdate()
    {
        if (moving)
        {
            float step = movementSpeed * Time.fixedDeltaTime;
            transform.position = Vector2.MoveTowards(transform.position, player.transform.position, step);
            
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
            }
        }

    }

}
