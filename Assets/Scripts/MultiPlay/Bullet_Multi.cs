using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Bullet_Multi : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Wall")
        {
            Destroy(gameObject);
        }
    }
}
