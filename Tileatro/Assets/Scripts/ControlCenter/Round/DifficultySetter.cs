using UnityEngine;

public class DifficultySetter : MonoBehaviour
{
    public int tries_Normal = 4;
    public int rerolls_Normal = 2;

    public int tries_Hard = 2;
    public int rerolls_Hard = 2;

    public void setDifficulty(int difficulty)
    {
        if (difficulty == 0)
        {
            RoundResources.setTries(tries_Normal);
            RoundResources.setRerolls(rerolls_Normal);
        }
        else
        {
            RoundResources.setTries(tries_Hard);
            RoundResources.setRerolls(rerolls_Hard);
        }
    }
}
