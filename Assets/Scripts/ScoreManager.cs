using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using static PlayerDesign;
using System;

public class ScoreManager : MonoBehaviour
{
    public TMP_Text text;
    public static int score=0;
    public static int coinScore,totalScore;
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
            score++;
        }
        totalScore = coinScore + score;
        text.text = totalScore.ToString();
        if (PlayerController.playerDead())
        {
            float currHighScore = PlayerPrefs.GetFloat("highscore");
            if(currHighScore > totalScore)
            {
                return;
            }
            switch (FindObjectOfType<PlayerDesign>().index)
            {
                case (int)team.PKPeach:
                    PlayerPrefs.SetString("Teamname", "PK Peach");
                    break;
                case (int)team.BooMario:
                    PlayerPrefs.SetString("Teamname", "Boomario");
                    break;
            }
            PlayerPrefs.SetFloat("highcore",totalScore);
        }
    }
    public static void ResetScore()
    {
        score = 0;
    }


}
