using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    private GameObject player;
    private Rigidbody2D rb;
    public float speed = 1f;
    public float lookRadius = 10f;

    // Start is called before the first frame update
    void Start()
    {
        player = FindObjectOfType<PlayerController>().gameObject;
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        rb.velocity = (player.transform.position - transform.position).normalized * speed;
        transform.localScale = rb.velocity.x > 0 ? new Vector3(1, 1) : new Vector3(-1, 1);
    }
}
