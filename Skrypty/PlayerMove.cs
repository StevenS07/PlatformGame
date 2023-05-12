using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class PlayerMove : MonoBehaviour
{
    
    //Start Valables
    private Rigidbody2D rb;
    private Animator anim;
    private Collider2D coll;

    //FSM
    private enum State { idle, runing, jumping, falling, hurt }
    private State state = State.idle;

    //Player Valibles
    [SerializeField] private LayerMask ground;
    [SerializeField] private float speed = 5f;
    [SerializeField] private float jumpFroce = 15f;
    [SerializeField] private int cherries = 0;
    [SerializeField] private TextMeshProUGUI CherryText;
    [SerializeField] private float hurtforce = 10f;
    [SerializeField] private int health;
    [SerializeField] private Text healthAmount;


   

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        coll = GetComponent<Collider2D>();
        healthAmount.text = health.ToString();
    }
    private void Update()
    {
        if (state != State.hurt)
        {
            Movment();
        }


        AnimationState();
        anim.SetInteger("state", (int)state);

        if (gameObject.transform.position.y <= -30)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }

        if (Input.GetButtonDown("Cancel") && coll.IsTouchingLayers(ground))
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene(0);
        }
    }
    
    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Collectable")
        {
            Destroy(collision.gameObject);
            cherries += 1;
            CherryText.text = cherries.ToString();
        }
        
    }


    private void OnCollisionEnter2D(Collision2D other)
    {
        Enemy enemy = other.gameObject.GetComponent<Enemy>();
        if (other.gameObject.tag == "Enemy")
        {
            if (state == State.falling)
            {
                enemy.JumpOn();
                Jump();
            }
            else
            {
                state = State.hurt;
                HealthLose();
                if (other.gameObject.transform.position.x > transform.position.x)
                {
                    rb.velocity = new Vector2(-hurtforce, rb.velocity.y);
                }
                else
                {
                    rb.velocity = new Vector2(hurtforce, rb.velocity.y);
                }
            }

        }
    }

    private void HealthLose()
    {
        health -= 1;
        healthAmount.text = health.ToString();
        if (health <= 0)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }

    private void Movment()
    {
        float hDirection = Input.GetAxis("Horizontal");

        if (Input.GetKey(KeyCode.LeftShift))
        {
            speed = 7f;
        }
        else
        {
            speed = 5f;
        }

        //Moving right
        if (hDirection < 0)
        {
            rb.velocity = new Vector2(-speed, rb.velocity.y);
            transform.localScale = new Vector2(-1, 1);
        }
        //Moving left
        else if (hDirection > 0)
        {
            rb.velocity = new Vector2(speed, rb.velocity.y);
            transform.localScale = new Vector2(1, 1);
        }

        //Jumping
        if (Input.GetButtonDown("Jump") && coll.IsTouchingLayers(ground))
        {
            Jump();
        }
        

       

    }

    private void Jump()
    {
        rb.velocity = new Vector2(rb.velocity.x, jumpFroce);
        state = State.jumping;
    }

    private void AnimationState()
    {
        if (state == State.jumping)
        {
            if (rb.velocity.y < .1f)
            {
                state = State.falling;
            }
        }
        else if (state == State.falling)
        {
            if (coll.IsTouchingLayers(ground))
            {
                state = State.idle;
            }
        }
        else if (state == State.hurt)
        {
            if (Mathf.Abs(rb.velocity.x) < .1f)
            {
                state = State.idle;
            }
        }
        else if (Mathf.Abs(rb.velocity.x) > 2f)
        {
            state = State.runing;
        }
        else
        {
            state = State.idle;
        }


    }

    
}

