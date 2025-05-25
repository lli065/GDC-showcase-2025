using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EdibleMushroom : MonoBehaviour
{
    private bool playerNearby = false;
    [SerializeField] private MushroomType type;

    void Update()
    {
        if (playerNearby && Input.GetKeyDown(KeyCode.E))
        {
            MushroomManager.Instance.AddMushroom(type);
            if (type == MushroomType.Poison)
            {
                Destroy(gameObject);
            }
            else
            {
                GetComponent<SpriteRenderer>().enabled = false;
                GetComponent<Collider2D>().enabled = false;
                Invoke("Respawn", 120f);
            }
            
        }
    }
    public void Respawn()
    {
        GetComponent<SpriteRenderer>().enabled = true;
        GetComponent<Collider2D>().enabled = true;
    }
        private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            playerNearby = true;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            playerNearby = false;
        }
    }
}
