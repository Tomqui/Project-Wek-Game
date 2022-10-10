using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : MonoBehaviour
{
    [SerializeField] ParticleSystem particle;
    [SerializeField] SimpleFlash flash;
    [SerializeField] Rigidbody2D rb;
    [SerializeField] GameObject bombObj;
    [SerializeField] GameObject shadowObj;
    public int dmg;
    public float timer;
    public float timeLimit;

    bool detonating;
    bool exploded;
    bool hit;

    float xVariance;
    float yVariance;
    Vector2 player;

    bool destroyed;

    [SerializeField] AudioSource bombSound;
 
    public void Start()
    {
        hit = false;
        exploded = false;
        destroyed = false;

        xVariance = Random.Range(-3f, 3f);
        yVariance = Random.Range(-3f, 3f);
        player = GameObject.Find("Player").transform.position;
    }

    public void SetDmg(int d)
    {
        dmg = d;
    }

    void FixedUpdate()
    {
        timer += Time.fixedDeltaTime;
        if (timer >= timeLimit)
        {
            if (!detonating)
            {
                StartCoroutine(Detonate());
            }
            
        }

        if (!detonating)
        {
            float step = Mathf.Clamp((10+(-Mathf.Log10(timeLimit)*30)),5,100) * Time.fixedDeltaTime;
            Vector2 variance = new Vector3(xVariance, yVariance);
            transform.position = Vector2.MoveTowards(transform.position, player + variance, step);
        }

        if (Vector2.Distance(gameObject.transform.position,GameObject.Find("Player").transform.position) <=2.75)
        {
            if (!hit && exploded && !destroyed)
            {
                hit = true;
                GameObject.Find("Player").GetComponent<Player>().GetHit(dmg);
            }
        }
        
    }

    public IEnumerator Detonate()
    {
        detonating = true;
        yield return new WaitForSeconds(0.5f);
        flash.Flash();
        yield return new WaitForSeconds(0.2f);
        flash.Flash();
        yield return new WaitForSeconds(0.2f);
        flash.Flash();
        yield return new WaitForSeconds(0.2f);
        
        flash.Flash();

        //blowup
        bombSound.Play();
        exploded = true;
        bombObj.SetActive(false);
        shadowObj.SetActive(false);
        particle.Play();
        yield return new WaitForSeconds(0.2f);
        destroyed = true;
        yield return new WaitForSeconds(1.0f);
        Destroy(gameObject);

    }

    private void OnTriggerStay2D(Collider2D collision)
    {

        
    }

}
