using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Events;
using TMPro;
using static PlayerDesign;

public class PauseMenu : MonoBehaviour
{
    enum UIlist {Menu, Controls}
    public GameObject PauseUI,GameUI,DeathUI;
    public GameObject[] UIs;
    public AudioSource source;
    public TMP_Text highscore;
    public TMP_Text score;
    private bool gameState = true; //false = pause true= ingame
    
    //events lié à la pause
    public delegate void pauseDelegate(PauseMenu t) ; 
    public event pauseDelegate pauseEvent ;
    public delegate void unpauseDelegate(PauseMenu t) ; 
    public event unpauseDelegate resumeEvent ;

    private void Start()
    {
        //On s'abonne à l'event
        pauseEvent += PauseGame ; 
        resumeEvent += ResumeGame ;
        GameUI.SetActive(true);
        PauseUI.SetActive(false);
        DeathUI.SetActive(false);
        Time.timeScale = 1f;
        
    }

    private void Update()
    {
        
        if (Input.GetKeyDown(KeyCode.P ) )
        {
            //Debug.Log("P pressed");
            if (gameState)
            {
                //Debug.Log(gameState);
                if(pauseEvent != null)
                    pauseEvent(this);
            }
            else
            {
                //Debug.Log(gameState);
                Resume();
                    
            }
            
        }

        if (PlayerController.playerDead() || DinoController.playerDead())
        {
            Invoke("GameOverDisplay", 1f);
        }
    }

    void PauseGame(PauseMenu t)
    {
        gameState = false;
        Time.timeScale = 0f;
        source.Pause();
        GameUI.SetActive(false);
        PauseUI.SetActive(true);
        for(int i = 0; i < UIs.Length; i++)
        {
            UIs[i].gameObject.SetActive(false);
        }
        UIs[0].gameObject.SetActive(true);
    }
    public void ResumeGame(PauseMenu t)
    {
        gameState = true;
        Time.timeScale = 1f;
        source.Play();
        GameUI.SetActive(true);
        PauseUI.SetActive(false);  
    }
    public void Resume() {if (resumeEvent != null) resumeEvent(this);}
    public void DisplayControl()
    {
        UIs[(int)UIlist.Menu].gameObject.SetActive(false);
        UIs[(int)UIlist.Controls].gameObject.SetActive(true);
    }
    public void goBack()
    {
        UIs[(int)UIlist.Menu].gameObject.SetActive(true);
        UIs[(int)UIlist.Controls].gameObject.SetActive(false);
    }

    public void QuitGame()
    {
        SceneManager.LoadScene("Title Screen");
    }

    public void EndGame()
    {
        Application.Quit();
    }

    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        ScoreManager.ResetScore();
        GameUI.SetActive(true);
        PauseUI.SetActive(false);
        DeathUI.SetActive(false);
        PlayerController.Resurrect();
        CameraMove.TimerRestart(5);
    }

    public void GameOverDisplay()
    {
        DeathUI.SetActive(true);
        GameUI.SetActive(false);
        PauseUI.SetActive(false);
        string teamdisplay ="none"; 
        switch (FindObjectOfType<PlayerDesign>().index)
        {
            case (int)team.PKPeach:
                teamdisplay = "PK Peach";
                break;
            case (int)team.BooMario:
                teamdisplay = "BooMario";
                break;
        }
        score.text = "Your  Highscore : " + ScoreManager.totalScore + " by team " + teamdisplay;
        highscore.text = "Current Highscore : " + PlayerPrefs.GetFloat("highscore") + " \n by team " + PlayerPrefs.GetString("Teamname");
        PlayerPrefs.Save();
    }

    public bool StateOfGame()
    {
        return gameState; 
    }

}
    