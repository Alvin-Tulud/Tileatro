using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class TileBag : MonoBehaviour
{
    public GameObject baseTile;

    public Sprite[] sprite_ShapeColor;
    public Sprite[] sprite_Mats;

    public List<GameObject> totalBag;
    public List<GameObject> activeBag;
    private List<GameObject> usedBag;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        totalBag = new List<GameObject>();
        activeBag = new List<GameObject>();
        usedBag = new List<GameObject>();

        createBag();
    }

    //create a bag of 108 tiles
    //set total tiles static reference
    //set bag of active tiles player will use
    private void createBag()
    {
        //for each tile shape color
        for (int i = 0; i < 36; i++)
        {
            //make 3 copies and add it to total bag
            for (int j = 0; j < 3; j++)
            {
                GameObject newTile = Instantiate(baseTile);
                newTile.transform.SetParent(transform, false);

                TileInfo info = newTile.GetComponent<TileInfo>();

                //set tile values
                info.tile_ShapeColor = (ShapeColor)i;
                info.tile_Mat = (Mat)0;

                totalBag.Add(newTile);

                //set tile inactive in scene
                newTile.SetActive(false);
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
}
