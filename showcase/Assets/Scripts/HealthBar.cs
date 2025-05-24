using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HealthBar : MonoBehaviour
{

    public Slider slider;
    public TextMeshProUGUI healthText;

    public void UpdateHealthBar(float currentValue, float maxValue)
    {
        slider.value = currentValue / maxValue;
        UpdateText(currentValue, maxValue);
    }

    public void UpdateText(float currentHealth, float maxHealth)
    {
        if (healthText != null)
        {
            healthText.text = Mathf.Max(currentHealth, 0) + "/" + maxHealth;
        }
    }
}
