using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : MonoBehaviour
{
    public GameObject hitEffect;
    public float lifetime = 1f;
    public int attackDamage = 10;

    void Start() {
        Destroy(gameObject, lifetime);
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            collision.GetComponent<EnemyController>().TakeDamage(attackDamage);
        }
        else if (collision.CompareTag("Ghost"))
        {
            collision.GetComponent<GhostEnemy>().TakeDamage(attackDamage);
        }
        else if (collision.CompareTag("Witch"))
        {
            collision.GetComponent<WitchController>().TakeDamage(attackDamage);
        }
        GameObject effect = Instantiate(hitEffect, transform.position, Quaternion.identity);
        Destroy(effect, 1f);
        Destroy(gameObject);
    }
}
