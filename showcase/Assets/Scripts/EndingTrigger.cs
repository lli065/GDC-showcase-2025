using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndingTrigger : MonoBehaviour
{
    private bool triggered = false;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!GameManager.currentGameManager.wonBossFight) return;
        if (triggered) return;
        if (other.CompareTag("Player"))
        {
            triggered = true;
            GameManager.currentGameManager.DecreasePlayerHealth();
        }
    }
}
