using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    [SerializeField] PlayfabManager pm;

    public int currentHP = 100;
    public int maxHP = 100;

    public GameObject prefabAttack;

    public int fishAttack;
    public float fishKB;
    public float attackCooldown = 1f;
    public int iceAttack;
    public int iceCount;
    public float iceKB;
    public float iceSpeed;
    public int lifesteal;
    public int level;

    [SerializeField] int EXP;
    [SerializeField] int requiredEXP;

    [SerializeField] PlayerMovement playerMove;

    [SerializeField] RotateShield iceShield;
    [SerializeField] private GameObject ice1;
    [SerializeField] private GameObject ice2;
    [SerializeField] private GameObject ice3;

    [SerializeField] Slider hpSlide;
    [SerializeField] TextMeshProUGUI hpText;
    [SerializeField] Slider expSlide;

    [SerializeField] GameObject LevelUpBox;

    [SerializeField] GameObject preDamagePopup;
    [SerializeField] TextMeshProUGUI KillCounter;
    int killCount;

    [SerializeField] PostProcessVolume ppv;
    [SerializeField] Vignette vignette;
    [SerializeField] GameObject gameOver;

    [SerializeField] AudioSource shotSound;
    [SerializeField] AudioSource hitSound;

    Vector2 mousePos;
    public float totalDMG;

    bool isSpeedBoost;
    float speedTime;
    float speedTimeLimit;

    bool isAttackBoost;
    float attackTime;
    float attackTimeLimit;

    [SerializeField] Transform gunTip;
    [SerializeField] GameObject pauseMenu;
    [SerializeField] AudioListener audio;

    bool gotScore;

    private void SetUp() {
        currentHP = 25;
        maxHP = 25;
        EXP = 0;
        requiredEXP = 1;
        killCount = 0;

        iceCount = 1;
        iceAttack = 5;
        iceKB = 4;
        iceSpeed = 100;
        iceShield.rotateSpeed = iceSpeed;
        updateIce();

        fishAttack = 3;
        fishKB = 2.0f;
        attackCooldown = 1.1f;
        totalDMG = 1f;

        lifesteal = 0;
        playerMove.moveSpeed = 1.75f;
        level = 0;

        isSpeedBoost = false;
        speedTime = 0;
        speedTimeLimit = 5f;

        isAttackBoost = false;
        attackTime = 0;
        attackTimeLimit = 5;

    }

    private void Start()
    {
        SetUp();
        
        hpSlide.maxValue = maxHP;
        hpSlide.value = currentHP;
        hpText.text = "HP: "+currentHP+"/"+maxHP;

        expSlide.maxValue = requiredEXP;
        expSlide.value = EXP;

        ice1 = GameObject.Find("Rotating Shield");

        InvokeRepeating("Attack", attackCooldown, attackCooldown);
        ppv.profile.TryGetSettings(out vignette);
        ChangeScreen();

        level = 0;
        gotScore = false;
    }

    public void updateIce()
    {
        ice1.GetComponent<Projectile>().attack = (int)Mathf.Round(iceAttack*totalDMG);
        ice1.GetComponent<Projectile>().thrust = (int)iceKB;

        ice2.GetComponent<Projectile>().attack = (int)Mathf.Round(iceAttack * totalDMG);
        ice2.GetComponent<Projectile>().thrust = (int)iceKB;

        ice3.GetComponent<Projectile>().attack = (int)Mathf.Round(iceAttack * totalDMG);
        ice3.GetComponent<Projectile>().thrust = (int)iceKB;

    }

    public void changeAS()
    {
        CancelInvoke();
        InvokeRepeating("Attack", attackCooldown, attackCooldown);
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
    }

    public void iceMove()
    {
        iceSpeed += 70;
        iceShield.rotateSpeed += 70;
    }

    public void moreMove()
    {
        playerMove.IncreaseMove(0.65f);
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

        hitSound.Play();
        GameObject dmgpop = Instantiate(preDamagePopup, new Vector3(transform.position.x, transform.position.y - 0.5f, transform.position.z), Quaternion.identity);
        dmgpop.GetComponentInChildren<DamagePopup>().SetText(dmg);
        dmgpop.GetComponentInChildren<DamagePopup>().SetColor(Color.red);
        ChangeScreen();

        if(currentHP <= 0)
        {
            EndGame();
        }
    }

    public void EndGame()
    {

        gameOver.SetActive(true);
        if (!gotScore)
        {
            pm.GetLeaderboard();
            gotScore = true;
            if (killCount > GameObject.Find("Enemies").GetComponent<EnemySystem>().totalCount)
            {
                pm.SendLeaderboardTwo(killCount);
            }
            pm.SendLeaderboard(killCount);

            Time.timeScale = 0;
        }

    }

    public void Heal(int hp)
    {
        if(hp == 0)
        {

        }
        else
        {
            if (currentHP + hp > maxHP)
            {
                currentHP = maxHP;
            }
            else
            {
                currentHP += hp;
            }
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
            if (level <= 20)
            {
                requiredEXP += 1;
            }
            else if(level <= 25)
            {
                requiredEXP += 2;
            }
            else if(level <= 30)
            {
                requiredEXP += 3;
            }
            else if(level <= 25)
            {
                requiredEXP += 4;
            }
            else if(level <= 30)
            {
                requiredEXP += 5;
            }
            else if(level < 35)
            {
                requiredEXP += 10;
            }
            else if (level < 40)
            {
                requiredEXP += 25;
            }
            else if (level < 50)
            {
                requiredEXP += 50;
            }
            else if(level < 60)
            {
                requiredEXP += 75;
            }
            else
            {
                requiredEXP += 100;
            }


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

        playerMove.moveSpeed += 0.05f;
        totalDMG += 0.035f;
        maxHP += 2;
        currentHP += 2;
        level++;
        Heal(5);

        ChangeScreen();
        
    }

    public void StartSpeedBoost()
    {
        speedTime = 0;
        if (!isSpeedBoost)
        {
            playerMove.moveSpeed += 2f;
        }
        isSpeedBoost = true;
    }

    public void EndSpeedBoost()
    {
        isSpeedBoost = false;
        playerMove.moveSpeed -= 2f;
    }

    public void StartAttackBoost()
    {
        attackTime = 0;
        if (!isAttackBoost)
        {
            totalDMG += 1;
        }
        isAttackBoost = true;
        updateIce();
    }

    public void EndAttackBoost()
    {
        isAttackBoost = false;
        totalDMG -= 1;
        updateIce();
    }

    private void Update()
    {
        
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if(Time.timeScale > 0)
            {
                Pause();
            }
        }
        
      
    }

    public void Pause()
    {
        pauseMenu.SetActive(true);
        AudioListener.pause = true;
        Time.timeScale = 0;
    }

    public void UnPause()
    {
        pauseMenu.SetActive(true);
        AudioListener.pause = false;
        Time.timeScale = 1;
        pauseMenu.SetActive(false); 
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

        shotSound.Play();

        Vector3 mousePos = GameObject.Find("Main Camera").GetComponent<Camera>().ScreenToWorldPoint(Input.mousePosition);
        Vector2 dir = (mousePos - transform.position).normalized;
        //GameObject fish = Instantiate(prefabAttack, new Vector3(transform.position.x+(dir.x*1.4f),transform.position.y+(dir.y*1.4f),transform.position.z), Quaternion.identity);
        GameObject fish = Instantiate(prefabAttack,gunTip.position, gunTip.rotation);
        //(int)Mathf.Round((float)fishAttack*totalDMG)
    }

    private void FixedUpdate()
    {
        if (isSpeedBoost)
        {
            speedTime += Time.fixedDeltaTime;
            if(speedTime > speedTimeLimit)
            {
                EndSpeedBoost();
            }
        }

        if (isAttackBoost)
        {
            attackTime += Time.fixedDeltaTime;
            if(attackTime > attackTimeLimit)
            {
                EndAttackBoost();
            }
        }
    }

}
