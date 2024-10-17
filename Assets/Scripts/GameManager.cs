using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] TMP_InputField betInputField; // Поле для введення суми ставки
    [SerializeField] private Roulette roulette; // Рулетка
    [SerializeField] private float winMult; // Множник виграшу
    [SerializeField] private UIManager ui; // Контроль інтерфейсу

    private Dictionary<int, List<(Player, float)>> playerBets = new Dictionary<int, List<(Player, float)>>(); // Клітинка, список пар (гравець, ставка)
    private int currentPlayerIndex = 0; // Поточний гравець
    private int totalPlayers = 5; // Загальна кількість гравців
    private List<Player> players = new List<Player>(); // Список гравців
    private int selectedNumber; // Вибрана клітинка для ставки
    private float minBet; // Максимальна ставка
    private float maxBet; // Мінімальна ставка


    public void SetGameSettings(float minBet, float maxBet, int playerCount)
    {
        this.minBet = minBet;
        this.maxBet = maxBet;
        totalPlayers = playerCount;

        InitializePlayers(playerCount); // Ініціалізуємо гравців на основі кількості
    }

    private void InitializePlayers(int playerCount)
    {
        for (int i = 0; i < totalPlayers; i++)
        {
            players.Add(new Player($"Гравець {i + 1}", 1000)); // Початковий баланс — 1000
        }

        // Відобразити баланс першого гравця
        UpdateBalanceUI();
    }

    // Оновлення UI балансу
    void UpdateBalanceUI()
    {
        ui.UpdateBalanceText(players[currentPlayerIndex].Name, players[currentPlayerIndex].Balance);
    }

    public void OnNumberButtonClick(int number)
    {
        selectedNumber = number;
        ui.ShowBetPanel(); // Показуємо вікно введення ставки
    }

    // Обробка натискання кнопки з числом
    public void AcceptBet()
    {
        // Отримуємо суму ставки з InputField
        if (!float.TryParse(betInputField.text, out float betAmount) || betAmount <= 0)
        {
            ui.RaiseError("Введіть правильну суму ставки.");
            return;
        }

        // Перевіряємо, чи ставка в межах дозволеного діапазону
        if (betAmount < minBet)
        {
            ui.RaiseError($"Ставка не може бути меншою за мінімальну ставку: {minBet}$.");
            return;
        }
        if (betAmount > maxBet)
        {
            ui.RaiseError($"Ставка не може бути більшою за максимальну ставку: {maxBet}$.");
            return;
        }

        Player currentPlayer = players[currentPlayerIndex];

        // Перевіряємо, чи достатньо грошей на балансі
        if (currentPlayer.Balance < betAmount)
        {
            ui.RaiseError("У вас недостатньо грошей для цієї ставки.");
            return;
        }

        // Віднімаємо ставку з балансу гравця
        currentPlayer.Balance -= betAmount;

        // Додаємо ставку до словника
        if (!playerBets.ContainsKey(selectedNumber))
        {
            playerBets[selectedNumber] = new List<(Player, float)>();
        }
        playerBets[selectedNumber].Add((currentPlayer, betAmount));

        // Оновлюємо баланс в UI
        UpdateBalanceUI();
        betInputField.text = "";
        ui.HideBetPanel();
    }

    // Обробка завершення ходу
    public void OnEndTurn()
    {
        // Перевірка, чи гравець зробив хоча б одну ставку
        if (playerBets.Count == 0)
        {
            ui.RaiseError("Ви повинні зробити хоча б одну ставку перед завершенням ходу.");
            return;
        }

        // Переходимо до наступного гравця
        currentPlayerIndex++;

        // Якщо це був останній гравець, починається обертання рулетки
        if (currentPlayerIndex >= totalPlayers)
        {
            SpinRoulette();
        }
        else
        {
            // Оновлюємо баланс для наступного гравця
            UpdateBalanceUI();
        }
    }

    // Метод обертання рулетки
    void SpinRoulette()
    {
        roulette.StartSpinning();
        // Після завершення обертання рулетки викликаємо перевірку виграшів
        StartCoroutine(CheckResultsAfterSpin());
    }

    // Метод для перевірки результатів після обертання рулетки
    IEnumerator CheckResultsAfterSpin()
    {
        // Чекаємо завершення обертання рулетки
        yield return new WaitUntil(() => !roulette.isRouletteSpinning());

        // Отримуємо виграшне число
        int winningNumber = roulette.GetWinningNum();

        // Змінні для підрахунку
        float totalCasinoProfit = 0; // Загальний прибуток казино
        Dictionary<Player, float> playerProfits = new Dictionary<Player, float>(); // Підсумковий прибуток або програш кожного гравця

        // Програшні ставки
        foreach (var betList in playerBets.Values)
        {
            foreach (var (player, betAmount) in betList)
            {
                if (!playerProfits.ContainsKey(player))
                {
                    playerProfits[player] = 0; // Ініціалізуємо прибуток для кожного гравця
                }

                // Якщо ставка не на виграшне число, казино забирає ці гроші
                if (!playerBets.ContainsKey(winningNumber) || !playerBets[winningNumber].Contains((player, betAmount)))
                {
                    totalCasinoProfit += betAmount; // Казино отримує ці гроші
                    playerProfits[player] -= betAmount; // Віднімаємо програш гравця
                }
            }
        }

        // Виграші на правильне число
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

        // Формування результатів для показу в UI
        List<(Player, float)> orderedResults = new List<(Player, float)>();

        foreach (var playerProfit in playerProfits)
        {
            var player = playerProfit.Key;
            var profit = playerProfit.Value;

            orderedResults.Add((player, profit));
        }

        // Вивести результати в порядку черговості гравців
        string resultMessage = $"Виграшне число: {winningNumber}\n";

        foreach (var player in players) // Проходимо список гравців у початковому порядку
        {
            var result = orderedResults.Find(x => x.Item1 == player); // Шукаємо відповідні результати для кожного гравця

            if (result.Item2 >= 0)
            {
                resultMessage += $"{result.Item1.Name} отримав прибуток: {result.Item2}$\n";
            }
            else
            {
                resultMessage += $"{result.Item1.Name} програв: {-result.Item2}$\n";
            }
        }

        resultMessage += $"Казино заробило: {totalCasinoProfit}$\n";

        // Викликаємо функцію для відображення результатів у UI
        ui.ShowResults(resultMessage);

        // Підготовка до нового раунду
        ResetGame();
    }

    // Скидання гри для наступного раунду
    void ResetGame()
    {
        currentPlayerIndex = 0;
        playerBets.Clear(); // Скидаємо всі ставки
        UpdateBalanceUI();
    }
}
