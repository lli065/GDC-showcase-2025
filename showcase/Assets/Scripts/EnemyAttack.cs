using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    public GameObject hitEffect;
    public int attackDamage = 40;

    void OnCollisionEnter2D(Collision2D collision) {
        collision.gameObject.GetComponent<PlayerController>().TakeDamage(attackDamage);
        GameObject effect = Instantiate(hitEffect, transform.position, Quaternion.identity);
        Destroy(effect, 5f);
        Destroy(gameObject);
    }
}
