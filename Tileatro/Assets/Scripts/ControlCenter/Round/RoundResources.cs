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
    private static int rerolls;

    //-------------------getters and setters------------------------------
    public static void setTries(int t)
    {
        tries = t;
    }

    public static void setRerolls(int r)
    {
        rerolls = r;
    }

    public static int getTries()
    {
        return tries;
    }

    public static int getRerolls()
    {
        return rerolls;
    }

    public static void addTries(int t)
    {
        tries += t;
    }

    public static void addRerolls(int r)
    {
        rerolls += r;
    }

    public static void subtractTries()
    {
        tries -= 1;
    }

    public static void subtractRerolls()
    {
        rerolls -= 1;
    }
}
