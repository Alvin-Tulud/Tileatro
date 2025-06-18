using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class TileGenerator : MonoBehaviour
{
    private TileBag tb;
    private TileValidity tv;
    private PlayGridGenerator pgg;

    private GameObject[,] playGrid = new GameObject[6, 6];
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        tb = FindAnyObjectByType<TileBag>();
        tv = FindAnyObjectByType<TileValidity>();
        pgg = FindAnyObjectByType<PlayGridGenerator>();
    }

    public void setTiles(int tileCount)
    {
        List<GameObject> tiles = tb.getTiles(tileCount);
        List<Vector3> tileSpots = new List<Vector3>();
        tileSpots = getTileSpots();

        foreach (GameObject tile in tiles)
        {
            tile.SetActive(true);
            tile.GetComponent<draggable>().playerMovable = false;
            tile.GetComponent<TileInfo>().setOnePlayBoard(true);
        }

        for (int i = 0; i < tileCount; i++)
        {
            tiles[i].transform.position = tileSpots[i];
        }
        Debug.Log("place tiles in spots");

        tv.checkValidity();

        while (!tv.checkValidity())
        {
            tileSpots = getTileSpots();

            for (int i = 0; i < tileCount; i++)
            {
                tiles[i].transform.position = tileSpots[i];
            }
            Debug.Log("non valid placements");
        }

        Debug.Log("done");
    }
    
    //converts tile grid into list and randomizes its order
    private List<Vector3> getTileSpots()
    {
        playGrid = pgg.getTileRack();

        List<Vector3> tileSpots = new List<Vector3>();

        for (int i = 0; i < playGrid.GetLength(0); i++)
        {
            for (int j = 0; j < playGrid.GetLength(1); j++)
            {
                tileSpots.Add(playGrid[i, j].transform.position);
            }
        }

        tileSpots = tileSpots.OrderBy(x => Random.value).ToList();

        return tileSpots;
    }
}
