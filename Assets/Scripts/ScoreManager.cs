using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ScoreManager : MonoBehaviour
{
    public TMP_Text text;
    public static int score=0;
    private float lastTime=0f;
    public float cdScore;
    private void Start()
    {
        score = 0;
    }
    // Update is called once per frame
    void Update()
    {
        if(CameraMove.CountDown())
        {
            lastTime = Time.time;
            text.enabled  = false;
            return;
        }
        {
            text.enabled = true;
        }
        if (Time.time -  lastTime >= cdScore && Time.time >1f && !PlayerController.playerDead())
        {
            lastTime = Time.time;
            score += 100;
        }
        text.text = score.ToString();
    }
    public static void ResetScore()
    {
        score = 0;
    }

}
