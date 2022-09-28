using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Unity.VisualScripting;
using System.Diagnostics.CodeAnalysis;
using UnityEngine.Tilemaps;

public class CameraMove : MonoBehaviour
{
    public Transform cameraPos;
    public float speed;
    public static  float countDown = 3;
    public TMP_Text text;
    public static float defaultValue;
    private static float loadTime;
    private static float effectiveTime;  // EFFECTIVE TIME SINCE LOAD TO AVOID PAUSE STATE
    public float cdIncrease;
    private float lastCameraCall ;
    public float incrementValue;
    public PlayerController player;
    public float speedCap;
    private static bool maxSpeed;
    public Generation generationScript;
    private static bool genEnabled= false;
    public float generationDelay = 10f;
    public float lastGen;
    private float defaultDelay;
    public float spawnDelay;
    public PauseMenu pauseScript;
    int count = 0;
    GameObject[] windowArray;
    private bool screenState;
    public float despawnDist;
    private void Start()
    {
        //Subscribing to pause event
        //FindObjectOfType<PauseMenu>().pauseEvent += StopGen; 
        defaultDelay = generationDelay; 
        text.enabled = true;
        defaultValue = countDown;
        lastCameraCall = Time.time;
        loadTime = Time.timeSinceLevelLoad;
        TimerRestart(3f);
        genEnabled = true;
        StartCoroutine(Auto_Generate());
    }
    public static void TimerRestart(float resetValue)
    {
        //Remettre le timer � 0 
        loadTime = Time.timeSinceLevelLoad;
        countDown = resetValue;

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
        ////Debug.Log(genEnabled);
        if (pauseScript.StateOfGame())
        {
            effectiveTime += Time.deltaTime;
            /*if (genEnabled)
            {
                genEnabled = false;
                StartCoroutine(Auto_Generate());
            }*/
        }
       
        if ( Time.time >0 && Time.time - lastCameraCall> cdIncrease)
        {

            if(speed > speedCap - incrementValue)
            {
                maxSpeed = true;
                return;
            }
            lastCameraCall = Time.time;
            speed += incrementValue;
            if(player!= null)
            {
                player.increaseSpeed(incrementValue);
            }
            
        }

        if (!PlayerController.playerDead())
        {
            
            cameraPos.Translate(speed * Time.deltaTime * Vector2.right);
        }
        UpdateWindow();
    }

    /*void StopGen(PauseMenu t)
    {
        StopAllCoroutines();
        genEnabled = true;
    }*/
    
    public static bool CountDown()
    {
        return countDown > 0f;
    }

    public static bool isMaxed()
    {
        return maxSpeed == true;
    }

    IEnumerator Periodic_Generator()
    {
        //        //Debug.Log("Update chunk window"+Time.time.ToString()); // Suppression et spawn de chunks (Voir methode de Valentin)
        
        if (Time.timeSinceLevelLoad < spawnDelay){
            //Debug.Log("SPAWN DELAY");
            genEnabled = true;
            yield break;
        }
        if (PlayerController.playerDead())
        {
            yield break;
        }
        if (!isMaxed())
            generationDelay -= 0f;
        //Debug.Log("SPAWN CHUNK WAIT");
        yield return new WaitForSeconds(generationDelay);
        //Debug.Log("SPAWN CHUNK CALLED");
        generationScript.SpawnRandomChunk();
        genEnabled = true;

    }

    IEnumerator Auto_Generate()
    {
        //Debug.Log("Entre d'autoGen"+count++.ToString());
        if (countDown > 0f)
            yield return null;
        //Debug.Log("debut de traitement");
        if (PlayerController.playerDead())
        {
            //Debug.Log("PLAYER DEAD");
            yield return null;
        }
        else if (!pauseScript.StateOfGame())
        {
            ////Debug.Log("GAME PAUSED");
            yield return null;
        }
        else if (effectiveTime < spawnDelay)
        {
            //////Debug.Log("SPAWN DELAY");
            yield return null;
        }
        else
        {
            ////Debug.Log("SPAWN CHUNK WAIT");
            lastGen = Time.time;
            yield return new WaitForSeconds(defaultDelay); // A faire attention avec la pause (LastGen, a voir avec Val)
            ////Debug.Log("SPAWN CHUNK CALLED");
            //if((effectiveTime - lastGen) % generationDelay <  generationDelay)
                //yield return new WaitForSeconds((effectiveTime - lastGen) % generationDelay);  // A vérifier
            generationScript.SpawnRandomChunk();
            if (!isMaxed())
                defaultDelay -= 0.1f;
        }
        ////Debug.Log("Fin de Gen");

        genEnabled = true; 
    }

    void UpdateWindow()
    {
        windowArray = generationScript.getTerrain();
        TilemapRenderer render = windowArray[0].transform.GetChild(0).GetComponent<TilemapRenderer>();
        //screenState = cameraPos.GetComponent<Camera>().WorldToScreenPoint(windowArray[0].transform.position).x < -10 * windowArray[0].transform.GetChild(0).GetComponent<UnityEngine.Tilemaps.Tilemap>().size.x ? true : false;
        if (!render.isVisible && cameraPos.GetComponent<Camera>().WorldToScreenPoint(windowArray[0].transform.position).x <0 )
        {
            //Debug.Log("UPDATE CHUNK");
            generationScript.SpawnRandomChunk();
        }
        /*if (screenState)
        {
            Debug.Log("UPDATE CHUNK");
            generationScript.SpawnRandomChunk();
        }*/

    }
    
}
