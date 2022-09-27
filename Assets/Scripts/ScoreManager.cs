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
    
    //event passage au biome suivant
    public delegate void nextBiomeDelegate(ScoreManager t) ; 
    public event nextBiomeDelegate nextBiomeEvent ;
    public int nextBiomeStep;
    private int currentBiomeStep = 0 ; 
    private void Start()
    {
        score = 0;
        currentBiomeStep = nextBiomeStep; 
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
            if (score >= currentBiomeStep && nextBiomeEvent != null)
            {
                nextBiomeEvent(this);
                Debug.Log("Changement de biome. "+score);
                currentBiomeStep += nextBiomeStep; 
            }
        }
        text.text = score.ToString();
    }
    public static void ResetScore()
    {
        score = 0;
    }

}
