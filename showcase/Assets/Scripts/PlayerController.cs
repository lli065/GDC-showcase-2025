using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public Rigidbody2D playerRigid;
    public Animator animator;
    public HealthBar healthBar;

    public Vector2 movement;
    public float speed;
    
    public int maxHealth = 100;
    int currentHealth;

    public Transform attackPoint;
    public LayerMask enemyLayers;
    public float attackRate = 2;
    float nextAttackTime = 0;
    public float attackForce = 20f;
    public GameObject attackPrefab;
    
    private void Start()
    {
        currentHealth = maxHealth;
        playerRigid = gameObject.GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        if (DialogueManager.isTalking) {
            playerRigid.velocity = Vector2.zero;
            return;
        }

        if (Time.time >= nextAttackTime) {
            if (Input.GetKeyDown(KeyCode.C)) {
                Attack();
                nextAttackTime = Time.time + 1f / attackRate;
            }
        }
        

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

    void Attack() {
        GameObject attack =  Instantiate(attackPrefab, attackPoint.position, attackPoint.rotation);
        Rigidbody2D rb = attack.GetComponent<Rigidbody2D>();
        rb.AddForce(attackPoint.up * attackForce, ForceMode2D.Impulse);
    }

    public void TakeDamage(int damage) {
        currentHealth -= damage;
        if (currentHealth <= 0) {
            Die();
        }
    }

    public void Die() {

    }

    private void FixedUpdate() {
        playerRigid.MovePosition(playerRigid.position + movement * speed * Time.fixedDeltaTime);
    }

    void OnCollisionEnter2D(Collision2D collision) {
        if (collision.gameObject.name == "Enemy") {
            currentHealth -= 3;
            healthBar.SetHealth(currentHealth);
        }
    }
}
