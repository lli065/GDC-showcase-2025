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
    public float attackForce = 6f;
    public GameObject attackPrefab;

    private DamageFlash damageFlash;
    public int healAmount = 5;
    
    private void Start()
    {
        currentHealth = maxHealth;
        playerRigid = gameObject.GetComponent<Rigidbody2D>();
        damageFlash = GetComponent<DamageFlash>();
    }

    private void Update()
    {
        if (DialogueManager.isTalking) {
            playerRigid.velocity = Vector2.zero;
            return;
        }

        if (Time.time >= nextAttackTime) {
            if (Input.GetKeyDown(KeyCode.Space)) {
                Attack();
                nextAttackTime = Time.time + 1f / attackRate;
            }
        }

        if (Input.GetKeyDown(KeyCode.T))
        {
            MushroomManager.Instance.EatMushroom(healAmount);
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
        Vector2 direction = new Vector2(animator.GetFloat("LastHorizontal"), animator.GetFloat("LastVertical")).normalized;
        if (direction == Vector2.zero) {
            direction = Vector2.up;
        }
        GameObject attack =  Instantiate(attackPrefab, transform.position, Quaternion.identity);
        Rigidbody2D rb = attack.GetComponent<Rigidbody2D>();
        rb.velocity = direction * attackForce;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90;
        attack.transform.rotation = Quaternion.Euler(0, 0, angle);
    }

    public void TakeDamage(int damage) {
        currentHealth -= damage;
        healthBar.SetHealth(currentHealth);
        damageFlash.CallDamageFlash();
        if (currentHealth <= 0) {
            Die();
        }
    }

    public void Heal(int amt)
    {
        currentHealth += amt;
        currentHealth = Mathf.Min(maxHealth, currentHealth);
        healthBar.SetHealth(currentHealth);
    }

    public void Die()
    {
        Debug.Log("Dead");
    }

    private void FixedUpdate() {
        playerRigid.MovePosition(playerRigid.position + movement * speed * Time.fixedDeltaTime);
    }

    void OnCollisionEnter2D(Collision2D collision) {
        if (collision.gameObject.CompareTag("Enemy")) {
            TakeDamage(3);
        }
    }
}