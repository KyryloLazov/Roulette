using System.Collections.Generic;
using UnityEngine;

public class BetManager : MonoBehaviour
{
    public Dictionary<int, float> totalBets = new Dictionary<int, float>(); // Використовуйте int для виграшних номерів
    public Dictionary<int, Dictionary<int, float>> playerBets = new Dictionary<int, Dictionary<int, float>>(); // playerBets[winningNumber][playerID]

    public void PlaceBet(int playerID, int betNumber, float amount)
    {
        if (!playerBets.ContainsKey(betNumber))
        {
            playerBets[betNumber] = new Dictionary<int, float>();
        }

        if (!playerBets[betNumber].ContainsKey(playerID))
        {
            playerBets[betNumber][playerID] = 0;
        }

        playerBets[betNumber][playerID] += amount;

        // Додаємо до загальних ставок
        if (!totalBets.ContainsKey(betNumber))
        {
            totalBets[betNumber] = 0;
        }
        totalBets[betNumber] += amount;
    }

    public void ResetBets()
    {
        playerBets.Clear();
        totalBets.Clear();
    }
}
