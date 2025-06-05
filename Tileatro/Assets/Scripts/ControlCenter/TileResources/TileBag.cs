using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class TileBag : MonoBehaviour
{
    public GameObject baseTile;

    public List<GameObject> totalBag;
    public List<GameObject> activeBag;
    private List<GameObject> usedBag;
    private List<GameObject> handBag;

    private TileRackGenerator tileGen;
    private GameObject[] tileRack;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        totalBag = new List<GameObject>();
        activeBag = new List<GameObject>();
        usedBag = new List<GameObject>();
        handBag = new List<GameObject>();

        tileGen = FindAnyObjectByType<TileRackGenerator>();
        tileGen.generateGrid();
        tileRack = tileGen.getTileRack();

        createBag();
    }

    //create a bag of 108 tiles
    //set total tiles static reference
    //set bag of active tiles player will use
    private void createBag()
    {
        //for each tile color
        for (int i = 0; i < 6; i++)
        {
            //for each tile shape
            for (int j = 0; j < 6; j++)
            {
                //make 3 copies and add it to total bag
                for (int k = 0; k < 3; k++)
                {
                    GameObject newTile = Instantiate(baseTile, transform.position, transform.rotation, transform);

                    TileInfo info = newTile.GetComponent<TileInfo>();

                    //set tile values
                    info.setValues((Color)i, (Shape)j, (Mat)0);

                    totalBag.Add(newTile);

                    //set tile inactive in scene
                    newTile.SetActive(false);
                }
            }
        }
        //shuffle active tile bag contents
        activeBag = shuffleBag(totalBag);
    }

    //shuffle
    private List<GameObject> shuffleBag(List<GameObject> bagtoShuffle)
    {
        List<GameObject> shuffledBag = new List<GameObject>();

        shuffledBag = bagtoShuffle.OrderBy(x => Random.value).ToList();

        return shuffledBag;
    }

    public void rerollTiles()
    {

    }

    private void dealHand()
    {

    }
}
