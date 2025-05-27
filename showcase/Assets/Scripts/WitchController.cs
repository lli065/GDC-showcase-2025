using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WitchController : MonoBehaviour
{
    private Transform player;
    private Rigidbody2D rb;
    private Vector3 originalPosition;
    [SerializeField] private float attackCooldown = 3f;
    [SerializeField] private float range = 5f;
    [SerializeField] private float teleportRange = 10f;
    [SerializeField] private float teleportCooldown = 8f;
    [SerializeField] private float teleportDelay = 1f;
    private float nextTeleportTime;

    [SerializeField] private float spawnCooldown = 20f;
    private float nextSpawnTime;
    private int enemiesPerSpawn = 1;
    [SerializeField] private float attackForce = 6f;
    public GameObject attackPrefab;
    public GameObject enemyPrefab;
    public GameObject teleportPrefab;
    private bool isTeleporting = false;

    [SerializeField] Vector2 areaMin;
    [SerializeField] Vector2 areaMax;


    public int maxHealth = 30;
    int currentHealth;
    [SerializeField] HealthBar healthBar;

    private DamageFlash damageFlash;
    private bool isAttacking = false;
    private bool fightStarted = false;
    public GameObject deathTrigger;

    public Animator animator;
    public AudioClip attackSound;
    public AudioClip damageSound;
    public AudioClip teleportSound;


    void Start()
    {
        player = FindObjectOfType<PlayerController>().transform;
        rb = GetComponent<Rigidbody2D>();
        originalPosition = transform.position;
        animator = GetComponent<Animator>();

        currentHealth = maxHealth;
        damageFlash = GetComponent<DamageFlash>();
        healthBar = GetComponentInChildren<HealthBar>();
        healthBar.UpdateHealthBar(currentHealth, maxHealth);
    }

    void Update()
    {
        if (GameManager.currentGameManager == null) return;
        if (!GameManager.currentGameManager.inBossFight) return;
        if (!fightStarted)
        {
            nextSpawnTime = spawnCooldown + Time.time;
            fightStarted = true;
            nextTeleportTime = teleportCooldown + Time.time;
            if (GameManager.currentGameManager.hasPoisonMushrooms)
            {
                teleportCooldown = 10f;
            }
        }
        if (currentHealth < maxHealth / 3)
        {
            spawnCooldown = 12f;
        }
        else if (currentHealth < maxHealth * 2 / 3)
        {
            spawnCooldown = 15f;
        }
        if (Vector3.Distance(player.position, transform.position) <= range)
        {
            if (!isAttacking && !isTeleporting)
            {
                StartCoroutine(AttackCoroutine());
            }
        }

        if (Time.time >= nextSpawnTime)
        {
            StartCoroutine(SpawnEnemies());
            nextSpawnTime = Time.time + spawnCooldown;
        }

        if (Time.time >= nextTeleportTime)
        {
            Teleport();
            nextTeleportTime = Time.time + teleportCooldown;
        }
    }

    IEnumerator AttackCoroutine()
    {
        isAttacking = true;
        while (Vector3.Distance(player.position, transform.position) <= range)
        {
            Attack();
            SoundManager.instance.PlaySound(attackSound, transform, 1f);
            yield return new WaitForSeconds(attackCooldown);
        }
        isAttacking = false;
    }

    void Attack()
    {
        animator.SetTrigger("Attack");

        Vector2 direction = (player.position - transform.position).normalized;
        GameObject attack = Instantiate(attackPrefab, transform.position, Quaternion.identity);
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        attack.transform.rotation = Quaternion.Euler(0, 0, angle);
        attack.GetComponent<Rigidbody2D>().velocity = direction * attackForce;
    }

    void Teleport()
    {
        isTeleporting = true;
        StartCoroutine(TeleportRoutine());
    }

    IEnumerator TeleportRoutine()
    {
        SoundManager.instance.PlaySound(teleportSound, transform, 1f);

        GameObject effect1 = Instantiate(teleportPrefab, transform.position, Quaternion.identity);
        Destroy(effect1, 3f);
        yield return new WaitForSeconds(0.4f);

        GetComponent<SpriteRenderer>().enabled = false;
        GetComponent<Collider2D>().enabled = false;
        healthBar.gameObject.SetActive(false);
        yield return new WaitForSeconds(teleportDelay);

        float angle = Random.Range(0, Mathf.PI * 2);
        float distance = Random.Range(5f, 7f);
        Vector3 teleportPosition = player.position + new Vector3(Mathf.Cos(angle), Mathf.Sin(angle), 0) * distance;
        transform.position = new Vector3(Mathf.Clamp(teleportPosition.x, areaMin.x, areaMax.x), Mathf.Clamp(teleportPosition.y, areaMin.y, areaMax.y), 0);


        GameObject effect2 = Instantiate(teleportPrefab, transform.position, Quaternion.identity);
        Destroy(effect2, 3f);
        yield return new WaitForSeconds(0.4f);
        GetComponent<SpriteRenderer>().enabled = true;
        GetComponent<Collider2D>().enabled = true;
        healthBar.gameObject.SetActive(true);
        isTeleporting = false;
    }

    IEnumerator SpawnEnemies()
    {
        animator.SetTrigger("Attack");
        for (int i = 0; i < enemiesPerSpawn; i++)
        {
            Vector3 direction = (player.position - transform.position).normalized;
            Vector3 spawnPosition = transform.position + direction * Random.Range(3.5f, 5.5f);
            GameObject effect = Instantiate(teleportPrefab, spawnPosition, Quaternion.identity);
            Destroy(effect, 3f);
            yield return new WaitForSeconds(0.4f);
            Instantiate(enemyPrefab, spawnPosition, Quaternion.identity);
        }
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        healthBar.UpdateHealthBar(currentHealth, maxHealth);
        damageFlash.CallDamageFlash();
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    public void Die()
    {
        GameManager.currentGameManager.EndBossFight();
        GameManager.currentGameManager.wonBossFight = true;
        deathTrigger.SetActive(true);
        Destroy(gameObject);
    }

    public void ResetStats()
    {
        currentHealth = maxHealth;
        healthBar.UpdateHealthBar(currentHealth, maxHealth);
        fightStarted = false;
        teleportCooldown = 8f;
        spawnCooldown = 20f;
        transform.position = originalPosition;
    }
}
