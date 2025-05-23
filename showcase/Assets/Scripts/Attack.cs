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

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            collision.gameObject.GetComponent<EnemyController>().TakeDamage(attackDamage);
        }
        else if (collision.gameObject.CompareTag("Ghost"))
        {
            collision.gameObject.GetComponent<GhostEnemy>().TakeDamage(attackDamage);
        }
        GameObject effect = Instantiate(hitEffect, transform.position, Quaternion.identity);
        Destroy(effect, 1f);
        Destroy(gameObject);
    }
}
