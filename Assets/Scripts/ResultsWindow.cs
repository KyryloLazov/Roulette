//using System.Collections;
//using System.Collections.Generic;
//using TMPro;
//using UnityEngine;

//public class ResultsWindow : MonoBehaviour
//{
//    [SerializeField] private GameObject resultsPanel; // Панель з результатами, яка містить текстові поля
//    [SerializeField] private TMP_Text resultsText; // Текстове поле для відображення результатів

//    // Метод для відкриття вікна з результатами
//    public void ShowResults(List<Player> players, Dictionary<int, float> playerResults)
//    {
//        resultsPanel.SetActive(true); // Відкрити вікно з результатами
//        resultsText.text = ""; // Очищуємо текст

//        foreach (Player player in players)
//        {
//            float winLossAmount = 0;

//            // Якщо гравець виграв, відображаємо виграш
//            if (playerResults.ContainsKey(player.PlayerID)) // або можна використовувати player.Name як ключ
//            {
//                winLossAmount = playerResults[player.PlayerID]; // або гравець виграв або програв
//            }

//            string result = winLossAmount >= 0 ? "+" + winLossAmount.ToString() : winLossAmount.ToString();
//            resultsText.text += $"{player.Name}: {result}\n";
//        }
//    }

//    // Метод для закриття вікна з результатами
//    public void HideResults()
//    {
//        resultsPanel.SetActive(false);
//    }
//}
