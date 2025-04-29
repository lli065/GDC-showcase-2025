using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(Animator))]
public class AnimationController : MonoBehaviour
{
    private PlayerController playerController;
    private Animator animator;
    void Start()
    {
        playerController = GetComponent<PlayerController>();
        animator = GetComponent<Animator>();
        transform.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeRotation;

    }

    void Update()
    {
        if (playerController.movement.x != 0) {
            animator.SetInteger("Direction", 1);
        }
        else if (playerController.movement.y > 0) {
            animator.SetInteger("Direction", 2);
        }
        else if (playerController.movement.y < 0) {
            animator.SetInteger("Direction", 3);
        }
        else {
            animator.SetInteger("Direction", 0);
        }

        if (playerController.movement.x > 0) {
            playerController.transform.localScale = new Vector3(1, 1, 1);
        }
        else if (playerController.movement.x < 0) {
            playerController.transform.localScale = new Vector3(-1, 1, 1);
        }
    }
}
