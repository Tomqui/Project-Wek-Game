using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 2f;
    public Rigidbody2D rb;
    public Animator animator;

    Vector2 movement;
    bool canDash;
    bool isDash;
    [SerializeField] private Image haste;
    [SerializeField] private TrailRenderer tr;

    [SerializeField] AudioSource dash1;
    [SerializeField] AudioSource dash2;

    Vector3 mousePos;
    Vector2 dir;

    [SerializeField] GameObject shotgun;

    void Start()
    {
        canDash = true;
    }

    public void IncreaseMove(float move)
    {
        moveSpeed += move;
    }

    void Update()
    {

        if (gameObject.GetComponent<Player>().currentHP>0)
        {
            movement.x = Input.GetAxisRaw("Horizontal");
            movement.y = Input.GetAxisRaw("Vertical");

            if (movement.x != 0 || movement.y != 0)
            {
                animator.SetBool("Moving", true);
            }
            else
            {
                animator.SetBool("Moving", false);
            }

            if (Input.GetKeyDown(KeyCode.Space) && canDash)
            {
                StartCoroutine(Dash());
            }
        }

        mousePos = GameObject.Find("Main Camera").GetComponent<Camera>().ScreenToWorldPoint(Input.mousePosition);
        dir = (mousePos - transform.position).normalized;
    }

    
    private void FixedUpdate()
    {
        if (!isDash)
        {
            if (dir.x < 0)
            {
                transform.rotation = Quaternion.Euler(0, 0, 0);
            }
            else if (dir.x > 0)
            {
                transform.rotation = Quaternion.Euler(0, 180, 0);
            }
            rb.MovePosition(rb.position + movement * moveSpeed * Time.fixedDeltaTime);
        }

        float angle = (Mathf.Atan2(dir.x, dir.y) * Mathf.Rad2Deg - 90f);
        
        if (dir.x > 0)
        {
            shotgun.transform.rotation = Quaternion.Euler(0, 0, -angle);
        }
        else
        {
            shotgun.transform.rotation = Quaternion.Euler(180, 0, angle);
        }
        
    }

    private IEnumerator Dash()
    {
        canDash = false;
        isDash = true;

        haste.color = Color.gray;

        rb.velocity = new Vector2(movement.x, movement.y).normalized * (10+moveSpeed);
        tr.emitting = true;
        if (Random.Range(1,3)==1)
        {
            dash1.Play();
        }
        else
        {
            dash2.Play();
        }
        yield return new WaitForSeconds(0.2f);
        isDash = false;
        yield return new WaitForSeconds(0.1f);
        tr.emitting = false;
        yield return new WaitForSeconds(2.0f);
        canDash = true;

        haste.color = Color.white;

    }
}
