using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parallax : MonoBehaviour
{
    [Serializable]
    public struct BackgroundElement
    {
        public GameObject sprite;
        public float depth; 
    }

    public List<BackgroundElement> backgroundElementList = new(); 
    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < 50; i++)
        {
            foreach (BackgroundElement backgroundElement in backgroundElementList)
            {
                Transform t = backgroundElement.sprite.transform;
                t.localScale = backgroundElement.depth * backgroundElement.sprite.transform.localScale ; 
                Instantiate(backgroundElement.sprite); 
                
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
