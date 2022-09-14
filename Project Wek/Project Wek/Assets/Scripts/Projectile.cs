using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float moveSpeed = 10.0f;

    public float timer = 0.0f;
    
    const float expiry = 10.0f;

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

    private void Start()
    {
        
        player = GameObject.Find("Player").GetComponent<Player>();
        rb = GetComponent<Rigidbody2D>();
        if (transform.rotation.eulerAngles.y == 180) {    
            direction = -1;
        }
        else{
            direction = 1;
        }
        if (isFish)
        {
            SetStats(player.fishAttack, 10f, player.fishKB);
        }
        if (isCube)
        {
            SetStats(player.iceCount, 0f, player.iceKB);
        }
    }

    public void SetStats(int dmg, float move, float kb)
    {
        attack = dmg;
        moveSpeed = move;
        thrust = kb;
    }

    private void FixedUpdate()
    {
        if(moveSpeed > 0)
        {
            rb.MovePosition(rb.position + new Vector2(direction, 0) * moveSpeed * Time.fixedDeltaTime);
        }

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
                enemy.isKinematic = false;
                if (Laser)
                {
                    dif = enemy.transform.position - player.transform.position;
                }
                else
                {
                    dif = enemy.transform.position - player.transform.position;
                }
                
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
                enemy.isKinematic = true;
            }
            
        }
            
    }

}
