using UnityEngine;
using System.Collections.Generic;

public class TileGenerator : MonoBehaviour
{
    private TileBag tb;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        tb = FindAnyObjectByType(typeof(TileBag)) as TileBag;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void setTiles(int tileCount)
    {
        List<GameObject> tiles = tb.getTiles(tileCount);

        foreach (GameObject tile in tiles)
        {
            tile.SetActive(true);
            tile.GetComponent<draggable>().playerMovable = false;
            tile.GetComponent<TileInfo>().setOnePlayBoard(true);
        }
    }
}
