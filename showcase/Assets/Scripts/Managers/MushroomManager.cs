using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MushroomManager : MonoBehaviour
{
    public static MushroomManager Instance;

    public int numMushrooms = 0;
    public TextMeshProUGUI mushroomText;

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }
    public void AddMushroom()
    {
        numMushrooms++;
        UpdateUI();
    }

    public void EatMushroom(int amount)
    {
        numMushrooms--;
        FindObjectOfType<PlayerController>().Heal(amount);
        UpdateUI();
    }

    private void UpdateUI()
    {
        mushroomText.text = numMushrooms + "";
    }
}
