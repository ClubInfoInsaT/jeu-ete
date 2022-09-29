using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Transition : MonoBehaviour
{
    private List<SpriteRenderer> SpriteRendererList = new();

    private int fade = 0;
    // Start is called before the first frame update
    void Start()
    {
        foreach (Transform biomeBackground in transform)
        {
            //Debug.Log(child);
            SpriteRendererList.Add(biomeBackground.GetComponent<SpriteRenderer>());
            foreach (Transform layer in biomeBackground.transform)
            {
                //Debug.Log(child);
                SpriteRendererList.Add(layer.GetComponent<SpriteRenderer>());
            }
        }

        //Debug.Log(gameObject.name+" "+SpriteRendererList.ToCommaSeparatedString()); 

    }

    // Update is called once per frame
    void FixedUpdate() 
    {
        switch (fade)
        { 
            case 1 :
                //Fade in
                foreach (SpriteRenderer spriteRenderer in SpriteRendererList)
                {
                    //Debug.Log("f in "+ spriteRenderer.color.a);
                    if (spriteRenderer.color.a < 1)
                        spriteRenderer.color += new Color(0,0,0,0.010f); 
                    else fade = 0; 
                }
                break; 
            case 2:
                // Fade out
                foreach (SpriteRenderer spriteRenderer in SpriteRendererList)
                {
                    //Debug.Log("f out "+ spriteRenderer.color.a);
                    if (spriteRenderer.color.a > 0)
                        spriteRenderer.color -= new Color(0, 0, 0, 0.010f);
                    else fade = 0; 
                }
                break;
        }
    }

    public void FadeIn()
    {
        fade = 1;
        Debug.Log(gameObject.name+" Fading in");
    }
    
    
    public void FadeOut()
    {
        fade = 2; 
        Debug.Log(gameObject.name+" Fading out");
    }
    
    public void Hide(bool h)
    {
        foreach (SpriteRenderer spriteRenderer in SpriteRendererList)
        {
            spriteRenderer.color = new Color(spriteRenderer.color.r, spriteRenderer.color.g, spriteRenderer.color.b, h ? 0 : 1);
            Debug.Log(h ? "Unhiding" : "Hiding" + spriteRenderer.gameObject.name);
        }
    }
    
}
