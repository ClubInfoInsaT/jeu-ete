using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Unity.VisualScripting.Member;

public class DinoController : MonoBehaviour
{
    public Rigidbody2D player;
    public Transform CameraHolder;
    public Camera Cam;
    public float moveSpeed;
    public float jumpForce;
    private float horizontal; // Transfert de fonctionnalité => Sprint, Slow
    private float vertical; // Transfert de fonctionnalité => Jump, Crouch
    private float defaultMS, defaultJump;
    public Animator anim;

    public Transform wallCheck,groundCheck;
    public LayerMask wallMask, groundMask;
    private bool wallCollide,isGrounded;
    private static bool isDead;
    public Transform CamResetTransform,CamDeathTransform;
    public float jumpHeight;
    public Collider2D bodyCollider;
    public float sprintMultiplier;
    public float crouchMultiplier;
    public float jumpMultiplier;
    public Transform spawn;
    private bool isSpiked; 



    void Start()
    {
        
        player = GetComponentInChildren<Rigidbody2D>();
        CameraHolder = gameObject.transform;  
        defaultMS = moveSpeed;
        defaultJump = jumpForce;
        isDead = false;
        anim.SetBool("isWalking", true);
        if (spawn != null)
        {
            CameraHolder.position =new Vector3(spawn.position.x, spawn.position.y, CameraHolder.position.z) ;
        }
    }

    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        horizontal = Input.GetAxisRaw("Horizontal");
        vertical = Input.GetAxisRaw("Vertical");
        wallCollide = wallCheck == null ? false : Physics2D.OverlapBox(wallCheck.position, new Vector2(0.09f, 0.6f), 0f, wallMask);
        isSpiked = Physics2D.OverlapBox(groundCheck.position, new Vector2(0.3f, 0.03f), 0f, groundMask) != null ? Physics2D.OverlapBox(groundCheck.position, new Vector2(0.3f, 0.03f), 0f, groundMask).CompareTag("Spike"):false;
        
        if (CameraMove.countDown > 0)
        {
            return;
        }
        if (isDead)
        {
            //anim.CrossFade("Death", 0f, 0);
            return;
        }
        Speed();
        MoveCam();
        DeathManager();
        JumpBehavior();
    
    }
    void MoveCam()
    {
        if (!wallCollide)
        {
            CameraHolder.Translate(Vector2.right * moveSpeed * Time.deltaTime);
            Cam.gameObject.transform.position = new Vector3(CamResetTransform.position.x, Cam.gameObject.transform.position.y, Cam.gameObject.transform.position.z);
        }
        else
        {
            Cam.gameObject.transform.Translate(Vector2.right * moveSpeed * Time.deltaTime);
 
        }
    }
    void Speed()
    {
        if (vertical < -0.99f || Input.GetButton("Crouch"))
        {
            moveSpeed = defaultMS * crouchMultiplier;
            Crouch();
        }
        else if (Input.GetButton("Sprint") || horizontal > 0.01f)
        {
            moveSpeed = defaultMS * sprintMultiplier;
            anim.SetBool("isRunning", true);
        }
        else
        {
            moveSpeed = defaultMS;
            anim.SetBool("isRunning", false);
            anim.SetBool("isCrouching", false);
        }
        if (!isGrounded)
        {
            moveSpeed *= jumpMultiplier;
        }
    }
    void DeathManager()
    {
        if (isSpiked)
        {
            SpikeManager();
        }
        if (Cam.WorldToScreenPoint(player.position).x < 0f || Cam.WorldToScreenPoint(player.position).y < 0f)
        {
            isDead = true;
            Cam.gameObject.transform.position = new Vector3(CamResetTransform.position.x, Cam.gameObject.transform.position.y, Cam.gameObject.transform.position.z);
            player.position = new Vector2(player.position.x,Cam.WorldToScreenPoint(player.position).y);   
            Death();
        }
    }
    void JumpBehavior()
    {
        isGrounded = Physics2D.OverlapBox(groundCheck.position, new Vector2(0.3f, 0.03f), 0f, groundMask);
        if (isGrounded)
        {
            anim.SetBool("Jump", false);
        }
        if (isGrounded && (Input.GetButton("Jump")||vertical > 0.01f))
        {
            
            player.AddForce(new Vector2(0f, jumpForce), ForceMode2D.Impulse);
            isGrounded = false;
            anim.SetBool("Jump", true);
        }
    }
    void Crouch()
    {
        anim.SetBool("isCrouching", true);
    }
    
    void SpikeManager()
    {
        Death();
    }
    void Death()
    {
        bodyCollider.enabled = false;
        isDead = true; 
        anim.SetBool("Jump", false);
        anim.SetBool("isDead", true);
        anim.CrossFade("Death", 0f, 0);
        player.AddForce(Vector2.up * jumpHeight, ForceMode2D.Impulse);
        
    }
    public static bool playerDead()
    {
        return isDead;
    }



}
