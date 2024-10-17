using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    private List<Player> players = new List<Player>(); // Список гравців
    private int currentPlayerIndex = 0; // Поточний гравець

    public void InitializePlayers(int totalPlayers)
    {
        for (int i = 0; i < totalPlayers; i++)
        {
            players.Add(new Player($"Гравець {i + 1}", 1000)); // Початковий баланс — 1000
        }
    }

    public Player GetCurrentPlayer()
    {
        return players[currentPlayerIndex];
    }

    public void NextPlayer()
    {
        currentPlayerIndex++;
    }

    public bool IsLastPlayer(int totalPlayers)
    {
        return currentPlayerIndex >= totalPlayers;
    }

    public void ResetCurrentPlayerIndex()
    {
        currentPlayerIndex = 0;
    }

    public List<Player> GetPlayers()
    {
        return players;
    }

    public void UpdatePlayerBalance(string playerName, float amount)
{
    Player player = players.Find(p => p.Name == playerName);
    if (player != null)
    {
        player.Balance += amount;
        Debug.Log($"{player.Name} тепер має баланс: {player.Balance}");
    }
}

}
