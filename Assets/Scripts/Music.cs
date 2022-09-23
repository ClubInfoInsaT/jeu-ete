/* Author: Valentin */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Music : MonoBehaviour
{
    public Slider sliderSon;
    public AudioClip CrescendoClip;
    public AudioClip[] LoopClips;
    public bool SkipIntro; 
    private AudioSource audioSource;
    public AudioSource playerSource;
    public float maxVolume;

    public static bool pause = false; 
    // Start is called before the first frame update
    IEnumerator Start()
    {
        audioSource = transform.GetComponent<AudioSource>();
        if(sliderSon == null)
        {
            throw new System.Exception("Slider non d�fini");
        }
        sliderSon.maxValue = maxVolume;
        sliderSon.value = sliderSon.maxValue;
        if (!SkipIntro)
        {
            audioSource.clip = CrescendoClip;
            Debug.Log("Now Playing : " + audioSource.clip.name);
            audioSource.Play();
            yield return new WaitForSeconds(audioSource.clip.length);
            
            //Subscribing to Pause / Unpause Events
            //...
            // Note de Gwen, j'ai fait quelques modifs pour la musique notament la pause du jeu et les clips qui ne doivent �tre jou�s que si la vitese est au max 
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
            setVolume();
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

    
    void setVolume(){audioSource.volume = sliderSon.value; PlayerController.source.volume = sliderSon.value; }

    
}
