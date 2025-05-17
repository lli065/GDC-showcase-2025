using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    private GameObject player;
    private Rigidbody2D rb;
    public float speed = 1f;
    public float lookRadius = 10f;

    public int maxHealth = 100;
    int currentHealth;

    private DamageFlash damageFlash;

    // Start is called before the first frame update
    void Start()
    {
        player = FindObjectOfType<PlayerController>().gameObject;
        rb = GetComponent<Rigidbody2D>();

        currentHealth = maxHealth;
        damageFlash = GetComponent<DamageFlash>();
    }

    // Update is called once per frame
    void Update()
    {
        rb.velocity = (player.transform.position - transform.position).normalized * speed;
        transform.localScale = rb.velocity.x > 0 ? new Vector3(1, 1) : new Vector3(-1, 1);
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
