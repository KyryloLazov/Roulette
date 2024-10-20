using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] TMP_InputField betInputField; // ���� ��� �������� ���� ������
    [SerializeField] private Roulette roulette; // �������
    [SerializeField] private float winMult; // ������� �������
    [SerializeField] private UIManager ui; // �������� ����������

    private Dictionary<int, List<(Player, float)>> playerBets = new Dictionary<int, List<(Player, float)>>(); // �������, ������ ��� (�������, ������)
    private int currentPlayerIndex = 0; // �������� �������
    private int totalPlayers = 5; // �������� ������� �������
    private List<Player> players = new List<Player>(); // ������ �������
    private int selectedNumber; // ������� ������� ��� ������
    private float minBet; // ����������� ������
    private float maxBet; // ̳�������� ������


    public void SetGameSettings(float minBet, float maxBet, int playerCount)
    {
        this.minBet = minBet;
        this.maxBet = maxBet;
        totalPlayers = playerCount;

        InitializePlayers(playerCount); // ����������� ������� �� ����� �������
    }

    private void InitializePlayers(int playerCount)
    {
        for (int i = 0; i < totalPlayers; i++)
        {
            players.Add(new Player($"������� {i + 1}", 1000)); // ���������� ������ � 1000
        }

        // ³��������� ������ ������� ������
        UpdateBalanceUI();
    }

    // ��������� UI �������
    void UpdateBalanceUI()
    {
        ui.UpdateBalanceText(players[currentPlayerIndex].Name, players[currentPlayerIndex].Balance);
    }

    public void OnNumberButtonClick(int number)
    {
        selectedNumber = number;
        ui.ShowBetPanel(); // �������� ���� �������� ������
    }

    // ������� ���������� ������ � ������
    public void AcceptBet()
    {
        // �������� ���� ������ � InputField
        if (!float.TryParse(betInputField.text, out float betAmount) || betAmount <= 0)
        {
            ui.RaiseError("������ ��������� ���� ������.");
            return;
        }

        // ����������, �� ������ � ����� ����������� ��������
        if (betAmount < minBet)
        {
            ui.RaiseError($"������ �� ���� ���� ������ �� ��������� ������: {minBet}$.");
            return;
        }
        if (betAmount > maxBet)
        {
            ui.RaiseError($"������ �� ���� ���� ������ �� ����������� ������: {maxBet}$.");
            return;
        }

        Player currentPlayer = players[currentPlayerIndex];

        // ����������, �� ��������� ������ �� ������
        if (currentPlayer.Balance < betAmount)
        {
            ui.RaiseError("� ��� ����������� ������ ��� ���� ������.");
            return;
        }

        // ³������� ������ � ������� ������
        currentPlayer.Balance -= betAmount;

        // ������ ������ �� ��������
        if (!playerBets.ContainsKey(selectedNumber))
        {
            playerBets[selectedNumber] = new List<(Player, float)>();
        }
        playerBets[selectedNumber].Add((currentPlayer, betAmount));

        // ��������� ������ � UI
        UpdateBalanceUI();
        betInputField.text = "";
        ui.HideBetPanel();
    }

    // ������� ���������� ����
    public void OnEndTurn()
    {
        // ��������, �� ������� ������ ���� � ���� ������
        if (playerBets.Count == 0)
        {
            ui.RaiseError("�� ������� ������� ���� � ���� ������ ����� ����������� ����.");
            return;
        }

        // ���������� �� ���������� ������
        currentPlayerIndex++;

        // ���� �� ��� �������� �������, ���������� ��������� �������
        if (currentPlayerIndex >= totalPlayers)
        {
            SpinRoulette();
        }
        else
        {
            // ��������� ������ ��� ���������� ������
            UpdateBalanceUI();
        }
    }

    // ����� ��������� �������
    void SpinRoulette()
    {
        roulette.StartSpinning();
        // ϳ��� ���������� ��������� ������� ��������� �������� ��������
        StartCoroutine(CheckResultsAfterSpin());
    }

    // ����� ��� �������� ���������� ���� ��������� �������
    IEnumerator CheckResultsAfterSpin()
    {
        // ������ ���������� ��������� �������
        yield return new WaitUntil(() => !roulette.isRouletteSpinning());

        // �������� �������� �����
        int winningNumber = roulette.GetWinningNum();

        // ����� ��� ���������
        float totalCasinoProfit = 0; // ��������� �������� ������
        Dictionary<Player, float> playerProfits = new Dictionary<Player, float>(); // ϳ��������� �������� ��� ������� ������� ������

        // ��������� ������
        foreach (var betList in playerBets.Values)
        {
            foreach (var (player, betAmount) in betList)
            {
                if (!playerProfits.ContainsKey(player))
                {
                    playerProfits[player] = 0; // ����������� �������� ��� ������� ������
                }

                // ���� ������ �� �� �������� �����, ������ ������ �� �����
                if (!playerBets.ContainsKey(winningNumber) || !playerBets[winningNumber].Contains((player, betAmount)))
                {
                    totalCasinoProfit += betAmount; // ������ ������ �� �����
                    playerProfits[player] -= betAmount; // ³������� ������� ������
                }
            }
        }

        // ������� �� ��������� �����
        if (playerBets.ContainsKey(winningNumber))
        {
            float totalBetOnNumber = 0;
            foreach (var bet in playerBets[winningNumber])
            {
                totalBetOnNumber += bet.Item2;
            }

            foreach (var (player, betAmount) in playerBets[winningNumber])
            {
                float winMultiplier = (betAmount / totalBetOnNumber);
                float winAmount = winMultiplier * totalBetOnNumber * winMult;

                player.Balance += winAmount;
                playerProfits[player] += winAmount;
            }
        }

        // ���������� ���������� ��� ������ � UI
        List<(Player, float)> orderedResults = new List<(Player, float)>();

        foreach (var playerProfit in playerProfits)
        {
            var player = playerProfit.Key;
            var profit = playerProfit.Value;

            orderedResults.Add((player, profit));
        }

        // ������� ���������� � ������� ��������� �������
        string resultMessage = $"�������� �����: {winningNumber}\n";

        foreach (var player in players) // ��������� ������ ������� � ����������� �������
        {
            var result = orderedResults.Find(x => x.Item1 == player); // ������ �������� ���������� ��� ������� ������

            if (result.Item2 >= 0)
            {
                resultMessage += $"{result.Item1.Name} ������� ��������: {result.Item2}$\n";
            }
            else
            {
                resultMessage += $"{result.Item1.Name} �������: {-result.Item2}$\n";
            }
        }

        resultMessage += $"������ ��������: {totalCasinoProfit}$\n";

        // ��������� ������� ��� ����������� ���������� � UI
        ui.ShowResults(resultMessage);

        // ϳ�������� �� ������ ������
        ResetGame();
    }

    // �������� ��� ��� ���������� ������
    void ResetGame()
    {
        currentPlayerIndex = 0;
        playerBets.Clear(); // ������� �� ������
        UpdateBalanceUI();
    }
}
