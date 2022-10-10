using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class EnemySystem : MonoBehaviour
{

    public GridManager gridManager;
    public GameObject frontFish;
    public GameObject sandWek;
    public GameObject handWek;
    public GameObject player;

    float totalTimer;
    float fishTimer;
    float sandTimer;
    float handTimer;

    public int enemyCount;
    public int totalCount;

    private void Start()
    {
        Time.timeScale = 1;
    }

    public GameObject SpawnFish()
    {
        int count = 0;
        float x = gridManager.width;
        float y = gridManager.height;
        float pad = gridManager.wallPadding;
        Vector3 coord = new Vector3(Random.Range(pad+1,x-1-pad),Random.Range(pad+1,y-1-pad), 0);
        while(Vector3.Distance(coord, player.transform.position)<10 || count == 20){
            coord = new Vector3(Random.Range(pad+1, x-1-pad), Random.Range(pad, y-1-pad), 0);
            count++;
        }
        GameObject mob = Instantiate(frontFish, coord, Quaternion.identity, GameObject.Find("Enemies").transform);
        enemyCount++;
        totalCount++;
        return mob;
    }

    public GameObject SpawnSand()
    {
        int count = 0;
        float x = gridManager.width;
        float y = gridManager.height;
        float pad = gridManager.wallPadding;
        Vector3 coord = new Vector3(Random.Range(pad + 1, x - 1 - pad), Random.Range(pad + 1, y - 1 - pad), 0);
        while (Vector3.Distance(coord, player.transform.position) < 12.5 || count == 20)
        {
            coord = new Vector3(Random.Range(pad + 1, x - 1 - pad), Random.Range(pad, y - 1 - pad), 0);
            count++;
        }
        GameObject mob = Instantiate(sandWek, coord, Quaternion.identity, GameObject.Find("Enemies").transform);
        enemyCount++;
        totalCount++;
        return mob;
    }

    public GameObject SpawnHand()
    {
        int count = 0;
        float x = gridManager.width;
        float y = gridManager.height;
        float pad = gridManager.wallPadding;
        Vector3 coord = new Vector3(Random.Range(pad + 1, x - 1 - pad), Random.Range(pad + 1, y - 1 - pad), 0);
        while (Vector3.Distance(coord, player.transform.position) < 15 || count == 50)
        {
            coord = new Vector3(Random.Range(pad + 1, x - 1 - pad), Random.Range(pad, y - 1 - pad), 0);
            count++;
        }
        GameObject mob = Instantiate(handWek, coord, Quaternion.identity, GameObject.Find("Enemies").transform);
        enemyCount++;
        totalCount++;
        return mob;
    }

    private void FixedUpdate()
    {
        totalTimer += Time.fixedDeltaTime;

        fishTimer += Time.fixedDeltaTime;
        float fishRespawn = Mathf.Clamp(7f-totalTimer/70,1.2f,10)-Mathf.Clamp(((-1/(0.1f*totalTimer))+1),0,1);
        int fishDmgRamp = 2 + (int)(totalTimer/25);
        int fishHpRamp = 8 + (int)(totalTimer/12);
        float fishMoveRamp = 0.75f + (totalTimer / 240);
        float fishChargeRamp = 7 + Mathf.Clamp((totalTimer / 200),0,5);
        float fishRangeRamp = 4 + Mathf.Clamp((totalTimer/450), 0,2.5f);

        sandTimer += Time.fixedDeltaTime;
        float sandRespawn = Mathf.Clamp(8f - totalTimer / 75, 1.225f, 10)-Mathf.Clamp(((-1 / (0.1f * totalTimer)) + 1), 0, 1);
        int sandTouchRamp = 1+ (int)(totalTimer / 75);
        int sandHpRamp = 5+ (int)(totalTimer / 16);
        int sandBombRamp = 4 + (int)(totalTimer / 21);
        float sandMoveRamp = 0.6f + (totalTimer / 270);
        float sandDetonateRamp = Mathf.Clamp(2.0f-(totalTimer/200),0.1f,10);

        handTimer += Time.fixedDeltaTime;
        float handRespawn = Mathf.Clamp(10.5f - totalTimer / 80, 1.25f, 10) - Mathf.Clamp(((-1 / (0.1f * totalTimer)) + 1), 0, 1);
        int handTouchRamp = 3 + (int)(totalTimer / 50);
        int handHpRamp = 7 + (int)(totalTimer / 14);
        int handLaserRamp = 1 + (int)(totalTimer / 55);
        float handMoveRamp = Mathf.Clamp(0.25f + (totalTimer / 300), 0, 1);
        float handTime1Ramp = Mathf.Clamp(0.25f + (int)(totalTimer/500),0.5f,2.5f);
        float handTime2Ramp = Mathf.Clamp(2-(int)(totalTimer/500),0.25f,3);

        //first minute
        if (totalTimer <= 10)
        {
            if (fishTimer >= 2)
            {
                SpawnFish().GetComponent<Enemy>().SetUpFish(fishDmgRamp, fishHpRamp, fishMoveRamp, fishChargeRamp,fishRangeRamp);
                fishTimer = 0.0f;
            }
            if (sandTimer >= 3)
            {
                SpawnSand().GetComponent<Enemy>().SetUpSand(sandTouchRamp, sandHpRamp, 1, sandBombRamp, sandDetonateRamp);
                sandTimer = 0.0f;
            }
        }
        else if (totalTimer <= 60)
        {
            if (fishTimer >= 4)
            {
                SpawnFish().GetComponent<Enemy>().SetUpFish(fishDmgRamp, fishHpRamp,fishMoveRamp,fishChargeRamp,fishRangeRamp);
                fishTimer = 0.0f;
            }
            if(sandTimer >= 11)
            {
                SpawnSand().GetComponent<Enemy>().SetUpSand(sandTouchRamp,sandHpRamp,1,sandBombRamp,sandDetonateRamp);
                sandTimer = 0.0f;
            }
            if (handTimer >= 25)
            {
                SpawnHand().GetComponent<Enemy>().SetUpHand(handTouchRamp, handHpRamp, handMoveRamp, handLaserRamp, handTime1Ramp, handTime2Ramp);
                handTimer = 0.0f;
            }
        }
        //start ramping
        else if(totalTimer >= 60 && enemyCount <= 1000)
        {
            if (fishTimer >= fishRespawn)
            {
                SpawnFish().GetComponent<Enemy>().SetUpFish(fishDmgRamp, fishHpRamp, fishMoveRamp, fishChargeRamp, fishRangeRamp);
                fishTimer = 0.0f;
            }
            if (sandTimer >= sandRespawn)
            {
                SpawnSand().GetComponent<Enemy>().SetUpSand(sandTouchRamp, sandHpRamp,1, sandBombRamp,sandDetonateRamp);
                sandTimer = 0.0f;
            }
            if (handTimer >= handRespawn)
            {
                SpawnHand().GetComponent<Enemy>().SetUpHand(handTouchRamp, handHpRamp, handMoveRamp, handLaserRamp, handTime1Ramp, handTime2Ramp);
                handTimer = 0.0f;
            }
        }


        
    }
}
