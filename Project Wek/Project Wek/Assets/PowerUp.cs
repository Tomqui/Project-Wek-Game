using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUp : MonoBehaviour
{
    [SerializeField] bool isSpeed;
    [SerializeField] bool isAttack;

    float time = 0;
    float timeLimit;

    private void Start()
    {
        timeLimit = 15f;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {

            if (isSpeed)
            {
                collision.gameObject.GetComponent<Player>().StartSpeedBoost();
            }

            if (isAttack)
            {
                collision.gameObject.GetComponent<Player>().StartAttackBoost();
            }


            Destroy(gameObject);
        }
    }

    private void FixedUpdate()
    {
        time += Time.fixedDeltaTime;
        if(time > timeLimit)
        {
            Destroy(gameObject);
        }
    }

}
