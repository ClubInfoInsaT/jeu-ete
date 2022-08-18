using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Move : MonoBehaviour
{
    [Header("Visualizer")]

    [Header("Basic Variables")]

    private Rigidbody2D rb;
    private Collider2D other;
    private Vector2 velocity;
    private bool canJump = false;
    private float tol = 2f;
    public float gravityForce = -9.81f;
    public Transform groundCheck;
    //public Transform wallCheck;


    [Header("Basic Moves")]
    
    private float horizontalInput;
    private float moveForce;
    private float jumpForce = 1f;
    private Vector3 defaultScale;
    private float jumps;
    private float lastJump;


    public float crouchJump = 4f;
    public float normalJump = 7.5f;
    public float stopForce;
    public float SprintSpeed, WalkSpeed;
    public float CrouchSpeed;
    public float jumpNumber=2;
    public float jumpCD=0.5f;


    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        defaultScale = rb.transform.localScale;
        jumps = jumpNumber;
        lastJump = Time.time;
    }

    void Update()
    {
        velocity = rb.velocity;
        gravity();
        move();
        flip();
        jump();
        Crouch();
        canJump = true;
        rb.velocity = velocity;
    }

    void move()
    {
        if (canJump)
        {
            if (Input.GetButton("Sprint"))
            {
                moveForce = SprintSpeed;
            }
            else if (Input.GetButton("Crouch"))
            {
                moveForce = CrouchSpeed;
            }
            else
            {
                moveForce = WalkSpeed;
            }
        }

        horizontalInput = Input.GetAxisRaw("Horizontal");

        if (horizontalInput*velocity.x < 0)
        {
            velocity.x = 0f;   // Pour fluidifier le demi tour et emp�cher les glissement       
        }

        if (horizontalInput != 0)
        {
            rb.AddForce(new Vector2(horizontalInput * moveForce * Time.deltaTime, 0f)); // Ajout de force pour le d�placement | Modification possible en modifiant la v�locit� 
        }
        else
        {
            if(Mathf.Abs(velocity.x)>= tol)
            {
                rb.AddForce(new Vector2(-10f*rb.velocity.normalized.x * moveForce * Time.deltaTime, 0f)); // Force contraire pour arr�ter le mouvement progressivement | Modification possible avec la v�locit� 
            }
            else
            {
                velocity.x = 0f;
            }
            
        }

    }
    void jump()
    {
        other = Physics2D.OverlapCircle(groundCheck.position, 0.1f, LayerMask.GetMask("Ground"));
        
        if (other == null)
        {
            canJump = false;
           
        }
        else
        {
            jumps = jumpNumber;
            canJump = true; 
        }
        
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            velocity.y += stopForce;
        }
        if ( Input.GetButtonDown("Jump") && (jumps > 0 || canJump))
        {
            if(Mathf.Abs(Time.time - lastJump) >= jumpCD)
            {
                velocity.y = 0f;
                velocity.y += jumpForce;
                canJump = false;
                jumps--;
            }
        }   
    } 
    
    void gravity()
    {
        if (!canJump)
        {
            velocity.y += gravityForce * Time.deltaTime;
        }
                   
    }
    void flip()
    {
        switch (horizontalInput)
        {
            case -1:
                GetComponent<SpriteRenderer>().flipX = true;
                break;
            case 1:
                GetComponent<SpriteRenderer>().flipX = false;
                break;
            default:
                break;
        }   
    }

    void Crouch()
    {
        Vector3 Scale = rb.transform.localScale;
        if (Input.GetButton("Crouch"))
        {
            Scale.y = defaultScale.y * 0.5f; //A modifier mais tr�s marrant � voir 
            jumpForce = crouchJump;    
        }
        else
        {
            Scale.y = defaultScale.y;
            jumpForce = normalJump;
        }
        rb.transform.localScale = Scale;
    }
   
}
