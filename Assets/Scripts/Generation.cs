using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Generation : MonoBehaviour
{


	private ChunkTemplate templates;
    private Transform gridTrans;
    private GameObject newChunk;
    // Start is called before the first frame update
    void Start()
    {
        templates = GameObject.FindGameObjectWithTag("Chunks").GetComponent<ChunkTemplate>();
        gridTrans = GameObject.FindGameObjectWithTag("Grid").transform;
        Invoke("Spawn", 0.1f);
    }

    void Spawn()
    {
        float newPos = 0f;
        newChunk = templates.chunks[Random.Range(0, templates.chunks.Length)];
        for (int i = 0; i < 10; i++)
        {
            Debug.Log("["+ newChunk.transform.name + "]"+"New Position : "+new Vector3(newPos, 0, 0));
            Instantiate(newChunk, new Vector3(newPos,0,0) , Quaternion.identity, gridTrans);
            Debug.Log("[" + newChunk.transform.name + "]" + " size :" + newChunk.transform.GetChild(0).GetComponent<UnityEngine.Tilemaps.Tilemap>().size.x);
            newPos += newChunk.transform.GetChild(0).GetComponent<UnityEngine.Tilemaps.Tilemap>().size.x;
            newChunk = templates.chunks[Random.Range(0, templates.chunks.Length)];
            

            //newChunk = templates.chunks[Random.Range(0, templates.chunks.Length)];

            //newPos += new Vector3(newChunk.transform.GetComponent<UnityEngine.Tilemaps.Tilemap>().size.x / 2, 0, 0);
            //Instantiate(newChunk, newPos, Quaternion.identity, gridTrans);
            //newPos += new Vector3(newChunk.transform.GetComponent<UnityEngine.Tilemaps.Tilemap>().size.x / 2, 0,0); 
        }
    }
}
