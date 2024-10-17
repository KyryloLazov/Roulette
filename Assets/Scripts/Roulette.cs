using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Roulette : MonoBehaviour
{
    [SerializeField] float spinDuration = 5f; // ��������� ���������
    [SerializeField] int additionalSpins = 5; // �������� ���� ������ �������
    private float spinTimeRemaining;
    private bool isSpinning = false;
    private float targetAngle;
    private int winningNumber;
    void Update()
    {
        if (isSpinning)
        {
            if (spinTimeRemaining > 0)
            {
                // ���������� ��� ���������
                spinTimeRemaining -= Time.deltaTime;

                // ���, �� ����� �� ������ ���������� ����
                float t = (spinDuration - spinTimeRemaining) / spinDuration;

                // ������������� ������� ������� EaseOut ��� �������� ��������� ��������
                float easedT = EaseOutCubic(t);

                // ���������� �������� ��� ��������� � ����������� �������� �����������
                float currentAngle = Mathf.Lerp(0, targetAngle, easedT);
                transform.eulerAngles = new Vector3(0, 0, currentAngle);
            }
            else
            {
                // ���� ��� ��������� ���������, ��������� �������
                isSpinning = false;
            }
        }
    }

    // ����� ��������� �������
    public void StartSpinning()
    {
        isSpinning = true;
        spinTimeRemaining = spinDuration;

        // ��������� �������� �����
        winningNumber = Random.Range(0, 10);

        // ��������� ��� ��� ������� ������� � ����������� ���������� ������
        targetAngle = (winningNumber * 36f) + (360f * additionalSpins); // 36 ������� �� ���� ����� + ����� ������
    }

    // EaseOutCubic ������� ��� �������� ��������� ��������
    private float EaseOutCubic(float t)
    {
        return 1 - Mathf.Pow(1 - t, 3);
    }

    public bool isRouletteSpinning()
    {
        return isSpinning;
    }

    public int GetWinningNum()
    {
        return winningNumber;
    }
}
