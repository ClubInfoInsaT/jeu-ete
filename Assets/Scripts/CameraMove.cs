using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Unity.VisualScripting;

public class CameraMove : MonoBehaviour
{
    public Transform cameraPos;
    public float speed;
    public static  float countDown = 3;
    public TMP_Text text;
    public static float defaultValue;
    private static float loadTime; 
    public float cdIncrease;
    private float lastCameraCall ;
    public float incrementValue;
    public PlayerController player;
    public float speedCap;
    private static bool maxSpeed;
    public Generation generationScript;
    public static bool genEnabled= false;
    public float generationDelay = 10f;
    private void Start()
    {
        //Subscribing to pause event
        FindObjectOfType<PauseMenu>().pauseEvent += StopGen; 
        
        text.enabled = true;
        defaultValue = countDown;
        lastCameraCall = Time.time;
        TimerRestart(3f);
        genEnabled = true;
    }
    public static void TimerRestart(float resetValue)
    {
        //Remettre le timer � 0 
        loadTime = Time.timeSinceLevelLoad;
        countDown = resetValue;

    }
    // Update is called once per frame
    void Update()
    {
        if(countDown > 0f)
        {
            countDown -= Time.deltaTime;
            text.enabled = true;
            text.text = ((int)countDown+1).ToString();
            return; 
        }else
        {
            text.text = "";
            text.enabled = false;
        }
        
        if (genEnabled)
        {
            StartCoroutine(Periodic_Generator());
            genEnabled = false;
        }

        if ( Time.time >0 && Time.time - lastCameraCall> cdIncrease)
        {
            if(speed > speedCap - incrementValue)
            {
                maxSpeed = true;
                return;
            }
            lastCameraCall = Time.time;
            speed += incrementValue;
            if(player!= null)
            {
                player.increaseSpeed(incrementValue);
            }
            
        }
        if (!PlayerController.playerDead())
        {
            
            cameraPos.Translate(speed * Time.deltaTime * Vector2.right);
        }
    }

    void StopGen(PauseMenu t)
    {
        StopAllCoroutines();
        genEnabled = true;
    }
    
    public static bool CountDown()
    {
        return countDown > 0f;
    }

    public static bool isMaxed()
    {
        return maxSpeed == true;
    }

    IEnumerator Periodic_Generator()
    {   
//        Debug.Log("Update chunk window"+Time.time.ToString()); // Suppression et spawn de chunks (Voir methode de Valentin)
         
        if (PlayerController.playerDead())
        {
            yield break;
        }
        if (!isMaxed())
            generationDelay -= 0.1f;
        yield return new WaitForSeconds(generationDelay);
        generationScript.SpawnRandomChunk();
        genEnabled = true;

    }

}
