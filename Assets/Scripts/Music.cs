using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Music : MonoBehaviour
{

    public AudioClip CrescendoClip;
    public AudioClip[] LoopClips;
    public bool SkipIntro; 
    private AudioSource audioSource;

    public static bool pause = false; 
    // Start is called before the first frame update
    IEnumerator Start()
    {
        audioSource = transform.GetComponent<AudioSource>();

        if (!SkipIntro)
        {
            audioSource.clip = CrescendoClip;
            Debug.Log("Now Playing : " + audioSource.clip.name);
            audioSource.Play();
            yield return new WaitForSeconds(audioSource.clip.length);
            
            //Subscribing to Pause / Unpause Events
            //...
            // Note de Gwen, j'ai fait quelques modifs pour la musique notament la pause du jeu et les clips qui ne doivent être joués que si la vitese est au max 
        }

    }

    // Update is called once per frame
    void Update()
    {
        if (PlayerController.playerDead())
        {
            audioSource.Pause();
            return;
        }
        if (pause)
        { 
            if (audioSource.isPlaying)
                audioSource.Pause();
        }
        else
        {
            if (!audioSource.isPlaying)
            {
                if (CameraMove.isMaxed()) {
                    audioSource.clip = LoopClips[Random.Range(0, LoopClips.Length)];
                    Debug.Log("Now Playing : " + audioSource.clip.name);
                }
                audioSource.Play();
            }
        }
    }

    
}
