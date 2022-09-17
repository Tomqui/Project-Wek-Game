using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class EnemySystem : MonoBehaviour
{

    public GridManager gridManager;
    public GameObject frontFish;
    public GameObject sandWek;
    public GameObject player;

    float totalTimer;
    float fishTimer;
    float sandTimer;

    public int enemyCount;

    private void Start()
    {

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
        return mob;
    }

    public GameObject SpawnSand()
    {
        int count = 0;
        float x = gridManager.width;
        float y = gridManager.height;
        float pad = gridManager.wallPadding;
        Vector3 coord = new Vector3(Random.Range(pad + 1, x - 1 - pad), Random.Range(pad + 1, y - 1 - pad), 0);
        while (Vector3.Distance(coord, player.transform.position) < 15 || count == 20)
        {
            coord = new Vector3(Random.Range(pad + 1, x - 1 - pad), Random.Range(pad, y - 1 - pad), 0);
            count++;
        }
        GameObject mob = Instantiate(sandWek, coord, Quaternion.identity, GameObject.Find("Enemies").transform);
        enemyCount++;
        return mob;
    }

    private void FixedUpdate()
    {
        totalTimer += Time.fixedDeltaTime;
        fishTimer += Time.fixedDeltaTime;

        float fishRespawn = Mathf.Clamp(7.5f-totalTimer/30,0.5f,10);
        int fishDmgRamp = 2 + (int)(totalTimer/19);
        int fishHpRamp = 10 + (int)(totalTimer/18);
        float fishMoveRamp = 0.75f + (totalTimer / 200);
        float fishChargeRamp = 7 + Mathf.Clamp((totalTimer / 120),0,5);
        float fishRangeRamp = 4 + Mathf.Clamp((totalTimer/150), 0,2.5f);

        sandTimer += Time.fixedDeltaTime;
        float sandRespawn = Mathf.Clamp(10f - totalTimer / 30, 0.7f, 10);
        int sandTouchRamp = 1+ (int)(totalTimer / 75);
        int sandHpRamp = 7+ (int)(totalTimer / 25);
        int sandBombRamp = 4 + (int)(totalTimer / 15);
        float sandMoveRamp = 0.4f + (totalTimer / 250);
        float sandDetonateRamp = Mathf.Clamp(1.5f-(totalTimer/60),0.25f,10);

        //first minute
        if(totalTimer <= 10)
        {
            if (fishTimer >= 2 && enemyCount <= 15)
            {
                SpawnFish().GetComponent<Enemy>().SetUpFish(fishDmgRamp, fishHpRamp, fishMoveRamp, fishChargeRamp,fishRangeRamp);
                fishTimer = 0.0f;
            }
            if (sandTimer >= 3 && enemyCount <= 15)
            {
                SpawnSand().GetComponent<Enemy>().SetUpSand(sandTouchRamp, sandHpRamp, 1, sandBombRamp, sandDetonateRamp);
                sandTimer = 0.0f;
            }
        }
        else if (totalTimer <= 60)
        {
            if (fishTimer >= 5 && enemyCount <= 15)
            {
                SpawnFish().GetComponent<Enemy>().SetUpFish(fishDmgRamp, fishHpRamp,fishMoveRamp,fishChargeRamp,fishRangeRamp);
                fishTimer = 0.0f;
            }
            if(sandTimer >= 10 && enemyCount <= 15)
            {
                SpawnSand().GetComponent<Enemy>().SetUpSand(sandTouchRamp,sandHpRamp,1,sandBombRamp,sandDetonateRamp);
                sandTimer = 0.0f;
            }
        }
        //start ramping
        else if(totalTimer >= 60)
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
        }


        
    }
}
