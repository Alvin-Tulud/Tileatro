using UnityEngine;

public class TileRackGenerator : MonoBehaviour
{
    public GameObject ValidTile;
    public Vector3 startingLocalPos;
    public float offset;

    private GameObject[] tileRack = new GameObject[6];
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        generateGrid();
    }

    private void generateGrid()
    {
        Vector3 nextLocalPos = startingLocalPos;
        for (int i = 0; i < tileRack.GetLength(0); i++)
        {
            GameObject spawnedTile = Instantiate(ValidTile, transform);
            spawnedTile.transform.SetParent(transform, false);
            spawnedTile.transform.localPosition = nextLocalPos;

            tileRack[i] = spawnedTile;

            nextLocalPos.x += offset;
        }
    }

    public GameObject[] getTileRack()
    {
        return tileRack;
    }
}
