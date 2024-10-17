//using System.Collections;
//using System.Collections.Generic;
//using TMPro;
//using UnityEngine;

//public class ResultsWindow : MonoBehaviour
//{
//    [SerializeField] private GameObject resultsPanel; // ������ � ������������, ��� ������ ������� ����
//    [SerializeField] private TMP_Text resultsText; // �������� ���� ��� ����������� ����������

//    // ����� ��� �������� ���� � ������������
//    public void ShowResults(List<Player> players, Dictionary<int, float> playerResults)
//    {
//        resultsPanel.SetActive(true); // ³������ ���� � ������������
//        resultsText.text = ""; // ������� �����

//        foreach (Player player in players)
//        {
//            float winLossAmount = 0;

//            // ���� ������� ������, ���������� ������
//            if (playerResults.ContainsKey(player.PlayerID)) // ��� ����� ��������������� player.Name �� ����
//            {
//                winLossAmount = playerResults[player.PlayerID]; // ��� ������� ������ ��� �������
//            }

//            string result = winLossAmount >= 0 ? "+" + winLossAmount.ToString() : winLossAmount.ToString();
//            resultsText.text += $"{player.Name}: {result}\n";
//        }
//    }

//    // ����� ��� �������� ���� � ������������
//    public void HideResults()
//    {
//        resultsPanel.SetActive(false);
//    }
//}
