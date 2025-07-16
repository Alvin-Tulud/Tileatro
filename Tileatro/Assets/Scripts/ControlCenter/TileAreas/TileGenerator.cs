using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System.Collections;

public class TileGenerator : MonoBehaviour
{
    private TileBag tb;
    private TileValidity tv;
    private PlayGridGenerator pgg;

    private GameObject[,] playGrid = new GameObject[6, 6];

    private List<GameObject> tiles = new List<GameObject>();
    public List<Vector3> tileSpots = new List<Vector3>();
    private int tileCount;

    private bool canMoveTiles;
    private bool canCheck;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        tb = FindAnyObjectByType<TileBag>();
        tv = FindAnyObjectByType<TileValidity>();
        pgg = FindAnyObjectByType<PlayGridGenerator>();

        canMoveTiles = true;
        canCheck = true;
    }

    private void FixedUpdate()
    {
        if (!tv.checkValidity() && canMoveTiles)
        {
            Debug.Log("invalid positions");
            SetTilePosValid();
        }
        if (canCheck)
        {
            StartCoroutine(checkValid());
        }
    }

    private IEnumerator checkValid()
    {
        canCheck = false;

        yield return new WaitForSeconds(0.1f);

        if (tv.checkValidity())
        {
            canMoveTiles = false;
        }

        canCheck = true;
    }

    public void setTiles(int Count)
    {
        tiles = tb.getTiles(Count);
        tileCount = Count;

        foreach (GameObject tile in tiles)
        {
            tile.SetActive(true);
            tile.GetComponent<draggable>().playerMovable = false;
            tile.GetComponent<TileInfo>().setOnePlayBoard(true);
            tile.GetComponent<draggable>().playerMovable = false;
            Debug.Log(tile.GetComponent<draggable>().playerMovable);
        }

        SetTilePosValid();
    }

    private void SetTilePosValid()
    {
        tileSpots = getTileSpots();

        for (int i = 0; i < tileCount; i++)
        {
            tiles[i].transform.position = tileSpots[i];
        }

        if (tv.checkValidity())
        {
            Debug.Log("Valid Spots Found");
        }
    }
    
    //converts tile grid into list and randomizes its order
    private List<Vector3> getTileSpots()
    {
        playGrid = pgg.getTileRack();

        List<Vector3> Spots = new List<Vector3>();

        for (int i = 0; i < playGrid.GetLength(0); i++)
        {
            for (int j = 0; j < playGrid.GetLength(1); j++)
            {
                Spots.Add(playGrid[i, j].transform.position);
            }
        }

        Spots = Spots.OrderBy(x => Random.value).ToList();

        return Spots;
    }
}
