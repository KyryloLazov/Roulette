using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Roulette : MonoBehaviour
{
    [SerializeField] float spinDuration = 5f; // Тривалість обертання
    [SerializeField] int additionalSpins = 5; // Додаткові повні оберти рулетки
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
                // Залишковий час обертання
                spinTimeRemaining -= Time.deltaTime;

                // Час, що минув як частка загального часу
                float t = (spinDuration - spinTimeRemaining) / spinDuration;

                // Використовуємо нелінійну функцію EaseOut для плавного зменшення швидкості
                float easedT = EaseOutCubic(t);

                // Обчислюємо поточний кут обертання з урахуванням плавного уповільнення
                float currentAngle = Mathf.Lerp(0, targetAngle, easedT);
                transform.eulerAngles = new Vector3(0, 0, currentAngle);
            }
            else
            {
                // Коли час обертання закінчився, зупиняємо рулетку
                isSpinning = false;
            }
        }
    }

    // Старт обертання рулетки
    public void StartSpinning()
    {
        isSpinning = true;
        spinTimeRemaining = spinDuration;

        // Визначаємо виграшне число
        winningNumber = Random.Range(0, 10);

        // Визначаємо кут для зупинки рулетки з урахуванням додаткових обертів
        targetAngle = (winningNumber * 36f) + (360f * additionalSpins); // 36 градусів на одне число + кілька обертів
    }

    // EaseOutCubic функція для плавного зменшення швидкості
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
