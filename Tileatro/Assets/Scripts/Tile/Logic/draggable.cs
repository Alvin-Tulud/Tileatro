using UnityEngine;
using System.Collections.Generic;

public class draggable : MonoBehaviour
{
    public bool playerMovable;
    private bool canMove;
    private bool dragging;
    private bool swapping;
    private Collider2D cardCollider;

    public GameObject SpawnDragTarget;
    public GameObject SpawnTargetChecker;
    private GameObject DragTarget;
    private GameObject TargetChecker;
    private GameObject SwapTarget;
    private Vector3 LastValidPosition;
    public Vector3 BeforeMovePosition;

    [SerializeField]
    private LayerMask OtherObjectMask;
    [SerializeField]
    private LayerMask PlayAreaMask;
    [SerializeField]
    private LayerMask TileRackMask;

    private SpriteRenderer tileShapeColor;
    private SpriteRenderer tileMat;

    void Awake()
    {
        InitializeVariables();
    }

    // Initialize variables such as colliders and layer masks
    void InitializeVariables()
    {
        // Get the collider attached to this game object
        cardCollider = GetComponent<Collider2D>();
        canMove = false;
        dragging = false;
        swapping = false;

        BeforeMovePosition = transform.position;

        tileShapeColor = transform.GetChild(0).GetComponent<SpriteRenderer>();
        tileMat = transform.GetChild(1).GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        HandleMouseInput();
        HandleDragging();
        HandleMouseUp();
    }

    // Handle mouse input to determine if dragging should start
    void HandleMouseInput()
    {
        // Get mouse position in world coordinates
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        // Handle mouse button down event
        if (Input.GetMouseButtonDown(0)) //on click down
        {

            // Check if the object is movable and if the mouse is over it
            if (playerMovable && cardCollider == Physics2D.OverlapPoint(mousePos, OtherObjectMask))//figure out how to make it so the tiles cant be placed on eachother
            {
                canMove = true;
                // Create DragTarget and TargetChecker objects
                CreateDragTargetObjects();
            }
            else
            {
                canMove = false;
            }
            if (canMove)
            {
                dragging = true;
            }
        }

        //if mouse hovering over object
        if (cardCollider == Physics2D.OverlapPoint(mousePos, OtherObjectMask))
        {
            //transform.localScale = Vector3.one * 1.1f;
        }
        else if (cardCollider != Physics2D.OverlapPoint(mousePos, OtherObjectMask))
        {
            //transform.localScale = Vector3.one;
        }
    }

    // Create DragTarget and TargetChecker objects
    void CreateDragTargetObjects()
    {
        GameObject g;//store as gameobject and set them to be 
        g = Instantiate(SpawnDragTarget, transform.position, transform.rotation); //Creates DragTarget
        g.transform.SetParent(transform, false);//Alvin: changed transform.position to vector3.zero because it wasn't instantiating in the middle of the draggable object
        g.transform.position = Vector3.zero;
        DragTarget = g;

        //DragTarget.transform.position = transform.position;
        LastValidPosition = transform.position;//sets it to be where the card initially is

        g = Instantiate(SpawnTargetChecker, transform.position, transform.rotation); //Creates TargetChecker
        g.transform.SetParent(transform, false);
        g.transform.position = Vector3.zero;
        //Debug.Log("IM TRANSFORMING");
        TargetChecker = g;

        // Initialize collision logic for targetchecker here

        // Play on click down SFX
    }

    // Handle dragging behavior
    void HandleDragging()
    {
        // Handle dragging
        if (dragging) //while dragging
        {
            // Move the tile with the mouse
            MoveTileWithMouse();
            // Check the validity of the tile's position
            CheckTileValidity();
        }
    }

    // Move the tile with the mouse
    void MoveTileWithMouse()
    {
        // Get mouse position in world coordinates
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        tileShapeColor.sortingOrder = 3;
        tileMat.sortingOrder = 2;

        tileShapeColor.transform.localScale = Vector3.one * 1.25f;
        tileMat.transform.localScale = Vector3.one * 1.25f;

        //moves tile to mouse pos
        transform.position = mousePos;
    }

    // Check the validity of the tile's position
    void CheckTileValidity()
    {
        // Get mouse position in world coordinates
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        // Round TargetChecker's position to the nearest grid item
        TargetChecker.transform.position = mousePos; //1. Moves TargetChecker and rounds its pos to the tile grid
        //Debug.Log("targetChecker: " + TargetChecker.transform.position);
        //Debug.Log("DragTarget: " + DragTarget.transform.position);

        // Raycast check
        // Temporarily move tile layer to ignore raycast
        int oldLayer = this.gameObject.layer;
        this.gameObject.layer = 2;

        // Raycast from TargetChecker to check for valid tile and belts
        //RaycastHit2D hitValidTile = Physics2D.Raycast(TargetChecker.transform.position, TargetChecker.transform.forward, 0.1f, PlacementlayerMask);
        RaycastHit2D hitOtherObstacle = Physics2D.Raycast(TargetChecker.transform.position, TargetChecker.transform.forward, 0.1f, OtherObjectMask);
        RaycastHit2D hitPlayGrid = Physics2D.Raycast(TargetChecker.transform.position, TargetChecker.transform.forward, 0.1f, PlayAreaMask);
        RaycastHit2D hitTileRack = Physics2D.Raycast(TargetChecker.transform.position, TargetChecker.transform.forward, 0.1f, TileRackMask);

        // Check if the position is valid
        if (hitOtherObstacle)//invalid placement spot
        {
            //Debug.Log("Invalid Spot");
            //handle tiles swapping positions
            if (hitOtherObstacle.transform.GetComponent<draggable>().playerMovable)
            {
                Debug.Log("overlap");
                SwapTarget = hitOtherObstacle.transform.gameObject;
                swapping = true;
            }
            else
            {
                Debug.Log("cant swap");
                swapping = false;
                DragTarget.transform.position = LastValidPosition; //4. DragTarget stays in place
            }
        }
        else if (!hitPlayGrid && !hitTileRack)//not a spot on the playgrid or tilerack
        {
            swapping = false;
            DragTarget.transform.position = LastValidPosition; //4. DragTarget stays in place
        }
        else//valid placement spot on playgrid
        {
            if (hitPlayGrid)
            {
                DragTarget.transform.position = hitPlayGrid.transform.position; //3. move DragTarget to the checker's spot if it is valid
            }
            else if (hitTileRack)
            {
                DragTarget.transform.position = hitTileRack.transform.position; //3. move DragTarget to the checker's spot if it is valid
            }

            //Debug.Log("Valid Spot");
            swapping = false;
            
            LastValidPosition = DragTarget.transform.position;
        }

        // Restore the tile layer
        this.gameObject.layer = oldLayer; //put tile back to its proper layer
    }

    // Handle mouse up event
    void HandleMouseUp()
    {
        // Handle mouse button up event
        if (canMove && Input.GetMouseButtonUp(0)) //when letting go
        {
            canMove = false;
            dragging = false;

            tileShapeColor.sortingOrder = 1;
            tileMat.sortingOrder = 0;

            tileShapeColor.transform.localScale = Vector3.one;
            tileMat.transform.localScale = Vector3.one;

            // Place the dragged tile at target location
            // Destroy target and targetChecker
            if (DragTarget != null)
            {
                // check swapping
                if (swapping)
                {
                    Debug.Log("swapping: " + SwapTarget.transform.position);
                    DragTarget.transform.position = SwapTarget.transform.position;

                    SwapTarget.transform.position = BeforeMovePosition;
                    Debug.Log("moved: " + SwapTarget.transform.position);

                    SwapTarget.transform.GetComponent<draggable>().setBeforeMovePosition(BeforeMovePosition);
                    SwapTarget.transform.GetComponent<draggable>().setLastValidPosition(BeforeMovePosition);

                    SwapTarget = null;
                    swapping = false;
                }

                this.transform.position = DragTarget.transform.position;
                BeforeMovePosition = transform.position;
                    
                Destroy(DragTarget);
                Destroy(TargetChecker);
            }
        }
    }

    public void setLastValidPosition(Vector3 pos)
    {
        LastValidPosition = pos;
    }

    public void setBeforeMovePosition(Vector3 pos)
    {
        BeforeMovePosition = pos;
    }
}
