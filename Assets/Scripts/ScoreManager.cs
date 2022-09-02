using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ScoreManager : MonoBehaviour
{
    public TMP_Text text;
    public static int score=0;
    private float lastTime=0f;

    // Update is called once per frame
    void Update()
    {
        if(CameraMove.countDown > 0)
        {
            lastTime = Time.time;
            text.enabled  = false;
            return;
        }
        {
            text.enabled = true;
        }
        if (Time.time -  lastTime >= 10f && Time.time >1f && !PlayerController.playerDead())
        {
            lastTime = Time.time;
            score += 10;
        }
        text.text = score.ToString();
    }
    
}
