using Packages.Rider.Editor.UnitTesting;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UpgradeSelect : MonoBehaviour
{
    [SerializeField] private UpgradeBox box1;
    [SerializeField] private UpgradeBox box2;
    [SerializeField] private UpgradeBox box3;

    [SerializeField] private Player player;

    [SerializeField]Animator ani;

    private void Start()
    {
        ani.updateMode = AnimatorUpdateMode.UnscaledTime;
    }

    public void LevelUp()
    {
        int choice1,choice2, choice3;
        
        choice1 = GetChoice();
        choice2 = GetChoice(choice1);
        choice3 = GetChoice(choice1,choice2);

        box1.SetUp(choice1);
        box2.SetUp(choice2);
        box3.SetUp(choice3);
    }

    private int GetChoice(int c1 = 0,int c2 = 0)
    {
        bool acceptable = false;
        int upperRange = 11;
        int num = Random.Range(1,upperRange);

        while (!acceptable)
        {
            if(num == c1)
            {
                acceptable = false;
                num = Random.Range(1, upperRange);
            }
            else if (num == c2)
            {
                acceptable = false;
                num = Random.Range(1, upperRange);
            }
            else if (player.iceCount >= 4 && num == 2)
            {
                acceptable = false;
                num = Random.Range(1, upperRange);
            }
            else if(player.attackCooldown <= 0.1 && num == 7)
            {
                acceptable = false;
                num = Random.Range(1, upperRange);
            }
            else
            {
                acceptable = true;
            }
        }

        return num;
    }

}
