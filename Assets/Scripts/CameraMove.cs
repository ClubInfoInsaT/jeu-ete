using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CameraMove : MonoBehaviour
{
    public Transform cameraPos;
    public float speed;
    public static  float countDown = 5;
    public TMP_Text text;
    public static float defaultValue;
    private static float loadTime; 
    public float cdIncrease;
    private float lastCameraCall ;
    public float incrementValue;
    public PlayerController player;

    private void Start()
    {
        text.enabled = true;
        defaultValue = countDown;
        lastCameraCall = Time.time;
        TimerRestart(5f); 
    }
    public static void TimerRestart(float resetValue)
    {
        //Remettre le timer à 0 
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
        if ( Time.time >0 && Time.time - lastCameraCall> cdIncrease)
        {
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
    public static bool CountDown()
    {
        return countDown > 0f;
    }

}
