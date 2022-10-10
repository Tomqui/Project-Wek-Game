using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour
{
    int damage;
    [SerializeField] Rigidbody2D rb;
    [SerializeField] Animator ani;

    float timer;
    float timeLimit;
    bool touchCooldown;

    private void Start()
    {
        timer = 0;
        timeLimit = 0.2f;
        /*
        Vector2 dir = GameObject.Find("Player").transform.position - transform.parent.position;
        dir.Normalize();
        float angle = Mathf.Atan2(dir.x, dir.y)*Mathf.Rad2Deg;
        
        
        transform.parent.GetComponent<Rigidbody2D>().rotation = angle;
        */

        float xVariance = Random.Range(-5f, 5f);
        float yVariance = Random.Range(-5f, 5f);

        Vector2 dir = GameObject.Find("Player").transform.position - transform.parent.position + new Vector3(xVariance,yVariance) ;
        dir.Normalize();
        float angle = Mathf.Atan2(dir.y, dir.x);
        //transform.parent.GetComponent<Rigidbody2D>().rotation = new Vector3(0,0,angle);
        transform.parent.eulerAngles = new Vector3(0, 0, angle*Mathf.Rad2Deg);
    }

    public void SetUp(int dmg,float s1,float s2)
    {
        damage = dmg;
        ani.SetFloat("speed1", s1);
        ani.SetFloat("speed2", s2);
    }

    void FixedUpdate()
    {
        if (touchCooldown)
        {
            timer += Time.fixedDeltaTime;
            if (timer >= timeLimit)
            {
                touchCooldown = false;
            }
        }
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
                collision.gameObject.GetComponent<Player>().GetHit(damage);

            }
        }
    }
    public void Finish()
    {
        Destroy(gameObject);
    }
}
