using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Music : MonoBehaviour
{

    public AudioClip CrescendoClip;
    public AudioClip[] LoopClips;
    public bool SkipIntro; 
    private AudioSource audioSource;

    private bool pause = false; 
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
        }

    }

    // Update is called once per frame
    void Update()
    {
        if (pause)
        { 
            if (audioSource.isPlaying)
                audioSource.Pause();
        }
        else
        {
            if (!audioSource.isPlaying)
            {
                audioSource.clip = LoopClips[Random.Range(0, LoopClips.Length)];
                Debug.Log("Now Playing : " + audioSource.clip.name);
                audioSource.Play();
            }
        }
    }

    
}
