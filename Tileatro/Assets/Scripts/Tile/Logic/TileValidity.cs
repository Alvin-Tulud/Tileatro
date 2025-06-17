using System.Collections.Generic;
using System.Data;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class TileValidity : MonoBehaviour
{
    private PlayGridGenerator pgg;
    private GameObject[,] playGrid = new GameObject[6, 6];

    public LayerMask TileMask;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        pgg = FindAnyObjectByType<PlayGridGenerator>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //check if placed tile is by itself
    //check if placed tiles are in a line
    //check if placed tiles are matching shape or color in row and column
    //check if repeating tiles in row and column
    public bool checkValidity()
    {
        playGrid = pgg.getTileRack();

        if (!checkTilesAlign())
        {
            return false;
        }

        if (!checkNoRepeat())
        {
            return false;
        }

        //traverse through entire grid
        for (int i = 0; i < playGrid.GetLength(0); i++)
        {
            for (int j = 0; j < playGrid.GetLength(1); j++)
            {
                //if raycast finds a tile
                RaycastHit2D findTile = Physics2D.CircleCast(playGrid[i, j].transform.position, 0.1f, Vector2.zero, 0f, TileMask);

                if (findTile)
                {
                    if (!checkNotAlone(findTile.transform.gameObject, i, j))
                    {
                        return false;
                    }
                    if (!checkMatching(findTile.transform.gameObject))
                    {
                        return false;
                    }
                }
            }
        }

        return true;
    }

    //if tile is placed check make sure it has neighbors
    private bool checkNotAlone(GameObject g, int posX, int posY)
    {
        //if false checks = 4 theres no tile around the tile so its invalid
        int falseChecks = 0;

        if (g.GetComponent<draggable>().playerMovable)
        {
            if (posX >= 0 && posX < playGrid.Length)//check right, obj not on right wall
            {
                RaycastHit2D findTile = Physics2D.CircleCast(g.transform.position + Vector3.right, 0.1f, Vector2.zero, 0f, TileMask);

                if (!findTile)
                {
                    falseChecks++;
                }
            }
            if (posY >= 0 && posY < playGrid.Length)//check up, obj not on bottom wall
            {
                RaycastHit2D findTile = Physics2D.CircleCast(g.transform.position + Vector3.up, 0.1f, Vector2.zero, 0f, TileMask);

                if (!findTile)
                {
                    falseChecks++;
                }
            }
            if (posX > 0 && posX <= playGrid.Length)//check left, obj not on left wall
            {
                RaycastHit2D findTile = Physics2D.CircleCast(g.transform.position + Vector3.left, 0.1f, Vector2.zero, 0f, TileMask);

                if (!findTile)
                {
                    falseChecks++;
                }
            }
            if (posY > 0 && posY <= playGrid.Length)//check down, obj not on top wall
            {
                RaycastHit2D findTile = Physics2D.CircleCast(g.transform.position + Vector3.down, 0.1f, Vector2.zero, 0f, TileMask);

                if (!findTile)
                {
                    falseChecks++;
                }
            }
        }

        if (falseChecks == 4)
        {
            return false;
        }

        return true;
    }

    //iterate through all tiles and add them to the list
    //check after if any aren't matching x or y and throw false
    private bool checkTilesAlign()
    {
        List<Vector3> FoundPlacedTilesPos = new List<Vector3>();

        //traverse through entire grid
        for (int i = 0; i < playGrid.GetLength(0); i++)
        {
            for (int j = 0; j < playGrid.GetLength(1); j++)
            {
                //if raycast finds a tile
                RaycastHit2D findTile = Physics2D.CircleCast(playGrid[i, j].transform.position, 0.1f, Vector2.zero, 0f, TileMask);

                if (findTile && findTile.transform.GetComponent<draggable>().playerMovable)
                {
                    FoundPlacedTilesPos.Add(findTile.transform.position);
                }
            }
        }

        bool checkHorizontal = false;
        bool checkVertical = false;
        Vector3 prevTile = FoundPlacedTilesPos[0];

        for (int i = 1; i < FoundPlacedTilesPos.Count; i++)
        {
            //set if we're checking horizontal or vertical
            //if next tile doesn't match either return false
            if (i == 1)
            {
                if (Mathf.RoundToInt(prevTile.x) == Mathf.RoundToInt(FoundPlacedTilesPos[i].x))
                {
                    checkHorizontal = true;
                }
                else if (Mathf.RoundToInt(prevTile.y) == Mathf.RoundToInt(FoundPlacedTilesPos[i].y))
                {
                    checkVertical = true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                if (checkHorizontal)
                {
                    if (Mathf.RoundToInt(prevTile.x) != Mathf.RoundToInt(FoundPlacedTilesPos[i].x))
                    {
                        return false;
                    }
                }
                else if (checkVertical)
                {
                    if (Mathf.RoundToInt(prevTile.y) == Mathf.RoundToInt(FoundPlacedTilesPos[i].y))
                    {
                        return false;
                    }
                }
            }

            prevTile = FoundPlacedTilesPos[i];
        }

        return true;
    }

    //get tile and check its neighbors if they match
    private bool checkMatching(GameObject g)
    {
        //return false if it finds a row or column thats mismatched
        //doesn't care if obj is movable or not

        bool checkingColor = false;
        bool checkingShape = false;

        //check left right
        RaycastHit2D findTile = Physics2D.CircleCast(g.transform.position + Vector3.right, 0.1f, Vector2.zero, 0f, TileMask);
        RaycastHit2D findOtherTile = Physics2D.CircleCast(g.transform.position + Vector3.left, 0.1f, Vector2.zero, 0f, TileMask);

        if (findTile)
        {
            if (findTile.transform.GetComponent<TileInfo>().getColor() == g.GetComponent<TileInfo>().getColor())
            {
                checkingColor = true;
            }
            else if (findTile.transform.GetComponent<TileInfo>().getShape() == g.GetComponent<TileInfo>().getShape())
            {
                checkingShape = true;
            }
            else
            {
                return false;
            }
        }

        if (checkingColor)
        {
            if (findOtherTile.transform.GetComponent<TileInfo>().getColor() != g.GetComponent<TileInfo>().getColor())
            {
                return false;
            }
        }
        else if (checkingShape)
        {
            if (findOtherTile.transform.GetComponent<TileInfo>().getShape() != g.GetComponent<TileInfo>().getShape())
            {
                return false;
            }
        }


        checkingColor = false;
        checkingShape = false;

        //check up down
        findTile = Physics2D.CircleCast(g.transform.position + Vector3.up, 0.1f, Vector2.zero, 0f, TileMask);
        findOtherTile = Physics2D.CircleCast(g.transform.position + Vector3.down, 0.1f, Vector2.zero, 0f, TileMask);

        if (findTile)
        {
            if (findTile.transform.GetComponent<TileInfo>().getColor() == g.GetComponent<TileInfo>().getColor())
            {
                checkingColor = true;
            }
            else if (findTile.transform.GetComponent<TileInfo>().getShape() == g.GetComponent<TileInfo>().getShape())
            {
                checkingShape = true;
            }
            else
            {
                return false;
            }
        }

        if (checkingColor)
        {
            if (findOtherTile.transform.GetComponent<TileInfo>().getColor() != g.GetComponent<TileInfo>().getColor())
            {
                return false;
            }
        }
        else if (checkingShape)
        {
            if (findOtherTile.transform.GetComponent<TileInfo>().getShape() != g.GetComponent<TileInfo>().getShape())
            {
                return false;
            }
        }

        return true;
    }

    //make sure not tiles in row or column are repeating if they are not connected in a line
    private bool checkNoRepeat()
    {
        List<Vector3> FoundTilesPos = new List<Vector3>();
        List<TileInfo> FoundTiles = new List<TileInfo>();

        //traverse through entire grid
        for (int i = 0; i < playGrid.GetLength(0); i++)
        {
            for (int j = 0; j < playGrid.GetLength(1); j++)
            {
                //if raycast finds a tile
                RaycastHit2D findTile = Physics2D.CircleCast(playGrid[i, j].transform.position, 0.1f, Vector2.zero, 0f, TileMask);

                if (findTile)
                {
                    FoundTilesPos.Add(findTile.transform.position);
                    FoundTiles.Add(findTile.transform.GetComponent<TileInfo>());
                }
            }
        }

        for (int i = 0; i < FoundTiles.Count - 1; i++)
        {
            for (int j = i; j < FoundTiles.Count; j++)
            {
                if (FoundTiles[i].getColor() == FoundTiles[j].getColor() &&
                    FoundTiles[i].getShape() == FoundTiles[j].getShape())
                {
                    if (Mathf.RoundToInt(FoundTilesPos[i].x) == Mathf.RoundToInt(FoundTilesPos[j].x))
                    {
                        //check all tiles along y axis to see if they meet
                        int distance = Mathf.RoundToInt(FoundTilesPos[i].y - FoundTilesPos[j].y);

                        bool touching = true;

                        for (int k = 1; k < distance; k++)
                        {
                            RaycastHit2D findTile = Physics2D.CircleCast(FoundTilesPos[i] + Vector3.down * distance, 0.1f, Vector2.zero, 0f, TileMask);

                            if (!findTile)
                            {
                                touching = false;
                            }
                        }

                        if (touching)
                        {
                            return false;
                        }
                    }
                    else if (Mathf.RoundToInt(FoundTilesPos[i].y) == Mathf.RoundToInt(FoundTilesPos[j].y))
                    {
                        //check all tiles along x axis to see if they meet
                        int distance = Mathf.RoundToInt(FoundTilesPos[i].x - FoundTilesPos[j].x);

                        bool touching = true;

                        for (int k = 1; k < distance; k++)
                        {
                            RaycastHit2D findTile = Physics2D.CircleCast(FoundTilesPos[i] + Vector3.right * distance, 0.1f, Vector2.zero, 0f, TileMask);

                            if (!findTile)
                            {
                                touching = false;
                            }
                        }

                        if (touching)
                        {
                            return false;
                        }
                    }
                }
            }
        }


        return true;
    }
}
