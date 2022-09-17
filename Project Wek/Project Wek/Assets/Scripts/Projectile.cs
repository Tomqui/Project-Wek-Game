using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float moveSpeed = 10.0f;

    public float timer = 0.0f;
    
    const float expiry = 1.0f;

    public float thrust = 1.0f;
    public float knockTime = 0.5f;

    public int attack = 1;

    Rigidbody2D rb;
    public bool expires = true;
    public bool Laser = false;
    bool isHit;

    int direction;

    [SerializeField] GameObject preDamagePopup;
    Player player;

    [SerializeField] bool isCube;
    [SerializeField] bool isFish;
    [SerializeField] AudioSource hitSound;

    Vector3 mousePos;

    private void Start()
    {
        player = GameObject.Find("Player").GetComponent<Player>();
        rb = GetComponent<Rigidbody2D>();
        
        if (isFish)
        {
            SetStats((int)Mathf.Round(player.fishAttack*player.totalDMG), player.fishKB);
        }
        if (isCube)
        {
            SetStats((int)Mathf.Round(player.iceAttack*player.totalDMG), player.iceKB);
        }

        
        mousePos = GameObject.Find("Main Camera").GetComponent<Camera>().ScreenToWorldPoint(Input.mousePosition);
        
        Vector2 dir = mousePos-GameObject.Find("Player").transform.position;
        if (moveSpeed > 0)
        {
            float angle = -1*(Mathf.Atan2(dir.x, dir.y) * Mathf.Rad2Deg - 90f);
            rb.rotation = angle;

            if (dir.x < 0)
            {
                transform.eulerAngles = new Vector3(180,0,-1*angle);
                
            }
            
            rb.velocity = new Vector2(dir.x, dir.y).normalized *(moveSpeed+GameObject.Find("Player").GetComponent<PlayerMovement>().moveSpeed);
        }
    }

    public void SetStats(int dmg, float kb)
    {
        attack = dmg;
        thrust = kb;
    }

    private void FixedUpdate()
    {
        /*
        if(moveSpeed > 0)
        {
            rb.MovePosition(rb.position + new Vector2(direction, 0) * moveSpeed * Time.fixedDeltaTime);
        }*/

        if (expires)
        {
            timer += Time.fixedDeltaTime;
            if (timer > expiry)
            {
                Destroy(gameObject);
            }
        }

    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Enemy") && !isHit){
            other.GetComponent<Enemy>().GetHit(attack);

            GameObject dmgpop = Instantiate(preDamagePopup, new Vector3(transform.position.x, transform.position.y - 0.5f, transform.position.z), Quaternion.identity);
            dmgpop.GetComponentInChildren<DamagePopup>().SetText(attack);

            Rigidbody2D enemy = other.GetComponent<Rigidbody2D>();

            try{
                Vector2 dif;
                //enemy.isKinematic = false;
                if (Laser)
                {
                    dif = enemy.transform.position - player.transform.position;
                }
                else
                {
                    dif = enemy.transform.position - player.transform.position;
                }

                hitSound.Play();
                dif = dif.normalized * thrust;
                enemy.AddForce(dif,ForceMode2D.Impulse);
                StartCoroutine(KnockCo(enemy));
            }
            catch{
                Debug.Log("no enemy");
            }

            if (!Laser)
            {
                isHit = true;
                gameObject.GetComponent<SpriteRenderer>().enabled = false;
            }
        }
    }

    private IEnumerator KnockCo(Rigidbody2D enemy)
    {
        if (enemy != null)
        {
            yield return new WaitForSeconds(knockTime);
            if(enemy!= null)
            {
                enemy.velocity = Vector2.zero;
            }
            
        }
            
    }

}
