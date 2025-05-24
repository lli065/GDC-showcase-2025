using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostEnemy : MonoBehaviour
{
    private Transform player;
    private Rigidbody2D rb;
    [SerializeField] private float damageCooldown = 1f;
    [SerializeField] private float speed = 2f;
    [SerializeField] private int attackDamage = 5;
    private bool canDamage = true;
    public float minDistance = 7f;
    public float maxDistance = 10f;

    public int maxHealth = 30;
    int currentHealth;
    [SerializeField] HealthBar healthBar;

    private DamageFlash damageFlash;
    public GameObject ghostPrefab;
    public GameObject mushroomPrefab;
    public float spawnInterval = 10;

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
            float angle = Random.Range(0, Mathf.PI * 2);
            float distance = Random.Range(minDistance, maxDistance);
            Vector3 offset = new Vector3(Mathf.Cos(angle), Mathf.Sin(angle), 0) * distance;
            Instantiate(ghostPrefab, player.position + offset, Quaternion.identity);
            EnemyManager.Instance.numEnemies++;
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
        Instantiate(mushroomPrefab, transform.position, Quaternion.identity);
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
