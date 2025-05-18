using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    private Transform target;
    private Rigidbody2D rb;
    [SerializeField] private float attackRate = 1f;
    [SerializeField] private float range = 5f;
    [SerializeField] private GameObject attackPrefab;

    public int maxHealth = 100;
    int currentHealth;

    private DamageFlash damageFlash;
    private bool isAttacking = false;

    // Start is called before the first frame update
    void Start()
    {
        target = FindObjectOfType<PlayerController>().transform;
        rb = GetComponent<Rigidbody2D>();

        currentHealth = maxHealth;
        damageFlash = GetComponent<DamageFlash>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Vector3.Distance(target.position, transform.position) <= range) {
            if (!isAttacking) {
                StartCoroutine(AttackCoroutine());
            }
        }
    }

    IEnumerator AttackCoroutine() {
        isAttacking = true;
        while (Vector3.Distance(target.position, transform.position) <= range) {
            Attack();
            yield return new WaitForSeconds(1f / attackRate);
        }
        isAttacking = false;
    }

    void Attack() {
        Vector2 direction = (target.position - transform.position).normalized;
        GameObject attack = Instantiate(attackPrefab, transform.position, Quaternion.identity);
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        attack.transform.rotation = Quaternion.Euler(0, 0, angle);
        attack.GetComponent<Rigidbody2D>().velocity = direction * 5f;
    }

    public void TakeDamage(int damage) {
        currentHealth -= damage;
        damageFlash.CallDamageFlash();
        if (currentHealth <= 0) {
            Die();
        }
    }

    public void Die() {

    }
}
