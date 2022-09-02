using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Nouvelle version du Platform Character Controller de Gwen

public class PlayerController : MonoBehaviour
{
    private enum clips {Jump};
    [Header("Main Components")]
    [Tooltip("Position de départ du personnage")] public Transform spawn;
    [Tooltip("Corps du personnage responsable des forces et mouvements")] public Rigidbody2D rb2D;
    [Tooltip("Corps du personnage responsable des collisions")] public Collider2D bodyCollider; 
    [Tooltip("Position du vérificateur de sol nécessaire au saut")] public Transform groundCheck;
    [Tooltip("Layer à utiliser pour identifier le sol")] public LayerMask groundMask;
    [Tooltip("Position du vérificateur de mur nécessaire au wall jump")] public Transform wallCheck;
    [Tooltip("Layer a utiliser pour identifier un mur")] public LayerMask wallMask;
    [Tooltip("Liste de sons jouable par le joueur (exemple : Saut)")] public AudioClip[] cliplist;
    [Tooltip("Source d'émission audio")] public AudioSource source; 

    [Header("Move Variables")]
    [Tooltip("Vitesse de marche")] public float defaultSpeed;
    [Tooltip("Multiplieur amplificateur de vitesse")][Range(1f,3f)] public float sprintMultiplier;
    [Tooltip("Multiplieur réducteur de vitesse")][Range(0,1f)] public float crouchMultiplier;
    private float moveSpeed;
    private float horizontal; // Movement Input
    private float vertical; // Crouch Input

    [Header("Jump Variables")]
    [Tooltip("Hauteur du saut")][Min(20)]public float jumpForce;
    [Tooltip("Temps d'attente entre 2 sauts")]public float jumpCooldown;
    [Tooltip("Multiplieur réducteur pour ralentir la vitesse pendant le saut et la distance par la même occasion")][Range(0,1f)] public float jumpMultiplier;
    private float lastJump;
    private bool isGrounded;
    private Collider2D groundCollider;

    [Header("Death Variables")]
    [Tooltip("Hauteur du saut lors de l'animation de mort")][Min(15)] public float jumpHeight;
    private static bool isDead = false;

    [Header("Animation Variables")]
    public Animator anim; 

    // [Header("Wall Sliding and Jumping variables")]


    // Start is called before the first frame update
    void Start()
    {
        rb2D = gameObject.GetComponent<Rigidbody2D>();
        rb2D.gameObject.transform.position= spawn.position;
        moveSpeed = defaultSpeed;
        isGrounded = false;
        lastJump = Time.time;
        source.volume = 0.5f;
    }

    // Update is called once per frame
    void Update()
    {
        if(CameraMove.countDown > 0)
        {
            return;
        }
        horizontal = Input.GetAxisRaw("Horizontal");
        vertical = Input.GetAxisRaw("Vertical");
        Speed();
    }

    void FixedUpdate()
    {
        if (CameraMove.countDown > 0)
        {
            return;
        }
        if (!isDead)
        {
            Moving();
            Flip();
            Jumping();
        }
        
    }

    void Speed()
    {
        if (vertical < -0.99f || Input.GetButton("Crouch")) 
        {
            moveSpeed = defaultSpeed * crouchMultiplier;
            Crouch();
        }
        else if(Input.GetButton("Sprint"))
        {
            moveSpeed = defaultSpeed * sprintMultiplier;
            anim.SetBool("isRunning", true);
        }
        else
        {
            moveSpeed = defaultSpeed;
            anim.SetBool("isRunning", false);
            anim.SetBool("isCrouching", false);
        }
        if (!isGrounded)
        {
            moveSpeed *= jumpMultiplier;
        }
    }
    void Moving()
    {
        if(horizontal > 0.1f ||horizontal < -0.99f)
        {
            anim.SetBool("isWalking", true);
            rb2D.AddForce(new Vector2(horizontal * moveSpeed,0f),ForceMode2D.Impulse);
        }
        else
        {
            anim.SetBool("isWalking", false);
        }
    }
    void Jumping()
    {
        isGrounded = Physics2D.OverlapBox(groundCheck.position, new Vector2(0.3f, 0.03f),0f, groundMask) ;
        if (isGrounded)
        {
            anim.SetBool("Jump", false);
            source.Stop();
        }
        if (isGrounded && Input.GetButton("Jump"))
        {
            if(Time.time - lastJump > jumpCooldown)
            {
                lastJump = Time.time;   
                rb2D.AddForce(new Vector2(0f, jumpForce), ForceMode2D.Impulse);
                isGrounded = false;
                anim.SetBool("Jump", true);
                source.clip = cliplist[0];
                source.Play();
                
            }
            
        }
    }
    void Crouch()
    {
        anim.SetBool("isCrouching", true);
    }

    void Flip()
    {
        if(horizontal < -0.99f)
        {
            gameObject.GetComponent<SpriteRenderer>().flipX = true;
        }
        else
        {
            gameObject.GetComponent<SpriteRenderer>().flipX = false;
        }
    }


    void SpikeManager()
    {
        Death(); 
    }

    void SlimyManager()
    {
        return;
    }

    void Death()
    {
        bodyCollider.enabled = false;
        rb2D.gravityScale *= 0.5f;
        rb2D.AddForce(Vector2.up * jumpHeight, ForceMode2D.Impulse);
        isDead = true;
        anim.SetBool("isDead", true);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Spike"))
        {
            SpikeManager();
        }
    }

    public static bool playerDead()
    {
        return isDead;
    }


}
