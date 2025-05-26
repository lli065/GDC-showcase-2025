using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ButtonSelect : MonoBehaviour
{
    public Button[] buttons;
    public GameObject[] arrows;


    void Update()
    {
        GameObject selected = EventSystem.current.currentSelectedGameObject;
        for (int i = 0; i < buttons.Length; i++)
        {
            arrows[i].SetActive(selected == buttons[i].gameObject);
        }
    }
}
