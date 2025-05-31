using UnityEngine;

public class PlayGridGenerator : MonoBehaviour
{
    public GameObject ValidTile;
    public Vector3 startingLocalPos;
    public float offset;

    private GameObject[,] playGrid = new GameObject[6,6];
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        generateGrid();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void generateGrid()
    {
        Vector3 nextLocalPos = startingLocalPos;
        for (int i = 0; i < playGrid.GetLength(0); i++)
        {
            for (int j = 0; j < playGrid.GetLength(1); j++)
            {
                Debug.Log(i + " " +  j);

                GameObject spawnedTile = Instantiate(ValidTile, transform);
                spawnedTile.transform.SetParent(transform, false);
                spawnedTile.transform.localPosition = nextLocalPos;

                playGrid[i,j] = spawnedTile;

                nextLocalPos.x += offset;
            }

            nextLocalPos.x = startingLocalPos.x;
            nextLocalPos.y -= offset;
        }
    }
}
