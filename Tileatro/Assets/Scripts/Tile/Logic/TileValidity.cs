using System.Collections.Generic;
using System.Data;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class TileValidity : MonoBehaviour
{
    public LayerMask TileMask;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    //check if placed tile is by itself
    //check if placed tiles are in a line
    //check if placed tiles are matching shape or color in row and column
    //check if repeating tiles in row and column
    public bool checkValidity()
    {
        draggable[] TilesFound = FindObjectsByType<draggable>(FindObjectsSortMode.None);
        List<GameObject> TilePos = new List<GameObject>();
        for (int i = 0; i < TilesFound.Length; i++)
        {
            TilePos.Add(TilesFound[i].gameObject);
        }

        if (!checkTilesAlign(TilePos))
        {
            Debug.Log("not aligned");
            return false;
        }

        if (!checkNoRepeat(TilePos))
        {
            Debug.Log("repeating tiles");
            return false;
        }

        //traverse through entire grid
        for (int i = 0; i < TilePos.Count; i++)
        {
            if (!checkNotAlone(TilePos[i]))
            {
                Debug.Log("tile alone");
                return false;
            }
            if (!checkMatching(TilePos[i]))
            {
                Debug.Log("nearby mismatch");
                return false;
            }
        }

        return true;
    }

    //if tile is placed check make sure it has neighbors
    private bool checkNotAlone(GameObject g)
    {
        if (g.GetComponent<draggable>().playerMovable)
        {
            RaycastHit2D findTileRight = Physics2D.CircleCast(g.transform.position + Vector3.right, 0.1f, Vector2.zero, 0f, TileMask);
            RaycastHit2D findTileUp = Physics2D.CircleCast(g.transform.position + Vector3.up, 0.1f, Vector2.zero, 0f, TileMask);
            RaycastHit2D findTileLeft = Physics2D.CircleCast(g.transform.position + Vector3.left, 0.1f, Vector2.zero, 0f, TileMask);
            RaycastHit2D findTileDown = Physics2D.CircleCast(g.transform.position + Vector3.down, 0.1f, Vector2.zero, 0f, TileMask);

            if (!findTileRight ||
                !findTileUp ||
                !findTileLeft ||
                !findTileDown)
            {
                return false;
            }
        }

        return true;
    }

    //iterate through all tiles and add them to the list
    //check after if any aren't matching x or y and throw false
    private bool checkTilesAlign(List<GameObject> TilePos)
    {
        List<Vector3> FoundPlacedTilesPos = new List<Vector3>();

        foreach (GameObject g in TilePos)
        {
            if (g.GetComponent<draggable>().playerMovable)
            {
                Debug.Log(g.GetComponent<draggable>().playerMovable);
                FoundPlacedTilesPos.Add(g.gameObject.transform.position);
            }
        }

        bool checkHorizontal = false;
        bool checkVertical = false;

        if (FoundPlacedTilesPos.Count > 0)
        {
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

        if (findOtherTile)
        {
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

        if (findOtherTile)
        {
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
        }

        return true;
    }

    //make sure not tiles in row or column are repeating if they are not connected in a line
    private bool checkNoRepeat(List<GameObject> TilePos)
    {
        for (int i = 0; i < TilePos.Count - 1; i++)
        {
            for (int j = i + 1; j < TilePos.Count; j++)
            {
                if (TilePos[i].GetComponent<TileInfo>().getColor() == TilePos[j].GetComponent<TileInfo>().getColor() &&
                    TilePos[i].GetComponent<TileInfo>().getShape() == TilePos[j].GetComponent<TileInfo>().getShape())
                {
                    if (Mathf.RoundToInt(TilePos[i].transform.position.x) == Mathf.RoundToInt(TilePos[j].transform.position.x))
                    {
                        //check all tiles along y axis to see if they meet
                        int distance = Mathf.RoundToInt(TilePos[i].transform.position.y - TilePos[j].transform.position.y);

                        bool touching = true;

                        for (int k = 1; k < distance; k++)
                        {
                            RaycastHit2D findTile = Physics2D.CircleCast(TilePos[i].transform.position + Vector3.down * distance, 0.1f, Vector2.zero, 0f, TileMask);

                            if (!findTile)
                            {
                                touching = false;
                            }
                        }

                        if (touching)
                        {
                            //Debug.Log("found repeat X");
                            return false;
                        }
                    }
                    else if (Mathf.RoundToInt(TilePos[i].transform.position.y) == Mathf.RoundToInt(TilePos[j].transform.position.y))
                    {
                        //check all tiles along x axis to see if they meet
                        int distance = Mathf.RoundToInt(TilePos[i].transform.position.x - TilePos[j].transform.position.x);

                        bool touching = true;

                        for (int k = 1; k < distance; k++)
                        {
                            RaycastHit2D findTile = Physics2D.CircleCast(TilePos[i].transform.position + Vector3.right * distance, 0.1f, Vector2.zero, 0f, TileMask);

                            if (!findTile)
                            {
                                touching = false;
                            }
                        }

                        if (touching)
                        {
                            //Debug.Log("found repeat Y");
                            return false;
                        }
                    }
                }
            }
        }


        return true;
    }
}
