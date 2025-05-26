using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    public GameObject hitEffect;
    public float lifetime = 1f;
    public int attackDamage = 10;
    private bool hasHit = false;
    public bool witch = false;

    void Start()
    {
        Destroy(gameObject, lifetime);
        if (witch && GameManager.currentGameManager.inBossFight && GameManager.currentGameManager.hasPoisonMushrooms)
        {
            attackDamage = 5;
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (hasHit) return;
        if (collision.CompareTag("Player"))
        {
            hasHit = true;
            if (collision.GetComponent<PlayerController>() != null)
            {
                collision.GetComponent<PlayerController>().TakeDamage(attackDamage);
            }
            GameObject effect = Instantiate(hitEffect, transform.position, Quaternion.identity);
            Destroy(effect, 1f);
            Destroy(gameObject);
        }
        else
        {
            GameObject effect = Instantiate(hitEffect, transform.position, Quaternion.identity);
            Destroy(effect, 1f);
            Destroy(gameObject);
        }
    }
}
