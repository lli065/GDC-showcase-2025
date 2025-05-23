using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostEnemy : MonoBehaviour
{
    private Transform player;
    private Rigidbody2D rb;
    [SerializeField] private float damageCooldown = 1f;
    [SerializeField] private float speed = 2f;
    [SerializeField] private int attackDamage = 10;
    private bool canDamage = true;

    public int maxHealth = 30;
    int currentHealth;
    [SerializeField] HealthBar healthBar;

    private DamageFlash damageFlash;
    public GameObject ghostPrefab;
    public float spawnInterval = 5;

    // Start is called before the first frame update
    void Start()
    {
        player = FindObjectOfType<PlayerController>().transform;
        rb = GetComponent<Rigidbody2D>();

        currentHealth = maxHealth;
        damageFlash = GetComponent<DamageFlash>();
        healthBar = GetComponentInChildren<HealthBar>();
        healthBar.UpdateHealthBar(currentHealth, maxHealth);

        InvokeRepeating("SpawnGhosts", spawnInterval, spawnInterval);
    }

    // Update is called once per frame
    void Update()
    {
        rb.velocity = (player.position - transform.position).normalized * speed;
        transform.localScale = rb.velocity.x > 0 ? new Vector3(1, 1) : new Vector3(-1, 1);
    }

    public void ResetDamageCooldown()
    {
        canDamage = true;
    }

    public void SpawnGhosts()
    {
        if (EnemyManager.Instance.CanSpawnEnemy())
        {
            Vector3 randomOffset = new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), 0);
            Instantiate(ghostPrefab, transform.position + randomOffset, Quaternion.identity);
            EnemyManager.Instance.AddEnemy();
        }
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        healthBar.UpdateHealthBar(currentHealth, maxHealth);
        damageFlash.CallDamageFlash();
        canDamage = false;
        Invoke("ResetDamageCooldown", damageCooldown);
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    public void Die()
    {
        EnemyManager.Instance.RemoveEnemy();
        Destroy(gameObject);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && canDamage)
        {
            collision.gameObject.GetComponent<PlayerController>().TakeDamage(attackDamage);
        }
    }
}
