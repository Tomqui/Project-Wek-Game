using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    public int currentHP = 100;
    public int maxHP = 100;

    public GameObject prefabAttack;

    public int fishAttack;
    public float fishKB;
    public float attackCooldown = 1f;
    public float iceAttack;
    public int iceCount;
    public float iceKB;
    public float iceSpeed;
    public int lifesteal;

    [SerializeField] int EXP;
    [SerializeField] int requiredEXP;

    [SerializeField] PlayerMovement playerMove;

    [SerializeField] RotateShield iceShield;
    [SerializeField] GameObject ice1;
    [SerializeField] GameObject ice2;
    [SerializeField] GameObject ice3;
    [SerializeField] GameObject ice4;

    [SerializeField] Slider hpSlide;
    [SerializeField] TextMeshProUGUI hpText;
    [SerializeField] Slider expSlide;

    [SerializeField] GameObject LevelUpBox;

    private void SetUp() {
        currentHP = 25;
        maxHP = 25;
        EXP = 0;
        requiredEXP = 15;

        iceCount = 1;
        iceAttack = 1;
        iceKB = 2;
        iceSpeed = 100;
        iceShield.rotateSpeed = iceSpeed;

        fishAttack = 3;
        fishKB = 1;
        attackCooldown = 1;

        lifesteal = 0;
        playerMove.moveSpeed = 25.0f;
    }

    private void Start()
    {
        SetUp();
        
        hpSlide.maxValue = maxHP;
        hpSlide.value = currentHP;
        hpText.text = "HP: "+currentHP+"/"+maxHP;

        expSlide.maxValue = requiredEXP;
        expSlide.value = EXP;

        InvokeRepeating("Attack", attackCooldown, attackCooldown);

        LevelUp();

    }

    public void moreIce()
    {
        iceCount++;
        if(iceCount == 2)
        {
            ice2.SetActive(true);
        }
        else if(iceCount == 3)
        {
            ice3.SetActive(true);
        }
        else if(iceCount == 4){
            ice4.SetActive(true);
        }
    }

    public void iceMove()
    {
        iceSpeed += 50;
        iceShield.rotateSpeed += 50;
    }

    public void moreMove()
    {
        playerMove.IncreaseMove(0.5f);
    }

    public void GetHit(int dmg)
    {
        GetComponent<SimpleFlash>().Flash();
        currentHP -= dmg;
        if (currentHP <= 0)
        {
            Die();
        }
        hpSlide.maxValue = maxHP;
        hpSlide.value = currentHP;
        hpText.text = "HP: " + currentHP + "/" + maxHP;
    }

    public void Heal(int hp)
    {
        if(currentHP + hp > maxHP)
        {
  
        }
        else
        {
            currentHP += hp;
        }
        hpSlide.maxValue = maxHP;
        hpSlide.value = currentHP;
        hpText.text = "HP: " + currentHP + "/" + maxHP;
    }

    public void GetEXP(int e)
    {
        EXP += e;
        if(EXP >= requiredEXP)
        {
            EXP = 0;
            LevelUp();
        }
        expSlide.maxValue = requiredEXP;
        expSlide.value = EXP;

    }

    public void LevelUp()
    {
        Time.timeScale = 0;
        LevelUpBox.SetActive(true);
        LevelUpBox.GetComponent<UpgradeSelect>().LevelUp();
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
        GameObject fish = Instantiate(prefabAttack, new Vector3(transform.position.x,transform.position.y,transform.position.z), Quaternion.Euler(transform.rotation.x,projRotate,transform.rotation.z));
        fish.GetComponent<Projectile>().SetStats(fishAttack, 10f, fishKB);
    }

    
}
