using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMove : MonoBehaviour
{
    public Transform cameraPos;
    public float speed;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!PlayerController.playerDead())
        {
            cameraPos.Translate(speed * Time.deltaTime * Vector2.right);
        }
       
    

    }
}
