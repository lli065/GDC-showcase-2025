using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Npc : MonoBehaviour
{
    public Dialogue dialogue;
    private bool playerNearby = false;

    void Update() {
        if (playerNearby && Input.GetKeyDown(KeyCode.Space) && 
        !DialogueManager.isTalking && DialogueManager.canTrigger) {
            TriggerDialogue();
        }
    }

    public void TriggerDialogue() {
        FindObjectOfType<DialogueManager>().StartDialogue(dialogue);
    }

    private void OnCollisionEnter2D(Collision2D collision) {
        if (collision.gameObject.CompareTag("Player")) {
            playerNearby = true;
        }
    }

    private void OnCollisionExit2D(Collision2D collision) {
        if (collision.gameObject.CompareTag("Player")) {
            playerNearby = false;
        }
    }
}
