using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;
using Button = UnityEngine.UI.Button;
using Image = UnityEngine.UI.Image;

public class UpgradeBox : MonoBehaviour
{
    [SerializeField] GameObject LevelUpContainer;
    [SerializeField] Player player;
 
    [SerializeField] private Button button;
    [SerializeField] private TextMeshProUGUI title;
    [SerializeField] private TextMeshProUGUI description;
    [SerializeField] private Image icon;
    public int upgradeNum;

    [SerializeField] private Sprite iceCube;
    [SerializeField] private Sprite fish;
    [SerializeField] private Sprite wek;
    [SerializeField] private Sprite heart;
    [SerializeField] private Sprite haste;
    [SerializeField] private Sprite rage;

    private void Start()
    {
       
    }

    public void SetUp(int num)
    {
        switch (num)
        {
            case 1:
                icon.sprite = iceCube;
                title.text = "Ice Cube";
                description.text = "Increase DAMAGE by 2";
                break;
            case 2:
                icon.sprite = iceCube;
                title.text = "Ice Cube";
                description.text = "Increase NUMBER of Ice Cubes by 1";
                break;
            case 3:
                icon.sprite = iceCube;
                title.text = "Ice Cube";
                description.text = "Increase KNOCKBACK";
                break;
            case 4:
                icon.sprite = iceCube;
                title.text = "Ice Cube";
                description.text = "Increase rotation SPEED";
                break;
            case 5:
                icon.sprite = fish;
                title.text = "Fish Shot";
                description.text = "Increase DAMAGE by 2";
                break;
            case 6:
                icon.sprite = fish;
                title.text = "Fish Shot";
                description.text = "Increase KNOCKBACK";
                break;
            case 7:
                icon.sprite = fish;
                title.text = "Fish Shot";
                description.text = "Increase ATTACKSPEED by 0.12s";
                break;
            case 8:
                icon.sprite = haste;
                title.text = "Wek";
                description.text = "Increase MOVESPEED";
                break;
            case 9:
                icon.sprite = wek;
                title.text = "Wek";
                description.text = "Increase LIFESTEAL by 1";
                break;
            case 10:
                icon.sprite = heart;
                title.text = "Wek";
                description.text = "Increase MAXHEALTH by 30";
                break;
            case 11:
                icon.sprite = rage;
                title.text = "Wek";
                description.text = "Increase TOTALDMG by x1.2";
                break;
            case 12:
                icon.sprite = wek;
                title.text = "Wek";
                description.text = "Nothing";
                break;


        }
        upgradeNum = num;
    }

    public void OnClick()
    {
        switch (upgradeNum) { 
            case 1:
                player.iceAttack += 2;
                player.updateIce();
                break;
            case 2:
                player.moreIce();
                break;
            case 3:
                player.iceKB += 3f;
                break;
            case 4:
                player.iceMove();
                break;
            case 5:
                player.fishAttack += 2;
                break;
            case 6:
                player.fishKB += 2f;
                break;
            case 7:
                player.attackCooldown -= 0.12f;
                if(player.attackCooldown <=0.15f)
                {
                    player.attackCooldown = 0.15f;
                }
                player.changeAS();
                break;
            case 8:
                player.moreMove();
                break;
            case 9:
                player.lifesteal++;
                break;
            case 10:
                player.maxHP += 30;
                player.Heal(30);
                break;
            case 11:
                player.totalDMG += 0.2f;
                player.updateIce();
                break;
        }
        LevelUpContainer.SetActive(false);
        Time.timeScale = 1;
    }

}
