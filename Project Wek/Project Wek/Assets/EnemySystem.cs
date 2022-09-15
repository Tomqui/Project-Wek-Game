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

        float fishRespawn = Mathf.Clamp(10f-totalTimer/100,1,10);
        int fishDmgRamp = 2 + (int)(totalTimer/75);
        int fishHpRamp = 7 + (int)(totalTimer/20);

        sandTimer += Time.fixedDeltaTime;
        float sandRespawn = Mathf.Clamp(15f - totalTimer / 100, 1, 10);
        int sandTouchRamp = 1+ (int)(totalTimer / 100);
        int sandHpRamp = 4+ (int)(totalTimer / 40);
        int sandBombRamp = 5 + (int)(totalTimer / 60);

        //first minute
        if (totalTimer <= 60)
        {
            if (fishTimer >= 5 && enemyCount <= 15)
            {
                SpawnFish().GetComponent<Enemy>().SetUpFish(fishDmgRamp, fishHpRamp);
                fishTimer = 0.0f;
            }
            if(sandTimer >= 10 && enemyCount <= 15)
            {
                SpawnSand().GetComponent<Enemy>().SetUpSand(sandTouchRamp,sandHpRamp,sandBombRamp);
                sandTimer = 0.0f;
            }
        }
        //start ramping
        else if(totalTimer >= 60)
        {
            if (fishTimer >= fishRespawn)
            {
                SpawnFish().GetComponent<Enemy>().SetUpFish(fishDmgRamp,fishHpRamp);
                fishTimer = 0.0f;
            }
            if (sandTimer >= sandRespawn)
            {
                SpawnSand().GetComponent<Enemy>().SetUpSand(sandTouchRamp, sandHpRamp, sandBombRamp);
                sandTimer = 0.0f;
            }
        }


        
    }
}
