using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class EnemySystem : MonoBehaviour
{

    public GridManager gridManager;
    public GameObject frontFish;
    public GameObject player;

    float totalTimer;
    float fishTimer;

    public int enemyCount;

    public GameObject SpawnEnemy()
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

    private void FixedUpdate()
    {
        totalTimer += Time.fixedDeltaTime;
        fishTimer += Time.fixedDeltaTime;

        float fishRespawn = Mathf.Clamp(10f-totalTimer/100,1,10);
        int fishDmgRamp = 2 + (int)(totalTimer/75);
        int fishHpRamp = 7 + (int)(totalTimer/20);

        //first minute
        if (totalTimer <= 60)
        {
            if (fishTimer >= 5 && enemyCount <= 10)
            {
                SpawnEnemy();
                fishTimer = 0.0f;
            }
        }
        //start ramping
        else if(totalTimer >= 60)
        {
            if (fishTimer >= fishRespawn && enemyCount <= 10)
            {
                SpawnEnemy().GetComponent<Enemy>().SetUpFish(fishDmgRamp,fishHpRamp);
                fishTimer = 0.0f;
            }
        }


        
    }
}
