using UnityEngine;

public class RoundResources : MonoBehaviour
{
    private static RoundResources instanceRef;

    private void Awake()
    {
        if (instanceRef == null)
        {
            instanceRef = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (instanceRef != this)
            Destroy(gameObject);
    }

    private static int tries;

    //-------------------getters and setters------------------------------
    public static void setTries(int t)
    {
        tries = t;
    }

    public static int getTries()
    {
        return tries;
    }

    public static void addTries(int t)
    {
        tries += t;
    }

    public static void subtractTries()
    {
        tries -= 1;
    }
}
