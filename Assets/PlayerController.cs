using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Nouvelle version du Platform Character Controller de Gwen

public class PlayerController : MonoBehaviour
{
    [Header("Main Components")]
    [Tooltip("Corps du personnage responsable des forces et mouvements")] public Rigidbody2D rb2D;
    [Tooltip("Position du v�rificateur de sol n�cessaire au saut")] public Transform groundCheck;
    [Tooltip("Layer � utiliser pour identifier le sol")] public LayerMask groundMask;
    [Tooltip("Position du v�rificateur de mur n�cessaire au wall jump")] public Transform wallCheck;
    [Tooltip("Layer a utiliser pour identifier un mur")] public LayerMask wallMask;

    [Header("Move Variables")]
    [Tooltip("Vitesse de marche")] public float defaultSpeed;
    [Tooltip("Multiplieur amplificateur de vitesse")][Range(1f,3f)] public float sprintMultiplier;
    [Tooltip("Multiplieur r�ducteur de vitesse")][Range(0,1f)] public float crouchMultiplier;
    private float moveSpeed;
    private float horizontal; // Movement Input
    private float vertical; // Crouch Input

    [Header("Jump Variables")]
    [Tooltip("Hauteur du saut")][Min(20)]public float jumpForce;
    [Tooltip("Temps d'attente entre 2 sauts")]public float jumpCooldown;
    [Tooltip("Multiplieur r�ducteur pour ralentir la vitesse pendant le saut et la distance par la m�me occasion")][Range(0,1f)] public float jumpMultiplier;
    private float lastJump;
    private bool isGrounded; 


    // [Header("Wall Sliding and Jumping variables")]


    // Start is called before the first frame update
    void Start()
    {
        rb2D = gameObject.GetComponent<Rigidbody2D>();

        moveSpeed = defaultSpeed;
        isGrounded = false;
        lastJump = Time.time;
    }

    // Update is called once per frame
    void Update()
    {
        horizontal = Input.GetAxisRaw("Horizontal");
        vertical = Input.GetAxisRaw("Vertical");
        Speed();
        Flip();
    }

    void FixedUpdate()
    {
        Moving();
        Jumping();
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
        }
        else
        {
            moveSpeed = defaultSpeed; 
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
            rb2D.AddForce(new Vector2(horizontal * moveSpeed,0f),ForceMode2D.Impulse);
        }
    }
    void Jumping()
    {
        isGrounded = Physics2D.OverlapBox(groundCheck.position, new Vector2(0.3f, 0.03f),0f, groundMask) ; 

        Debug.Log(isGrounded);
        if (isGrounded && Input.GetButton("Jump"))
        {
            if(Time.time - lastJump > jumpCooldown)
            {
                lastJump = Time.time;   
                rb2D.AddForce(new Vector2(0f, jumpForce), ForceMode2D.Impulse);
                isGrounded = false;
            }
            
        }
    }
    void Crouch()
    {
        return; 
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
}
