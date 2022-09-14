using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    [SerializeField] public int width,height,wallPadding;

    [SerializeField] private GameObject tile1,tile2,tile3,tile4,tile5,wall1,wall2;

    [SerializeField] private GameObject playerObject;
   
    void GenerateGrid()
    {
        GameObject[] tiles = {tile1,tile2,tile3,tile4,tile5};

        for(int x = 0; x < width; x++)
        {
            for(int y = 0; y < height; y++)
            {
                GameObject tile;
                if ( ((x==wallPadding) && ((y>=wallPadding+1)&&(y<=height-wallPadding))) || ((x==width-wallPadding) && ((y >= wallPadding+1) && (y <= height - wallPadding)))) 
                {
                    tile = wall2;
                    var spawnedWall = Instantiate(tile, new Vector3(x, y, 5), Quaternion.identity, gameObject.transform);
                    spawnedWall.name = $"Wall {x} {y}";
                }
                else if ( ((y==wallPadding) && (x>=wallPadding && x<=width-wallPadding)) || ((y == height-wallPadding) && (x >= wallPadding && x <= width - wallPadding))) 
                {
                    tile= wall1;
                    var spawnedWall = Instantiate(tile, new Vector3(x, y, 5), Quaternion.identity, gameObject.transform);
                    spawnedWall.name = $"Wall {x} {y}";
                }
                
                int r = Random.Range(0, 4);
                tile = tiles[r];
             
                var spawnedTile = Instantiate(tile, new Vector3(x, y,5), Quaternion.identity,gameObject.transform);
                spawnedTile.name = $"Tile {x} {y}";
            }
        }

        playerObject.transform.position = new Vector3((float)width / 2 - 0.5f, (float)height / 2 - 0.5f);
    }

    private void Start()
    {
        GenerateGrid();
    }
}
