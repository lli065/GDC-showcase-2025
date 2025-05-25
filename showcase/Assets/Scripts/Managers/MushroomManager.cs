using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public enum MushroomType
{
    Heal,
    Poison,
    White,
    Orange
}
public class MushroomManager : MonoBehaviour
{
    public PlayerController player;
    public static MushroomManager Instance;
    private Dictionary<MushroomType, int> mushroomCounts = new Dictionary<MushroomType, int>();
    private MushroomType selected = MushroomType.Heal;
    public TextMeshProUGUI mushroomText;
    public TextMeshProUGUI poisonText;
    public TextMeshProUGUI whiteText;
    public TextMeshProUGUI orangeText;
    public Image healBg;
    public Image poisonBg;
    public Image whiteBg;
    public Image orangeBg;
    public Color selectedColor;
    public Color normalColor;
    public ShieldController shield;

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);

        player = FindObjectOfType<PlayerController>();
        foreach (MushroomType type in System.Enum.GetValues(typeof(MushroomType)))
        {
            mushroomCounts[type] = 0;
        }
        UpdateUI();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
            SelectMushroom(MushroomType.Heal);
        if (Input.GetKeyDown(KeyCode.Alpha2))
            SelectMushroom(MushroomType.Poison);
        if (Input.GetKeyDown(KeyCode.Alpha3))
            SelectMushroom(MushroomType.White);
        if (Input.GetKeyDown(KeyCode.Alpha4))
            SelectMushroom(MushroomType.Orange);

        if (Input.GetKeyDown(KeyCode.F))
        {
            EatMushroom(selected);
        }
    }

    public void AddMushroom(MushroomType type)
    {
        mushroomCounts[type]++;
        UpdateUI();
    }

    public void EatMushroom(MushroomType type)
    {
        if (mushroomCounts[type] == 0)
        {
            return;
        }
        mushroomCounts[type]--;
        switch (type)
        {
            case MushroomType.Heal:
                player.Heal(5);
                break;
            case MushroomType.Poison:
                break;
            case MushroomType.White:
                player.speed = player.speed + 3;
                Invoke("ResetPlayerSpeed", 5f);
                break;
            case MushroomType.Orange:
                shield.ActivateShield();
                break;
        }
        UpdateUI();
    }

    public void ResetPlayerSpeed()
    {
        player.speed = 5f;
    }

    public void RemoveMushrooms(int amt, MushroomType type)
    {
        mushroomCounts[type] = Mathf.Max(mushroomCounts[type] - amt, 0);
        UpdateUI();
    }

    public void SelectMushroom(MushroomType type)
    {
        selected = type;
        UpdateUI();
    }

    public int GetMushroomCount(MushroomType type)
    {
        return mushroomCounts[type];
    }

    private void UpdateUI()
    {
        mushroomText.text = mushroomCounts[MushroomType.Heal] + "";
        poisonText.text = mushroomCounts[MushroomType.Poison] + "";
        whiteText.text = mushroomCounts[MushroomType.White] + "";
        orangeText.text = mushroomCounts[MushroomType.Orange] + "";

        healBg.color = (selected == MushroomType.Heal) ? selectedColor : normalColor;
        poisonBg.color = (selected == MushroomType.Poison) ? selectedColor : normalColor;
        whiteBg.color = (selected == MushroomType.White) ? selectedColor : normalColor;
        orangeBg.color = (selected == MushroomType.Orange) ? selectedColor : normalColor;
    }
}
