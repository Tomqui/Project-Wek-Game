using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;
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
    public int level;

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

    [SerializeField] GameObject preDamagePopup;
    [SerializeField] TextMeshProUGUI KillCounter;
    int killCount;

    [SerializeField] PostProcessVolume ppv;
    [SerializeField] Vignette vignette;

    private void SetUp() {
        currentHP = 25;
        maxHP = 25;
        EXP = 0;
        requiredEXP = 15;
        killCount = 0;

        iceCount = 1;
        iceAttack = 1;
        iceKB = 2;
        iceSpeed = 100;
        iceShield.rotateSpeed = iceSpeed;
        updateIce();

        fishAttack = 3;
        fishKB = 1;
        attackCooldown = 1;

        lifesteal = 0;
        playerMove.moveSpeed = 2;
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
        ppv.profile.TryGetSettings(out vignette);
        ChangeScreen();
        LevelUp();
       
        
    }

    public void updateIce()
    {
        ice1.GetComponent<Projectile>().attack = (int)iceAttack;
        ice1.GetComponent<Projectile>().thrust = (int)iceKB;
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

        GameObject dmgpop = Instantiate(preDamagePopup, new Vector3(transform.position.x, transform.position.y - 0.5f, transform.position.z), Quaternion.identity);
        dmgpop.GetComponentInChildren<DamagePopup>().SetText(dmg);
        dmgpop.GetComponentInChildren<DamagePopup>().SetColor(Color.red);
        ChangeScreen();
    }

    public void Heal(int hp)
    {
        if(currentHP + hp > maxHP || hp == 0)
        {
        
        }
        else
        {
            currentHP += hp;
            GameObject dmgpop = Instantiate(preDamagePopup, new Vector3(transform.position.x, transform.position.y - 0.5f, transform.position.z), Quaternion.identity);
            dmgpop.GetComponentInChildren<DamagePopup>().SetText(hp);
            dmgpop.GetComponentInChildren<DamagePopup>().SetColor(Color.green);
        }
        hpSlide.maxValue = maxHP;
        hpSlide.value = currentHP;
        hpText.text = "HP: " + currentHP + "/" + maxHP;
        ChangeScreen();
        
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

        killCount++;
        KillCounter.text = killCount+"";
    }

    public void LevelUp()
    {
        Time.timeScale = 0;
        LevelUpBox.SetActive(true);
        LevelUpBox.GetComponent<UpgradeSelect>().LevelUp();
        updateIce();
        ChangeScreen();
    }

    public void Die()
    {
        //Destroy(gameObject);
    }

    public void ChangeScreen()
    {
        vignette.color.Override(new Color(Mathf.Clamp(1-((float)currentHP / (float)maxHP), 0, 1f), 0, 0,1f));
        vignette.intensity.Override(Mathf.Clamp(0.7f-((float)currentHP)/(float)maxHP,0.3f,0.5f));
    }
    void Attack()
    {
        float xPad = 0.5f;
        float projRotate = 0;
        if (transform.rotation.y%180==0){
            projRotate = 180;
            xPad *= -1;
        }
        else
        {
            projRotate = 0;
            
        }

        GameObject fish = Instantiate(prefabAttack, new Vector3(transform.position.x+xPad,transform.position.y,transform.position.z), Quaternion.Euler(transform.rotation.x,projRotate,transform.rotation.z));
        fish.GetComponent<Projectile>().SetStats(fishAttack, 10f, fishKB);
    }

    
}
