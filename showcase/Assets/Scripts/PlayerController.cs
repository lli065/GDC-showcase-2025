using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public Rigidbody2D playerRigid;
    public Animator animator;

    public Vector2 movement;
    public float speed;
    public float health;
    
    private void Start()
    {
        playerRigid = gameObject.GetComponent<Rigidbody2D>();
    }
    private void Update()
    {
        movement = new Vector2();
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");
        movement.Normalize();

        animator.SetFloat("Horizontal", movement.x);
        animator.SetFloat("Vertical", movement.y);
        animator.SetFloat("Speed", movement.sqrMagnitude);

        if (movement.x == 1 || movement.x == -1 || movement.y == 1 || movement.y == -1) {
            animator.SetFloat("LastHorizontal", movement.x);
            animator.SetFloat("LastVertical", movement.y);
        }
    }

    private void FixedUpdate() {
        playerRigid.MovePosition(playerRigid.position + movement * speed * Time.fixedDeltaTime);
    }
}
