using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{

    public float currentHP = 10;
    public float maxHP = 10;
    public float movementSpeed;

    GameObject player;

    void Start()
    {
        player = GameObject.Find("Player");
        Physics2D.IgnoreCollision(player.GetComponent<BoxCollider2D>(), GetComponent<BoxCollider2D>());
    }

    private void FixedUpdate()
    {
        float step = movementSpeed * Time.fixedDeltaTime;
        transform.position = Vector2.MoveTowards(transform.position, player.transform.position, step);

        if (gameObject.transform.position.x > player.transform.position.x){
            transform.rotation = Quaternion.Euler(0, 0, 0);
        }
        else{
            transform.rotation = Quaternion.Euler(0, 180, 0);
        }
    }
}
