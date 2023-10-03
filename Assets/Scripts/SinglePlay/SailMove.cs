using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SailMove : MonoBehaviour
{
    Rigidbody2D RB;
    SpriteRenderer SR;

    public UImanager_SinglePlay UImanager;
    public Animator Animator;
    public int PlayerSpeed = 3;
    public bool Isinvincibility = false;

    void Start()
    {
        RB = GetComponent<Rigidbody2D>();
        SR = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        if (!UImanager.GameOver)
        {
            playerMove();
            UseBooster();
        }
    }
    public void playerMove()
    {
        float axis_width = Input.GetAxisRaw("Horizontal");
        float axis_length = Input.GetAxisRaw("Vertical");

        if (axis_width != 0)
        {
            SR.flipX = axis_width == 1;
        }

        RB.velocity = new Vector2(PlayerSpeed * axis_width, PlayerSpeed * axis_length);
    }

    public void UseBooster()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            UImanager.slider.value -= Time.deltaTime;
            PlayerSpeed = 5;
        }
        else
        {
            UImanager.slider.value += Time.deltaTime;
            PlayerSpeed = 3;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Obstacle" && !Isinvincibility)
        {
            UImanager.DropHeart();
            StartCoroutine(invincibilityTime());
        }
    }

    IEnumerator invincibilityTime()
    {
        Animator.SetBool("IsHitted", true);
        Isinvincibility = true;
        yield return new WaitForSeconds(2);
        Animator.SetBool("IsHitted", false);
        Isinvincibility = false;
    }
}
