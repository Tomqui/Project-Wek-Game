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


    public void SpawnEnemy()
    {
        int count = 0;
        float x = gridManager.width;
        float y = gridManager.height;
        float pad = gridManager.wallPadding;
        Vector3 coord = new Vector3(Random.Range(pad+1,x-1-pad),Random.Range(pad+1,y-1-pad), 0);
        while(Vector3.Distance(coord, player.transform.position)<5 || count == 20){
            coord = new Vector3(Random.Range(pad+1, x-1-pad), Random.Range(pad, y-1-pad), 0);
            count++;
        }
        GameObject mob = Instantiate(frontFish, coord, Quaternion.identity, GameObject.Find("Enemies").transform);
    }

    void Update()
    {
        totalTimer += Time.deltaTime;
        fishTimer += Time.deltaTime;
        if (fishTimer >= 1f)
        {
            SpawnEnemy();
            fishTimer = 0.0f;
        }
    }
}
