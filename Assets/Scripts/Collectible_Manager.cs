using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

public class Collectible_Manager : MonoBehaviour
{
    public SpriteRenderer SpriteRenderer; 
    public Sprite[] sprites;

    private void Start()
    {
        int index = Random.Range(0,sprites.Length);
        SpriteRenderer.sprite = sprites[index];
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            ScoreManager.score+=100;
            Destroy(gameObject);
        }
    }
}
