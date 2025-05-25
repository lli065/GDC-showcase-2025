using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    private Transform target;
    private Rigidbody2D rb;
    [SerializeField] private float attackRate = 3f;
    [SerializeField] private float range = 5f;
    [SerializeField] private float attackForce = 6f;
    [SerializeField] private GameObject attackPrefab;
    [SerializeField] private int droppedMushrooms = 3;

    public int maxHealth = 30;
    int currentHealth;
    [SerializeField] HealthBar healthBar;

    private DamageFlash damageFlash;
    private bool isAttacking = false;
    public GameObject ghostPrefab;
    public GameObject mushroomPrefab;

    // Start is called before the first frame update
    void Start()
    {
        target = FindObjectOfType<PlayerController>().transform;
        rb = GetComponent<Rigidbody2D>();

        currentHealth = maxHealth;
        damageFlash = GetComponent<DamageFlash>();
        healthBar = GetComponentInChildren<HealthBar>();
        healthBar.UpdateHealthBar(currentHealth, maxHealth);
    }

    // Update is called once per frame
    void Update()
    {
        if (Vector3.Distance(target.position, transform.position) <= range)
        {
            if (!isAttacking)
            {
                StartCoroutine(AttackCoroutine());
            }
        }
    }

    IEnumerator AttackCoroutine()
    {
        isAttacking = true;
        while (Vector3.Distance(target.position, transform.position) <= range)
        {
            Attack();
            yield return new WaitForSeconds(1f / attackRate);
        }
        isAttacking = false;
    }

    void Attack()
    {
        Vector2 direction = (target.position - transform.position).normalized;
        GameObject attack = Instantiate(attackPrefab, transform.position, Quaternion.identity);
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        attack.transform.rotation = Quaternion.Euler(0, 0, angle);
        attack.GetComponent<Rigidbody2D>().velocity = direction * attackForce;
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
        if (!GameManager.currentGameManager.inBossFight)
        {
            DropMushrooms();
        }
        Invoke("SpawnGhost", 10);
        gameObject.SetActive(false);
    }

    public void SpawnGhost()
    {
        if (EnemyManager.Instance.CanSpawnEnemy())
        {
            float angle = Random.Range(0, Mathf.PI * 2);
            float distance = 6;
            Vector3 offset = new Vector3(Mathf.Cos(angle), Mathf.Sin(angle), 0) * distance;
            Instantiate(ghostPrefab, target.position + offset, Quaternion.identity);
            EnemyManager.Instance.numEnemies++;
        }
        Destroy(gameObject);
    }

    public void DropMushrooms()
    {
        for (int i = 0; i < droppedMushrooms; i++)
        {
            Vector3 randomOffset = new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), 0);
            Instantiate(mushroomPrefab, transform.position + randomOffset, Quaternion.identity);
        }
    }
}
