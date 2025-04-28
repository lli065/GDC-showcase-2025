using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public Rigidbody2D playerRigid;
    public float speed;
    
    private void Start()
    {
        playerRigid = gameObject.GetComponent<Rigidbody2D>();
    }
    private void Update()
    {
        Vector2 movement = new Vector2();
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");
        movement.Normalize();
        playerRigid.velocity = movement * speed;
    }
}
