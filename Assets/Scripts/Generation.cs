using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Generation : MonoBehaviour
{


	private ChunkTemplate templates;
    private Transform gridTrans;
    private Vector3 newPos;
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
        newPos = transform.position; 
        for (int i = 0; i < 10; i++)
        {
            Debug.Log(newPos);
            newChunk = templates.chunks[Random.Range(0, templates.chunks.Length)];
            Debug.Log(newChunk.transform.GetComponent<UnityEngine.Tilemaps.Tilemap>().size);
            newPos += new Vector3(newChunk.transform.GetComponent<UnityEngine.Tilemaps.Tilemap>().size.x / 2, 0, 0);
            Instantiate(newChunk, newPos, Quaternion.identity, gridTrans);
            newPos += new Vector3(newChunk.transform.GetComponent<UnityEngine.Tilemaps.Tilemap>().size.x / 2, 0,0); 
        }
    }
}
