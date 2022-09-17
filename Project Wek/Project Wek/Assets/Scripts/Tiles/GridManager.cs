using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    [SerializeField] public int width,height,wallPadding;

    [SerializeField] private GameObject tile1,tile2,tile3,tile4,tile5,wall1,wall2;
    [SerializeField] private GameObject extra1,extra2,extra3,extra4,extra5,extra6;
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
             
                if(Random.Range(0, 75) == 1)
                {
                    GameObject extra = null;
                    switch (Random.Range(0,6))
                    {
                        case (0):
                            extra = extra1;
                            break;
                        case (1):
                            extra = extra2;
                            break;
                        case (2):
                            extra = extra3;
                            break;
                        case (3):
                            extra = extra4;
                            break;
                        case (4):
                            extra = extra5;
                            break;
                        case (5):
                            extra = extra6;
                            break;
                    }
                    var extraTile = Instantiate(extra, new Vector3(x+0.5f, y+0.5f, 5), Quaternion.identity, gameObject.transform);
                    extraTile.name = $"Extra {x} {y}";
                }

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
