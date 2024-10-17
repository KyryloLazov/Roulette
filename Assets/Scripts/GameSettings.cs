using UnityEngine;
using TMPro;

public class GameSettings : MonoBehaviour
{
    [SerializeField] private TMP_InputField minBetInputField; // ���� ��� �������� ������
    [SerializeField] private TMP_InputField maxBetInputField; // ���� ��� ����������� ������
    [SerializeField] private TMP_InputField playerCountInputField; // ���� ��� ������� �������
    [SerializeField] private GameObject settingsPanel; // ������ �����������
    [SerializeField] private GameManager gameManager; // ��������� �� �������� ���, ��� �������� ������������
    [SerializeField] private TextMeshProUGUI errorText; // ����� �������

    private float minBet;
    private float maxBet;
    private int playerCount;

    void Start()
    {
        // ³��������� ������ ����������� �� ������� ���
        settingsPanel.SetActive(true);
    }

    // ����� ��� ������������ �����������
    public void AcceptSettings()
    {
        // ������ ������ ��������
        if (!float.TryParse(minBetInputField.text, out minBet) || minBet <= 0)
        {
            errorText.text = "������ �������� �������� ��� �������� ������.";
            return;
        }

        if (!float.TryParse(maxBetInputField.text, out maxBet) || maxBet <= minBet)
        {
            errorText.text = "����������� ������ �� ���� ������ �� ��������.";
            return;
        }

        if (!int.TryParse(playerCountInputField.text, out playerCount) || playerCount <= 0)
        {
            errorText.text = "������ �������� ������� �������.";
            return;
        }

        // �������� ������������ � GameManager ��� ����� ��������� ������
        gameManager.SetGameSettings(minBet, maxBet, playerCount);

        // ��������� ������ ���� ������������
        settingsPanel.SetActive(false);

        Debug.Log($"������������ �������: ̳������� ������: {minBet}, ����������� ������: {maxBet}, ʳ������ �������: {playerCount}");
    }
}
