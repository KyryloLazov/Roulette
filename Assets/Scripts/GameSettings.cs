using UnityEngine;
using TMPro;

public class GameSettings : MonoBehaviour
{
    [SerializeField] private TMP_InputField minBetInputField; // Поле для мінімальної ставки
    [SerializeField] private TMP_InputField maxBetInputField; // Поле для максимальної ставки
    [SerializeField] private TMP_InputField playerCountInputField; // Поле для кількості гравців
    [SerializeField] private GameObject settingsPanel; // Панель налаштувань
    [SerializeField] private GameManager gameManager; // Посилання на менеджер гри, щоб передати налаштування
    [SerializeField] private TextMeshProUGUI errorText; // Текст помилки

    private float minBet;
    private float maxBet;
    private int playerCount;

    void Start()
    {
        // Відображаємо панель налаштувань на початку гри
        settingsPanel.SetActive(true);
    }

    // Метод для підтвердження налаштувань
    public void AcceptSettings()
    {
        // Читаємо введені значення
        if (!float.TryParse(minBetInputField.text, out minBet) || minBet <= 0)
        {
            errorText.text = "Введіть коректне значення для мінімальної ставки.";
            return;
        }

        if (!float.TryParse(maxBetInputField.text, out maxBet) || maxBet <= minBet)
        {
            errorText.text = "Максимальна ставка має бути більшою за мінімальну.";
            return;
        }

        if (!int.TryParse(playerCountInputField.text, out playerCount) || playerCount <= 0)
        {
            errorText.text = "Введіть коректну кількість гравців.";
            return;
        }

        // Передаємо налаштування в GameManager або інший відповідний скрипт
        gameManager.SetGameSettings(minBet, maxBet, playerCount);

        // Приховуємо панель після налаштування
        settingsPanel.SetActive(false);

        Debug.Log($"Налаштування прийняті: Мінімальна ставка: {minBet}, Максимальна ставка: {maxBet}, Кількість гравців: {playerCount}");
    }
}
