using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField] private GameObject resultsPanel;  // ������ � ������������
    [SerializeField] private TextMeshProUGUI resultsText;  // ����� ��� ����������
    [SerializeField] private TextMeshProUGUI balanceText;
    [SerializeField] private GameObject betPanel; // ������ ��� �������� ������

    void Start()
    {
        // ������ ������ ���������� �� �������
        resultsPanel.SetActive(false);
    }

    public void UpdateBalanceText(string playerName, float balance)
    {
        balanceText.text = $"������ {playerName}: {balance}$";
    }

    // ������� ��� ��������� ������ �� ������ ����������
    public void ShowResults(string results)
    {
        resultsPanel.SetActive(true);  // �������� ������ ����������
        resultsText.text = results;    // ��������� ����� ������������ ���
    }

    // ��������� ������
    public void HideResultsPanel()
    {
        resultsPanel.SetActive(false); // ������ ������
    }

    public void ShowBetPanel()
    {
        betPanel.SetActive(true);
    }

    public void HideBetPanel()
    {
        betPanel.SetActive(false);
    }
}