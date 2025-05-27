using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public Rigidbody2D playerRigid;
    public WitchController witch;
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
    public Npc lastWords;
    private bool isDecreasing;

    private DamageFlash damageFlash;

    public AudioClip damageSound;
    public AudioClip healSound;
    
    private void Start()
    {
        currentHealth = maxHealth;
        playerRigid = gameObject.GetComponent<Rigidbody2D>();
        damageFlash = GetComponent<DamageFlash>();
    }

    private void Update()
    {
        if (GameManager.currentGameManager.isPaused) return;
        if (DialogueManager.isTalking)
        {
            playerRigid.velocity = Vector2.zero;
            animator.SetFloat("Horizontal", 0);
            animator.SetFloat("Vertical", 0);
            animator.SetFloat("Speed", 0);
            return;
        }

        if (Time.time >= nextAttackTime) {
            if (Input.GetKeyDown(KeyCode.Space)) {
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
        healthBar.UpdateHealthBar(currentHealth, maxHealth);
        damageFlash.CallDamageFlash();
        SoundManager.instance.PlaySound(damageSound, transform, 1f);

        if (DialogueManager.isTalking && !GameManager.currentGameManager.wonBossFight)
        {
            FindObjectOfType<DialogueManager>().EndDialogue();
        }

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    public void Heal(int amt)
    {
        if (currentHealth == maxHealth) return;
        currentHealth += amt;
        currentHealth = Mathf.Min(maxHealth, currentHealth);
        healthBar.UpdateHealthBar(currentHealth, maxHealth);
        SoundManager.instance.PlaySound(healSound, transform, 1f);
    }

    public void ResetHealth()
    {
        currentHealth = maxHealth;
        healthBar.UpdateHealthBar(currentHealth, maxHealth);
    }

    public void Die()
    {
        if (GameManager.currentGameManager.wonBossFight) return;
        if (GameManager.currentGameManager.inBossFight)
        {
            EnemyManager.Instance.RemoveEnemiesInBossFight();
            MushroomManager.Instance.ResetMushrooms();
            witch.ResetStats();
            ResetHealth();
            GameManager.currentGameManager.RemoveAllGhosts();
            EnemyManager.Instance.RemoveEnemiesInBossFight();
            GameManager.currentGameManager.inBossFight = false;
            GameManager.currentGameManager.EndBossFight();
        }
        else
        {
            GameManager.currentGameManager.ShowGameOverScreen();
        }
    }

    public void StartDecreasingHealth()
    {
        if (!isDecreasing)
        {
            StartCoroutine(DecreaseHealth());
        }
    }

    public IEnumerator DecreaseHealth()
    {
        isDecreasing = true;
        while (currentHealth > 0)
        {
            TakeDamage(5);
            if (currentHealth == 80)
            {
                lastWords.TriggerDialogue();
            }
            if (currentHealth == 10)
            {
                FindObjectOfType<DialogueManager>().EndDialogue();
            }
            yield return new WaitForSeconds(0.5f);
        }
        StartCoroutine(GameManager.currentGameManager.StartEndingScene());
    }

    private void FixedUpdate()
    {
        playerRigid.MovePosition(playerRigid.position + movement * speed * Time.fixedDeltaTime);
    }

    void OnCollisionEnter2D(Collision2D collision) {
        if (collision.gameObject.CompareTag("Enemy") || collision.gameObject.CompareTag("Witch")) {
            TakeDamage(3);
        }
    }
}