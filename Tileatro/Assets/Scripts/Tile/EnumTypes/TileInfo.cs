using UnityEngine;

public class TileInfo : MonoBehaviour
{
    public ShapeColor tile_ShapeColor;
    public Mat tile_Mat;

    public Sprite[] sprite_ShapeColor;
    public Sprite[] sprite_Mat;

    private Sprite displayShapeColor;
    private Sprite displayMat;

    private SpriteRenderer renderShapeColor;
    private SpriteRenderer renderMat;

    private bool onPlayBoard;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        renderShapeColor = transform.GetChild(0).GetComponent<SpriteRenderer>();
        renderMat = transform.GetChild(1).GetComponent<SpriteRenderer>();

        setValues((ShapeColor)0, (Mat)0);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void setValues(ShapeColor sc, Mat m)
    {
        tile_ShapeColor = sc;
        tile_Mat = m;

        displayShapeColor = sprite_ShapeColor[(int)sc];
        displayMat = sprite_Mat[(int)m];

        renderShapeColor.sprite = displayShapeColor;
        renderMat.sprite = displayMat;
    }
}
