using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    int index;
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
    public void Test()
    {
        return;
    }
}
