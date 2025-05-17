using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : MonoBehaviour
{
    public GameObject hitEffect;
    public int attackDamage = 40;

    void OnCollisionEnter2D(Collision2D collision) {
        collision.gameObject.GetComponent<EnemyController>().TakeDamage(attackDamage);
        GameObject effect = Instantiate(hitEffect, transform.position, Quaternion.identity);
        Destroy(effect, 5f);
        Destroy(gameObject);
    }
}
