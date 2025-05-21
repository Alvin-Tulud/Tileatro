using UnityEngine;

public class gridposition : MonoBehaviour
{
    public Vector3 gridpos;
    Grid playfield;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        playfield = transform.parent.GetComponent<Grid>();
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = playfield.LocalToCell(gridpos);
    }
}
