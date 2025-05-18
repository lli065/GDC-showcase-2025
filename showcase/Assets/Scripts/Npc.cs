using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Npc : MonoBehaviour
{
    [SerializeField] private GameObject visualCue;

    public Dialogue dialogue;
    private bool playerNearby;

    private void Awake() {
        playerNearby = false;
        visualCue.SetActive(false);
    }

    void Update() {
        if (playerNearby && Input.GetKeyDown(KeyCode.Space) && 
        !DialogueManager.isTalking && DialogueManager.canTrigger) {
            TriggerDialogue();
        }
        if (playerNearby && !DialogueManager.isTalking) {
            visualCue.SetActive(true);
        } else {
            visualCue.SetActive(false);
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
