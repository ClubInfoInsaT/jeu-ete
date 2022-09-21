using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Events;

public class PauseMenu : MonoBehaviour
{
    enum UIlist {Menu, Controls}
    public GameObject PauseUI,GameUI,DeathUI;
    public GameObject[] UIs;
    public AudioSource source;
    public static UnityEvent PauseEvent,ResumeEvent;
    private bool gameState = false; //false = pause true= ingame

    private void Start()
    {
        if (PauseEvent == null)
            PauseEvent = new UnityEvent();
        PauseEvent.AddListener(PauseGame);
        if (ResumeEvent == null)
            ResumeEvent = new UnityEvent();
        ResumeEvent.AddListener(ResumeGame);
        GameUI.SetActive(true);
        PauseUI.SetActive(false);
        DeathUI.SetActive(false);
        Time.timeScale = 1f;
        
    }

    private void Update()
    {
        
        if (Input.GetKeyDown(KeyCode.P ) )
        {
            if (gameState)
            {
                ResumeEvent.Invoke();
            }
            else
            {
                PauseEvent.Invoke();
            }
            
        }

        if (PlayerController.playerDead() || DinoController.playerDead())
        {
            Invoke("GameOverDisplay", 1f);
        }
    }

    void PauseGame()
    {
        gameState = true;
        Time.timeScale = 0f;
        source.Pause();
        Music.pause = true;
        CameraMove.genEnabled = false;
        GameUI.SetActive(false);
        PauseUI.SetActive(true);
        for(int i = 0; i < UIs.Length; i++)
        {
            UIs[i].gameObject.SetActive(false);
        }
        UIs[0].gameObject.SetActive(true);
    }
    public void ResumeGame()
    {
        gameState = false; 
        Music.pause = false;
        Time.timeScale = 1f;
        source.Play();
        GameUI.SetActive(true);
        PauseUI.SetActive(false);  
        CameraMove.genEnabled = true;
    }
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
    }
    void TimerRestart()
    {
        Debug.Log("Resume Game");

    }
}
    