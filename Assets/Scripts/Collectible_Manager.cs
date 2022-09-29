using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

public class Collectible_Manager : MonoBehaviour
{
    public SpriteRenderer SpriteRenderer; 
    public Sprite[] sprites;
    public AudioSource source;
    public AudioClip clip;

    private void Start()
    {
        int index = Random.Range(0,sprites.Length);
        SpriteRenderer.sprite = sprites[index];
        source = gameObject.GetComponent<AudioSource>();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            ScoreManager.coinScore+=100;
            source.clip = clip;
            source.Play();
            Destroy(gameObject);
        }
    }
}
