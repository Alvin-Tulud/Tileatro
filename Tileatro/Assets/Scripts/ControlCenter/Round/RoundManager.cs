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

    private State state;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
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
        }
        else
        {
            state = State.Shop;
        }
    }

    private void ShopState()
    {
        if (true)
        {
            //run state
        }
        else
        {
            state = State.Game;
        }
    }
}
