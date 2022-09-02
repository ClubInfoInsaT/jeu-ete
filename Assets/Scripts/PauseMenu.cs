using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    enum UIlist {Menu, Controls}
    public GameObject PauseUI,GameUI;
    public GameObject[] UIs;
    public AudioSource source;

    private void Start()
    {
        ResumeGame();
    }

    private void Update()
    {
        
        if (Input.GetKey(KeyCode.Escape))
        {
            PauseGame();
        }
        Debug.Log("1");
    }

    void PauseGame()
    {
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
    public void ResumeGame()
    {
        Time.timeScale = 1f;
        source.Play();
        GameUI.SetActive(true);
        PauseUI.SetActive(false);  
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
}
