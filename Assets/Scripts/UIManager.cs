using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField] private GameObject resultsPanel;  // Панель з результатами
    [SerializeField] private TextMeshProUGUI resultsText;  // Текст для результатів
    [SerializeField] private TextMeshProUGUI balanceText; // Текст балансу гравця
    [SerializeField] private GameObject betPanel; // Панель для введення ставки
    [SerializeField] private TextMeshProUGUI errorText;

    void Start()
    {
        // Ховаємо панель результатів на початку
        resultsPanel.SetActive(false);
    }

    public void UpdateBalanceText(string playerName, float balance)
    {
        balanceText.text = $"Баланс {playerName}: {balance}$";
    }

    // Функція для оновлення тексту та показу результатів
    public void ShowResults(string results)
    {
        resultsPanel.SetActive(true);  // Показуємо панель результатів
        resultsText.text = results;    // Оновлюємо текст результатами гри
    }

    // Приховуємо панель
    public void HideResultsPanel()
    {
        resultsPanel.SetActive(false); // Ховаємо панель
    }

    public void ShowBetPanel()
    {
        betPanel.SetActive(true);
    }

    public void HideBetPanel()
    {
        betPanel.SetActive(false);
    }

    public void RaiseError(string error)
    {
        errorText.text = error;
    }
}