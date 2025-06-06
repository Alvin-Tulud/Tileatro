using UnityEngine;

public class TileInfo : MonoBehaviour
{
    private Color tile_Color;
    private Shape tile_Shape;
    private Mat tile_Mat;

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

        setValues((Color)0, (Shape)0, (Mat)0);

        onPlayBoard = false;
    }

    public void setValues(Color c,Shape s, Mat m)
    {
        tile_Color = c;
        tile_Shape = s;
        tile_Mat = m;

        displayShapeColor = sprite_ShapeColor[(6 * (int)c) + (int)s];
        displayMat = sprite_Mat[(int)m];

        renderShapeColor.sprite = displayShapeColor;
        renderMat.sprite = displayMat;
    }

    //-------------------getters and setters------------------------------
    public void setColor(Color c)
    {
        setValues(c, tile_Shape, tile_Mat);
    }

    public void setShape(Shape s)
    {
        setValues(tile_Color, s, tile_Mat);
    }

    public void setMat(Mat m)
    {
        setValues(tile_Color, tile_Shape, m);
    }

    public void setOnePlayBoard(bool b)
    {
        onPlayBoard = b;
    }

    public Color getColor()
    {
        return tile_Color;
    }

    public Shape getShape()
    {
        return tile_Shape;
    }

    public Mat getMat()
    {
        return tile_Mat;
    }
    public bool getOnePlayBoard()
    {
        return onPlayBoard;
    }
}
