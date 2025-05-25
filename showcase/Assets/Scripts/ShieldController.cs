using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldController : MonoBehaviour
{
    public bool isActive = false;
    public GameObject hitEffect;
    public ParticleSystem shieldParticles;
    void Awake()
    {
        DeactivateShield();
    }

    public void ActivateShield()
    {
        shieldParticles.Play();
        GetComponent<Collider2D>().enabled = true;
        isActive = true;
    }

    public void DeactivateShield()
    {
        shieldParticles.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
        GetComponent<Collider2D>().enabled = false;
        isActive = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!isActive) return;
        if (collision.gameObject.layer == LayerMask.NameToLayer("Enemy") || collision.gameObject.layer == LayerMask.NameToLayer("Attack"))
        {
            DeactivateShield();
            if (collision.gameObject.layer == LayerMask.NameToLayer("Attack"))
            {
                Destroy(collision.gameObject);
            }
            else
            {
                GhostEnemy ghost = collision.GetComponent<GhostEnemy>();
                if (ghost != null)
                {
                    ghost.Knockback(transform.position, 10f);
                }
            }
        }
    }
}
