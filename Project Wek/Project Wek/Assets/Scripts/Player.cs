using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public int currentHP = 100;
    public int maxHP = 100;

    float attackCooldown = 1f;
    public GameObject prefabAttack;

    private void Start()
    {
        InvokeRepeating("Attack", 1, attackCooldown);
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

    public void Die()
    {
        //Destroy(gameObject);
    }

    void Attack()
    {
        float projRotate = 0;
        if (transform.rotation.y%180==0){
            projRotate = 180;

        }
        else
        {
            projRotate = 0;
        }
        Instantiate(prefabAttack, new Vector3(transform.position.x,transform.position.y,transform.position.z), Quaternion.Euler(transform.rotation.x,projRotate,transform.rotation.z));
    }

    
}
