using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SailMove : MonoBehaviour
{
    Rigidbody2D RB;
    SpriteRenderer SR;

    public UImanager_SinglePlay UIManager;

    void Start()
    {
        RB = GetComponent<Rigidbody2D>();
        SR = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        if(!UIManager.GameOver) playerMove();
    }

    public void playerMove()
    {
        float axis_width = Input.GetAxisRaw("Horizontal");
        float axis_length = Input.GetAxisRaw("Vertical");

        if (axis_width != 0)
        {
            SR.flipX = axis_width == 1;
        }

        RB.velocity = new Vector2(3 * axis_width, 3 * axis_length);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Obstacle")
        {
            RB.velocity = Vector2.zero;
            UIManager.GameOver = true;
        }
    }
}
