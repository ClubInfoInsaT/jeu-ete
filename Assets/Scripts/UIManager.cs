using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    int index;
    enum UIList {Title, Main,Credit, Control, Rule }
    public GameObject[] UIs;

    private void Start()
    {
        index = 0;  
        for(int i=0; i<UIs.Length; i++)
        {
            if(i == 0)
            {
                UIs[i].SetActive(true);
            }
            else
            {
                UIs[i].SetActive(false);
            }
        }
    }

    private void Update()
    {
        if (Input.anyKeyDown && index == 0){
            UIs[index].SetActive(false);
            UIs[++index].SetActive(true);
        }
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void Back()
    {
        UIs[index].SetActive(false);
        index = (int)UIList.Main;
        UIs[index].SetActive(true);
    }
    public void Controls()
    {
        UIs[index].SetActive(false);
        index = (int)UIList.Control;
        UIs[index].SetActive(true);
    }
    public void Rule()
    {
        UIs[index].SetActive(false);
        index = (int)UIList.Rule;
        UIs[index].SetActive(true);
    }
    public void Credits()
    {
        UIs[index].SetActive(false);
        index = (int)UIList.Credit;
        UIs[index].SetActive(true);
    }
    public void Launch()
    {
        SceneManager.LoadScene("Generation");
    }
}
