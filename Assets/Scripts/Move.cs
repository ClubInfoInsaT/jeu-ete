using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Move : MonoBehaviour
{
    
    [Header("Basic Variables")]

    private Rigidbody2D rb;
    private Collider2D other;
    private Vector2 velocity;
    private bool canJump = false;
    private float tol = 2f;
    private float gravityForce = -9.81f;
    public Transform groundCheck;
    //public Transform wallCheck;


    [Header("Basic Moves")]
    
    private float horizontalInput;
    private float moveForce;
    private float jumpForce = 1f;
    private Vector3 defaultScale;
    private float jumps;
    private float lastJump;
    private bool isRunning;
    private bool isWalking;
    private bool isCrouching;

    
    public float crouchJump = 4f;
    public float normalJump = 7.5f;
    public float stopForce;
    public float SprintSpeed, WalkSpeed;
    public float CrouchSpeed;
    public float jumpNumber=2;
    public float jumpCD=0.5f;
    public Animator Anim;



    //public Vector2 wallJumpDirection;


    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        defaultScale = rb.transform.localScale;
        jumps = jumpNumber;
        lastJump = Time.time;
    }

    // Update is called once per frame
    void Update()
    {
        velocity = rb.velocity;
        move();
        jump();
        flip();
        gravity();
        Crouch();
        rb.velocity = velocity;
        canJump = true;
    }
    void move()
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

        horizontalInput = Input.GetAxisRaw("Horizontal");

        if (horizontalInput*velocity.x < 0)
        {
            velocity.x = 0f;   // Pour fluidifier le demi tour et empêcher les glissement       
        }

        if (horizontalInput != 0)
        {
            rb.AddForce(new Vector2(horizontalInput * moveForce * Time.deltaTime, 0f)); // Ajout de force pour le déplacement | Modification possible en modifiant la vélocité 
            
        }
        else
        {
            if(Mathf.Abs(velocity.x)>= tol)
            {
                rb.AddForce(new Vector2(-10f*rb.velocity.normalized.x * moveForce * Time.deltaTime, 0f)); // Force contraire pour arrêter le mouvement progressivement | Modification possible avec la vélocité 
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
        }

        if (!canJump)
        {
            Anim.SetBool("Jump", true);
        }
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            velocity.y += stopForce;
        }

        if ( Input.GetButtonDown("Jump") && (jumps > 1 || canJump))
        {
            if(Mathf.Abs(Time.time - lastJump) >= jumpCD)
            {
                velocity.y = 0f;
                canJump = false;
                jumps--;
                velocity.y += jumpForce;
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
        Vector3 Scale = rb.transform.localScale;
        if(velocity.x <0f)
         {
            Scale.x = defaultScale.x*-1f;
        }
         else if (velocity.x > 0)    
         {
            Scale.x = defaultScale.x;
         }
        rb.transform.localScale = Scale;
    }

    void Crouch()
    {
        Vector3 Scale = rb.transform.localScale;
        if (Input.GetButton("Crouch"))
        {
            Scale.y = defaultScale.y * 0.5f; //A modifier mais très marrant à voir 
            jumpForce = crouchJump;    
        }
        else
        {
            Scale.y = defaultScale.y;
            jumpForce = normalJump;
        }
            
        rb.transform.localScale = Scale;
    }
    /*private bool canJump()
    {
        return Physics2D.OverlapCircle(groundCheck.position, 0.3f, LayerMask.GetMask("Ground"));    // Fonction Vérification
    }*/ 
}
