using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SailMove : MonoBehaviour
{
    Rigidbody2D RB;
    SpriteRenderer SR;

    void Start()
    {
        RB = GetComponent<Rigidbody2D>();
        SR = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        float axis_width = Input.GetAxisRaw("Horizontal");
        float axis_length = Input.GetAxisRaw("Vertical");

        if (axis_width != 0)
        {
            SR.flipX = axis_width == 1;
        }


        RB.velocity = new Vector2(3 * axis_width,3 * axis_length);
    }
}
