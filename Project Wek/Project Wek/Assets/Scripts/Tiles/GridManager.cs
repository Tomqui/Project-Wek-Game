using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    [SerializeField] private int width,height;

    [SerializeField] private GameObject tile1,tile2,tile3,tile4,tile5;

    [SerializeField] private GameObject playerObject;

    void GenerateGrid()
    {
        GameObject[] tiles = {tile1,tile2,tile3,tile4,tile5};

        for(int x = 0; x < width; x++)
        {
            for(int y = 0; y < height; y++)
            {
                int r = Random.Range(0, 4);
                var spawnedTile = Instantiate(tiles[r], new Vector3(x, y,5), Quaternion.identity,gameObject.transform);
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
