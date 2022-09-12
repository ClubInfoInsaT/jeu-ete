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
    private float defaultValue;

    private void Start()
    {
        text.enabled = true;
        defaultValue = countDown;
        
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
        
        if (!PlayerController.playerDead())
        {
            cameraPos.Translate(speed * Time.deltaTime * Vector2.right);
        }
    }

}
