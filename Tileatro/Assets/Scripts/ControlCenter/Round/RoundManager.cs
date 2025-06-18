using System.Runtime.CompilerServices;
using UnityEngine;

public class RoundManager : MonoBehaviour
{
    private enum State
    {
        Game,
        Shop
    }

    private int RoundCount;
    private bool doOnce;

    private TileGenerator tg;
    public int tileGenerateAmount = 8;
    private State state;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        tg = FindAnyObjectByType<TileGenerator>();
        doOnce = true;
    }

    // Update is called once per frame
    void Update()
    {
        runState();
    }

    private void runState()
    {
        if (state == State.Game)
        {
            GameState();
        }
        else if (state == State.Shop)
        {
            ShopState();
        }
    }

    private void GameState()
    {
        if (true)
        {
            //run state
            if (doOnce)
            {
                tg.setTiles(tileGenerateAmount);
                doOnce = false;
            }
        }
        else
        {
            doOnce = true;
            state = State.Shop;
        }
    }

    private void ShopState()
    {
        if (true)
        {
            //run state
            if (doOnce)
            {
                doOnce = false;
            }
        }
        else
        {
            doOnce = true;
            state = State.Game;
        }
    }
}
